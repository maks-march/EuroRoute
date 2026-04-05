using Microsoft.AspNetCore.HttpOverrides;
using Persistence;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    builder.Services.AddPersistenceSqlite();
}
if (builder.Environment.IsProduction()) {
    builder.Services.AddPersistence(builder.Configuration);
}

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Доверяем только Nginx???
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();

app.UseStaticAssets(builder.Environment);

app.Map("api/", () =>  Results.Ok("Живой"));
app.Run();