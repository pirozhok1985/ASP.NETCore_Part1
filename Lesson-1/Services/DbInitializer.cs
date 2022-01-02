using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services;
public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDB _db;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(WebStoreDB db, ILogger<DbInitializer> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task InitializeAsync(bool removeBeforeInit, CancellationToken token)
    {
        _logger.LogInformation("Инициализация базы данных...");
        if (removeBeforeInit == true)
           await RemoveAsync(token).ConfigureAwait(false);
        var pendingMig = await _db.Database.GetPendingMigrationsAsync(token);
        if (pendingMig.Any())
        {
            _logger.LogInformation("Миграция базы данных...");
            await _db.Database.MigrateAsync(token).ConfigureAwait(false);
            _logger.LogInformation("Миграция базы данных выполнена успешно");
        }

        await InitializeProductAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Инициализация базы данных выполнена успешно");
    }

    public async Task RemoveAsync(CancellationToken token)
    {
        _logger.LogInformation("Удаление базы данных...");
        var delTask = await _db.Database.EnsureDeletedAsync(token).ConfigureAwait(false);

        if (delTask)
        {
            _logger.LogCritical("Выполнено удаление базы!!");
        }
        else
        {
            _logger.LogCritical("Удаление не было выполнено!!");
        }
    }

    private async Task InitializeProductAsync(CancellationToken token)
    {
        if (_db.Sections.Any())
        {
            _logger.LogInformation("Инициализация тестовых данных не требуется");
            return;
        }

        _logger.LogInformation("Инициализация тестовых данных ...");
        _logger.LogInformation("Добавление секций в бд ...");
        await using (await _db.Database.BeginTransactionAsync(token))
        {
            await _db.Sections.AddRangeAsync(TestData.Sections, token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections ON", token);
            await _db.SaveChangesAsync(token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections OFF", token);
        }
        _logger.LogInformation("Добавление брендов в бд ...");
        await using (await _db.Database.BeginTransactionAsync(token))
        {
            await _db.Brands.AddRangeAsync(TestData.Brands, token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections ON", token);
            await _db.SaveChangesAsync(token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections OFF", token);
        }
        _logger.LogInformation("Добавление товаров в бд ...");
        await using (await _db.Database.BeginTransactionAsync(token))
        {
            await _db.Products.AddRangeAsync(TestData.Products, token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections ON", token);
            await _db.SaveChangesAsync(token);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections OFF", token);
        }
        _logger.LogInformation("Инициализация тестовых данных выполнена успешно");
    }
}

