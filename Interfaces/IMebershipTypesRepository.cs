using System.Collections.Generic;
using System.Threading.Tasks;
using LibApp.Models;

namespace LibApp.Interfaces
{
	public interface IMembershipTypesRepository
	{
		Task Add(MembershipType type);
		Task AddRange(IEnumerable<MembershipType> types);
		Task<MembershipType> Get(int id);
		Task<IEnumerable<MembershipType>> Get();
	}

}