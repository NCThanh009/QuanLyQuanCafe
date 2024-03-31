using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Security.Cryptography;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe.DAO
{
    internal class AccountDAO
    {
        private static AccountDAO instance;//ctrl + r + e
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return AccountDAO.instance;
            }
            private set => instance = value;
        }
        private AccountDAO() { }
        public string MD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public bool Login(string username, string password)
        {
            password= MD5Hash(password);
            //String query = "SELECT *FROM dbo.Account WHERE UserName = '"+username+"' COLLATE Latin1_General_CS_AS AND PassWord = '"+password+"' COLLATE Latin1_General_CS_AS";
            String query = "USP_Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query,new Object[]{username, password});
            return result.Rows.Count > 0;
        }
        public DTO.Account GetAccountByUsername(string username)
        {
            DataTable data= DataProvider.Instance.ExecuteQuery("SELECT * FROM ACCOUNT WHERE UserName = '"+username+"'");
            foreach (DataRow row in data.Rows)
            {
                return new DTO.Account(row);
            }
            return null;
        } 
        public bool UpadateAccount(string user, string displayname, string pass, string newpass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @userName , @displayName , @password , @newPassword ", new object[] {user,displayname,pass,newpass});
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName as N'Tài khoản', DisplayName as N'Tên người dùng', Type as N'Quyền' FROM dbo.Account");
        }

        public bool InsertAccount(string user, string dis, int type)
        {
            string query = null;
            if (type == 1)
            {
                query = string.Format("INSERT dbo.Account (UserName, DisplayName, PassWord, Type)\r\nvalues (N'{0}', N'{1}', CONVERT(nvarchar(32), HASHBYTES('MD5','admin'), 2), {2})", user, dis, type);
            }
            else
            {
                query = string.Format("INSERT dbo.Account (UserName, DisplayName, PassWord, Type)\r\nvalues (N'{0}', N'{1}', CONVERT(nvarchar(32), HASHBYTES('MD5','user'), 2), {2})", user, dis, type);
            }
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string username, string disname, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{0}', type = {1} WHERE UserName = N'{2}' ",disname,type,username );
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string username)
        {
            string query = string.Format("DELETE dbo.Account WHERE UserName = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string username)
        {
            string query = string.Format("UPDATE dbo.Account SET PassWord = CONVERT(nvarchar(32), HASHBYTES('MD5','user'), 2) WHERE UserName = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public DataTable SearchAccountByname(string userName,string displayName)
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName as N'Tài khoản', DisplayName as N'Tên người dùng', PassWord as N'Mật khẩu', Type as N'Quyền' FROM dbo.Account WHERE UserName COLLATE Latin1_General_CI_AI LIKE N'%" + userName+"%' OR DisplayName COLLATE Latin1_General_CI_AI LIKE N'%"+displayName+"%'");
        }


    }

}
