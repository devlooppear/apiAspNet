using apiAspNet.Data;
using apiAspNet.Middleware;
using apiAspNet.Data.Seeders;
using apiAspNet.Services;
using apiAspNet.Repositories;
using Microsoft.EntityFrameworkCore;

// Load environment variables from .env file
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices method
builder.Host.ConfigureServices((context, services) =>
{
    services.AddControllers();

    // Configure the database context with separate environment variables
    services.AddDbContext<ApplicationDbContext>(options =>
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}";
        options.UseNpgsql(connectionString);
    });

    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRepository, UserRepository>();

    // Add Swagger services
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
});

// Create application
var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Call the SeedUsers method of the UserSeeder class
    UserSeeder.SeedUsers(dbContext);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Enable HTTPS Redirection middleware
app.UseHttpsRedirection(); 

app.UseRouting();

// Add the custom authentication middleware to the pipeline
app.UseMiddleware<TokenAuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllers(); 

// Sample weatherforecast endpoint
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
