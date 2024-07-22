using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using RunWebAppGroup.Data.Enum;
using RunWebAppGroup.Models;
using static Azure.Core.HttpHeader;

namespace RunWebAppGroup.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();

                context.Database.EnsureCreated();

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Running Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                        new Club()
                        {
                            Title = "Running Club 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.Endurance,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Running Club 3",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.Trail,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Running Club 3",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Michigan",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
                //Races
                if (!context.Races.Any())
                {
                    context.Races.AddRange(new List<Race>()
                    {
                        new Race()
                        {
                            Title = "Running Race 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            RaceCategory = RaceCategory.Marathon,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Race()
                        {
                            Title = "Running Race 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            RaceCategory = RaceCategory.Ultra,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
        //Define un método estático y asíncrono que devuelve una tarea (Task).
        //Esto significa que el método puede ejecutarse de manera asíncrona y no
        //bloquea el hilo de ejecución principal.
        //SeedUsersAndRolesAsync: El nombre del método que se encarga de sembrar
        //(crear) usuarios y roles.
        //IApplicationBuilder applicationBuilder: Un parámetro que se usa para
        //configurar la aplicación. Este parámetro proporciona servicios de la
        //aplicación.
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            //using: Garantiza que los recursos se liberen una vez que se complete
            //el bloque de código.
            //var serviceScope = applicationBuilder.ApplicationServices.CreateScope(): Crea
            //un alcance de servicios (serviceScope) que se utiliza para obtener
            //servicios de dependencia necesarios para la operación.
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                //var roleManager: Declara una variable para el administrador de roles.
                //serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>:
                //Obtiene una instancia del administrador de roles (RoleManager<IdentityRole>)
                //del proveedor de servicios.
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                
                //Comprueba si el rol de administrador (Admin) no existe de forma asíncrona.
               
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))

                    //Si el rol no existe, lo crea.
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                //Comprueba si el rol de usuario (User) no existe de forma asíncrona.
                if (!await roleManager.RoleExistsAsync(UserRoles.User))

                    // Si el rol no existe, lo crea.
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                //var userManager: Declara una variable para el administrador de usuarios.
                //serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>:
                //Obtiene una instancia del administrador de usuarios (UserManager<AppUser>)
                //del proveedor de servicios.
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                //string adminUserEmail = "teddysmithdeveloper@gmail.com": Declara
                //y asigna el correo electrónico del usuario administrador.
                string adminUserEmail = "teddysmithdeveloper@gmail.com";

                //Busca un usuario administrador por correo electrónico de forma asíncrona.
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    //Crea una nueva instancia de AppUser con los detalles del
                    //usuario administrador.
                    var newAdminUser = new AppUser()
                    {
                        UserName = "teddysmithdev",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    //Crea el nuevo usuario administrador con una contraseña.
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");

                    //Asigna al nuevo usuario administrador el rol de administrador.
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                //Busca un usuario común por correo electrónico de forma asíncrona.
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    //Crea el nuevo usuario común con una contraseña.
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");

                  
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
