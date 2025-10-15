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
        public int NoteId { get{return _noteId; } set{_noteId = value; } }
        private int _noteId;

        public int AssociatedCourseId { get{return _associatedCourseId; } set{_associatedCourseId = value; } }
        private int _associatedCourseId;

        public string NoteTitle { get{return _noteTitle; } set{_noteTitle = value; } }
        private string _noteTitle;

        public string NoteBody { get{return _noteBody; } set{_noteBody = value; } }
        private string _noteBody;

    }
}
