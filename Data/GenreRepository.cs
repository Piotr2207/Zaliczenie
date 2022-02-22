using System.Collections.Generic;
using LibApp.Interfaces;
using LibApp.Models;
using LibApp.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibApp.Data
{
	public class GenreRepository : IGenreRepository
	{
		private readonly ApplicationDbContext _ctx;

		public GenreRepository(ApplicationDbContext ctx)
		{
			_ctx = ctx;
		}
		public async Task Add(Genre type)
		{
			await _ctx.Genre.AddAsync(type);
		}

		public async Task<Genre> Get(int id)
		{
			return await _ctx.Genre.SingleOrDefaultAsync(g => g.Id == id);
		}

		public async Task<Genre> Get(string name)
		{
			return await _ctx.Genre.SingleOrDefaultAsync(g => g.Name == name);
		}

		public async Task<IEnumerable<Genre>> Get()
		{
			return await _ctx.Genre.ToListAsync();
		}
	}
}