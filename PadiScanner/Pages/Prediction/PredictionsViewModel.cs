using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using PadiScanner.Data;
using PadiScanner.Infra;
using PadiScanner.Infra.Services;
using System.ComponentModel;
using System.Security.Claims;

namespace PadiScanner.Pages.Prediction;

public record PredictionImage(string ImageUrl, string Title);

public interface IPredictionsViewModel
{
    Task<bool> CanDelete();
    Task<PredictionHistory?> Get(Ulid id);
    Task<PredictionHistory?> GetWithRelation(Ulid id);
    Task Delete(Ulid id);
    Task Upload(Ulid userId, string location, double latitude, double longitude, IEnumerable<IBrowserFile> files);
    Task<(int TotalItems, IList<PredictionHistory> Items)> PopulateTable(string searchText, TableState state);
}

public class PredictionsViewModel : IPredictionsViewModel
{
    private readonly PadiDataContext _context;
    private readonly IBlobStorageService _blobStorage;
    private readonly AuthenticationStateProvider _authProvider;

    public PredictionsViewModel(PadiDataContext context, IBlobStorageService blobStorage, AuthenticationStateProvider authProvider)
    {
        _context = context;
        _blobStorage = blobStorage;
        _authProvider = authProvider;
    }

    public async Task<bool> CanDelete()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        return authState.User.Claims.Single(x => x.Type == ClaimTypes.Role).Value == AppPolicies.Administrator;
    }

    public async Task<PredictionHistory?> Get(Ulid id)
    {
        return await _context.Predictions.FindAsync(id);
    }

    public async Task<PredictionHistory?> GetWithRelation(Ulid id)
    {
        return await _context.Predictions.Include(x => x.Uploader).SingleAsync(x => x.Id.Equals(id));
    }

    public async Task Delete(Ulid id)
    {
        // check if we can delete
        if (!await CanDelete())
        {
            throw new PadiException("Anda tidak dapat menghapus entri ini.");
        }

        // find the history
        var prediction = await _context.Predictions.FindAsync(id);
        if (prediction == null)
        {
            return;
        }

        // delete related blobs
        if (prediction.OriginalImageUrl != null)
        {
            await _blobStorage.Delete(BlobStorageService.BlobNameFromUri(prediction.OriginalImageUrl));
        }
        if (prediction.HeatmapImageUrl != null)
        {
            await _blobStorage.Delete(BlobStorageService.BlobNameFromUri(prediction.HeatmapImageUrl));
        }
        if (prediction.OverlayedImageUrl != null)
        {
            await _blobStorage.Delete(BlobStorageService.BlobNameFromUri(prediction.OverlayedImageUrl));
        }
        if (prediction.ClippedImageUrl != null)
        {
            await _blobStorage.Delete(BlobStorageService.BlobNameFromUri(prediction.ClippedImageUrl));
        }

        // update
        _context.Predictions.Remove(prediction);

        // save changes
        await _context.SaveChangesAsync();
    }

    public async Task Upload(Ulid userId, string location, double latitude, double longitude, IEnumerable<IBrowserFile> files)
    {
        // get the uploader user
        var uploader = await _context.Users.FindAsync(userId);
        if (uploader == null)
        {
            uploader = await _context.Users.FirstAsync(x => x.Role == UserRole.Guest);
        }

        // process each file
        foreach (var file in files)
        {
            // generate new Ulid
            var id = Ulid.NewUlid();

            // prepare blob name
            var ext = file.ContentType == "image/png" ? ".png" : ".jpg";
            var path = $"{userId}/{id}/original{ext}";

            // upload
            var stream = file.OpenReadStream(PadiConfiguration.MaxUploadSize);
            var uploadedUri = await _blobStorage.Upload(path, stream);

            // pick location
            location = location.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(location))
            {
                location = "LAINNYA";
            }

            // add history
            await _context.Predictions.AddAsync(new PredictionHistory
            {
                Id = id,
                UploadedAt = DateTime.Now,
                Location = location,
                Latitude = latitude,
                Longitude = longitude,
                OriginalImageUrl = uploadedUri,
                Uploader = uploader,
                Status = PredictionStatus.Queued
            });
        }

        // save changes
        await _context.SaveChangesAsync();
    }

    public async Task<(int TotalItems, IList<PredictionHistory> Items)> PopulateTable(string searchText, TableState state)
    {
        // base query
        var query = _context.Predictions
            .Include(x => x.Uploader)
            .AsNoTracking();

        // check if we have a search string
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x => x.Location.Contains(searchText) || x.Uploader.FullName.Contains(searchText));
        }

        // check sorting
        query = state.SortLabel switch
        {
            "full_name" => query.OrderByDirection(state.SortDirection, x => x.Uploader.FullName),
            "location" => query.OrderByDirection(state.SortDirection, x => x.Location),
            "uploaded_at" => query.OrderByDirection(state.SortDirection, x => x.UploadedAt),
            "processed_at" => query.OrderByDirection(state.SortDirection, x => x.ProcessedAt),
            "status" => query.OrderByDirection(state.SortDirection, x => x.Status),
            "result" => query.OrderByDirection(state.SortDirection, x => x.Result),
            _ => query.OrderBy(x => x.UploadedAt)
        };

        // get the total count
        var totalItems = await query.CountAsync();

        // page data
        var pagedData = await query
            .Skip(state.Page * state.PageSize)
            .Take(state.PageSize)
            .ToListAsync();

        return (totalItems, pagedData);
    }
}
