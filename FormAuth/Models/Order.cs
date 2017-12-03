using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormAuth.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int PriceID { get; set; }

        public string UserName { get; set; }

        public string date { get; set; }
    }
}