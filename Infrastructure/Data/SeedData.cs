using Loner.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.Enums;

namespace Loner.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<UserEntity> userManager)
        {
            var context = serviceProvider.GetRequiredService<LonerDbContext>();

            var role = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            List<string> listUrl = new List<string>
            {
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905361/mmm_oecscs.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905352/image_user_2_zpuxen.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905352/image_user_2_zpuxen.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905341/image_user_1_qqy7jl.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905317/bbbb_edwkwg.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_2_s5awql.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_1_igwazz.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_rqxe4b.jpg"
            };

            if (!context.Roles.Any())
            {
                var roles = new[] { "Admin", "User" };

                foreach (var item in roles)
                {
                    if (!await role.RoleExistsAsync(item))
                        await role.CreateAsync(new IdentityRole(item));
                }
            }

            //add list user
            if (!userManager.Users.Any())
            {
                var users = new List<UserEntity>();
                int countUser = 0;
                foreach (var url in listUrl)
                {
                    var user = new UserEntity
                    {
                        IsVerifyAccount = true,
                        UserName = $"user{countUser}@test.com",
                        Email = $"user{countUser}@test.com",
                        IsActive = countUser <= 4,
                        CreatedAt = DateTime.UtcNow.AddDays(-countUser),
                        Gender = false,
                        EmailConfirmed = true,
                        AvatarUrl = url,
                        Age = 18 + countUser,
                        About = "Hello, I'm a new user! " + countUser,
                        Address = "Hanoi, Vietnam",
                        LastActive = DateTime.UtcNow,
                    };

                    countUser++;
                    var result = await userManager.CreateAsync(user, "ABCd123!@#");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                        if (countUser == 1)
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

                // add some records otp
                foreach (var item in users)
                {
                    if (!string.IsNullOrEmpty(item.Email))
                    {
                        var otp = new OTPEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Code = "123456",
                            ExpiresAt = DateTime.UtcNow.AddDays(30),
                            Email = item.Email,
                        };

                        await context.OTPs.AddAsync(otp);
                    }
                }

                // await context.SaveChangesAsync();

                //add 4 records swipes
                var user1 = users.FirstOrDefault();
                int countSwipe = 0;
                foreach (var item in users)
                {
                    if (item.Id != user1?.Id && countSwipe < 4)
                    {
                        countSwipe++;
                        var swipe = new SwipeEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            SwiperId = user1?.Id ?? "",
                            SwipedId = item.Id,
                            CreatedAt = DateTime.UtcNow,
                            Action = true,
                        };

                        await context.Swipes.AddAsync(swipe);
                    }
                }

                //add example data for interest
                var vietnameseNames = new Dictionary<InterestEnum, string>
                {
                    { InterestEnum.Am_nhac, "Âm nhạc" },
                    { InterestEnum.The_thao, "Thể thao" },
                    { InterestEnum.Du_lich, "Du lịch" },
                    { InterestEnum.Nau_an, "Nấu ăn" },
                    { InterestEnum.Lap_trinh, "Lập trình" },
                    { InterestEnum.Chup_anh, "Chụp ảnh" },
                    { InterestEnum.Ve_tranh, "Vẽ tranh" },
                    { InterestEnum.Doc_sach, "Đọc sách" },
                    { InterestEnum.Xem_phim, "Xem phim" },
                    { InterestEnum.Choi_game, "Chơi game" },
                    { InterestEnum.Viet_lach, "Viết lách" },
                    { InterestEnum.Hoc_ngoai_ngu, "Học ngoại ngữ" },
                    { InterestEnum.Di_bo, "Đi bộ" },
                    { InterestEnum.Yoga, "Yoga" },
                    { InterestEnum.Thoi_trang, "Thời trang" },
                    { InterestEnum.Cau_ca, "Câu cá" },
                    { InterestEnum.Cam_trai, "Cắm trại" },
                    { InterestEnum.Khieu_vu, "Khiêu vũ" },
                    { InterestEnum.Lam_vuon, "Làm vườn" },
                    { InterestEnum.Choi_nhac_cu, "Chơi nhạc cụ" }
                };

                foreach (var item in vietnameseNames)
                {
                    var interest = new InterestEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.Value
                    };

                    await context.Interests.AddAsync(interest);
                }

                //add some record photo foreach user
                foreach (var item in users)
                {
                    foreach (var url in listUrl)
                    {
                        var photo = new PhotoEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = item.Id,
                            Url = url,
                            CreatedAt = DateTime.UtcNow.AddDays(-1),
                        };
                        await context.Photos.AddAsync(photo);
                    }
                }

                // add some record Matches
                int countMatch = 0;
                foreach (var item in users)
                {
                    if (item.Id != user1?.Id && countMatch < 4)
                    {
                        countMatch++;
                        var match = new MatchesEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            User1Id = user1?.Id ?? "",
                            User2Id = item.Id,
                            CreatedAt = DateTime.UtcNow,
                        };

                        await context.Matches.AddAsync(match);
                    }
                }
                await context.SaveChangesAsync();

                //add some record Message
                var matches = await context.Matches.ToListAsync();
                foreach (var item in matches)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var message = new MessageEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            MatchId = item.Id,
                            SenderId = item.User1Id,
                            Content = "Hello, this is a test message " + i,
                            CreatedAt = DateTime.UtcNow.AddDays(-i),
                        };

                        await context.Messages.AddAsync(message);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}