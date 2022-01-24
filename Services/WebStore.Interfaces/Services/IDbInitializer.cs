namespace WebStore.Interfaces.Services;

public interface IDbInitializer
{
    Task RemoveAsync(CancellationToken token);
    Task InitializeAsync(bool removeBeforeInit, CancellationToken token); 
}

