using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SClarkC971PA.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int NoteId { get; set; }
        public int AssociatedCourseId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteBody { get; set; }

    }
}
