using System.Threading.Tasks;
using LibApp.Interfaces;
namespace LibApp.Data;
public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _ctx;

	public UnitOfWork(ApplicationDbContext ctx)
	{
		_ctx = ctx;
	}

	public ICustomersRepository Customers => new CustomersRepository(_ctx);
	public IBooksRepository Books => new BooksRepository(_ctx);
	public IMembershipTypesRepository MembershipTypes => new MembershipTypesRepository(_ctx);
	public IRentalsRepository Rentals => new RentalsRepository(_ctx);
	public IGenreRepository Genre => new GenreRepository(_ctx);

	public async Task<bool> Complete()
	{
		return await _ctx.SaveChangesAsync() > 0;
	}

	public bool HasChanges()
	{
		return _ctx.ChangeTracker.HasChanges();
	}
}