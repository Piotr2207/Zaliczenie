using System.Collections.Generic;
using LibApp.Interfaces;
using LibApp.Models;
using LibApp.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data
{
	public class RentalsRepository : IRentalsRepository
	{
		private readonly ApplicationDbContext _ctx;

		public RentalsRepository(ApplicationDbContext ctx)
		{
			_ctx = ctx;
		}
		public async Task Add(Rental rental)
		{
			await _ctx.Rentals.AddAsync(rental);
		}

		public async Task<Rental> Get(int id)
		{
			return await _ctx.Rentals.SingleOrDefaultAsync(r => r.Id == id);
		}

		public async Task<IEnumerable<Rental>> Get()
		{
			return await _ctx.Rentals.ToListAsync();
		}
	}
}