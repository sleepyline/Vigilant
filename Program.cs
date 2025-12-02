using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VigiLant.Contratos;
using VigiLant.Data;
using VigiLant.Hubs;
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
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();

// Servicos
builder.Services.AddSingleton<IMqttService, MqttService>();
builder.Services.AddSingleton<IHashService, HashService>();

// Registro do Serviço de Background (A ponte entre MQTT e SignalR)
builder.Services.AddHostedService<MqttHostedService>();

// Registro do SignalR
builder.Services.AddSignalR();

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

app.UseEndpoints(endpoints =>
{
    // Mapeamento do SignalR Hub
    endpoints.MapHub<MedicaoHub>("/medicaoHub");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
