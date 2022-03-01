using System;
using System.Collections.Generic;
using System.Text;
using LibApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<Customer, ApplicationRole, int, IdentityUserClaim<int>, ApplicationCustomerRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DbSet<MembershipType> MembershipTypes { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Genre> Genre { get; set; }
		public DbSet<Rental> Rentals { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Customer>(b => {
				b.HasMany(ur => ur.Roles)
				.WithOne(ur => ur.Customer)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
			});

			builder.Entity<ApplicationRole>(b =>
			{
				b.HasMany(ar => ar.CustomerRoles)
				.WithOne(u => u.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();
			});
		}
	}
}
