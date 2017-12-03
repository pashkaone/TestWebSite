using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormAuth.Models
{
    public class Price
    {
        public int id { get; set; }
        
        public string item { get; set; }
        
        public int cash { get; set; }
    }
}