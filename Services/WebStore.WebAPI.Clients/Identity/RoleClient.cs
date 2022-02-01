using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity;

public class RoleClient : BaseClient
{
    public RoleClient(HttpClient client) : base(client, WebApiAddress.Identity.Roles)
    {
    }
}