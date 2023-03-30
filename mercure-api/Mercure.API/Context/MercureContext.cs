using System;
using System.IO;
using Mercure.API.Models;
using Mercure.API.Utils.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mercure.API.Context;

public class MercureContext : DbContext
{
    protected MercureContext()
    {
    }

    // Lors de la configuration du contexte, cette méthode est appelée pour définir les options de base de données pour ce contexte.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            string isRunningInDockerEnv = Environment.GetEnvironmentVariable("RUN_IN_DOCKER");
            Boolean.TryParse(isRunningInDockerEnv, out bool isRunningInDockerEnvBoolean);

            var connectionString = isRunningInDockerEnvBoolean
                ? configuration.GetConnectionString("MercureDb")
                : configuration.GetConnectionString("MercureDbNoDocker");
            
            Logger.LogInfo("Configuration de connexion à la base de données : " + (isRunningInDockerEnvBoolean ? "Docker" : "Non Docker"));
            options.UseNpgsql(connectionString);
        }
    }

    public MercureContext(DbContextOptions<MercureContext> options) : base(options)
    {
    }

    public DbSet<Animal> Animals { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OAuth2Credentials> OAuth2Credentials { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Species> Speciess { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<User> Users { get; set; }
}