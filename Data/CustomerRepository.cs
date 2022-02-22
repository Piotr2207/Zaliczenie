using System.Collections.Generic;
using LibApp.Interfaces;
using LibApp.Models;
using LibApp.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

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
			await _ctx.Customers.AddAsync(customer);
		}
		public void Delete(Customer customer)
		{
			_ctx.Customers.Remove(customer);
		}
		public void Update(Customer customer)
		{
			_ctx.Update(customer);
		}

		public async Task<Customer> Get(int id)
		{
			return await _ctx.Customers.SingleOrDefaultAsync(c => c.Id == id);
		}

		public async Task<IEnumerable<Customer>> Get(Expression<Func<Customer, bool>> filter)
		{
			return await _ctx.Customers.Where(filter).Include(c => c.MembershipType).ToListAsync();
		}

		public async Task<IEnumerable<Customer>> Get()
		{
			return await _ctx.Customers.Include(c => c.MembershipType).ToListAsync();
		}


	}
}