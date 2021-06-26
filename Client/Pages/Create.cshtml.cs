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


        //temperature
        [BindProperty]
        public string start_temp { get; set; }

        [BindProperty]
        public string start_cool { get; set; }

        [BindProperty]
        public string end_cool { get; set; }

        [BindProperty]
        public string start_heat { get; set; }

        [BindProperty]
        public string end_heat { get; set; }

        [BindProperty]
        public string max_temp { get; set; }

        [BindProperty]
        public string min_temp { get; set; }

        [BindProperty]
        public string interval_temp { get; set; }

        //pressure

        [BindProperty]
        public string interval_press { get; set; }

        //movement

        [BindProperty]
        public string sensetivity { get; set; }

        //lightning

        [BindProperty]
        public string interval_light { get; set; }

        [BindProperty]
        public string start_light_day { get; set; }

        [BindProperty]
        public string end_light_day { get; set; }

        //humidity

        [BindProperty]
        public string start_humi { get; set; }

        [BindProperty]
        public string start_pouring { get; set; }

        [BindProperty]
        public string end_pouring { get; set; }

        [BindProperty]
        public string interval_humi { get; set; }



        public static string username = "";

        public string type { get; set; }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Controller.User = username;

            type = Controller.Type;

            Controller.Values = new string[0];

            if (Controller.Type == "Датчик Температуры")
            {
                Controller.Type = "temperature";
                Controller.Settings = start_temp + '|' + start_cool + '|' + end_cool + '|' + start_heat + '|' + end_heat + '|' + max_temp + '|' + min_temp + '|' + interval_temp;
            }
            else if (Controller.Type == "Датчик Давления")
            {
                Controller.Type = "pressure";
                Controller.Settings = interval_press;

            }
            else if (Controller.Type == "Датчик Движения")
            {
                Controller.Type = "movement";
                Controller.Settings = sensetivity;
            }
            else if (Controller.Type == "Датчик Освещения")
            {
                Controller.Type = "lightning";
                Controller.Settings = interval_light + '|' + start_light_day + '|' + end_light_day;
            }
            else if (Controller.Type == "Датчик Влажности")
            {
                Controller.Type = "humidity";
                Controller.Settings = start_humi + '|' + start_pouring + '|' + end_pouring + '|' + interval_humi;
            }
            else
                return RedirectToPage("./AddController");

            if (_context.Controllers.Find(Controller.Id) == null)
                _context.Controllers.Add(Controller);

            await _context.SaveChangesAsync();

            return RedirectToPage("./AddController");
        }
    }
}
