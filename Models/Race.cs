using System.Collections.Generic;

namespace wikiracer.Models
{
    public class Race
    {
        public string Start { get; set; }

        public string End { get; set; }

        public IEnumerable<string> Path { get; set; }
    }
}