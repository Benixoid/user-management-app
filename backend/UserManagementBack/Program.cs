
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagementBack.Data;
using UserManagementBack.Helpers;

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

                // Add services to the container.

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

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
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
