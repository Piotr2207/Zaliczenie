using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibApp.Controllers
{
	public class CustomersController : Controller
	{
		private static readonly HttpClient _http = new(new HttpClientHandler()
		{
			ServerCertificateCustomValidationCallback = (a, b, c, d) => true
		});
		public CustomersController()
		{

		}

		[Authorize(Roles = "Owner,StoreManager")]
		public ViewResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Owner,StoreManager")]
		public async Task<IActionResult> Details(int id)
		{
			var response = await _http.GetAsync($"https://localhost:5001/api/customers/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return NotFound("Customer not found");
			}

			var customer = await response.Content.ReadAsAsync<CustomerDto>();

			return View(customer);
		}

		[Authorize(Roles = "Owner")]
		public async Task<IActionResult> New()
		{

			var response = await _http.GetAsync("https://localhost:5001/api/MembershipTypes");
			var membershipTypes = await response.Content.ReadAsAsync<IEnumerable<MembershipType>>();

			var viewModel = new CustomerFormViewModel()
			{
				MembershipTypes = membershipTypes
			};


			return View("CustomerForm", viewModel);
		}

		[Authorize(Roles = "Owner")]
		public async Task<IActionResult> Edit(int id)
		{
			var conRes = await _http.GetAsync($"https://localhost:5001/api/customers/{id}");

			if (conRes.StatusCode == HttpStatusCode.NotFound)
			{
				return NotFound("Customer not found");
			}

			var customer = await conRes.Content.ReadAsAsync<Customer>();

			var msTypeRes = await _http.GetAsync("https://localhost:5001/api/MembershipTypes");
			var membershipTypes = await msTypeRes.Content.ReadAsAsync<IEnumerable<MembershipType>>();

			var viewModel = new CustomerFormViewModel(customer)
			{
				MembershipTypes = membershipTypes
			};

			return View("CustomerForm", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Owner")]
		public async Task<IActionResult> Save(Customer customer)
		{
			if (!ModelState.IsValid)
			{

				var msTypeRes = await _http.GetAsync("https://localhost:5001/api/MembershipTypes");
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
					return BadRequest("Cannot create the new customer");
			}
			else
			{
				var cusRes = await _http.PutAsJsonAsync($"https://localhost:5001/api/customers/{customer.Id}", customer);

				if (cusRes.StatusCode != HttpStatusCode.OK)
					return BadRequest("Cannot update the selected customer");
			}

			return RedirectToAction("Index", "Customers");
		}
	}
}