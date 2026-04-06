using Application;
using Microsoft.AspNetCore.HttpOverrides;
using Persistence;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistence(builder.Configuration)
    .AddApplication()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddConfiguredControllers()
    .AddConfiguredAutoMapper();

// Доверяем только Nginx???
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseCustomExceptionHandler();
app.UseStaticAssets(builder.Environment);
app.UseRouting();

app.UseStatusCodePages();
app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    // app.UseExceptionHandler("/Error");
}

app.ApplyMigrations();


app.Map("api/", () =>  Results.Ok("Живой"));

app.MapControllers();

app.Run();