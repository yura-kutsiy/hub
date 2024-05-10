using Services;
using Nodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Clear the default logging providers
    loggingBuilder.AddConsole(); // Add console logger
    loggingBuilder.AddDebug(); // Add debug logger
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KubernetesServices>();
builder.Services.AddSingleton<KubernetesNodes>();
builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

// Enable CORS middleware
app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
