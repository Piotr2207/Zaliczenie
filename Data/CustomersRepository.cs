using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LibApp.Data;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data;

public class CustomersRepository : ICustomersRepository
{
	private readonly ApplicationDbContext _ctx;

	public CustomersRepository(ApplicationDbContext ctx)
	{
		_ctx = ctx;
	}

	public async Task Add(Customer customer)
	{
		await _ctx.Users.AddAsync(customer);
	}
	public void Delete(Customer customer)
	{
		_ctx.Users.Remove(customer);
	}
	public void Update(Customer customer)
	{
		_ctx.Update(customer);
	}

	public async Task<Customer> Get(int id)
	{
		var user = await _ctx.Users.Include(c => c.MembershipType).Include(c => c.Roles).SingleOrDefaultAsync(c => c.Id == id);

		foreach (var role in user.Roles) {
			role.Role = await _ctx.Roles.SingleOrDefaultAsync(r => r.Id == role.RoleId);
		}

		return user;
	}

	public async Task<IEnumerable<Customer>> Get(Expression<Func<Customer, bool>> filter)
	{
		var users =  await _ctx.Users.Where(filter).Include(c => c.MembershipType).Include(c => c.Roles).ToListAsync();

		foreach (var user in users) {
			foreach (var role in user.Roles) {
				role.Role = await _ctx.Roles.SingleOrDefaultAsync(r => r.Id == role.RoleId);
			}
		}

		return users;
	}

	public async Task<IEnumerable<Customer>> Get()
	{
		var users = await _ctx.Users.Include(c => c.MembershipType).Include(c => c.Roles).ToListAsync();

		foreach (var user in users) {
			foreach (var role in user.Roles) {
				role.Role = await _ctx.Roles.SingleOrDefaultAsync(r => r.Id == role.RoleId);
			}
		}

		return users;
	}
}