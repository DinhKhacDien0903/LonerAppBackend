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
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491641/girl-5531217_640_tycd46.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491640/girl-8539256_640_kz7qcn.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491640/girl-7115394_640_hqge2j.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491639/fashion-6364998_640_mt3bzp.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491639/girl-7883816_640_at7dd1.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491639/girl-4733999_640_djcn13.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491638/girl-4755130_640_pylukw.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491638/girl-4436130_640_fbfito.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491638/girl-5539094_640_est4v3.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491900/pexels-photo-31663381_xxqiuf.webp",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491899/pexels-photo-30123498_t3a75p.webp",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491899/pexels-photo-32017036_xkdrdd.webp",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491898/pexels-photo-30123497_djlc8s.webp",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491898/free-photo-of-elegant-vietnamese-woman-in-traditional-dress-with-fan_ricxsl.jpg",
                "https://res.cloudinary.com/de0werx80/image/upload/v1747491898/free-photo-of-serene-asian-woman-with-yellow-flowers-outdoors_lxafxo.jpg",
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
                string address = string.Empty;
                double longitude, latitude;
                int urlsLength = listUrl.Count;
                double deltaLatitude = 0.045;
                double deltaLongitude = 0.048;
                for (int i = 0; i < 50; i++)
                {
                    if (i % 2 == 0)
                    {
                        latitude = Math.Round(21.0285 + (i * deltaLatitude), 4);
                        longitude = Math.Round(105.8544 + (i * deltaLongitude), 4);
                        address = "Hanoi, Vietnam";
                    }
                    else
                    {
                        latitude = Math.Round(20.2500 + (i * deltaLatitude), 4);
                        longitude = Math.Round(105.9745 + (i * deltaLongitude), 4);
                        address = "Ninh Binh, Vietnam";
                    }
                    var user = new UserEntity
                    {
                        IsVerifyAccount = true,
                        UserName = $"user{i}",
                        Email = $"user{i}@test.com",
                        IsActive = i <= 15,
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        Gender = i % 2 == 0,
                        EmailConfirmed = true,
                        AvatarUrl = listUrl[countUser],
                        Age = 18 + i,
                        About = "Hello, I'm a new user " + i,
                        Address = address,
                        Longitude = longitude,
                        Latitude = latitude,
                        LastActive = DateTime.UtcNow,
                        University = "Hanoi University of Science and Technology",
                        Work = "Developer at " + i,
                        DateOfBirth = DateTime.UtcNow.AddYears(-18 - i),
                    };

                    countUser = (countUser + 1 >= urlsLength) ? 0 : countUser + 1;
                    var result = await userManager.CreateAsync(user, "ABCd123!@#");

                    if (result.Succeeded)
                    {
                        users.Add(user);
                        if (i == 1)
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

                        await context.OTP.AddAsync(otp);
                    }
                }

                // await context.SaveChangesAsync();

                //add 4 records swipes
                var user1 = users.FirstOrDefault();
                int countSwipe = 0;
                foreach (var item in users)
                {
                    if (countSwipe > 5)
                        break;

                    if (item.Id != user1?.Id)
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

                        await context.Swipe.AddAsync(swipe);
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

                    await context.Interest.AddAsync(interest);
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
                        await context.Photo.AddAsync(photo);
                    }
                }

                // add some record Matches
                int countMatch = 0;
                foreach (var item in users)
                {
                    if (countMatch > 10)
                        break;

                    if (item.Id != user1?.Id)
                    {
                        countMatch++;
                        var match = new MatchesEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            User1Id = user1?.Id ?? "",
                            User2Id = item.Id,
                            CreatedAt = DateTime.UtcNow,
                        };

                        await context.Matche.AddAsync(match);
                    }
                }
                await context.SaveChangesAsync();

                //add some record Message
                var matches = await context.Matche.ToListAsync();
                foreach (var item in matches)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        string fromUser = i < 5 ? "user1" : "user2";
                        var message = new MessageEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            MatchId = item.Id,
                            SenderId = i < 5 ? item.User1Id : item.User2Id,
                            Content = i >= 5 ? $"Hello, this is a test message {i} from {fromUser}" : listUrl[i],
                            IsImage = i < 5,
                            CreatedAt = DateTime.UtcNow.AddDays(-i),
                        };

                        await context.Message.AddAsync(message);
                    }
                }

                //add some interest for user;
                var interests = await context.Interest.ToListAsync();
                foreach (var item in users)
                {
                    int count = 0;
                    foreach (var interest in interests)
                    {
                        if (count > 6)
                            break;
                        count++;
                        var userInterest = new User_InterestEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = item.Id,
                            InterestId = interest.Id
                        };

                        await context.User_Interest.AddAsync(userInterest);
                    }
                }

                int countPreference = 0;
                foreach (var user in users)
                {
                    var preference = new PreferenceEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        PreferenceGender = countPreference % 2 != 0,
                        MinAge = 18,
                        MaxAge = 50,
                    };

                    await context.Preference.AddAsync(preference);
                }

                var firstMatch = await context.Matche.FirstOrDefaultAsync();
                var sender = await context.Users.FirstOrDefaultAsync(x => x.Id == firstMatch.User2Id);
                var Recivre = await context.Users.FirstOrDefaultAsync(x => x.Id == firstMatch.User1Id);
                for (var i = 0; i < 10; i++)
                {
                    var notification = new NotificationEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReceiverId = Recivre.Id,
                        SenderId = sender.Id,
                        Type = 1,
                        RelatedId = sender.Id,
                        Content = "Hello, this is a match test notification",
                        Title = "Test Match Notification",
                        Subtitle = "This is a subtitle",
                        IsRead = false,
                        NotificationImage = sender.AvatarUrl,
                    };

                    await context.Notification.AddAsync(notification);
                }

                for (var i = 0; i < 10; i++)
                {
                    var notification = new NotificationEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReceiverId = Recivre.Id,
                        SenderId = sender.Id,
                        Type = 0,
                        RelatedId = sender.Id,
                        Content = "Hello, this is a like test notification",
                        Title = "Test Like Notification",
                        Subtitle = "This is a subtitle",
                        IsRead = false,
                        NotificationImage = Recivre.AvatarUrl,
                    };

                    await context.Notification.AddAsync(notification);
                }

                for (var i = 0; i < 10; i++)
                {
                    var notification = new NotificationEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReceiverId = Recivre.Id,
                        SenderId = sender.Id,
                        Type = 2,
                        RelatedId = firstMatch.Id,
                        Content = "Hello, this is a message test notification",
                        Title = "Test Message Notification",
                        Subtitle = "This is a subtitle",
                        IsRead = false,
                        NotificationImage = sender.AvatarUrl,
                    };

                    await context.Notification.AddAsync(notification);
                }
                //add some record notifications
                await context.SaveChangesAsync();
            }
        }
    }
}