using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Shared.Models
{
    public class Row
    {
        public Row(int id, RowValue[] values)
        {
            Id = id;
            Values = values;
            Won = false;
        }
        public int Id { get; set; }
        public RowValue[] Values { get; set; }
        public bool Won { get; set; }
    }
}
