using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Models
{
    public class Customer
    {
        public int    Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IncludedItems { get; set; }
        public string LogoUrl { get; set; }
        public int CustomerPrice { get; set; }
        public string LunchPrice { get; set; }
        public string OpenHours { get; set; }

        //public Customer(int id,
        //                string name,
        //                string description,
        //                string includedItems,
        //                string logoUrl,
        //                int customerPrice,
        //                string lunchPrice,
        //                string openHours)
        //{
        //    Id = id;
        //    Name = name;
        //    Description = description;
        //    IncludedItems = includedItems;
        //    LogoUrl = logoUrl;
        //    CustomerPrice = customerPrice;
        //    LunchPrice = lunchPrice;
        //    OpenHours = openHours;
        //}
    }
}
