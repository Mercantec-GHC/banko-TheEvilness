using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Models
{
	public class RowValue
	{
		public RowValue(int? number, bool marked = false)
		{
			Number = number;
			Marked = marked;
		}
		public int? Number { get; set; }
		public bool Marked { get; set; }
	}
}
