using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Client.Data;

namespace Client.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Client.Data.ApplicationDbContext _context;

        public DetailsModel(Client.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Client.Data.Controller Controller { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Controller = await _context.Controllers.FirstOrDefaultAsync(m => m.Id == id);

            if (Controller == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
