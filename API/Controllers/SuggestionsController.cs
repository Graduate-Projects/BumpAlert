using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using BLL.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly APIContext _context;

        public SuggestionsController(APIContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Suggestion>> PostSuggestion(Suggestion suggestion)
        {
            _context.Suggestions.Add(suggestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuggestion", new { id = suggestion.ID }, suggestion);
        }
    }
}
