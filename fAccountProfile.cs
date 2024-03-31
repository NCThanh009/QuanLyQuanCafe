using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private DTO.Account loginAcc;

        public DTO.Account LoginAcc
        {
            get { return loginAcc; }
            set { loginAcc = value; }
        }

        public fAccountProfile(DTO.Account acc)
        {
            LoginAcc = acc;
            InitializeComponent();
            ChangeAccout(loginAcc);
        }
        public void ChangeAccout(DTO.Account acc)
        {
            txbUsername.Text = acc.Username;
            txbPassword.Text = acc.Password;
            txbDisplayname.Text = acc.Displayname;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void UpdateAccount()
        {
            string displayname = txbDisplayname.Text;
            string username = txbUsername.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassword.Text;
            string reEnterpassword = txbReEnterPassword.Text;
            if (!newPassword.Equals(reEnterpassword))
            {
                MessageBox.Show("Nhập lại mật khẩu không đúng");
            }
            else
            {
                if (AccountDAO.Instance.UpadateAccount(username, displayname, password, newPassword))
                {
                    MessageBox.Show("Cập nhật thành công");  
                    if(updateAcc != null)
                    {
                        updateAcc(this,new AccountEvent(AccountDAO.Instance.GetAccountByUsername(username)));
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thông tin nhập vào không chính xác");
                }
            }
        }
        private event EventHandler<AccountEvent> updateAcc;
        public event EventHandler<AccountEvent> UpdateAcc
        {
            add { updateAcc += value; } 
            remove
            {
                updateAcc -= value;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
    }
    public class AccountEvent: EventArgs
    {
        private DTO.Account acc;

        public Account Acc { get => acc; set => acc = value; }
        public AccountEvent(Account acc)
        {
            this.acc = acc;
        }
    }
}
