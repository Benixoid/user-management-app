
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagementBack.Config;
using UserManagementBack.Data;
using UserManagementBack.Helpers;
using UserManagementBack.Services;

namespace UserManagementBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build())
               .CreateLogger();
            Log.Information("Starting up the UserTest backend API...");
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var corsOrigins = "UserAppTest";

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: corsOrigins,
                                      policy => {
                                          policy.WithOrigins("https://localhost:5073",
                                                              "http://localhost:7188",
                                                              "https://localhost:4200",
                                                              "http://localhost:4200"
                                                              )
                                          .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                                      });
                });

                builder.Services.AddApiVersioning(options => {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Api-Version"),
                        new MediaTypeApiVersionReader("v")
                    );
                }).AddApiExplorer(options => {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

                // Add services to the container.
                builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());
                builder.Services.AddSingleton(Log.Logger);
                builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IDataManager, DataManager>();
                builder.Services.AddControllers();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                //DB settings    
                var password = builder.Configuration.GetConnectionString("EncryptedDBPassword");
                if (string.IsNullOrEmpty(password))
                {
                    Log.Fatal("Encrypted DB password is not set in the configuration file");
                    return;
                }
                var decryptedPassword = MyEncryption.DecryptString(password);

                builder.Services.AddDbContext<AppDbContext>(opts =>
                    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") + ";Password=" + decryptedPassword));

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseCors(corsOrigins);

                app.UseAuthorization();

                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

                app.MapControllers();

                app.Run();
            }
            catch (HostAbortedException)
            {
                Log.Information("Application host was aborted, shutting down gracefully.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
    }
}
