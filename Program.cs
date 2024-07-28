using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunWebAppGroup.Data;
using RunWebAppGroup.Helpers;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;
using RunWebAppGroup.Repository;
using RunWebAppGroup.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient service
builder.Services.AddHttpClient();


builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));




builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// builder.Services: Se refiere al contenedor de servicios utilizado
// para registrar dependencias en una aplicación ASP.NET Core.

//AddTransient<TService>(): Este método registra el servicio especificado
//(Seed en este caso) con un tiempo de vida transitorio. Esto significa
//que cada vez que se solicita una instancia de este servicio, se crea
//una nueva instancia.

builder.Services.AddTransient<Seed>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//AddIdentity: Método de extensión que agrega los servicios necesarios
//para usar la identidad de ASP.NET Core.
//<AppUser, IdentityRole>: Especifica los tipos de usuario y rol que se
//utilizarán
//AppUser: Representa el tipo de usuario de la aplicación. Esta clase
//suele heredar de IdentityUser y puede contener propiedades adicionales
//específicas para la aplicación.
//IdentityRole: Representa el tipo de rol. IdentityRole es una clase
//proporcionada por ASP.NET Core Identity que se utiliza para gestionar roles
//en la aplicación.
//AddEntityFrameworkStores: Método de extensión que configura la identidad para
//que use Entity Framework Core como el almacén de datos.
//<DataContext>: Especifica el contexto de datos que se utilizará. DataContext
//es una clase que hereda de DbContext de Entity Framework Core y se utiliza
//para interactuar con la base de datos.

//En resumen: Esto permite a la aplicación manejar autenticación y autorización
//utilizando los servicios proporcionados por ASP.NET Core Identity,
//con persistencia de datos a través de Entity Framework Core.

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

//Registro de Servicio: AddMemoryCache() registra los servicios necesarios para
//utilizar una caché en memoria. Esto permite almacenar datos en la memoria
//del servidor durante la ejecución de la aplicación.

//Uso de la Caché: Una vez que AddMemoryCache() se ha llamado, puedes inyectar
//IMemoryCache en tus controladores o servicios a través de la inyección de
//dependencias y utilizarlo para almacenar y recuperar datos de la caché en memoria.

builder.Services.AddMemoryCache();

//registra los servicios necesarios para gestionar sesiones en la aplicación.
//Esto permite almacenar datos específicos del usuario entre solicitudes HTTP.
builder.Services.AddSession();
//AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme): Este método
//agrega servicios de autenticación a la aplicación y establece el esquema de
//autenticación predeterminado.

//CookieAuthenticationDefaults.AuthenticationScheme: Especifica que el esquema de
//autenticación predeterminado será la autenticación basada en cookies. El valor
//de esta constante es "Cookies".

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

// Inyecto servicio en el que se sembrara el contexto de
// datos antes que la aplicación antes de que se inicie la
// aplicación real..

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    Seed.SeedData(app);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
