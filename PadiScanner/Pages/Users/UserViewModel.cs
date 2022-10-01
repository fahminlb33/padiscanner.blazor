using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using PadiScanner.Data;
using PadiScanner.Infra;

namespace PadiScanner.Pages.Users;

public interface IUserViewModel
{
    Task<User?> Get(Ulid id);
    Task Create(CreateRequest model);
    Task Edit(EditRequest model);
    Task Delete(Ulid id);
    Task ChangePassword(ChangePasswordRequest model);
    Task<(int TotalItems, IList<User> Items)> PopulateTable(string searchText, TableState state);
}

public class UserViewModel : IUserViewModel
{
    private readonly PadiDataContext _context;
    private readonly IMapper _mapper;

    public UserViewModel(PadiDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Get(Ulid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task Create(CreateRequest model)
    {
        // map the data
        var user = _mapper.Map<User>(model);

        // save changes
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Edit(EditRequest model)
    {
        // find the user
        var user = await _context.Users.FindAsync(model.Id);
        if (user == null)
        {
            throw new PadiException("User not found.");
        }

        // update
        user.Role = model.Role;
        user.FullName = model.FullName;

        // save changes
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Ulid id)
    {
        // find the user
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return;
        }

        // update
        _context.Users.Remove(user);

        // save changes
        await _context.SaveChangesAsync();
    }

    public async Task ChangePassword(ChangePasswordRequest model)
    {
        // data vaildation
        if (model.NewPassword != model.ConfirmPassword)
        {
            throw new PadiException("Konfirmasi password tidak sama dengan password baru,");
        }

        // change password
        var user = await _context.Users.FindAsync(model.Id);
        if (user == null)
        {
            throw new PadiException("User not found.");
        }

        // update
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        // save changes
        await _context.SaveChangesAsync();
    }

    public async Task<(int TotalItems, IList<User> Items)> PopulateTable(string searchText, TableState state)
    {
        // base query
        var query = _context.Users.AsNoTracking();

        // check if we have a search string
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x => x.FullName.Contains(searchText) || x.Username.Contains(searchText));
        }

        // check sorting
        query = state.SortLabel switch
        {
            "full_name" => query.OrderByDirection(state.SortDirection, x => x.FullName),
            "username" => query.OrderByDirection(state.SortDirection, x => x.Username),
            "last_login" => query.OrderByDirection(state.SortDirection, x => x.LastLoginAt),
            _ => query.OrderBy(x => x.Id)
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
