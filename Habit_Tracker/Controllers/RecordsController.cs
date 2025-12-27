using Habit_Tracker.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Tracker.Controllers
{
    internal class RecordsController
    {
        internal static void InsertRecord()
        {
            Habit habit = HelperFunctions.ChooseHabit();
            string Date = HelperFunctions.GetUserDate();

            Console.WriteLine("\tPlease input the quantity");
            Console.WriteLine("\tAlternatively type 0 to cancel;");
            string? Quantity = Console.ReadLine();
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            using var AddRecord = Connection.CreateCommand();
            AddRecord.CommandText = $@"INSERT INTO Records(HabitId, Quantity, Date)
                                      VALUES({habit.HabitId},$Quantity, $Date)";
            AddRecord.Parameters.AddWithValue("$Quantity", Quantity);
            AddRecord.Parameters.AddWithValue("$Date", Date);

            int RowsInserted = AddRecord.ExecuteNonQuery();
            if (RowsInserted > 0) Console.WriteLine("\tInserted! Returning to Main Menu");
            else Console.WriteLine("\tInsert Failed");
            Connection.Close();
            Console.ReadKey();
            UserInterface.MainMenu();
        }

        internal static void InsertRecord(int habitId, string Quantity, string Date)
        {
            using var Connection = new SqliteConnection(Program.ConnectionString);
            Connection.Open();
            using var AddHabit = Connection.CreateCommand();
            AddHabit.CommandText = $@"INSERT INTO Records(HabitId, Quantity, Date)
                                      VALUES($HabitId,$Quantity, $Date)";
            AddHabit.Parameters.AddWithValue("$Quantity", Quantity);
            AddHabit.Parameters.AddWithValue("$Date", Date);
            AddHabit.Parameters.AddWithValue("$HabitId", habitId);

            int RowsInserted = AddHabit.ExecuteNonQuery();
            Connection.Close();
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            DatabaseController.ViewRecords();
            Console.WriteLine("Type the ID of a record you want to delete");
            var DeleteId = Console.ReadLine();
            try
            {
                using var connection = new SqliteConnection(Program.ConnectionString);
                connection.Open();

                var DeleteRecord = connection.CreateCommand();
                DeleteRecord.CommandText = $@"DELETE FROM Records
                                          WHERE RecordId = $DeleteId;";
                DeleteRecord.Parameters.AddWithValue("$DeleteId", DeleteId);
                DeleteRecord.ExecuteNonQuery();
                Console.WriteLine("\n\tRecord Deleted, click anything to return to main menu");
            }
            catch (SqliteException)
            {
                Console.WriteLine("RecordId not found, returning to Main Menu");
            }
            Console.ReadKey();
            UserInterface.MainMenu();
        }

        internal static void UpdateRecord()
        {
            Console.Clear();
            DatabaseController.ViewRecords();
            Console.WriteLine("\t Which Record would you like to update? (Choose RecordId) - Choose 0 to go back to Main Menu");
            string? recordId = Console.ReadLine();
            if (recordId == "0") UserInterface.MainMenu();

            Console.WriteLine("\tPress 1 to change associated Habit, Press 2 to change Quantity, Press 3 to change Date, Press 0 to cancel ");
            string? Choice = Console.ReadLine();
            using var connection = new SqliteConnection(Program.ConnectionString);
            connection.Open();
            var UpdateValue = connection.CreateCommand();
            UpdateValue.Parameters.AddWithValue("$RecordId", recordId.Trim());
            string? NewValue;
            switch (Choice)
            {
                case "0":
                    UserInterface.MainMenu();
                    break;
                case "1":
                    Habit habit = HelperFunctions.ChooseHabit();
                    UpdateValue.CommandText = $@"UPDATE Records
                                         SET HabitId = {habit.HabitId}
                                         WHERE RecordId = $RecordId";
                    break;
                case "2":
                    Console.WriteLine("Input New Quantity Value: (INTEGER)");
                    NewValue = Console.ReadLine();
                    UpdateValue.Parameters.AddWithValue("$NewValue", NewValue);
                    UpdateValue.CommandText = $@"UPDATE Records
                                         SET Quantity = $NewValue
                                         WHERE RecordId = $RecordId";
                    break;
                case "3":
                    NewValue = HelperFunctions.GetUserDate();
                    UpdateValue.CommandText = $@"UPDATE Records
                                         SET Date = '{NewValue}'
                                         WHERE RecordId = $RecordId";
                    break;
                default:
                    Console.WriteLine("Invalid Input, Try Again");
                    UpdateRecord();
                    break;
            }

            int SuccessCheck = UpdateValue.ExecuteNonQuery();
            if (SuccessCheck > 0)
            {
                Console.WriteLine($"Successfully Updated Habit with RecordId {recordId}, press anything to go back to main menu");
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
