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
        public int AssessmentId { get; set; }
        public int AssociatedCourseId { get; set; }
        public string AssessmentName { get; set; }
        //TODO: delete this: public string AssessmentType { get; set; }
        public DateTime AssessmentStartDate { get; set; }
        public DateTime AssessmentEndDate { get; set; }
        public bool AssessmentNotify { get; set; }
        public string AssessmentStatus { get; set; }
    }
}
