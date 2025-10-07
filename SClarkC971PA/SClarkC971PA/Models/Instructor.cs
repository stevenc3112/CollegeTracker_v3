using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SClarkC971PA.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorId { get; set; }
        public int AssociatedCourseId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
    }
}
