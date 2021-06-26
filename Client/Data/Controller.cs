using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data
{
    public class Controller
    {
        private static readonly char delimiter = ';';

        private string _id;
        [Key]
        public string Id { get { return _id; } set { _id = value; } }
        
        public string Type { get; set; }
        public string User { get; set; }
        public string Settings { get; set; }

        public string _values { get; set; }

        [NotMapped]
        public string[] Values
        {
            get { return _values.Split(delimiter); }
            set
            {
                _values = string.Join($"{delimiter}", value);
            }
        }

    }


}