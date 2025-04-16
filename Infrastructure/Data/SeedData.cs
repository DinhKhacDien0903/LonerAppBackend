using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Loner.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserEntity> userManager)
        {
            var context = serviceProvider.GetRequiredService<LonerDbContext>();

            var role = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.Roles.Any())
            {
                var roles = new[] { "Admin", "User" };

                foreach (var item in roles)
                {
                    if (!await role.RoleExistsAsync(item))
                        await role.CreateAsync(new IdentityRole(item));
                }
            }

            var x = userManager.Users.ToList();
            var y = userManager.Users.Count();
            var z = userManager.Users.FirstOrDefault();
            if (userManager.Users.Count() >= 1)
            {
                var users = new List<UserEntity>();

                for (int i = 1; i <= 5; i++)
                {
                    var user = new UserEntity
                    {
                        IsVerifyAccount = true,
                        UserName = $"user{i}@test.com",
                        Email = $"user{i}@test.com",
                        IsActive = i <= 4,
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        Gender = false,
                        EmailConfirmed = true,
                        AvatarUrl = "https://res.cloudinary.com/dlran3qvj/image/upload/v1732701622/file_1732701619587.jpg"
                    };

                    var result = await userManager.CreateAsync(user, "ABCd123!@#");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                        if(i == 1)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }

                    }
                }

                await context.SaveChangesAsync();
                users = await context.Users.Select(x => x).ToListAsync();

            }
        }
    }
}