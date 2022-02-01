using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Domain.DTO.Identity;

public abstract class UserDto
{
    public User User { get; set; }
}

public class LoginDto : UserDto
{
    public UserLoginInfo LoginInfo { get; set; }
}

public class PasswordHashDto : UserDto
{
    public string Hash { get; set; }
}

public class LockoutDto : UserDto
{
    public DateTimeOffset? LockoutEndTime { get; set; }
}