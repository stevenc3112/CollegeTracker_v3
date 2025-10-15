using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get { return _userId;} set { _userId = value; } }
        private int _userId;
        public string UserUsername { get { return _userUsername; } set { _userUsername = value; } }
        private string _userUsername;

        public string UserPassword { get { return _userPassword; } set { _userPassword = value; } }
        private string _userPassword;
    }
}
