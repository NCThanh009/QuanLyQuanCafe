using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList= new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource tableList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load(); 
        }
        public void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            AddBinding();

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);//LoadListBill();
            LoadListFood();
            LoadListCategoryFood();
            LoadListTabel();
            LoadListAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
        }

        public void AddBinding()
        {
            AddFoodBinding();
            AddAcccoutnBinding();
            AddTableBinding();
            AddCategoryBinding();
        }
        #region Load
        public void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        public void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month,1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        public void LoadListBill()
        {
            dtgvBill.DataSource =  BillDAO.Instance.GetListBill();
        }
        public void LoadListFood()
        {
            foodList.DataSource =FoodDAO.Instance.GetListFood();
        }
        public void LoadListCategoryFood()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategoryFood();
        }
        public void LoadListTabel()
        {
            tableList.DataSource = TableDAO.Instance.GetListTableAdmin();
        }
        public void LoadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        public void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }
        #endregion
        
        public void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Tên thức uống",true,DataSourceUpdateMode.Never));
            txbFoodId.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID",true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("value", dtgvFood.DataSource, "Giá",true, DataSourceUpdateMode.Never));
        }
        public void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID",true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Tên danh mục", true, DataSourceUpdateMode.Never));
        }
        public void AddTableBinding()
        {
           txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
           txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Tên bàn", true, DataSourceUpdateMode.Never));
           txbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Trạng thái", true, DataSourceUpdateMode.Never));
        }
        public void AddAcccoutnBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tài khoản", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên người dùng", true, DataSourceUpdateMode.Never));
            txbType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Quyền", true, DataSourceUpdateMode.Never));
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategoryFood();
        }
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTabel();
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }
        private void txbFoodId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["ID danh mục"].Value;
                    Category cate = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCategory.SelectedItem = cate;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.Id == cate.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {
                MessageBox.Show("Kết quả tìm kiếm của bạn không có dữ liệu", "Thông báo");
            }
            
        }

        private void txbTableID_TextChanged(object sender, EventArgs e)
        {
        }

        private void txbType_TextChanged(object sender, EventArgs e)
        {
            if (txbType.Text.Equals("1"))
            {
                lbType.Text = "Quản trị viên";
            }
            else
            {
                lbType.Text = "Nhân viên";
            }
        }
        private event EventHandler<AccountEventAd> updateAccount;
        public event EventHandler<AccountEventAd> UpdateAccount
        {
            add { updateAccount += value; }
            remove
            {
                updateAccount -= value;
            }
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int cateId = (cbFoodCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;
            if(FoodDAO.Instance.InsertFood(name,cateId,price))
            {
                MessageBox.Show("Thêm thức uống thành công", "Thông báo thêm món");
                LoadListFood();
                if(insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm thức uống thất bại", "Thông báo thêm món");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công", "Thông báo thêm danh mục");
                LoadListCategoryFood();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại", "Thông báo thêm danh mục");
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string status = txbTableStatus.Text;
            if (TableDAO.Instance.InsertTable(name,status))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo thêm bàn");
                LoadListTabel();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại", "Thông báo thêm bàn");
            }
        }

        private void btnAddAccount_Click_1(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type =  int.Parse(txbType.Text);
            DataTable listAccount = AccountDAO.Instance.GetListAccount();
            foreach (DataRow row in listAccount.Rows)
            {
                string userName = row["Tài khoản"].ToString();
                if (username.Equals(userName))
                {
                    MessageBox.Show("Tài khoản đã tồn tại", "Thông báo thêm tài khoản");return;
                }
            }
            if (AccountDAO.Instance.InsertAccount(username,displayName,type))
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo thêm tài khoản");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại", "Thông báo thêm tài khoản");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int cateId = (cbFoodCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;
            int id = int.Parse(txbFoodId.Text);
            if (FoodDAO.Instance.UpdateFood(id,name,cateId,price))
            {
                MessageBox.Show("Cập nhật thức uống thành công", "Thông báo cập nhật món");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhật thức uống thất bại", "Thông báo cập nhật món");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            int id= int.Parse(txbCategoryID.Text);
            if (CategoryDAO.Instance.UpdateCategory(name,id))
            {
                MessageBox.Show("Cập nhật danh mục thành công", "Thông báo cập nhật danh mục");
                LoadListCategoryFood();
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhật danh mục thất bại", "Thông báo cập nhật danh mục");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string status = txbTableStatus.Text;
            int id = int.Parse(txbTableID.Text);
            if (TableDAO.Instance.UpdateTable(id, name, status))
            {
                MessageBox.Show("Cập nhật bàn thành công", "Thông báo cập nhật bàn");
                LoadListTabel();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại", "Thông báo cập nhật bàn");
            }
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string disName = txbDisplayName.Text;
            string userName = txbUserName.Text;
            int type = int.Parse(txbType.Text);
            if (AccountDAO.Instance.UpdateAccount(userName, disName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công", "Thông báo cập nhật tài khoản");
                LoadListAccount();
                if (updateAccount != null)
                {
                    updateAccount(this, new AccountEventAd(AccountDAO.Instance.GetAccountByUsername(userName)));
                }
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại", "Thông báo cập nhật tài khoản");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txbFoodId.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa thức uống thành công", "Thông báo xóa thức uống");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa thức uống thất bại", "Thông báo xóa thức uống");
            }
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txbCategoryID.Text);
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công", "Thông báo xóa danh mục");
                LoadListCategoryFood();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa danh mục thất bại", "Thông báo xóa danh mục");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txbTableID.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công", "Thông báo xóa bàn");
                LoadListTabel();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa bàn thất bại", "Thông báo xóa bàn");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName= txbUserName.Text;
            if (loginAccount.Username.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản đang hoạt động", "Thông báo xóa tài khoản");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo xóa tài khoản");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại", "Thông báo xóa tài khoản");
            }
        }
        public DataTable SearchFoodByName(string nameFood)
        {
            DataTable list = FoodDAO.Instance.SearchFoodByname(nameFood);
            return list;   
        }
        public DataTable SearchTableByName(string nameTable)
        {
            DataTable list = TableDAO.Instance.SearchTableByname(nameTable);
            return list;
        }
        public DataTable SearchCategoryByName(string nameCategory)
        {
            DataTable list = CategoryDAO.Instance.SearchCategoryByname(nameCategory);
            return list;
        }
        public DataTable SearchAccountByName(string nameAccount,string displayName)
        {
            DataTable list = AccountDAO.Instance.SearchAccountByname(nameAccount,displayName);
            return list;
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource= SearchFoodByName(txbSearchFood.Text);
        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchCategoryByName(txbSearchCategory.Text);
        }

        private void btnSearchTable_Click(object sender, EventArgs e)
        {
            tableList.DataSource = SearchTableByName(txbSearchTable.Text);
        }

        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            accountList.DataSource = SearchAccountByName(txbSearchAccount.Text,txbSearchAccount.Text);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại", "Thông báo");
            }
        }
    }
    public class AccountEventAd : EventArgs
    {
        private DTO.Account acc;

        public Account Acc { get => acc; set => acc = value; }
        public AccountEventAd(Account acc)
        {
            this.acc = acc;
        }
    }
}
