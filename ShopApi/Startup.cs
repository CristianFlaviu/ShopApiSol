using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopApi.Config;
using ShopApi.Core.Email;
using ShopApi.Core.RabbitMQ;
using ShopApi.Core.SignalR;
using ShopApi.Database.Data;
using ShopApi.Email;
using ShopApi.Extensions;
using ShopApi.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ShopApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers();

            //services.AddDbContext<DataContext>(optionsBuilder =>
            //    optionsBuilder.UseNpgsql(Configuration.GetConnectionString("LocalDatabaseConnection")));


            services.AddDbContext<DataContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SqlServerConnectionString")));

            services.AddCors();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddHostedService<ConsumerService>();
            services.AddSingleton<MessageHub>();

            services.AddAuthentication();
            services.AddAuthorization();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {   
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetSection("JwtConfig")["Issuer"],
                        ValidAudience = Configuration.GetSection("JwtConfig")["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwtConfig")["key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<EmailSender, EmailSender>();

            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WebAPI", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.Configure<EmailConfig>(Configuration.GetSection("EmailCredentials"));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.AddScoped<ProductRepo>();
            services.AddScoped<CategoryRepo>();
            services.AddScoped<BrandRepo>();
            services.AddScoped<UserRepo>();

            services.AddScoped<ProductUserShoppingCartRepo>();
            services.AddScoped<ProductsUsersFavoriteRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/chathub");
                endpoints.MapControllers();
            });

            using var serviceScope = app.ApplicationServices.CreateScope();

            InitializeDatabase(serviceScope);
        }

        private static void InitializeDatabase(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                SeedData.InitializeAsync(services).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error occurred seeding the DB.");
            }
        }
    }
}
