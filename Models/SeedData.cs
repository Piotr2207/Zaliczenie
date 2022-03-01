using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using LibApp.Interfaces;
namespace LibApp.Models
{

	public static class SeedData
	{
		public static void InitMembershipTypes(IServiceProvider serviceProvider)
		{
			using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
			if (context.MembershipTypes.Any())
			{
				Console.WriteLine("MemberShipTypes already seeded");
				return;
			}

			context.MembershipTypes.AddRange(
				new MembershipType
				{
					Id = 1,
					Name = "Pay as you go",
					SignUpFee = 0,
					DurationInMonths = 0,
					DiscountRate = 0
				},
				new MembershipType
				{
					Id = 2,
					Name = "Monthly",
					SignUpFee = 30,
					DurationInMonths = 1,
					DiscountRate = 10
				},
				new MembershipType
				{
					Id = 3,
					Name = "Quaterly",
					SignUpFee = 90,
					DurationInMonths = 3,
					DiscountRate = 15
				},
				new MembershipType
				{
					Id = 4,
					Name = "Yearly",
					SignUpFee = 300,
					DurationInMonths = 12,
					DiscountRate = 20
				});

			context.SaveChanges();
		}

		public static async Task InitCustomers(IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();

			if (await userManager.Users.AnyAsync())
			{
				Console.WriteLine("Customers already seeded");
				return;
			}

			Customer[] users = {
			new Customer
			{
				Name = "Jan",
				HasNewsletterSubscribed = false,
				MembershipTypeId = 1,
			},
			new Customer
			{
				Name = "Karolina",
				HasNewsletterSubscribed = false,
				MembershipTypeId = 2,
			},
			new Customer
			{
				Name = "Michal",
				HasNewsletterSubscribed = true,
				MembershipTypeId = 3,
			}
		};

			foreach (var user in users)
			{
				user.UserName = user.Name.ToLower();
				user.Email = $"{user.Name.ToLower()}@example.com";
				user.EmailConfirmed = true;
				await userManager.CreateAsync(user, "Passwordzisko123!@#");
				await userManager.AddToRoleAsync(user, "User");
			}

			var owner = new Customer
			{
				Name = "Ryszardos",
				UserName = "owner",
				Email = "owner@owner.pl",
				HasNewsletterSubscribed = true,
				MembershipTypeId = 3,
				EmailConfirmed = true
			};

			await userManager.CreateAsync(owner, "Passwordzisko123!");
			await userManager.AddToRoleAsync(owner, "Owner");

		}

		public static async Task InitRoles(IServiceProvider service)
		{
			var roleManager = service.GetRequiredService<RoleManager<ApplicationRole>>();
			string[] rolesNames = { "User", "StoreManager", "Owner" };

			foreach (var roleName in rolesNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);

				if (!roleExist)
				{
					await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
				}
			}
		}

		public static async Task InitGenres(IServiceProvider service)
		{
			var context = service.GetRequiredService<IUnitOfWork>();
			var seeded = await context.Genre.Get();

			if (seeded.ToArray().Length != 0)
			{
				Console.WriteLine("Genres already seeded");
				return;
			}

			Genre[] genres = {
			new Genre {
				Id = 1,
				Name = "AAA"
			},
			new Genre {
				Id = 2,
				Name = "BBB"
			},
			new Genre {
				Id = 3,
				Name = "CCC"
			},
			new Genre {
				Id = 4,
				Name = "SSS"
			}
		};

			foreach (var genre in genres)
			{
				await context.Genre.Add(genre);
			}

			await context.Complete();
		}
	}
}