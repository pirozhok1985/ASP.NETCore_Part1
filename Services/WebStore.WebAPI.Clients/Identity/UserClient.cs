using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity;

public class UserClient : BaseClient
{
    public UserClient(HttpClient client) : base(client, WebApiAddress.Identity.Users)
    {
    }
}