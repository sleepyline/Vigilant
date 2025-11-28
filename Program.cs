using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VigiLant.Contratos;
using VigiLant.Data;
using VigiLant.Repositories;
using VigiLant.Repository;
using VigiLant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultDatabase");
builder.Services.AddDbContext<BancoCtx>(opt =>
{
    opt.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
});

// Repositorios e contratos
builder.Services.AddScoped<IRiscoRepository, RiscoRepository>();
builder.Services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IMqttService, MqttService>();

//Services
builder.Services.AddSingleton<IHashService, HashService>();

//Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Conta/Login"; // Redireciona usuários não autenticados
        options.AccessDeniedPath = "/Conta/AcessoNegado"; // Redireciona usuários sem permissão (Role)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

// The standard way to map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
