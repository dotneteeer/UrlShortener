using UrlShortener.Api;
using UrlShortener.Api.Middlewares;
using UrlShortener.Application.DependencyInjection;
using UrlShortener.Application.Settings;
using UrlShortener.DAL.DependencyInjection;
using UrlShortener.GraphQl.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BusinessRules>(builder.Configuration.GetSection(nameof(BusinessRules)));
builder.Services.Configure<PaginationRules>(builder.Configuration.GetSection(nameof(PaginationRules)));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddLocalization(options => options.ResourcesPath = nameof(UrlShortener.Application.Resources));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddGraphQl();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

builder.Host.AddLogging();
builder.Services.AddCors(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseStatusCodePages();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<WarningHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerUI();
app.UseSwagger();

app.UseRouting();
app.MapControllers();
app.UseLocalization();
app.UseGraphQl();
app.UseCors("DefaultCorsPolicy");

await builder.Services.MigrateDatabaseAsync();

app.LogListeningUrls();

await app.RunAsync();