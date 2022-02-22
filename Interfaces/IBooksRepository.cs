using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibApp.Interfaces
{

    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> Get();

        Task<Book> Get(int id);
        Task<IEnumerable<Book>> Get(Expression<Func<Book, bool>> filter);

        Task Add(Book newBook);

        void Update(Book book);

        Task Delete(int id);
    }
}