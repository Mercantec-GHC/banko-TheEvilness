using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banko.Models
{
	public class BankoPlate
	{
		public BankoPlate(int id, Row[] rows)
		{
			Id = id;
			Rows = rows;
		}
		public int Id { get; set; }
		public Row[] Rows { get; set; }
	}
}
