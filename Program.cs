using LocalizationWithJsonResource.Services.Interfaces;
using LocalizationWithJsonResource.Services;
using Microsoft.Extensions.Configuration;
using LocalizationWithJsonResource.Models.DTOs;
using LocalizationWithJsonResource.Helpers;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

var localizationOptions = new LocalizationOptionDTO
{
    SupportedCultures = builder.Configuration.GetSection("LocalizationOptions:SupportedCultures").Get<List<string>>(),
    DefaultCulture = builder.Configuration["LocalizationOptions:DefaultCulture"]
};
builder.Services.AddSingleton(localizationOptions);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
builder.Services.AddSingleton<MemoryCacheHelper>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

//builder.Services.Configure<LocalizationOptionDTO>(builder.Configuration.GetSection("LocalizationOptions"));

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
