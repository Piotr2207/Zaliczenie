using System.Collections.Generic;
using LibApp.Models;
using LibApp.Interfaces;
using LibApp.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data
{
	public class MembershipTypesRepository : IMembershipTypesRepository
	{
		private readonly ApplicationDbContext _ctx;
		public MembershipTypesRepository(ApplicationDbContext ctx)
		{
			_ctx = ctx;
		}
		public async Task Add(MembershipType type)
		{
			await _ctx.MembershipTypes.AddAsync(type);
		}

		public async Task AddRange(IEnumerable<MembershipType> types)
		{
			await _ctx.MembershipTypes.AddRangeAsync(types);
		}

		public async Task<MembershipType> Get(int id)
		{
			return await _ctx.MembershipTypes.SingleOrDefaultAsync(m => m.Id == id);
		}

		public async Task<IEnumerable<MembershipType>> Get()
		{
			return await _ctx.MembershipTypes.ToListAsync();
		}
	}
}