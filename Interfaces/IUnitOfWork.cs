using System.Threading.Tasks;
using LibApp.Interfaces;

namespace LibApp.Interfaces
{
	public interface IUnitOfWork
	{
		ICustomersRepository Customers { get; }
		IBooksRepository Books { get; }
		IMembershipTypesRepository MembershipTypes { get; }
		IRentalsRepository Rentals { get; }
		IGenreRepository Genre { get; }
		Task<bool> Complete();
		bool HasChanges();
	}
}