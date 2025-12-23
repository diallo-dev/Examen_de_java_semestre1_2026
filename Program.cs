using Microsoft.EntityFrameworkCore;
using Npgsql;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;
using BrasilBurger.Web.Repository.Impl;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.Service.Impl;
using Microsoft.AspNetCore.DataProtection;

// Permet de gérer les dates PostgreSQL correctement
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Configuration du port pour Render
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(10000);
});

// --- CONFIGURATION DE LA BASE DE DONNÉES (NEON) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<EtatStockEnum>("etatstock"); 
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource));

// --- CONFIGURATION DE LA SESSION ---
builder.Services.AddDistributedMemoryCache(); // Requis pour la session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Important pour RGPD et Render
    options.Cookie.Name = ".BrasilBurger.Session";
});

// --- PROTECTION DES DONNÉES (Pour Render) ---
builder.Services.AddDataProtection()
    .SetApplicationName("BrasilBurger")
    .PersistKeysToFileSystem(new DirectoryInfo(@"/tmp/keys"));

// --- INJECTION DES RÉPERTOIRES (REPOSITORIES) ---
builder.Services.AddScoped<IBurgerRepository, BurgerRepository>();
builder.Services.AddScoped<IComplementRepository, ComplementRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICommandeRepository, CommandeRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();

// --- INJECTION DES SERVICES ---
builder.Services.AddScoped<IBurgerService, BurgerService>();
builder.Services.AddScoped<IComplementService, ComplementService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// --- PIPELINE DE TRAITEMENT (ORDRE CRUCIAL) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Assure-toi que cette route existe
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

// *** LA SESSION DOIT ÊTRE ICI (Entre Routing et Authorization) ***
app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogue}/{action=Index}/{id?}");

app.Run();