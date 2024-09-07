
using System.Reflection;

namespace MagPie_Home_Control_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add environment variables
            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new()
                    {
                        Title = "MagPie Home Control API",
                        Version = "v1"
                    });

                c.IncludeXmlComments(Assembly.GetExecutingAssembly());

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return [api.GroupName];
                    }

                    return ["Default"];
                });

                c.DocInclusionPredicate((name, api) => true);
            });

            // Add HTTP client
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
