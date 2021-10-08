﻿using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkerController : ControllerBase
    {
        private readonly APIContext _context;

        public MarkerController(APIContext context)
        {
            _context = context;
        }
        //report about problem
        [HttpPost]
        public async Task<IActionResult> SetDanger(BLL.Models.Danger Danger)
        {
            try
            {
                _context.Dangers.Add(Danger);
                var result = await _context.SaveChangesAsync();

                return Ok(result == 0 ? "Danger was not added" : "Danger was added");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
