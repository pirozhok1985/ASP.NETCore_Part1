namespace WebStore.Services.Interfaces;

public interface IDbInitializer
{
    Task RemoveAsync(CancellationToken token);
    Task InitializeAsync(bool removeBeforeInit, CancellationToken token); 
}

