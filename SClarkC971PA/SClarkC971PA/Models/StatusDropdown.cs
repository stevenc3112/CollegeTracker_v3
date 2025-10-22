using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Models
{
    class StatusDropdown
    {
        [PrimaryKey, AutoIncrement]
        public int StatusIndex { get; set; }
        public string StatusItem { get; set; }
    }
}
