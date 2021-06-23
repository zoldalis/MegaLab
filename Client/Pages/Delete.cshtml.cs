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
    public class DeleteModel : PageModel
    {
        private readonly Client.Data.ApplicationDbContext _context;

        public DeleteModel(Client.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Controller = await _context.Controllers.FindAsync(id);

            if (Controller != null)
            {
                _context.Controllers.Remove(Controller);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./AddController");
        }
    }
}
