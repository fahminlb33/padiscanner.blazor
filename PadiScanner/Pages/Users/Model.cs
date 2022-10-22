using AutoMapper;
using PadiScanner.Data;

namespace PadiScanner.Pages.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateRequest, User>()
            .ForMember(x => x.Id, c => c.MapFrom(p => Ulid.NewUlid()))
            .ForMember(x => x.Password, c => c.MapFrom(p => BCrypt.Net.BCrypt.HashPassword(p.Password)));
        CreateMap<User, EditRequest>();
    }
}

public class CreateRequest
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}

public class EditRequest
{
    public Ulid Id { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }
}

public class ChangePasswordRequest
{
    public Ulid Id { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
