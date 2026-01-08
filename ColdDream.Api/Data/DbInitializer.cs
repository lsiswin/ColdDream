using ColdDream.Api.Models;

namespace ColdDream.Api.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Seed Banners
        if (!context.Banners.Any())
        {
            var banners = new Banner[]
            {
                new Banner
                {
                    Title = "新春特惠：全场8折",
                    ImageUrl = "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf",
                    Tag = "限时活动",
                    TargetUrl = "/pages/products/index",
                    SortOrder = 1,
                },
                new Banner
                {
                    Title = "探索未知的秘境",
                    ImageUrl = "https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1",
                    Tag = "热门推荐",
                    TargetUrl = "/pages/routes/detail?id=1",
                    SortOrder = 2,
                },
                new Banner
                {
                    Title = "加入我们，成为旅行体验师",
                    ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e",
                    Tag = "招募",
                    TargetUrl = "https://www.colddream.com/join",
                    SortOrder = 3,
                },
            };
            context.Banners.AddRange(banners);
        }

        // Seed Inspirations
        if (!context.Inspirations.Any())
        {
            var inspirations = new Inspiration[]
            {
                new Inspiration
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ImageUrl = "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e",
                    Description = "京都的秋天真是太美了！",
                    UserId = Guid.NewGuid(),
                    UserName = "旅行家小A",
                    UserAvatar = "",
                    Likes = 120,
                    Collects = 45,
                },
                new Inspiration
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ImageUrl = "https://images.unsplash.com/photo-1540541338287-41700207dee6",
                    Description = "北海道滑雪初体验",
                    UserId = Guid.NewGuid(),
                    UserName = "滑雪爱好者",
                    UserAvatar = "",
                    Likes = 88,
                    Collects = 20,
                },
                new Inspiration
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e",
                    Description = "冲绳潜水，见到了海龟",
                    UserId = Guid.NewGuid(),
                    UserName = "深蓝",
                    UserAvatar = "",
                    Likes = 230,
                    Collects = 100,
                },
            };
            context.Inspirations.AddRange(inspirations);
        }

        // Seed Butlers
        if (!context.Butlers.Any())
        {
            var butlers = new Butler[]
            {
                new Butler
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "金牌管家小王",
                    Price = 200,
                    Rating = 4.9,
                    Tags = "[\"热情\",\"专业\"]",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/1.jpg",
                },
                new Butler
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "资深向导老李",
                    Price = 300,
                    Rating = 5.0,
                    Tags = "[\"经验丰富\",\"幽默\"]",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                },
                new Butler
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "当地达人小美",
                    Price = 150,
                    Rating = 4.8,
                    Tags = "[\"细心\",\"活泼\"]",
                    AvatarUrl = "https://randomuser.me/api/portraits/women/1.jpg",
                },
            };
            context.Butlers.AddRange(butlers);
            // Seed TourRoutes
            if (!context.TourRoutes.Any())
            {
                var routes = new TourRoute[]
                {
                    new TourRoute
                    {
                        Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                        Title = "京都深度游",
                        Description = "体验古都风情，探访千年寺院。",
                        Price = 5999,
                        ImageUrl = "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e",
                        IsPrivate = false,
                        Tags = "[\"文化\",\"历史\"]",
                        Sales = 120,
                        RouteMapUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b",
                        Itinerary =
                            "[{\"day\": 1, \"title\": \"抵达京都\", \"content\": \"入住传统日式旅馆，体验怀石料理。\"}, {\"day\": 2, \"title\": \"清水寺与祗园\", \"content\": \"清晨参拜清水寺，下午漫步祗园花见小路。\"}, {\"day\": 3, \"title\": \"岚山竹林\", \"content\": \"乘坐小火车游览岚山，漫步竹林小径。\"}]",
                    },
                    new TourRoute
                    {
                        Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                        Title = "北海道雪国之旅",
                        Description = "在粉雪天堂尽情滑雪，享受露天温泉。",
                        Price = 8888,
                        ImageUrl = "https://images.unsplash.com/photo-1540541338287-41700207dee6",
                        IsPrivate = false,
                        Tags = "[\"滑雪\",\"温泉\"]",
                        Sales = 85,
                        RouteMapUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b",
                        Itinerary =
                            "[{\"day\": 1, \"title\": \"抵达札幌\", \"content\": \"品尝札幌拉面，参观白色恋人公园。\"}, {\"day\": 2, \"title\": \"二世谷滑雪\", \"content\": \"全天滑雪体验，晚上享受温泉。\"}, {\"day\": 3, \"title\": \"小樽运河\", \"content\": \"漫步小樽运河，选购玻璃工艺品。\"}]",
                    },
                };
                context.TourRoutes.AddRange(routes);
            }
            else
            {
                // Update existing routes with sample data if missing
                var routes = context.TourRoutes.Where(r => r.Itinerary == null).ToList();
                foreach (var route in routes)
                {
                    route.RouteMapUrl = "https://images.unsplash.com/photo-1524661135-423995f22d0b";
                    route.Itinerary =
                        "[{\"day\": 1, \"title\": \"行程第一天\", \"content\": \"抵达目的地，入住酒店，自由活动。\"}, {\"day\": 2, \"title\": \"深度游览\", \"content\": \"参观当地著名景点，体验特色文化。\"}, {\"day\": 3, \"title\": \"返程\", \"content\": \"购买纪念品，前往机场/车站。\"}]";
                    route.Sales = new Random().Next(10, 500); // Random sales for demo
                }
                if (routes.Any())
                {
                    context.SaveChanges();
                }
            }

            context.SaveChanges();

            context.SaveChanges();
        }
    }
}
