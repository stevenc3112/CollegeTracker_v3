using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SClarkC971PA.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get { return _courseId; } set { _courseId= value; } }
        private int _courseId;

        public int AssociatedTermId { get { return _associatedTermId; } set { _associatedTermId= value; } }
        private int _associatedTermId;

        public string CourseName { get { return _courseName; } set { _courseName = value; } }
        private string _courseName;

        public DateTime CourseStartDate { get { return _courseStartDate; } set { _courseStartDate = value; } }
        private DateTime _courseStartDate;

        public DateTime CourseEndDate { get { return _courseEndDate; } set { _courseEndDate= value; } }
        private DateTime _courseEndDate;

        public string CourseStatus { get { return _courseStatus; } set { _courseStatus = value; } }
        private string _courseStatus;

        public bool CourseNotify { get { return _courseNotify; } set { _courseNotify= value; } }
        private bool _courseNotify;
    }
}
