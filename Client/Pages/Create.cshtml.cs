using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Client.Data;
using Microsoft.AspNetCore.Identity;



namespace Client.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Client.Data.ApplicationDbContext _context;

        public CreateModel(Client.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Client.Data.Controller Controller { get; set; }

        public static string username = "";

        public string type { get; set; }

        //[BindProperty]
        //public 

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Controller.User = username;

            type = Controller.Type;

            if (Controller.Type == "Датчик Температуры")
                Controller.Type = "temperature";
            else if (Controller.Type == "Датчик Давления")
                Controller.Type = "pressure";
            else if (Controller.Type == "Датчик Движения")
                Controller.Type = "movement";
            else if (Controller.Type == "Датчик Освещения")
                Controller.Type = "lightning";
            else if (Controller.Type == "Датчик Влажнности")
                Controller.Type = "humidity";
            else 
                return RedirectToPage("./AddController");

            if(_context.Controllers.Find(Controller.Id) == null)
                _context.Controllers.Add(Controller);
            
            await _context.SaveChangesAsync();

            return RedirectToPage("./AddController");
        }
    }
}
