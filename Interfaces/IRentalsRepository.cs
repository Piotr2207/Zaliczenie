using System.Collections.Generic;
using System.Threading.Tasks;
using LibApp.Models;

namespace LibApp.Interfaces
{
	public interface IRentalsRepository
	{
		Task Add(Rental rental);
		Task<Rental> Get(int id);
		Task<IEnumerable<Rental>> Get();
	}
}