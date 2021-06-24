using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data
{
    public class Controller
    {
        [Key]
        public string Id { get; set; }
        
        public string Type { get; set; }
        public string User { get; set; }
        public string Settings { get; set; }
    }
}