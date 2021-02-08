using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsAppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAPI.Models
{
    public interface IDbInitializer
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();
    }
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DatabaseContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DatabaseContext>())
                {
                    context.Users.RemoveRange(context.Users.ToList());
                    context.SaveChanges();
                    if (!context.Users.Any())
                    {
                        var adminUser = new User
                        {
                            CreatedAt = DateTime.Now,
                            Username = "admin",
                            Password = BCrypt.Net.BCrypt.HashPassword("admin"), // should be hash
                            FullName = "Admin Test",
                            Role = "Admin"
                        };
                        context.Users.Add(adminUser);
                        var writer = new User
                        {
                            CreatedAt = DateTime.Now,
                            Username = "writer",
                            Password = BCrypt.Net.BCrypt.HashPassword("writer"), // should be hash
                            FullName = "Writer Test 1",
                            Role = "Writer"
                        };
                        context.Users.Add(writer);
                        var writer1 = new User
                        {
                            CreatedAt = DateTime.Now,
                            Username = "writer1",
                            Password = BCrypt.Net.BCrypt.HashPassword("writer1"), // should be hash
                            FullName = "Writer Test 2",
                            Role = "Writer"
                        };
                        context.Users.Add(writer1);
                        context.SaveChanges();
                    }

                    if (!context.Articles.Any())
                    {
                        var articleTitle = "Lorem Ipsum  test 1";
                        var articleText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
                        context.Articles.AddRange(new Article[]{ new Article()
                         {
                             Text = articleText,
                             Subject = articleTitle,
                             Likes = 0,
                             WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,
                             CreatedAt = DateTime.Now
                         },
                           new Article()
                          {
                              Text = articleText,
                              Subject = articleTitle,
                              Likes = 0,
                              WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,
                                CreatedAt = DateTime.Now

                          },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.Where(s=>s.Role == "Writer").ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           },
                           new Article()
                           {
                               Text = articleText,
                               Subject = articleTitle,
                               Likes = 0,
                               WriterId = context.Users.ToList()[new Random().Next(1,2)].UserID,  CreatedAt = DateTime.Now
                           }
                         });
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
