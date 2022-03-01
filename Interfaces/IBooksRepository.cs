using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LibApp.Models;

namespace LibApp.Interfaces;

public interface IBooksRepository
{
	Task<IEnumerable<Book>> Get();
	Task<Book> Get(int id);
	Task<IEnumerable<Book>> Get(Expression<Func<Book, bool>> filter);
	Task Add(Book book);
	void Update(Book book);
	Task Delete(int id);
}