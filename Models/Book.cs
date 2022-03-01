using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Models
{
	public class Book
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "The name of the book is required!")]
		[StringLength(255, ErrorMessage = "Max length of the book name is 255!")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Author name is required!")]
		public string AuthorName { get; set; }

		public Genre Genre { get; set; }

		[Required(ErrorMessage = "Genre is required!")]
		[Display(Name = "Genre")]
		public byte GenreId { get; set; }

		public DateTime DateAdded { get; set; }

		[Required(ErrorMessage = "The release date is required!")]
		[Display(Name = "Realease Date")]
		public DateTime ReleaseDate { get; set; }

		[Required(ErrorMessage = "The number of books stored in stock is required!")]
		[Range(1, 20, ErrorMessage = "Number of stocked books must be from range 1-20!")]
		public int NumberInStock { get; set; }

		public int NumberAvailable { get; set; }
	}

}
