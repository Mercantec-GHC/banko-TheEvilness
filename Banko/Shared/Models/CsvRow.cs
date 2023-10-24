using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Shared.Models
{
	public class CsvRow
	{
		[Index(0)]
		public int? Column1 { get; set; }
		[Index(1)]
		public int? Column2 { get; set; }
		[Index(2)]
		public int? Column3 { get; set; }
		[Index(3)]
		public int? Column4 { get; set; }
		[Index(4)]
		public int? Column5 { get; set; }
		[Index(5)]
		public int? Column6 { get; set; }
		[Index(6)]
		public int? Column7 { get; set; }
		[Index(7)]
		public int? Column8 { get; set; }
		[Index(8)]
		public int? Column9 { get; set; }
	}
}
