using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestionariesAppData;
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
                        context.SaveChanges();
                    }
                    if (!context.Questions.Any())
                    {

                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I feel that I am a person of worth, at least on an equal plane with others.",
                            GradePattern = "3,2,1,0"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I feel that I have a number of good qualities.",
                            GradePattern = "3,2,1,0"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "All in all, I am inclined to feel that I am a failure.",
                            GradePattern = "0,1,2,3"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I am able to do things as well as most other people.",
                            GradePattern = "3,2,1,0"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I feel I do not have much to be proud of.",
                            GradePattern = "0,1,2,3"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I take a positive attitude toward myself.",
                            GradePattern = "3,2,1,0"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "On the whole, I am satisfied with myself.",
                            GradePattern = "3,2,1,0"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I wish I could have more respect for myself.",
                            GradePattern = "0,1,2,3"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "I certainly feel useless at times.",
                            GradePattern = "0,1,2,3"
                        });
                        context.Questions.Add(new QuestionariesAppData.Models.Question
                        {
                            QuestionText = "At times I think I am no good at all.",
                            GradePattern = "0,1,2,3"
                        });
                        context.SaveChanges();
                    }

                }
            }
        }
    }
}
