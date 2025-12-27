using Habit_Tracker.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Habit_Tracker.Controllers
{
    internal class HabitsController
    {
        internal static void InsertHabit()
        {
            Console.WriteLine("\tPlease input a Habit name (string)");
            Console.WriteLine("\tAlternatively type 0 to cancel;");
            string? HabitName = Console.ReadLine();
            if (HabitName == "0")
            {
                Console.WriteLine("'\tInsertion Cancelled");
                Console.ReadKey();
                UserInterface.MainMenu();
            }

            Console.WriteLine("\tPlease input the Quantity type (string)");
            Console.WriteLine("\tAlternatively type 0 to cancel;");
            string? QuantityName = Console.ReadLine();
            if (QuantityName == "0")
            {
                Console.WriteLine("'\tInsertion Cancelled");
                Console.ReadKey();
                UserInterface.MainMenu();
            }

            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            using var AddHabit = Connection.CreateCommand();
            AddHabit.CommandText = $@"INSERT INTO Habits(HabitName, QuantityType)
                                      VALUES($HabitName, $QuantityType)";
            AddHabit.Parameters.AddWithValue("$HabitName", HabitName);
            AddHabit.Parameters.AddWithValue("$QuantityType", QuantityName);

            int RowsInserted = AddHabit.ExecuteNonQuery();
            if (RowsInserted > 0) Console.WriteLine("\tEntry Inserted");
            else Console.WriteLine("\tInsert Failed");
            Connection.Close();
            Console.ReadKey();
            UserInterface.MainMenu();
        }
        internal static void InsertHabit(string HabitName, string QuantityName)
        {
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            using var AddHabit = Connection.CreateCommand();
            AddHabit.CommandText = $@"INSERT INTO Habits(HabitName, QuantityType)
                                      VALUES($HabitName, $QuantityType)";
            AddHabit.Parameters.AddWithValue("$HabitName", HabitName);
            AddHabit.Parameters.AddWithValue("$QuantityType", QuantityName);

            int RowsInserted = AddHabit.ExecuteNonQuery();
            Connection.Close();
        }

        internal static void DeleteHabit()
        {
            Console.Clear();
            Habit habit = HelperFunctions.ChooseHabit();
            using var connection = new SqliteConnection(Program.ConnectionString);
            connection.Open();

            var DeleteHabit = connection.CreateCommand();
            DeleteHabit.CommandText = $@"DELETE FROM Records
                                         WHERE HabitId = '{habit.HabitId}';
                                         DELETE FROM Habits
                                         WHERE HabitId = '{habit.HabitId}';";

            DeleteHabit.ExecuteNonQuery();
            Console.WriteLine("\n\tHabit and records assosiated with it Deleted, click anything to return to main menu");
            Console.ReadKey();
            UserInterface.MainMenu();
        }

        internal static void UpdateHabit()
        {
            Console.Clear();
            Habit habit = HelperFunctions.ChooseHabit();
            Console.WriteLine("\tPress 1 to change HabitName, Press 2 to change QuantityName, Press 0 to cancel ");
            string? Choice = Console.ReadLine();
            using var connection = new SqliteConnection(Program.ConnectionString);
            connection.Open();
            var UpdateValue = connection.CreateCommand();
            string? NewValue;
            switch (Choice)
            {
                case "0":
                    UserInterface.MainMenu();
                    break;
                case "1":
                    Console.WriteLine("Input New HabitName Value: (String)");
                    NewValue = Console.ReadLine();
                    UpdateValue.Parameters.AddWithValue("$NewValue", NewValue);
                    UpdateValue.CommandText = $@"UPDATE Habits
                                         SET HabitName = $NewValue
                                         WHERE HabitId = {habit.HabitId}";
                    break;
                case "2":
                    Console.WriteLine("Input New QuantityName Value: (String)");
                    NewValue = Console.ReadLine();
                    UpdateValue.Parameters.AddWithValue("$NewValue", NewValue);
                    UpdateValue.CommandText = $@"UPDATE Habits
                                         SET QuantityType = $NewValue
                                         WHERE HabitId = {habit.HabitId}";
                    break;
                default:
                    Console.WriteLine("Invalid Input, Try Again");
                    UpdateHabit();
                    break;
            }

            int SuccessCheck = UpdateValue.ExecuteNonQuery();
            if (SuccessCheck > 0)
            {
                Console.WriteLine($"Successfully Updated Habit with HabitId {habit.HabitId}, press anything to go back to main menu");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            else
            {
                Console.WriteLine("Update Failed (HabitId Most likely not found), returning to Main Menu");
                Console.ReadKey();
                UserInterface.MainMenu();
            }
            connection.Close();

        }
    }
}
