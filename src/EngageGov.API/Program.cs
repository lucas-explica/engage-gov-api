using EngageGov.API.Middleware;
using EngageGov.Application;
using EngageGov.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel ports depending on environment to avoid HTTP/HTTPS confusion in development.
var env = builder.Environment.EnvironmentName;
if (string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
{
    // Use conventional dev ports: HTTP 5000, HTTPS 5001 (dev cert via dotnet dev-certs)
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5000); // HTTP
        options.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps()); // HTTPS
    });
}
else
{
    // Production/default: explicit localhost HTTP 5001 and HTTPS 5002 (if needed)
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5001);
        options.ListenLocalhost(5002, listenOptions => { listenOptions.UseHttps(); });
    });
}

// Add a minimal diagnostic hosted service to log lifecycle events (helps identify immediate shutdown causes)
builder.Services.AddHostedService<EngageGov.API.Services.StartupDiagnosticsService>();

// Add services to the container
// Configure Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Named HttpClient for external government data (Câmara Dados Abertos)
builder.Services.AddHttpClient("camara", client =>
{
    client.BaseAddress = new Uri("https://dadosabertos.camara.leg.br");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register ExternalGovService as a typed client using the named 'camara' HttpClient
builder.Services.AddHttpClient<EngageGov.Application.Services.ExternalGovService>(client =>
{
    client.BaseAddress = new Uri("https://dadosabertos.camara.leg.br");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<EngageGov.Application.Interfaces.IExternalGovService>(sp => sp.GetRequiredService<EngageGov.Application.Services.ExternalGovService>());

// Add controllers
builder.Services.AddControllers();

// Add API versioning
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI with security definitions
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EngageGov API",
        Version = "v1",
        Description = "Civic Engagement Platform API - A clean architecture REST API for citizen participation in governance",
        Contact = new OpenApiContact
        {
            Name = "EngageGov Team",
            Email = "contact@engagegov.com"
        }
    });

    // Include XML comments for better API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Add JWT Bearer authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Add CORS policy for security
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000" })
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add health checks
builder.Services.AddHealthChecks();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
// Use custom exception handling middleware
app.UseExceptionHandling();

// Enable HTTPS redirection for security
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Configure Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EngageGov API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// Authentication & Authorization (configured for future JWT implementation)
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map health checks
app.MapHealthChecks("/health");

app.Run();

// Make Program class accessible to integration tests
public partial class Program { }
