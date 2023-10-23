using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Models
{
	public class Row
	{
		public Row(RowValue[] values)
		{
			Values = values;
		}
		public RowValue[] Values { get; set; }
	}
}
