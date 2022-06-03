using System.Collections.Generic;

namespace EternityApp.Models
{
    public class City
    {
        public int? CityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<int?> References { get; set; }
        public string TitleImagePath { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
