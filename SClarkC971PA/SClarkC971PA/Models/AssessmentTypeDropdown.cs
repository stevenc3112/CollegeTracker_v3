using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Models
{
    class AssessmentTypeDropdown
    {

        [PrimaryKey, AutoIncrement]
        public int TypeIndex { get; set; }
        public string TypeItem { get; set; }
    }
}
