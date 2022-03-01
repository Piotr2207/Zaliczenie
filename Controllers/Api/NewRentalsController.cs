using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewRentalsController : ControllerBase
	{
		public NewRentalsController(IUnitOfWork unit)
		{
			_unit = unit;
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewRental([FromBody] NewRentalDto newRental)
		{
			var customer = (await _unit.Customers.Get(c => c.Id == newRental.CustomerId)).First();
			// _context.Customers
			// 	.Include(c => c.MembershipType)
			// 	.SingleOrDefault(c => c.Id == newRental.CustomerId);

			var books = await _unit.Books.Get(b => newRental.BookIds.Contains(b.Id));
			// _context.Books
			// 	.Include(b => b.Genre)
			// 	.Where(b => newRental.BookIds.Contains(b.Id)).ToList();

			foreach (var book in books)
			{
				if (book.NumberAvailable == 0)
					return BadRequest("Book is not available");

				book.NumberAvailable--;
				var rental = new Rental()
				{
					Customer = customer,
					Book = book,
					DateRented = DateTime.Now
				};

				await _unit.Rentals.Add(rental);
				// _context.Rentals.Add(rental);
			}

			await _unit.Complete();
			// _context.SaveChanges();
			return Ok();
		}

		private readonly IUnitOfWork _unit;
	}
}
