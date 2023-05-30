using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Middleware;
using Mercure.API.Models;
using Mercure.API.Utils;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Prometheus;
using StackExchange.Redis;
using Stripe;
using Product = Mercure.API.Models.Product;
using Role = Mercure.API.Models.Role;

namespace Mercure.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }
        private readonly DbContextOptions<MercureContext> _contextOptions;
        private const string PolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            _contextOptions = new DbContextOptions<MercureContext>();

            Configuration = configuration;
            StaticConfig = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS Policy Config
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, builder =>
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mercure API", Version = "v1"});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            string isRunningInDockerEnv = Environment.GetEnvironmentVariable("RUN_IN_DOCKER");
            Boolean.TryParse(isRunningInDockerEnv, out bool isRunningInDockerEnvBoolean);

            var connectionString = isRunningInDockerEnvBoolean
                ? Configuration.GetConnectionString("MercureDb")
                : Configuration.GetConnectionString("MercureDbNoDocker");

            services.AddDbContext<MercureContext>(opts => { opts.UseNpgsql(connectionString); });

            Logger.LogInfo(LogTarget.Database, "Méthode de connexion à la base de données : " +
                                               (isRunningInDockerEnvBoolean ? "Docker" : "Non Docker"));

            // Connexion à Redis
            services.AddStackExchangeRedisCache(options =>
            {
                var redis = Configuration.GetSection("Redis");
                var redisUrl = isRunningInDockerEnvBoolean
                    ? redis["RedisCacheURlDocker"]
                    : redis["RedisCacheURl"];
                var redisPort = int.Parse(redis["RedisCachePort"]);
                var redisPassword = redis["RedisCachePassword"];

                Logger.LogInfo(LogTarget.Database, "Méthode de connexion à Redis : " +
                                                   (isRunningInDockerEnvBoolean ? "Docker" : "Non Docker"));

                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        {
                            redisUrl, redisPort
                        }
                    },
                    Password = redisPassword,
                    Ssl = false,
                    AbortOnConnectFail = true
                };
                options.Configuration = configurationOptions.ToString();
                options.InstanceName = "Mercure_";
            });

            // Ajout la gestion d'un cache en mémoire
            services.AddMemoryCache();

            // évite la boucle infinie lors de la sérialisation des objets
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* FIX
             *   InvalidCastException: Cannot write DateTime with kind 
             */
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // Stripe API Key
            var stripe = Configuration.GetSection("Stripe");
            var apiKey = stripe["SecretKey"];
            if (apiKey == null)
            {
                Logger.LogError("Stripe API Key is null");
                throw new Exception("Stripe API Key is null");
            }

            StripeConfiguration.ApiKey = apiKey;

            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mercure.API v1");
                    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                    c.InjectJavascript("/swagger-auto-auth/SwaggerAutoAuth.js");
                });
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseHttpMetrics();

            // Config CORS
            app.UseCors(PolicyName);

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<HttpLoggerSystem>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

            // Normale qu'il n'y aille pas de await
#pragma warning disable CS4014
            CreateSeed(env);
#pragma warning restore CS4014
        }

        /// <summary>
        /// Lorsque l'environement est en DEV, il crée un utilisateur de dev avec des données.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="env"></param>
        private async Task CreateSeed(IWebHostEnvironment env)
        {
            using (var context = new MercureContext(_contextOptions))
            {
                Logger.Log(LogLevel.Info, LogTarget.EventLog, "Migration de la base de données si nécessaire");
                await context.Database.MigrateAsync();

                // Concernant les rôles
                var hasAlreadyRoles = context.Roles.Any();
                var countRoles = context.Roles.Count();
                Logger.LogInfo("La table Roles contient déjà des données : " + hasAlreadyRoles + "(" + countRoles +
                               ") rôles");

                var roleEnum = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>().ToList();
                if (!hasAlreadyRoles || countRoles != roleEnum.Count)
                {
                    // Suppression des rôles
                    if (hasAlreadyRoles)
                    {
                        var orders = context.Orders.ToList();
                        if (orders.Count > 0)
                        {
                            Logger.LogInfo("Changement du FK reliant les utilisateurs aux commandes, pour les mettre à null");
                            orders.ForEach(order => order.UserId = null);
                            await context.SaveChangesAsync();
                        }
                        
                        Logger.LogInfo("Suppression des rôles de la table Roles");
                        context.Roles.RemoveRange(context.Roles);
                        await context.SaveChangesAsync();
                    }

                    roleEnum.ToList().ForEach(role =>
                    {
                        Logger.LogInfo("Création du rôle " + role + " avec le numéro " + (int) role);
                        context.Roles.Add(new Role
                        {
                            RoleName = role.ToString(),
                            RoleNumber = (int) role
                        });
                    });
                    await context.SaveChangesAsync();
                } 
 
                // EN MODE DEV => Docker Débug ou lancer depuis l'IDE
                if (env.IsDevelopment())
                {
                    // Utilisateurs de tests
                    var devTestUser = context.Users.Where(u => u.ServiceId.StartsWith("DEV"));
                    if (devTestUser.Any())
                    {
                        Logger.LogInfo("Suppression des utilisateurs de dev de la table Users");
                        devTestUser.ToList().ForEach(u => context.Remove(u));
                        await context.SaveChangesAsync();
                    }

                    // Création des utilisateurs de test pour les rôles
                    var allRoles = context.Roles.ToList();
                    allRoles.ForEach(r =>
                    {
                        Logger.LogInfo("Création des utilisateurs de test pour le rôle " + r.RoleName);
                        User userRoleTest = new User
                        {
                            ServiceId = "DEV:" + r.RoleName + ":RandomDevServiceId",
                            LastName = "LastNameDev",
                            FirstName = "FirstNameDev",
                            Email = "dev@mercure.com",
                            CreatedAt = DateTime.Now,
                            LastUpdatedAt = DateTime.Now
                        };

                        userRoleTest.Role = r;
                        context.Users.Add(userRoleTest);
                    });
                    await context.SaveChangesAsync();

                    // Token Dev User
                    var devUser = context.Users.FirstOrDefault(u =>
                        u.ServiceId.StartsWith("DEV") && u.Role.RoleNumber == (int) RoleEnum.Dev);
                    var tokenDevUser = JwtUtils.GenerateJsonWebToken(devUser);
                    Logger.LogInfo("Token de l'utilisateur de dev : " + tokenDevUser);

                    string isRunningInDockerEnv = Environment.GetEnvironmentVariable("RUN_IN_DOCKER");
                    Boolean.TryParse(isRunningInDockerEnv, out bool isRunningInDockerEnvBoolean);

                    if (!isRunningInDockerEnvBoolean)
                    {
                        var linkSwagger = "http://localhost:5000/swagger/index.html?token=" + tokenDevUser;
                        OpenBrowser(linkSwagger);
                    }

                    // DEV STOCK
                    var devStock = context.Stocks.Where(s => s.StockQuantityAvailable > 999_999_999).ToList();
                    if (devStock.Count() > 0)
                    {
                        Logger.LogInfo("Suppression des stocks de dev de la table Stocks");
                        devStock.ForEach(s => context.Remove(s));
                        await context.SaveChangesAsync();
                    }

                    // Création des stocks de dev
                    Logger.LogInfo("Création des stocks de dev...");
                    var devStocks = new List<Stock>()
                    {
                        new Stock(1_999_999_999),
                        new Stock(1_999_999_999),
                        new Stock(1_999_999_999),
                    };

                    devStocks.ForEach(s => context.Stocks.Add(s));
                    await context.SaveChangesAsync();

                    // DEV CATEGORIES
                    var devCategories = context.Categories.Where(c => c.CategoryTitle.StartsWith("DEV:")).ToList();
                    if (devCategories.Count() > 0)
                    {
                        Logger.LogInfo("Suppression des catégories de dev de la table Categories");
                        devCategories.ForEach(c => context.Remove(c));
                        await context.SaveChangesAsync();
                    }

                    // Création des catégories de dev
                    Logger.LogInfo("Création des catégories de dev...");
                    var devCategoriesList = new List<Category>()
                    {
                        new Category("Chien", "pas de description: Chien"),
                        new Category("Chat", "pas de description: Chat"),
                        new Category("Rongeur", "pas de description: Rongeur"),
                        new Category("Oiseau", "pas de description: Oiseau"),
                        new Category("Poisson", "pas de description: Poisson"),
                        new Category("Reptile", "pas de description: Reptile"),
                        new Category("Autre", "pas de description: Autre"),
                    };

                    devCategoriesList.ForEach(c => context.Categories.Add(c));
                    await context.SaveChangesAsync();


                    // Si la base de données contient des produits commencant par DEV:, alors on supprime les données de la base de données et on les recréeq
                    var devProducts = context.Products.ToList();
                    if (devProducts.Any())
                    {
                        Logger.LogInfo("Suppression des produits commencant par 'DEV:' de la table Products");
                        devProducts.ForEach(p => context.Remove(p));
                        await context.SaveChangesAsync();
                    }

                    // Création des produits de dev
                    Logger.LogInfo("Création des produits de dev...");
                    var products = new List<Product>();
                    for (var i = 0; i < 100; i++)
                    {
                        products.Add(GenerateProduct(context));
                    }

                    products.ForEach(p => context.Add(p));
                    await context.SaveChangesAsync();

                    var allDevproducts = context.Products.Where(p => p.ProductName.StartsWith("DEV:")).ToList();
                    allDevproducts.ForEach(async p =>
                    {
                        p.Categories = new List<Category>();
                        p.Categories = await RandomCategories(context);
                    });
                    await context.SaveChangesAsync();


                    // DEV Animals
                    var devAnimals = context.Animals.ToList();
                    if (devAnimals.Count() > 0)
                    {
                        Logger.LogInfo("Suppression des animaux de dev de la table Animals");
                        devAnimals.ForEach(a => context.Remove(a));
                        await context.SaveChangesAsync();
                    }
                    List<Animal> animals = new List<Animal> {
                        new Animal("Pan-di", DateTime.Parse("2011-01-28T17:18:39.840229+02:00"), "White and grey", 1000000, DateTime.Now, DateTime.Now),
                        new Animal("Noisette", DateTime.Parse("2011-05-12T17:18:39.840229+02:00"), "Red", 1000000, DateTime.Now, DateTime.Now),
                        new Animal("Charlie", DateTime.Parse("2012-07-08T17:18:39.840229+02:00"), "Black", 1000000, DateTime.Now, DateTime.Now),
                        new Animal("Ernesto", DateTime.Parse("2001-01-28T17:18:39.840229+02:00"), "Blue", 1000000, DateTime.Now, DateTime.Now),
                        new Animal("Ulysse", DateTime.Parse("2010-02-14T17:18:39.840229+02:00"), "Yellow and purple", 1000000, DateTime.Now, DateTime.Now)
                    };
                    
                    await context.Animals.AddRangeAsync(animals);
                    await context.SaveChangesAsync();

                    // DEV Species
                    var devSpecies = context.Speciess.ToList();
                    if (devSpecies.Any())
                    {
                        Logger.LogInfo("Suppression des espèces de dev de la table Species");
                        devSpecies.ForEach(s => context.Remove(s));
                        await context.SaveChangesAsync();
                    }
                    
                    List<Species> species = new List<Species>
                    {
                        new Species("Labrador"),
                        new Species("Terrier du tibet"),
                        new Species("Berger allemand"),
                        new Species("Bouledogue français"),
                        new Species("Cocker"),
                        new Species("Cavalier King Charles")
                    };
                    await context.Speciess.AddRangeAsync(species);
                    await context.SaveChangesAsync();

                    Logger.LogInfo("Début de la création des liaisons entre les animaux et races");
                    
                    var animalsList = context.Animals.ToList();
                    animalsList.ForEach((a) =>
                    {
                        var animalSpecies = new List<AnimalSpecies>();
                        var randomSpecies = new Random().Next(1, 3);

                        for (int i = 0; i < randomSpecies; i++)
                        {
                            animalSpecies.Add(new AnimalSpecies(a.AnimalId, RandomSpecies(context)));
                        }

                        Logger.LogInfo(animalSpecies.Count.ToString() + " espèces pour l'animal " + a.AnimalName + " ajoutées");
                        
                        a.AnimalSpecies = new List<AnimalSpecies>();
                        a.AnimalSpecies = animalSpecies;
                    });
                    await context.SaveChangesAsync();

                    Logger.LogInfo("Fin du remplissage de la base de données");
                }
            }
        }


        private static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) {UseShellExecute = true});
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                Logger.LogInfo("Le navigateur par défaut n'a pas pu être ouvert");
            }
        }

        private string RandomProductName()
        {
            // generate list of random product for animal shopping
            var productNames = new List<string>()
            {
                "Croquettes",
                "Boites",
                "Friandises",
                "Jouets",
                "Litières",
                "Couchages",
                "Hygiène",
                "Antiparasitaires",
                "Compléments alimentaires",
                "Matériel de toilettage",
                "Matériel d'éducation",
                "Matériel de transport",
                "Matériel d'agility",
                "Matériel de dressage",
                "Matériel de sécurité",
                "Brosse",
                "Shampoing",
                "Collier",
                "Laisse",
                "Harnais",
                "Gamelle",
                "Cage",
                "Aquarium",
                "Terrarium",
                "Nourriture"
            };

            return "DEV:" + productNames[new Random().Next(0, productNames.Count)];
        }

        private string RandomBrandNames()
        {
            var brandNames = new List<string>()
            {
                "Advantix / Advantage",
                "Affinity",
                "Almo Nature",
                "Animonda",
                "bosch",
                "Briantos",
                "Carnilove",
                "Catsan",
                "Concept for Life",
                "Concept for Life Veterinary Diet",
                "Cosma",
                "Crave",
                "Curver",
                "DogSnagger",
                "Eukanuba",
                "FELIWAY",
                "Felix",
                "Feringa",
                "Ferplast",
                "FRONTLINE",
                "Gourmet",
                "Happy Dog / Happy Cat",
                "Hill's",
                "Hill's Prescription Diet",
                "Josera",
                "KONG",
                "Lukullus",
                "My Star",
                "Nature's Variety",
                "Nomad Tales",
                "Nutrivet",
                "Pedigree",
                "PetSafe",
                "Purina",
                "PURINA ONE",
                "PURINA PRO PLAN",
                "PURINA PRO PLAN pour chien",
                "PURINA PRO PLAN Veterinary Diets",
                "Purizon",
                "Rocco",
                "Royal Canin",
                "Royal Canin Veterinary",
                "Sanabelle",
                "Seresto",
                "Sheba",
                "Smilla",
                "Taste of the Wild",
                "Tiaki",
                "Tigerino",
                "Trixie",
                "Versele Laga",
                "Virbac",
                "Vitakraft",
                "Whiskas",
                "Wild Freedom",
                "Wolf of Wilderness",
                "Yarrah",
                "zoolove by zooplus"
            };

            return brandNames[new Random().Next(0, brandNames.Count)];
        }

        private async Task<List<Category>> RandomCategories(MercureContext context)
        {
            Random rand = new Random();
            int skipper = rand.Next(0, context.Categories.Count());

            return context.Categories
                .Skip(skipper)
                .Take(3)
                .ToList();
        }


        private static Species RandomSpecies(MercureContext context)
        {
            Random rand = new Random();
            int skipper = rand.Next(0, context.Speciess.Count());

            return context.Speciess
                .Skip(skipper)
                .Take(1)
                .FirstOrDefault();
        }

        private static Stock RandomStocks(MercureContext context)
        {
            var dbStocks = context.Stocks.ToList();
            return dbStocks[new Random().Next(0, dbStocks.Count)];
        }

        private static int ProductPrice()
        {
            return new Random().Next(1, 100);
        }

        private Product GenerateProduct(MercureContext context)
        {
            return new Product()
            {
                ProductName = RandomProductName() + " - " + RandomProductName(),
                ProductBrandName = RandomBrandNames(),
                ProductPrice = ProductPrice(),
                ProductDescription = "Description de test",
                ProductInfo = "",
                ProductType = "Product Type",
                ProductCreationDate = DateTime.Now,
                ProductLastUpdate = DateTime.Now,
                Stock = RandomStocks(context)
            };
        }
    }
}