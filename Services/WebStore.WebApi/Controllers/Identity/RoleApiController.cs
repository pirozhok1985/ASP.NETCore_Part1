using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces;

namespace WebStore.WebApi.Controllers.Identity;

[ApiController]
[Route(WebApiAddress.Identity.Roles)]
public class RoleApiController : ControllerBase
{
    private readonly RoleStore<Role> _roleStore;

    public RoleApiController(WebStoreDB db)
    {
        _roleStore = new RoleStore<Role>(db);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Role>> GetAll() => await _roleStore.Roles.ToArrayAsync();
    
    [HttpPost]
    public async Task<bool> CreateAsync(Role role)
    {
        var creationResult = await _roleStore.CreateAsync(role);
        // Добавить логирование в случае Succeeded == false
        return creationResult.Succeeded;
    }

    [HttpPut]
    public async Task<bool> UpdateAsync(Role role)
    {
        var updateResult = await _roleStore.UpdateAsync(role);
        return updateResult.Succeeded;
    }

    [HttpDelete]
    [HttpPost("Delete")]
    public async Task<bool> DeleteAsync(Role role)
    {
        var delete_result = await _roleStore.DeleteAsync(role);
        return delete_result.Succeeded;
    }

    [HttpPost("GetRoleId")]
    public async Task<string> GetRoleIdAsync([FromBody] Role role) => await _roleStore.GetRoleIdAsync(role);

    [HttpPost("GetRoleName")]
    public async Task<string> GetRoleNameAsync([FromBody] Role role) => await _roleStore.GetRoleNameAsync(role);

    [HttpPost("SetRoleName/{name}")]
    public async Task<string> SetRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.Name;
    }

    [HttpPost("GetNormalizedRoleName")]
    public async Task<string> GetNormalizedRoleNameAsync(Role role) => await _roleStore.GetNormalizedRoleNameAsync(role);

    [HttpPost("SetNormalizedRoleName/{name}")]
    public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetNormalizedRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.NormalizedName;
    }

    [HttpGet("FindById/{id}")]
    public async Task<Role> FindByIdAsync(string id) => await _roleStore.FindByIdAsync(id);

    [HttpGet("FindByName/{name}")]
    public async Task<Role> FindByNameAsync(string name) => await _roleStore.FindByNameAsync(name);
}