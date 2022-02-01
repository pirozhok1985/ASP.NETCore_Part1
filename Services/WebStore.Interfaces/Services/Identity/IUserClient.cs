using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Interfaces.Services.Identity;

public interface IUserClient :
    IQueryableUserStore<User>,
    IUserClaimStore<User>,
    IUserEmailStore<User>,
    IUserLockoutStore<User>,
    IUserLoginStore<User>,
    IUserPasswordStore<User>,
    IUserRoleStore<User>,
    IUserTwoFactorStore<User>,
    IUserPhoneNumberStore<User>
{
    
}