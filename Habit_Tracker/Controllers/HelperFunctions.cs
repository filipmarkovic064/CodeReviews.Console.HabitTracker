using Habit_Tracker.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Tracker.Controllers
{
    internal static class HelperFunctions
    {
        internal static void SeedData()
        {
            Dictionary<string, string> Habits = new();
            Habits.Add("Working Out", "Hours");
            Habits.Add("Drinking Water", "Glasses");
            Habits.Add("Reading a book", "Pages");
            Habits.Add("Practice coding", "Hours");
            Habits.Add("Play chess", "Games Played");
            Habits.Add("Run", "km");

            Random rand = new();
            int recordsAdded = 100;
            for (int i = 0; i < Habits.Count; i++)
            {
                string HabitName = Habits.ElementAt(i).Key;
                string QuantityName = Habits.ElementAt(i).Value;
                HabitsController.InsertHabit(HabitName, QuantityName);
            }

            for (int i = 0; i < recordsAdded; i++)
            {
                int habitId = rand.Next(1,7);

                string day = (rand.Next(1, 32)).ToString("D2");
                string month = "12"; //(rand.Next(1, 13)).ToString("D2");
                string year = "2025";
                string Quantity = (rand.Next(1, 11)).ToString();
                string DateString = $"{day}-{month}-{year}";
                DateTime Date = DateTime.ParseExact(DateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                RecordsController.InsertRecord(habitId, Quantity, DateString);

            }
            Console.WriteLine("Database seeded with random entries, returning to Main Menu");
         }

        internal static Habit ChooseHabit()
        {
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            var ShowHabits = Connection.CreateCommand();
            ShowHabits.CommandText = "SELECT * FROM Habits";
            var Reader = ShowHabits.ExecuteReader();
            List<Habit> AllHabits = new();

            if (!Reader.HasRows)
            {
                Console.WriteLine("No Habits Found! Returning to Main Menu");
                Console.ReadKey();
                UserInterface.MainMenu();

            }
            while (Reader.Read())
            {
                Habit habit = new();
                habit.HabitId = Reader.GetInt32(0);
                habit.HabitName = Reader.GetString(1);
                habit.QuantityType = Reader.GetString(2);
                AllHabits.Add(habit);
            }
            Console.Clear();
            foreach (var habit in AllHabits)
            {
                Console.WriteLine($"Habit#{habit.HabitId}: {habit.HabitName}, QuantityType: {habit.QuantityType}");
            }
            int UserInput;

            while (true)
            {
                Console.WriteLine("Choose the Habit ID you are interested in");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out UserInput))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input, please input a valid integer!");
                }
            }
            for (int i = 0; i < AllHabits.Count; i++)
            {
                if (AllHabits.ElementAt(i).HabitId == UserInput)
                {
                    return AllHabits.ElementAt(i);
                }
            }
            Console.WriteLine($"Habit with ID:{UserInput} not found. Returning to Main Menu");
            Console.ReadKey();
            UserInterface.MainMenu();
            return null;
        }

        internal static string GetUserDate()
        {
            Console.WriteLine("\n\tPlease input a valid date (Format: dd-mm-yyyy)");
            Console.WriteLine("\tAlternatively type 0 to cancel;");
            string? Date = Console.ReadLine();
            if (Date == "0") UserInterface.MainMenu();
            while (!DateTime.TryParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                Console.WriteLine("\n\tInvalid Date Format, please input a valid date (Format: dd-mm-yyyy)");
                Console.WriteLine("\tAlternatively type 0 to cancel;");
                Date = Console.ReadLine();
                if (Date == "0")
                {
                    Console.WriteLine("'\tInsertion Cancelled");
                    Console.ReadKey();
                    UserInterface.MainMenu();
                }
            }
            return Date;
        }
    }
}
