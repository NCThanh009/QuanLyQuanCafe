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
using System.Windows.Forms.VisualStyles;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private DTO.Account loginAcc;

        public DTO.Account LoginAcc 
        {
            get {return loginAcc;}
            set {loginAcc = value; ChangeAccout(loginAcc.Type);} 
        }

        public fTableManager(DTO.Account loginAcc)
        {
            InitializeComponent();

            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
            this.LoginAcc = loginAcc;

        }
        public void ChangeAccout(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text = loginAcc.Displayname.ToString();
        }
        public void LoadCategory()
        {
            List<Category> listCate = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCate;
            cbCategory.DisplayMember = "Name";

        }
        public void LoadFoodListByCateId(int cateId)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCateId(cateId);
            cbFood.DataSource = listFood;
            cbPrice.DataSource = listFood;
            cbFood.DisplayMember = "Name";
            cbPrice.DisplayMember = "Price";
            cbPrice.Format += (sender, e) =>
            {
                if (e.ListItem is Food priceItem)
                {
                    // Định dạng giá tiền
                    e.Value = priceItem.Price.ToString("N0")+" VND";
                }
            };
        }
        public void LoadTable()
        {
            flpTable.Controls.Clear();
            btnCheckOut.FlatAppearance.MouseOverBackColor = Color.LightGray;
            List<Table> listTable= TableDAO.Instance.LoadTableList();
            foreach (Table item in listTable)
            {
                Button but = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                but.Text= item.Name + Environment.NewLine + Environment.NewLine + item.Status;
                but.Click += Button_Click;
                but.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        but.BackColor= Color.DarkGreen;
                        but.ForeColor= Color.White;
                        //but.Click += (sender, e) => Button_Click(sender, e,Color.Yellow, Color.DarkGreen);
                        break;
                    case "Có người":
                        but.BackColor= Color.Red;
                        but.ForeColor= Color.White;
                        //but.Click += (sender, e) => Button_Click(sender, e, Color.Blue, Color.Red);
                        break;
                    default:break;        
                }
                flpTable.Controls.Add(but);
            }
        }
        public void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        public void ShowBill(int tableId)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo=MenuDAO.Instance.GetListMenuByTable(tableId);
            float totalPrice = 0;
            string totalPriceStr = totalPrice.ToString() + ",000 VND";
            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem listItem = new ListViewItem(item.FoodName.ToString());
                listItem.SubItems.Add(item.Count.ToString());
                listItem.SubItems.Add(item.Price.ToString("N0") + " VND");
                listItem.SubItems.Add(item.TotalPrice.ToString("N0") + " VND");
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(listItem);
            }

            txbTotalPrice.Text = (totalPrice == 0) ? totalPriceStr.ToString() : totalPrice.ToString("N0") + " VND";

        }
        private void Button_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).Id;
            lsvBill.Tag = (sender as Button).Tag;//lưu table vào listview
            txbTenban.Text= ((sender as Button).Tag as Table).Name;
            ShowBill(tableID);
        }
        private void fTableManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn thoát?", "Thông báo!", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f= new fAccountProfile(loginAcc);
            fAdmin fa = new fAdmin();
            f.UpdateAcc += f_UpdateAcc;
            fa.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAcc(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = e.Acc.Displayname.ToString();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f= new fAdmin();
            f.loginAccount= loginAcc;
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.InsertCategory += F_InsertCategory;
            f.DeleteCategory += F_DeleteCategory;
            f.UpdateCategory += F_UpdateCategory;
            f.InsertTable += F_InsertTable;
            f.DeleteTable += F_DeleteTable;
            f.UpdateTable += F_UpdateTable;
            f.ShowDialog();
        }
        private void F_UpdateAccount(object sender, AccountEventAd e)
        {
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
            thôngTinTàiKhoảnToolStripMenuItem.Text = e.Acc.Displayname.ToString();
        }
        private void F_UpdateTable(object sender, EventArgs e)
        {
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_DeleteTable(object sender, EventArgs e)
        {
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_InsertTable(object sender, EventArgs e)
        {
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_UpdateCategory(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            LoadCategory();
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
        }

        private void F_DeleteCategory(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            LoadCategory();
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
        }

        private void F_InsertCategory(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            LoadCategory();
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCateId((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).Id);
            }
            LoadTable();
        }

        //phương thức không dùng 
        #region method không dùng
        private void cb(object sender, EventArgs e)
        {

        }
        private void btnCheckOut_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void flpTable_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        /*
       //click button change color
       private bool buttonClicked = false;int i=0;
       private void Button_Click(object sender, EventArgs e, Color current, Color after)
       {
           Button clickedButton = (Button)sender;
           if (buttonClicked)
           {
               clickedButton.BackColor = after; // Trở về màu nền ban đầu
               buttonClicked = false;
               btnCheckOut.Text ="true"+ i++;
           }
           else
           {
               clickedButton.BackColor = current; // Đổi màu nền 
               buttonClicked = true;
               btnCheckOut.Text = "false" + i++;
           }
       }*/
        #endregion

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb= sender as ComboBox;
            if (cb.SelectedItem == null)
            {
                return;
            }
            Category category = cb.SelectedItem as Category;
            id = category.Id;

            LoadFoodListByCateId(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table= lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn", "Thông báo"); return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.Id);
            int idFood = (cbFood.SelectedItem as Food).Id;
            int count= (int) nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood,count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            

            ShowBill(table.Id);
            nmFoodCount.Value = 1;
            ReloadButton(table, "Có người");
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill= BillDAO.Instance.GetUncheckBillIDByTableID(table.Id);
            int discount= (int)nmDiscount.Value;
            //float totalPrice =(float)Convert.ToDouble(txbTotalPrice.Text.Split(',')[0] +"000");
            //string.Format("Thanh toán hóa đơn {0}\n Tổng tiền - Tổng tiền x Giảm giá/100 = {1} - {1}*({2}/100)={3} ", table.Name, totalPrice, discount, finalTotalPrice)
            //float finalTotalPrice = totalPrice - (totalPrice * (discount / 100));
            float totalPrice = (float)Convert.ToDouble(txbTotalPrice.Text.Split(' ')[0].Replace(",", ""));
            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Thanh toán hóa đơn {0}",table.Name),"Thông báo",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount,totalPrice);
                    ShowBill(table.Id);
                    ReloadButton(table, "Trống");
                    nmDiscount.Value = 0;
                }
            }
        }

        public void ReloadButton(Table table,string status)
        {
            //Button button = flpTable.Controls.OfType<Button>().FirstOrDefault(b => (b.Tag as Table).Id == table.Id);
            Button button = flpTable.Controls.OfType<Button>().FirstOrDefault(b => ((Table)b.Tag).Id == table.Id);

            if (button != null)
            {
                switch (status)
                {
                    case "Trống":
                        button.BackColor = Color.DarkGreen;
                        button.ForeColor = Color.White;
                        break;
                    case "Có người":
                        button.BackColor = Color.Red;
                        button.ForeColor = Color.White;
                        break;
                    default:
                        break;
                }

                button.Text = table.Name + Environment.NewLine + Environment.NewLine + status;
            }
        }

        private void khởiĐộngLạiDanhSáchBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int discount = (int)nmDiscount.Value;
            float totalPrice = (float)Convert.ToDouble(txbTotalPrice.Text.Split(' ')[0].Replace(",", ""));
            float finalTotalPrice = totalPrice - (totalPrice * (discount / 100f));
            if (MessageBox.Show(string.Format("Giảm giá {0} % hóa đơn \n\nTổng tiền: {1}", discount, finalTotalPrice.ToString("N0") + " VND"), "Thông báo thanh toán "+table.Name, MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                txbTotalPrice.Text = finalTotalPrice.ToString("N0") + " VND";
            }
            
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            Table table_1= lsvBill.Tag as Table;
            Table table_2 = cbSwitchTable.SelectedItem as Table;
            int idTable_1 = table_1.Id;
            int idTable_2 = table_2.Id;
            if (MessageBox.Show(string.Format("Chuyển {0} sang {1}", table_1.Name, table_2.Name ), "Thông báo chuyển bàn ", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(idTable_1, idTable_2);
                ReloadButton(table_1,"Trống");
                ReloadButton(table_2, "Có người");
                LoadComboBoxTable(cbSwitchTable);
            }
        }
    }
}
