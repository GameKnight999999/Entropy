using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Для dev-сцены создаем сервер и явно указываем порт 5184
builder.WebHost.UseUrls("http://*:5184");

var app = builder.Build();

// Включаем Razor Pages - корень '/' автоматически обрабатываем через MapRazorPages()
app.MapRazorPages();

await app.RunAsync();