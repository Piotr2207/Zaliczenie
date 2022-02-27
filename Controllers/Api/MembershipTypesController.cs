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
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{

	public class MembershipTypesController : BaseController
	{
		private readonly IUnitOfWork _unit;
		private readonly IMapper _mapper;
		public MembershipTypesController(IUnitOfWork unit, IMapper mapper)
		{
			_unit = unit;
			_mapper = mapper;
		}

		// GET /api/mstypes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetMembershipTypes()
		{
			IEnumerable<MembershipType> msTypes = await _unit.MembershipTypes.Get();

			var msTypeDtos = msTypes
				.ToList()
				.Select(_mapper.Map<MembershipType, MembershipTypeDto>);

			return Ok(msTypeDtos);
		}
	}
}