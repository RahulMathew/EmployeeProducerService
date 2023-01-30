namespace EmployeeProducerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;

            builder.Services.RegisterServices(configuration);

            var app = builder.Build();

            app.AddApplicationConfiguration(configuration);

            app.MapGet("/", () => "Employee Producer Service Starting");

            app.Run();
        }
    }
}


