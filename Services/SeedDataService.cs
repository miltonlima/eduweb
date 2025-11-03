using Microsoft.AspNetCore.Identity;
using MvcSaed.Models;

namespace MvcSaed.Services
{
    /// <summary>
    /// Serviço para inicialização de dados do sistema
    /// </summary>
    public static class SeedDataService
    {
        /// <summary>
        /// Inicializar dados básicos do sistema
        /// </summary>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRoles(roleManager);
            await CreateAdminUser(userManager);
        }

        /// <summary>
        /// Criar roles básicas do sistema
        /// </summary>
        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrador", "Professor", "Coordenador", "Usuario" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        /// <summary>
        /// Criar usuário administrador padrão
        /// </summary>
        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@saed.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nome = "Administrador do Sistema",
                    Funcao = "Administrador",
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                    Console.WriteLine($"Usuário administrador criado: {adminEmail} / Admin@123");
                }
                else
                {
                    Console.WriteLine("Erro ao criar usuário administrador:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
        }
    }
}