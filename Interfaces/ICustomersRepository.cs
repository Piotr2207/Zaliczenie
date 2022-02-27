using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LibApp.Models;

namespace LibApp.Interfaces
{

	public interface ICustomersRepository
	{
		Task<IEnumerable<Customer>> Get();
		Task<Customer> Get(int id);
		Task<IEnumerable<Customer>> Get(Expression<Func<Customer, bool>> filter);
		Task Add(Customer customer);
		void Update(Customer customer);
		void Delete(Customer customer);
	}
}