using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
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
				Name = "Marcin",
				HasNewsletterSubscribed = false,
				MembershipTypeId = 2,
			},
			new Customer
			{
				Name = "Aleksandra",
				HasNewsletterSubscribed = true,
				MembershipTypeId = 3,
			}
		};

			foreach (var user in users)
			{
				user.UserName = user.Name.ToLower();
				await userManager.CreateAsync(user, "User1!2#");
				await userManager.AddToRoleAsync(user, "User");
			}

			var owner = new Customer
			{
				Name = "Wojtek",
				UserName = "owner",
				Email = "owner@owner.pl",
				HasNewsletterSubscribed = true,
				MembershipTypeId = 3,
			};

			await userManager.CreateAsync(owner, "Wojtek1!2");
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
	}
}