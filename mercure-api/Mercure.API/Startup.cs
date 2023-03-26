using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Middleware;
using Mercure.API.Utils.Logger;
using Mercure.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using LogLevel = Mercure.API.Utils.Logger.LogLevel;

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

                /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);*/
            });
            
            string isRunningInDockerEnv = Environment.GetEnvironmentVariable("RUN_IN_DOCKER");
            Boolean.TryParse(isRunningInDockerEnv, out bool isRunningInDockerEnvBoolean);

            var connectionString = isRunningInDockerEnvBoolean
                ? Configuration.GetConnectionString("MercureDb")
                : Configuration.GetConnectionString("MercureDbNoDocker");

            services.AddDbContext<MercureContext>(opts =>
            {
                opts.UseNpgsql(connectionString);
            });
            
            Logger.LogInfo("Méthode de connexion à la base de données : " + (isRunningInDockerEnvBoolean ? "Docker" : "Non Docker"));

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

            // Config CORS
            app.UseCors(PolicyName);

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Normale qu'il n'y aille pas de await
#pragma warning disable CS4014
            CreateSeed();
#pragma warning restore CS4014
        }

        /// <summary>
        /// Lorsque l'environement est en DEV, il crée un utilisateur de dev avec des données.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="env"></param>
        private async Task CreateSeed()
        {
            using (var context = new MercureContext(_contextOptions))
            {
                Logger.Log(LogLevel.Info, LogTarget.EventLog, "Migration de la base de données si nécessaire");
                context.Database.Migrate();
                
                var hasAlreadyRoles = context.Roles.Any();
                Logger.LogInfo("La table Roles contient déjà des données : " + hasAlreadyRoles);
                
                if (hasAlreadyRoles)
                {
                    Logger.LogInfo("Suppression des données de la table Roles");
                    context.Database.ExecuteSqlRaw("TRUNCATE TABLE Roles CASCADE");
                }
                else
                {
                    Logger.LogInfo("Création des données de la table Roles");
                    var roles = new List<Role>
                    {
                        new Role {RoleName = "Admin", RoleNumber = 100},
                        new Role {RoleName = "User", RoleNumber = 1},
                        new Role {RoleName = "Visitor", RoleNumber = 0},
                    };

                    roles.ForEach((r) => { context.Roles.Add(r); });

                    await context.SaveChangesAsync();
                }
            }
        }

        private static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
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
    }
}