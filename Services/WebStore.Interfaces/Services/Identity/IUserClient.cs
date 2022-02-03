using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Interfaces.Services.Identity;

public interface IUserClient :
    IUserLockoutStore<User>,
    IUserClaimStore<User>,
    IUserLoginStore<User>,
    IUserEmailStore<User>,
    IUserPasswordStore<User>,
    IUserPhoneNumberStore<User>,
    IUserTwoFactorStore<User>,
    IUserRoleStore<User>
{
    
}