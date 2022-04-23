using System;
using System.Collections.Generic;
using System.Text;

namespace EternityApp.Models
{
    public class City
    {
        public int? CityId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TitleImagePath { get; set; }

        public IEnumerable<string> Images { get; set; }
    }
}
