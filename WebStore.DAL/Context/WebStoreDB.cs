﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;

namespace WebStore.DAL.Context;

public class WebStoreDB : IdentityDbContext<User,Role,string>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Brand> Brands { get; set; }

    public WebStoreDB(DbContextOptions options) : base(options)
    {
            
    }

    protected override void OnModelCreating(ModelBuilder db)
    {
        base.OnModelCreating(db);

        //db.Entity<Section>()
        //    .HasMany(section => section.Products)
        //    .WithOne(product => product.Section)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}