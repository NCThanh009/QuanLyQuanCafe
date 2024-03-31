using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        private string username;
        private string password;
        private string displayname;
        private int type;

        public Account(string username, string displayname, int type, string password = null)
        {
            this.username = username;
            this.password = password;
            this.displayname = displayname;
            this.type = type;
        }
        public Account(DataRow row)
        {
            this.username = row["Username"].ToString();
            this.password = row["PassWord"].ToString();
            this.displayname = row["DisplayName"].ToString();
            this.type = (int)row["Type"];
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Displayname { get => displayname; set => displayname = value; }
        public int Type { get => type; set => type = value; }
    }
}
