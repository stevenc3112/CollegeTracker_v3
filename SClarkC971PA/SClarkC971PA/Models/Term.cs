using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SClarkC971PA.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermId { get {return _termId; } set{ _termId = value;} }
        private int _termId;
        public string TermTitle { get{ return _termTitle;} set{ _termTitle = value; } }
        private string _termTitle;

        public DateTime TermStartDate { get{return _termStartDate; } set{ _termStartDate = value;} }
        private DateTime _termStartDate;

        public DateTime TermEndDate { get { return _termEndDate; } set{_termEndDate = value; } }
        private DateTime _termEndDate;

        public int AssociatedUserId { get { return _associatedUserId; } set{_associatedUserId = value; } }
        private int _associatedUserId;
    }
}
