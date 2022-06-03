using System.Collections.Generic;

namespace EternityApp.Models
{
    public class Attraction
    {
        public int? AttractionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Reference { get; set; }
        public string TitleImagePath { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
