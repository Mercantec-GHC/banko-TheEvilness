using Banko.Models;
using Banko.Shared;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;

namespace Banko
{
	internal class Program
	{
		static List<BankoPlate> Plates { get; set; }
		static bool OneRowWon { get; set; }
		static bool TwoRowsWon { get; set; }
		static void Main(string[] args)
		{
			Plates = new List<BankoPlate>();
			OneRowWon = false;
			TwoRowsWon = false;
			Plates = PlateGenerator.GeneratePlates(5);
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
							int rowsWon = plate.Rows.Count(x => x.Won);
							somethingWon = true;
							switch (rowsWon)
							{
								case 1:
									Console.WriteLine($"You have won a row on Plate {plate.Id}, Row {row.Id} !");
									break;
								case 2:
									Console.WriteLine($"You have won two rows on Plate {plate.Id} !");
									break;
								case 3:
									Console.WriteLine($"You have won on Plate {plate.Id} !");
									break;
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