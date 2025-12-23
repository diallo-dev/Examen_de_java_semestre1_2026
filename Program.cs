using Microsoft.EntityFrameworkCore;
using Npgsql;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;
using BrasilBurger.Web.Repository.Impl;
using BrasilBurger.Web.Service;
using BrasilBurger.Web.Service.Impl;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(10000);
});



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<EtatStockEnum>("etatstock"); 
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/tmp/keys")); // Dossier temporaire sur Render


builder.Services.AddScoped<IBurgerRepository, BurgerRepository>();
builder.Services.AddScoped<IComplementRepository, ComplementRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICommandeRepository, CommandeRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();

builder.Services.AddScoped<IBurgerService, BurgerService>();
builder.Services.AddScoped<IComplementService, ComplementService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogue}/{action=Index}/{id?}");

app.Run();