using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.MembershipTypes.Any())
                {
                    Console.WriteLine("Database already seeded");
                    return;
                }

                context.MembershipTypes.AddRange(
                    new MembershipType
                    {
                        Id = 1,
                        SignUpFee = 0,
                        DurationInMonths = 0,
                        DiscountRate = 0
                    },
                    new MembershipType
                    {
                        Id = 2,
                        SignUpFee = 30,
                        DurationInMonths = 1,
                        DiscountRate = 10
                    },
                    new MembershipType
                    {
                        Id = 3,
                        SignUpFee = 90,
                        DurationInMonths = 3,
                        DiscountRate = 15
                    },
                    new MembershipType
                    {
                        Id = 4,
                        SignUpFee = 300,
                        DurationInMonths = 12,
                        DiscountRate = 20
                    });
                context.SaveChanges();
            }
        }
        public static void InitialCustomers(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Customers.Any())
                {
                    Console.WriteLine("Database already seeded");
                    return;
                }
                context.Customers.AddRange(
                    new Customer
                    {
                        Name = "Jan",
                        HasNewsletterSubscribed = false,
                        MembershipTypeId = 1,
                    },
                     new Customer
                     {
                         Name = "Marek",
                         HasNewsletterSubscribed = true,
                         MembershipTypeId = 2,
                     },
                      new Customer
                      {
                          Name = "Tomek",
                          HasNewsletterSubscribed = false,
                          MembershipTypeId = 3,
                      });
                context.SaveChanges();
            }
        }
    }
}