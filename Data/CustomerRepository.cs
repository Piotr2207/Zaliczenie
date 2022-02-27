using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LibApp.Data;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data
{

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
			return await _ctx.Users.Include(c => c.MembershipType).SingleOrDefaultAsync(c => c.Id == id);
		}

		public async Task<IEnumerable<Customer>> Get(Expression<Func<Customer, bool>> filter)
		{
			return await _ctx.Users.Where(filter).Include(c => c.MembershipType).ToListAsync();
		}

		public async Task<IEnumerable<Customer>> Get()
		{
			return await _ctx.Users.Include(c => c.MembershipType).ToListAsync();
		}


	}
}