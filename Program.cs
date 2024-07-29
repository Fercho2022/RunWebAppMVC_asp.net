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
// para registrar dependencias en una aplicaci�n ASP.NET Core.

//AddTransient<TService>(): Este m�todo registra el servicio especificado
//(Seed en este caso) con un tiempo de vida transitorio. Esto significa
//que cada vez que se solicita una instancia de este servicio, se crea
//una nueva instancia.


builder.Services.AddTransient<Seed>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//AddIdentity: M�todo de extensi�n que agrega los servicios necesarios
//para usar la identidad de ASP.NET Core.
//<AppUser, IdentityRole>: Especifica los tipos de usuario y rol que se
//utilizar�n
//AppUser: Representa el tipo de usuario de la aplicaci�n. Esta clase
//suele heredar de IdentityUser y puede contener propiedades adicionales
//espec�ficas para la aplicaci�n.
//IdentityRole: Representa el tipo de rol. IdentityRole es una clase
//proporcionada por ASP.NET Core Identity que se utiliza para gestionar roles
//en la aplicaci�n.
//AddEntityFrameworkStores: M�todo de extensi�n que configura la identidad para
//que use Entity Framework Core como el almac�n de datos.
//<DataContext>: Especifica el contexto de datos que se utilizar�. DataContext
//es una clase que hereda de DbContext de Entity Framework Core y se utiliza
//para interactuar con la base de datos.

//En resumen: Esto permite a la aplicaci�n manejar autenticaci�n y autorizaci�n
//utilizando los servicios proporcionados por ASP.NET Core Identity,
//con persistencia de datos a trav�s de Entity Framework Core.

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

//Registro de Servicio: AddMemoryCache() registra los servicios necesarios para
//utilizar una cach� en memoria. Esto permite almacenar datos en la memoria
//del servidor durante la ejecuci�n de la aplicaci�n.

//Uso de la Cach�: Una vez que AddMemoryCache() se ha llamado, puedes inyectar
//IMemoryCache en tus controladores o servicios a trav�s de la inyecci�n de
//dependencias y utilizarlo para almacenar y recuperar datos de la cach� en memoria.

builder.Services.AddMemoryCache();

//registra los servicios necesarios para gestionar sesiones en la aplicaci�n.
//Esto permite almacenar datos espec�ficos del usuario entre solicitudes HTTP.
builder.Services.AddSession();
//AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme): Este m�todo
//agrega servicios de autenticaci�n a la aplicaci�n y establece el esquema de
//autenticaci�n predeterminado.

//CookieAuthenticationDefaults.AuthenticationScheme: Especifica que el esquema de
//autenticaci�n predeterminado ser� la autenticaci�n basada en cookies. El valor
//de esta constante es "Cookies".

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

// Inyecto servicio en el que se sembrara el contexto de
// datos antes que la aplicaci�n antes de que se inicie la
// aplicaci�n real..

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
