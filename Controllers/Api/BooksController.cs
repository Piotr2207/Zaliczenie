using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		public BooksController(IUnitOfWork unit, IMapper mapper)
		{
			_unit = unit;
			_mapper = mapper;
		}

		// GET /api/books/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Book>> GetBook(int id)
		{
			var book = await _unit.Books.Get(id);

			if (book == null)
			{
				return NotFound("Book doesn't exist");
			}

			return book;
		}

		// GET /api/books
		[HttpGet]
		public async Task<IEnumerable<Book>> GetBooks(string query = null)
		{
			var booksQuery = await _unit.Books.Get(b => b.NumberAvailable > 0);

			if (!String.IsNullOrWhiteSpace(query))
			{
				booksQuery = booksQuery.Where(b => b.Name.Contains(query));
			}

			return booksQuery.ToList();
		}

		// PUT api/books/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateBook(int id, Book book)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Cannot update book with the given ID");
			}

			var bookInDb = await _unit.Books.Get(book.Id);
			if (bookInDb == null)
			{
				return BadRequest("Cannot update book with the given ID");
			}


			_unit.Books.Update(_mapper.Map(book, bookInDb));
			await _unit.Complete();
			return Ok();
		}

		private readonly IUnitOfWork _unit;
		private readonly IMapper _mapper;
	}
}