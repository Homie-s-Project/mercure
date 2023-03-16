using System.IO;
using Mercure.API.Models;
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
            
            options.UseNpgsql(configuration.GetConnectionString("MercureDb"));
        }
    }

    public MercureContext(DbContextOptions<MercureContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OAuth2Credentials> OAuth2Credentials { get; set; }
}