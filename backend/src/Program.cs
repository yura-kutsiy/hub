using Services;
using Nodes;
using Config;
using Pods;
using kuberApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure settings
builder.Services.Configure<AppSettings>(builder.Configuration);

// Add services to the container.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Clear the default logging providers
    loggingBuilder.AddConsole(); // Add console logger
    loggingBuilder.AddDebug(); // Add debug logger
});

// Configure CORS
builder.Services.AddCors(options =>
{
    var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
    options.AddPolicy("AllowConfiguredOrigins",
        builder =>
        {
            builder.WithOrigins(corsSettings?.AllowedOrigins ?? Array.Empty<string>())
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddSingleton<KubernetesServices>();
builder.Services.AddSingleton<KubernetesNodes>();

// Configure Kestrel
builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

// Enable CORS middleware
app.UseCors("AllowConfiguredOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
