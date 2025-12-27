using Habit_Tracker.Controllers;
using Habit_Tracker.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Habit_Tracker
{
    internal class Program
    {
        public static string ConnectionString = "Data Source=HabitTracker.db";
        static void Main(string[] args)
        {

            UserInterface.MainMenu();
        }
    }
}