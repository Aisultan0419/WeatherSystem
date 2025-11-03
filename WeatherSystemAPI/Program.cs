using Microsoft.OpenApi.Models;
using WeatherApplication;
using Microsoft.AspNetCore.SignalR;
using WeatherApplication.Display;
using WeatherApplication.DTO;
using WeatherApplication.Factories;
using WeatherApplication.Interfaces;
using WeatherApplication.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
});
builder.Services.AddOpenApi();
builder.Services.AddScoped<IWeatherFactory, WeatherFactory>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<RealTimeStrategy>();
builder.Services.AddSingleton<WeatherCache>();
builder.Services.AddSignalR();
builder.Services.AddTransient<BatchStrategy>();
builder.Services.AddTransient<ManualStrategy>();

builder.Services.AddSingleton<WeatherStation>();
builder.Services.AddSingleton<IObserverFactory, ObserverFactory>();
builder.Services.AddScoped<WeatherFacade>();

builder.Services.Configure<OpenWeatherOptions>(
    builder.Configuration.GetSection("OpenWeather"));
builder.Services.AddOptions<OpenWeatherOptions>()
    .Bind(builder.Configuration.GetSection("OpenWeather"))
    .ValidateDataAnnotations()
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "Missing API key for OpenWeather");

builder.Services.AddScoped<WeatherService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() 
              .WithOrigins("http://localhost:3000");
    });
});

builder.Services.AddTransient<WebSocketObserver>(sp =>
    new WebSocketObserver(
        sp.GetRequiredService<IHubContext<WeatherHub>>(),
        "connectionIdValue",
        "cityValue"));

builder.Services.AddTransient<ConsoleObserver>(sp =>
       new ConsoleObserver("nameValue", "cityValue"));

builder.Services.AddTransient<FileObserver>(sp =>
    new FileObserver("cityValue"));

var app = builder.Build();


app.UseRouting();

app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API");
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<WeatherHub>("/weatherHub");

app.Run();
