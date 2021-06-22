using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client.Data;

namespace Client.Pages
{
    public class EditModel : PageModel
    {
        private readonly Client.Data.ApplicationDbContext _context;

        public EditModel(Client.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Controller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControllerExists(Controller.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./AddController");
        }

        private bool ControllerExists(string id)
        {
            return _context.Controllers.Any(e => e.Id == id);
        }
    }
}
