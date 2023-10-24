using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banko.Shared.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Banko.Shared
{
	public static class PlateGenerator
	{
		/// <summary>
		/// This generates plates to be processed in the game.
		/// </summary>
		public static List<BankoPlate> GeneratePlates(int plateCount = 1)
		{
			List<BankoPlate> plates = new List<BankoPlate>();
			for (int k = 0; k < plateCount; k++)
			{
				List<int?[]> columnData = GenerateColumnData();

				List<RowValue[]> rowValues = new List<RowValue[]>();
				for (int i = 0; i < 3; i++)
				{
					rowValues.Add(new RowValue[9]);
				}
				for (int i = 0; i < columnData.Count; i++)
				{
					for (int j = 0; j < columnData[i].Length; j++)
					{
						rowValues[j][i] = new RowValue(columnData[i][j]);
					}
				}
				Row[] rows = new Row[3];
				for (int i = 0; i < 3; i++)
				{
					rows[i] = new Row(i + 1, rowValues[i]);
				}
				plates.Add(new BankoPlate((k + 1).ToString(), rows));
			}
			CleanupRows(plates);
			return plates;
		}
		/// <summary>
		/// This generates data for each Column for each plate and returns it.
		/// </summary>
		/// <returns></returns>
		private static List<int?[]> GenerateColumnData()
		{
			List<int?[]> columnData = new List<int?[]>();
			Random random = new Random();
			int randomMin = 1;
			int randomMax = 9;
			for (int i = 0; i < 9; i++)
			{
				List<int?> numbersAdded = new List<int?>();
				for (int j = 0; j < 3; j++)
				{
					while (true)
					{
						int result = random.Next(randomMin, randomMax);
						if (!numbersAdded.Contains(result))
						{
							numbersAdded.Add(result);
							break;
						}
					}
				}
				if (i == 0)
				{
					//If this is the first run, we reset the min and max variables, to make it easier to increment for the 8 other columns.
					randomMin = 0;
					randomMax = 9;
				}
				if (i == 7)
				{
					//To make sure that 90 is possible in the last column.
					randomMax = 80;
				}
				randomMin += 10;
				randomMax += 10;
				numbersAdded.Sort();
				int?[] array = numbersAdded.ToArray();
				columnData.Add(array);
			}
			return columnData;
		}
		/// <summary>
		/// This removes, at random, numbers from each row until each row only has 5 numbers, as per the rules.
		/// </summary>
		private static void CleanupRows(List<BankoPlate> plates)
		{
			foreach (BankoPlate plate in plates)
			{
				foreach (Row row in plate.Rows)
				{
					Random random = new Random();
					int numbers = 9;
					while (numbers > 5)
					{
						int result = random.Next(0, 9);
						row.Values[result].Number = null;
						numbers = row.Values.Count(x => x.Number != null);
					}
				}
			}
		}

		public static List<BankoPlate> ImportByCSV()
		{
			List<BankoPlate> plates = new List<BankoPlate>();
			CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Csv");
			foreach (string file in files)
			{
				List<CsvRow> csvRows = new List<CsvRow>();
				using (var reader = new StreamReader(file))
				using (var csv = new CsvReader(reader, csvConfiguration))
				{
					csvRows = csv.GetRecords<CsvRow>().ToList();
				}
				plates.Add(GeneratePlateFromFile(Path.GetFileNameWithoutExtension(file), csvRows));
			}
			return plates;
		}

		private static BankoPlate GeneratePlateFromFile(string filename, List<CsvRow> csvRows)
		{
			Row[] rows = new Row[3];
			for (int i = 0; i < csvRows.Count; i++)
			{
				RowValue[] rowValues = new RowValue[9];
				rowValues[0] = new RowValue(csvRows[i].Column1);
				rowValues[1] = new RowValue(csvRows[i].Column2);
				rowValues[2] = new RowValue(csvRows[i].Column3);
				rowValues[3] = new RowValue(csvRows[i].Column4);
				rowValues[4] = new RowValue(csvRows[i].Column5);
				rowValues[5] = new RowValue(csvRows[i].Column6);
				rowValues[6] = new RowValue(csvRows[i].Column7);
				rowValues[7] = new RowValue(csvRows[i].Column8);
				rowValues[8] = new RowValue(csvRows[i].Column9);
				rows[i] = new Row(i, rowValues);
			}
			return new BankoPlate(filename, rows);
		}
	}
}
