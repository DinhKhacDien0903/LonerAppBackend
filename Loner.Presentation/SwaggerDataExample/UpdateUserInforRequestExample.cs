using Loner.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;
using static Loner.Application.DTOs.User;

namespace Loner.Presentation.SwaggerDataExample
{
    public class UpdateUserInforRequestExample : IExamplesProvider<UpdateUserInforRequest>
    {
        public UpdateUserInforRequest GetExamples()
        {
            return new UpdateUserInforRequest
            {
                EditRequest = new EditInforRequest
                {
                    UserId = "user123",
                    About = "Love coding and hiking.",
                    University = "Stanford University",
                    Work = "Software Engineer at OpenAI",
                    Gender = true,
                    Photos =
                    [
                        "https://res.cloudinary.com/de0werx80/image/upload/v1744905317/bbbb_edwkwg.jpg",
                        "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_2_s5awql.jpg",
                        "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_1_igwazz.jpg",
                        "https://res.cloudinary.com/de0werx80/image/upload/v1744905486/Design_a_logo_for_a_website_that_sells_second-hand_2nd_hand_clothing_and_goods_targeting_a_young_rqxe4b.jpg"
                    ],
                    Interests =
                    [
                        "Câu cá",
                        "Cắm trại",
                        "Khiêu vũ",
                        "Làm vườn",
                        "Chơi nhạc cụ"
                    ]
                }
            };
        }
    }
}