using System.Collections.Generic;
using System.Threading.Tasks;
using LibApp.Models;

namespace LibApp.Interfaces;
public interface IGenreRepository
{
	Task Add(Genre type);
	Task<Genre> Get(int id);
	Task<Genre> Get(string name);
	Task<IEnumerable<Genre>> Get();
}