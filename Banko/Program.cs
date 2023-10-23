using Banko.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;

namespace Banko
{
	internal class Program
	{
		public static List<BankoPlate> Plates { get; set; }
		static void Main(string[] args)
		{
			Plates = new List<BankoPlate>();
			GeneratePlates();
			//GenerateTestData();
			DisplayUI();
		}

		private static void DisplayUI()
		{
			foreach (BankoPlate plate in Plates)
			{
				Console.Write($" Plate ID: {plate.Id}\n");
				Console.Write("/=================================================\\\n");
				for (int i = 0; i < plate.Rows.Length; i++)
				{
					for (int j = 0; j < plate.Rows[i].Values.Length; j++)
					{
						RowValue rowValue = plate.Rows[i].Values[j];
						string input = "  ";
						if (rowValue.Number != null)
						{
							input = rowValue.Number.ToString();
						}
						if (input.Length == 1)
						{
							input = " " + input;
						}

						if (j == 0)
						{
							Console.Write("| ");
						}
						if (rowValue.Marked != null && rowValue.Marked)
						{
							Console.Write("XX | ");
						}
						else
						{
							Console.Write($"{input} | ");
						}
						if (i == plate.Rows.Length)
						{
							Console.Write("|\n");
						}
					}
					if (i < plate.Rows.Length - 1)
					{
						Console.Write("\n ------------------------------------------------- \n");
					}
				}
				Console.Write("\n\\=================================================/\n\n ");
			}
		}

		private static void GeneratePlates()
		{
			for (int plateCount = 0; plateCount < 5; plateCount++)
			{
				List<int?[]> columnData = GenerateColumnData();

				List<RowValue[]> rowValues = new List<RowValue[]>();
				for (int i = 0; i < 3; i++)
				{
					rowValues.Add(new RowValue[10]);
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
					rows[i] = new Row(rowValues[i]);
				}
				Plates.Add(new BankoPlate(plateCount + 1, rows));
			}
		}
		private static void GenerateTestData()
		{
			Random random = new Random();
			for (int plateCount = 0; plateCount < 5; plateCount++)
			{
				Row[] rows = new Row[3];
				for (int i = 0; i < 3; i++)
				{
					RowValue[] rowValues = new RowValue[10];
					for (int j = 0; j < 10; j++)
					{
						int? value = null;
						bool marked = false;
						if (random.Next(1, 10) > 5)
						{
							value = random.Next(1, 99);
							if (random.Next(1, 20) > 15)
							{
								marked = true;
							}
						}
						rowValues[j] = new RowValue(value, marked);
					}
					Row row = new Row(rowValues);
					rows[i] = row;
				}
				BankoPlate plate = new BankoPlate(plateCount + 1, rows);
				Plates.Add(plate);
			}
		}

		private static List<int?[]> GenerateColumnData()
		{
			List<int?[]> columnData = new List<int?[]>();
			Random random = new Random();
			int randomMin = 1;
			int randomMax = 9;
			for (int i = 0; i < 10; i++)
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
				randomMin += 10;
				randomMax += 10;
				numbersAdded.Sort();
				int?[] array = numbersAdded.ToArray();
				columnData.Add(array);
			}
			return columnData;
		}

		private static void CleanupRows()
		{

		}
	}
}