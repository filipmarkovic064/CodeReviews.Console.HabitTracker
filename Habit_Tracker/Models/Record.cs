using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Tracker.Models
{
    internal class Record
    {
        public int RecordId { get; set; }
        public int HabitId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
