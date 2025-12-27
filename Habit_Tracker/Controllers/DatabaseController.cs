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
    internal class DatabaseController
    {
        internal static void CreateTable()
        {
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            var CreateRecords = Connection.CreateCommand();
            CreateRecords.CommandText = @"CREATE TABLE IF NOT EXISTS Habits(
                                                                   HabitId INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                   HabitName TEXT,
                                                                   QuantityType TEXT);";
            CreateRecords.ExecuteNonQuery();

            var CreateTable = Connection.CreateCommand();
            CreateTable.CommandText = @"CREATE TABLE IF NOT EXISTS Records(
                                                                   RecordId INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                   HabitId INTEGER,
                                                                   Quantity INTEGER,
                                                                   Date TEXT,
                                                                   FOREIGN KEY (HabitId) REFERENCES Habits(HabitId))";
            CreateTable.ExecuteNonQuery();
            Connection.Close();
            //First solution was to run it every time the table is created by checking if the table is empty/non existant but i realised that this is not a good solution
            //So the solution i came up with is to create a .txt file that marks the first time the program is executed. Not sure if there was a smarter solution but i couldnt think of one
            string habitDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string firstRunFilePath = Path.Combine(habitDirectory, "FirstRun.txt");
            if (!File.Exists(firstRunFilePath))
            {
                Console.WriteLine("First run detected, seeding data");
                HelperFunctions.SeedData();
                using (StreamWriter file = new StreamWriter(firstRunFilePath))
                {
                    file.WriteLine("This file indicates that the program has already run.");
                }
            }
        }

        internal static void DeleteAll()
        {
            Console.WriteLine("Type y to confirm that you want to delete everything");
            var confirm = Console.ReadLine();
            if (confirm == "y")
            {
                var connection = new SqliteConnection(Program.ConnectionString);
                connection.Open();
                var DeleteAll = connection.CreateCommand();
                DeleteAll.CommandText = @"DROP TABLE IF EXISTS Records;            
                                          DROP TABLE IF EXISTS Habits;";
                DeleteAll.ExecuteNonQuery();
                Console.WriteLine("All habits and their records have been deleted, returning to Main Menu");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            Console.WriteLine("Deleting of everything cancelled, returning to main menu");
            Console.ReadKey();
            UserInterface.MainMenu();
        }

        internal static void ViewRecords()
        {
            Console.Clear();
            Habit ChosenHabit = HelperFunctions.ChooseHabit();
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();


            var PrintTable = Connection.CreateCommand();
            Console.Clear();
            Console.WriteLine("Type 1 to see all records for the chosen habit \nType 2 to see records for the chosen habit this week\nType 0 to cancel");
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    PrintTable.CommandText = $@"SELECT * FROM Records WHERE HabitId={ChosenHabit.HabitId}";
                    break;
                case "2":
                    string WeekAgo = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
                    string TimeNow = DateTime.Now.ToString("dd-MM-yyyy");
                    PrintTable.CommandText = $@"SELECT * FROM Records WHERE HabitId = {ChosenHabit.HabitId} AND Date >= '{WeekAgo}' AND Date < '{TimeNow}' ORDER BY DATE ASC";
                    break;
                case "0":
                    UserInterface.MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Input, try again.");
                    Console.ReadKey();
                    ViewRecords();
                    break;
            }

            try
            {
                var tableList = new List<Record>(); 
                using var reader = PrintTable.ExecuteReader();
                if (!reader.HasRows) 
                {
                    Console.WriteLine("\tHabit Tracker is empty, returning to Main Menu");
                    Console.ReadKey();
                    UserInterface.MainMenu();
                }
                
                else
                {
                    while (reader.Read())
                    {
                        string DateString = reader.GetString(3);
                        Record record = new()
                        {
                            RecordId = reader.GetInt32(0),
                            HabitId = reader.GetInt32(1),
                            Quantity = reader.GetInt32(2),
                            Date = DateTime.ParseExact(DateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        };
                        tableList.Add(record);
                    }
                }
                Console.Clear();
                foreach (Record record in tableList)
                {
                    Console.WriteLine($"Habit{ChosenHabit.HabitId} {ChosenHabit.HabitName}: RecordID{record.RecordId} -> {record.Quantity} {ChosenHabit.QuantityType}, Date: {record.Date.ToString("dd/MM/yyyy")}");
                }
            }
            catch (SqliteException)
            {
                Console.WriteLine($"Habit Not Found, returning to Main Menu");
                UserInterface.MainMenu();
            }
            Connection.Close();
        }
    }
}
