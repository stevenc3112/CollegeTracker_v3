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
        public int InstructorId { get{return _instructorId; } set{ _instructorId = value; } }
        private int _instructorId;

        public int AssociatedCourseId { get { return _associatedCourseId; } set { _associatedCourseId = value; } }
        private int _associatedCourseId;

        public string InstructorName { get { return _instructorName; } set { _instructorName= value; } }
        private string _instructorName;

        public string InstructorPhone { get { return _instructorPhone; } set { _instructorPhone = value; } }
        private string _instructorPhone;

        public string InstructorEmail { get { return _instructorEmail; } set { _instructorEmail= value; } }
        private string _instructorEmail;
    }
}
