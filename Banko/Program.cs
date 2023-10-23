using Banko.Models;
using System.Diagnostics;
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
			StartLoop();
		}

		private static void StartLoop()
		{
			List<int> SelectedNumbers = new List<int>();
			while (true)
			{
				DisplayPlateUI();
				Console.Write("Currently selected numbers: ");
				for (int i = 0; i < SelectedNumbers.Count; i++)
				{
					Console.Write($"{SelectedNumbers[i]},");
				}
				Console.Write("\nSelect number: ");
				string? input = Console.ReadLine();
				bool validInput = int.TryParse(input, out int result);
				if (validInput)
				{
					SelectedNumbers.Add(result);
					ProcessInput(result);
				}
				else
				{
					Console.WriteLine("Not a valid input.");
					Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// This displays the UI for each Plate.
		/// </summary>
		private static void DisplayPlateUI()
		{
			Console.Clear();
			foreach (BankoPlate plate in Plates)
			{
				Console.Write($" Plate ID: {plate.Id}\n");
				Console.Write("/============================================\\\n");
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
						Console.Write("\n -------------------------------------------- \n");
					}
				}
				Console.Write("\n\\============================================/\n\n");
			}
		}
		/// <summary>
		/// This generates plates to be processed in the game.
		/// </summary>
		private static void GeneratePlates()
		{
			for (int plateCount = 0; plateCount < 5; plateCount++)
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
				Plates.Add(new BankoPlate(plateCount + 1, rows));
			}
			CleanupRows();
		}
		/// <summary>
		/// This generates data for each Column for each plate and populates them.
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
		private static void CleanupRows()
		{
			foreach (BankoPlate plate in Plates)
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
		/// <summary>
		/// This processes the input and marks numbers based on it.
		/// Also triggers different win states after numbers are marked.
		/// </summary>
		private static void ProcessInput(int number)
		{
			bool somethingWon = false;
			foreach (BankoPlate plate in Plates)
			{
				foreach (Row row in plate.Rows)
				{
					if (row.Won)
					{
						continue;
					}
					RowValue rowValue = row.Values.FirstOrDefault(x => x.Number == number);
					if (rowValue != null)
					{
						rowValue.Marked = true;
						if (row.Values.Count(x => x.Marked == true) == 5)
						{
							row.Won = true;
							Console.WriteLine($"You have won a row on Plate {plate.Id}, Row {row.Id} !");
							somethingWon = true;
							if (plate.Rows.Count(x => x.Won) == 3)
							{
								Console.WriteLine($"You have won on Plate {plate.Id} !");
							}
						}
					}
				}
			}
			if (somethingWon)
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadKey();
			}
		}
	}
}