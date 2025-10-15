using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SClarkC971PA.Models
{
    public abstract class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get { return _assessmentId; } set { _assessmentId = value; } }
        private int _assessmentId;

        public int AssociatedCourseId { get { return _associatedCourseId; } set { _associatedCourseId = value; } }
        private int _associatedCourseId;

        public string AssessmentName { get { return _assessmentName; } set {_assessmentName = value; } }
        private string _assessmentName;

        public DateTime AssessmentStartDate { get { return _assessmentStartDate; } set { _assessmentStartDate = value; } }
        private DateTime _assessmentStartDate;

        public DateTime AssessmentEndDate { get { return _assessmentEndDate; } set { _assessmentEndDate= value; } }
        private DateTime _assessmentEndDate;

        public bool AssessmentNotify { get { return _assessmentNotify; } set {_assessmentNotify = value; } }
        private bool _assessmentNotify;

        public string AssessmentStatus { get { return _assessmentStatus; } set {_assessmentStatus = value; } }
        private string _assessmentStatus;
    }
}
