using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Domain.DTO.Identity;

public abstract class UserDTO
{
    public User User { get; set; }
}

public class AddLoginDto : UserDTO
{
    public UserLoginInfo UserLoginInfo { get; set; }
}

public class PasswordHashDto : UserDTO
{
    public string Hash { get; set; }
}

public class SetLockoutDto : UserDTO
{
    public DateTimeOffset? LockoutEnd { get; set; }
}