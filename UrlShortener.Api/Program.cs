using UrlShortener.Api;
using UrlShortener.Application.DependencyInjection;
using UrlShortener.Application.Settings;
using UrlShortener.DAL.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BusinessRules>(builder.Configuration.GetSection(nameof(BusinessRules)));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddLocalization(options => options.ResourcesPath = nameof(UrlShortener.Application.Resources));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

builder.Host.AddLogging();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerUI();
app.UseSwagger();

app.UseRouting();
app.MapControllers();
app.UseLocalization();

await builder.Services.MigrateDatabaseAsync();

app.LogListeningUrls();

await app.RunAsync();