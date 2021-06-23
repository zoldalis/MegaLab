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
    public class AddControllerModel : PageModel
    {
        private readonly Client.Data.ApplicationDbContext _context;

        public AddControllerModel(Client.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Client.Data.Controller> Controller { get;set; }

        public async Task OnGetAsync()
        {
            Controller = await _context.Controllers.ToListAsync();
        }
    }
}
