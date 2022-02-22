using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibApp.Data
{

    public class BooksRepository : IBooksRepository
    {
        private readonly ApplicationDbContext _ctx;

        public BooksRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task Add(Book newBook)
        {
            await _ctx.Books.AddAsync(newBook);
        }

        public async Task Delete(int id)
        {
            var toDelete = await _ctx.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (toDelete is not null)
            _ctx.Books.Remove(toDelete);
        }

        public void Update(Book book)
        {
           _ctx.Books.Update(book);
        }
        public async Task<IEnumerable<Book>> Get(Expression<Func<Book, bool>> filter)
        {
           return await _ctx.Books.Where(filter).Include(b => b.Genre).ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
           return await _ctx.Books.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task <IEnumerable<Book>> Get()
        {
            return await _ctx.Books.Include(b => b.Genre).ToListAsync();
        }
    }
}