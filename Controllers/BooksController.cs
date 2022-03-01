using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;

namespace LibApp.Controllers
{
	public class BooksController : Controller
	{
		private static readonly HttpClient _http = new(new HttpClientHandler()
		{
			ServerCertificateCustomValidationCallback = (a, b, c, d) => true
		});
		public BooksController()
		{
		}

		public async Task<IActionResult> Index()
		{
			var response = await _http.GetAsync("https://localhost:5001/api/books");
			var books = await response.Content.ReadAsAsync<IEnumerable<Book>>();

			return View(books);
		}

		public async Task<IActionResult> Details(int id)
		{

			var response = await _http.GetAsync($"https://localhost:5001/api/books/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return NotFound("Book not found");
			}

			var book = await response.Content.ReadAsAsync<Book>();

			return View(book);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var response = await _http.GetAsync($"https://localhost:5001/api/books/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return NotFound("Book not found");
			}

			var book = await response.Content.ReadAsAsync<Book>();

			var genRes = await _http.GetAsync($"https://localhost:5001/api/genre");
			var genres = await genRes.Content.ReadAsAsync<IEnumerable<Genre>>();

			var viewModel = new BookFormViewModel
			{
				Book = book,
				Genres = genres
			};

			return View("BookForm", viewModel);
		}

		public async Task<IActionResult> New()
		{
			var genRes = await _http.GetAsync($"https://localhost:5001/api/genre");
			var genres = await genRes.Content.ReadAsAsync<IEnumerable<Genre>>();

			var viewModel = new BookFormViewModel
			{
				Genres = genres
			};

			return View("BookForm", viewModel);
		}

		public async Task<IActionResult> Save(Book book)
		{
			if (book.Id == 0)
			{
				book.DateAdded = DateTime.Now;
				book.NumberAvailable = book.NumberInStock;
				var response = await _http.PostAsJsonAsync($"https://localhost:5001/api/books", book);

				if (response.StatusCode != HttpStatusCode.OK)
				{
					return BadRequest("Cannot create book");
				}
			}
			else
			{
				var response = await _http.PutAsJsonAsync($"https://localhost:5001/api/books/{book.Id}", book);

				if (response.StatusCode != HttpStatusCode.OK)
				{
					return BadRequest("Cannot update book");
				}
			}

			return RedirectToAction("Index", "Books");
		}


	}
}
