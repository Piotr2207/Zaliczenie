using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		public CustomersController(IUnitOfWork unit, IMapper mapper)
		{
			_unit = unit;
			_mapper = mapper;
		}

		// GET /api/customers
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(string query = null)
		{
			IEnumerable<Customer> customersQuery = await _unit.Customers.Get();

			if (!String.IsNullOrWhiteSpace(query))
			{
				customersQuery = customersQuery.Where(c => c.Name.Contains(query));
			}

			var customerDtos = customersQuery.Select(_mapper.Map<Customer, CustomerDto>).ToList();

			return customerDtos;
		}

		// GET /api/customers/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			var customer = await _unit.Customers.Get(id);

			if (customer == null)
			{
				return NotFound("Customer doesn't exist");
			}

			return _mapper.Map<CustomerDto>(customer);
		}

		// POST /api/customers/
		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Cannot add customer");
			}

			var customer = _mapper.Map<Customer>(customerDto);
			await _unit.Customers.Add(customer);
			await _unit.Complete();
			customerDto.Id = customer.Id;

			return customerDto;
		}

		// PUT /api/customers/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateCustomer(int id, CustomerDto customerDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Cannot update customer with the given ID");
			}

			var customerInDb = await _unit.Customers.Get(customerDto.Id);
			if (customerInDb == null)
			{
				return BadRequest("Cannot update customer with the given ID");
			}

			_unit.Customers.Update(_mapper.Map(customerDto, customerInDb));
			await _unit.Complete();
			return Ok();
		}

		// DELETE /api/customers
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteCusomer(int id)
		{
			var customerInDb = await _unit.Customers.Get(id);

			if (customerInDb == null)
			{
				return BadRequest("Cannot delete customer with the given ID");
			}

			_unit.Customers.Delete(customerInDb);
			await _unit.Complete();
			return Ok();
		}

		private readonly IUnitOfWork _unit;
		private readonly IMapper _mapper;
	}
}
