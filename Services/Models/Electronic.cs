using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Electronic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Specification Data { get; set; }
    }
}
