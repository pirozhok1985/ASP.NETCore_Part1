using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services;
public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDB _db;
    private readonly ILogger<DbInitializer> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public DbInitializer(WebStoreDB db, ILogger<DbInitializer> logger, RoleManager<Role> roleManager, UserManager<User>userManager)
    {
        _db = db;
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task InitializeAsync(bool removeBeforeInit, CancellationToken cancel)
    {
        _logger.LogInformation("Инициализация базы данных...");
        if (removeBeforeInit == true)
           await RemoveAsync(cancel).ConfigureAwait(false);
        var pendingMig = await _db.Database.GetPendingMigrationsAsync(cancel);
        if (pendingMig.Any())
        {
            _logger.LogInformation("Миграция базы данных...");
            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Миграция базы данных выполнена успешно");
        }

        await InitializeProductAsync(cancel).ConfigureAwait(false);
        await InitializeEmployeeAsync(cancel).ConfigureAwait(false);
        await InitializeIdentityAsync(cancel).ConfigureAwait(false);
        _logger.LogInformation("Инициализация базы данных выполнена успешно");
    }

    public async Task RemoveAsync(CancellationToken cancel)
    {
        _logger.LogInformation("Удаление базы данных...");
        var delTask = await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

        if (delTask)
        {
            _logger.LogCritical("Выполнено удаление базы!!");
        }
        else
        {
            _logger.LogCritical("Удаление не было выполнено!!");
        }
    }

    private async Task InitializeProductAsync(CancellationToken cancel)
    {
        if (_db.Sections.Any())
        {
            _logger.LogInformation("Инициализация тестовых данных не требуется");
            return;
        }

        _logger.LogInformation("Инициализация тестовых данных ...");
        _logger.LogInformation("Добавление секций в бд ...");
        await using (await _db.Database.BeginTransactionAsync(cancel))
        {
            await _db.Sections.AddRangeAsync(TestData.Sections, cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections ON", cancel);
            await _db.SaveChangesAsync(cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Sections OFF", cancel);
            await _db.Database.CommitTransactionAsync(cancel);
        }
        _logger.LogInformation("Добавление брендов в бд ...");
        await using (await _db.Database.BeginTransactionAsync(cancel))
        {
            await _db.Brands.AddRangeAsync(TestData.Brands, cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Brands ON", cancel);
            await _db.SaveChangesAsync(cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Brands OFF", cancel);
            await _db.Database.CommitTransactionAsync(cancel);
        }
        _logger.LogInformation("Добавление товаров в бд ...");
        await using (await _db.Database.BeginTransactionAsync(cancel))
        {
            await _db.Products.AddRangeAsync(TestData.Products, cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Products ON", cancel);
            await _db.SaveChangesAsync(cancel);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Products OFF", cancel);
            await _db.Database.CommitTransactionAsync(cancel);
        }
        _logger.LogInformation("Инициализация тестовых данных выполнена успешно");
    }

    private async Task InitializeEmployeeAsync(CancellationToken cancel)
    {
        if (await _db.Employees.AnyAsync(cancel))
        {
            _logger.LogInformation("Инициализация сотрудников не требуется.");
            return;
        }

        await using (await _db.Database.BeginTransactionAsync(cancel))
        { 
          _logger.LogInformation("Инициализация сотрудников...");
          await _db.Employees.AddRangeAsync(TestData.Employees, cancel);
          await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Employees ON", cancel);
          await _db.SaveChangesAsync(cancel);
          await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Employees OFF", cancel);
          await _db.Database.CommitTransactionAsync(cancel);
          _logger.LogInformation("Инициализация сотрудников завершена.");
        }
    }

    private async Task CheckRole(string role)
    {
        if (await _roleManager.RoleExistsAsync(role))
        {
            _logger.LogInformation("Роль {0} уже существует", role);
            return;
        }

        await _roleManager.CreateAsync(new Role() {Name = role});
        _logger.LogInformation("Роль {0} успешно создана", role);
    }

    private async Task InitializeIdentityAsync(CancellationToken cancel)
    {
        await CheckRole(Role.Administrators);
        await CheckRole(Role.Users);

        if (await _userManager.FindByNameAsync(User.Administrator) is null)
        {
            _logger.LogInformation("Пользователь {0} не найден, создаю...", User.Administrator);
            var user = new User() {UserName = User.Administrator};
            var creationResult = await _userManager.CreateAsync(user,User.DefaultAdminPassword);
            if (creationResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.Administrators);
                _logger.LogInformation("Пользователь {0} успешно создан", User.Administrator);
            }
            else
            {
                var errors = creationResult.Errors.Select(e => e.Description);
                _logger.LogError("Пользователь {0} не был создан по причине ошибок: {1}",User.Administrator,string.Join(", ", errors));
                throw new InvalidOperationException(string.Join(", ", errors));
            }
        }
        
    }
}

