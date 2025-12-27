using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Tracker.Models
{
    internal class Habit
    {
        public int HabitId { get; set; }
        public string HabitName { get; set; }
        public string QuantityType { get; set; }
    }
}
