using Application;
using Microsoft.AspNetCore.HttpOverrides;
using Persistance;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistence(builder.Configuration)
    .AddApplication()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer();

builder.Services.AddControllers();

// Доверяем только Nginx???
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseRouting();
app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();

app.UseStaticAssets(builder.Environment);

app.Map("api/", () =>  Results.Ok("Живой"));

app.MapControllers();

app.Run();