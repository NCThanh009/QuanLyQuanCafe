using QuanLyQuanCafe.DAO;
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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thực sự muốn thoát?","Thông báo!", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
        bool Login(String user, String pass)
        {
            return AccountDAO.Instance.Login(user, pass);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            String pass = txbPassword.Text;
            String user = txbUsername.Text;
            if (Login(user,pass))
            {
                DTO.Account loginAcc = AccountDAO.Instance.GetAccountByUsername(user); 
                fTableManager fTM = new fTableManager(loginAcc);
                this.Hide();
                fTM.ShowDialog();//đưa fTM lên top 
                this.Show();
                if (cbSaveUser.Checked)
                {
                    txbPassword.Text = "";
                }
                else
                {
                    txbUsername.Text = "";
                    txbPassword.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu bạn đã nhập không chính xác.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cbSaveUser_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
