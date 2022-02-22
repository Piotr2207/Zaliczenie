using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
namespace LibApp.Controllers
{
	public class CustomersController : Controller
	{
		private static readonly HttpClient _http = new();
		public CustomersController()
		{
		}

		public ViewResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int id)
		{
			var response = await _http.GetAsync($"https://localhost:5001/api/customers/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return Content("User not found");
			}

			var customer = await response.Content.ReadAsAsync<Customer>();

			return View(customer);
		}

		public async Task<IActionResult> New()
		{

			var response = await _http.GetAsync("https://localhost:5001/api/mstypes/");
			var membershipTypes = await response.Content.ReadAsAsync<IEnumerable<MembershipType>>();

			var viewModel = new CustomerFormViewModel()
			{
				MembershipTypes = membershipTypes
			};


			return View("CustomerForm", viewModel);
		}

		public async Task<IActionResult> Edit(int id)
		{

			var conRes = await _http.GetAsync($"https://localhost:5001/api/customers/{id}");

			if (conRes.StatusCode == HttpStatusCode.NotFound)
			{
				return NotFound();
			}

			var customer = await conRes.Content.ReadAsAsync<Customer>();

			var msTypeRes = await _http.GetAsync("https://localhost:5001/api/mstypes");
			var membershipTypes = await msTypeRes.Content.ReadAsAsync<IEnumerable<MembershipType>>();

			var viewModel = new CustomerFormViewModel(customer)
			{
				MembershipTypes = membershipTypes
			};

			return View("CustomerForm", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(Customer customer)
		{
			if (!ModelState.IsValid)
			{

				var msTypeRes = await _http.GetAsync("https://localhost:5001/api/mstypes/");
				var membershipTypes = await msTypeRes.Content.ReadAsAsync<IEnumerable<MembershipType>>();

				var viewModel = new CustomerFormViewModel(customer)
				{
					MembershipTypes = membershipTypes
				};

				return View("CustomerForm", viewModel);
			}

			if (customer.Id == 0)
			{
				var cusRes = await _http.PostAsJsonAsync("https://localhost:5001/api/customers", customer);

				if (cusRes.StatusCode != HttpStatusCode.OK)
					return Content("Cannot create the new customer");
			}
			else
			{
				var cusRes = await _http.PutAsJsonAsync($"https://localhost:5001/api/customers/{customer.Id}", customer);

				if (cusRes.StatusCode != HttpStatusCode.OK)
					return Content("Cannot update the selected customer");
			}

			return RedirectToAction("Index", "Customers");
		}
	}
}