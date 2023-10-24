using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Shared.Models
{
    public class BankoPlate
    {
        public BankoPlate(string id, Row[] rows)
        {
            Id = id;
            Rows = rows;
        }
        public string Id { get; set; }
        public Row[] Rows { get; set; }
    }
}
