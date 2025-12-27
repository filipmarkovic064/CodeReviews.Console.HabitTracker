using Habit_Tracker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Tracker
{
    internal static class UserInterface
    {
        internal static void MainMenu()
        {
            DatabaseController.CreateTable();
            Console.Clear();
            Console.WriteLine("\n Welcome to your Habit Tracker!");
            Console.WriteLine("\n\t Press 1 to view your Records");
            Console.WriteLine("\t Press 2 to insert a new Habit/Record of a Habit.");
            Console.WriteLine("\t Press 3 to delete things.");
            Console.WriteLine("\t Press 4 to update a Habit/Record.");
            Console.WriteLine("\t Press 0 to exit.");

            string? answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    DatabaseController.ViewRecords();
                    Console.WriteLine("\nPress Anything to return to the Main Menu");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case "2":
                    InsertMenu();
                    break;
                case "3":
                    DeleteMenu();
                    break;
                case "4":
                    UpdateMenu();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\t Invalid Input, try again!");
                    Console.ReadKey();
                    MainMenu();
                    break;
            }
        }

    internal static void InsertMenu()
        {
            Console.Clear();
            Console.WriteLine("\nPress 1 to Insert a new Habit \nPress 2 to insert a record of a Habit \nPress 0 to go to back to Main Menu");
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    HabitsController.InsertHabit();
                    break;
                case "2":
                    RecordsController.InsertRecord();
                    break;
                case "0":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    InsertMenu();
                    break;
            }
        }

        internal static void DeleteMenu()
        {
            Console.Clear();
            Console.WriteLine("\nPress 1 to Delete a Habit \nPress 2 to delete a record of a Habit \nPress 3 to Delete everything \nPress 0 to go to back to Main Menu");
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    HabitsController.DeleteHabit();
                    break;
                case "2":
                    RecordsController.DeleteRecord();
                    break;
                case "3":
                    DatabaseController.DeleteAll();
                    break;
                case "0":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    DeleteMenu();
                    break;
            }
        }

        internal static void UpdateMenu()
        {
            Console.Clear();
            Console.WriteLine("\nPress 1 to Update a Habit \nPress 2 to update a record of a Habit \nPress 0 to go to back to Main Menu");
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    HabitsController.UpdateHabit();
                    break;
                case "2":
                    RecordsController.UpdateRecord();
                    break;
                case "0":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    UpdateMenu();
                    break;
            }
        }
    }
}
