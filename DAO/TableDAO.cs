using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    internal class TableDAO
    {
        public static int TableHeight = 130;
        public static int TableWidth = 130;
        private static TableDAO instance;//ctrl + r + e
        public static TableDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TableDAO();
                }
                return TableDAO.instance;
            }
            private set => instance = value;
        }
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> list = new List<Table>();
            DataTable dataTable = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.TableFood");
            foreach (DataRow row in dataTable.Rows)
            {
                Table table = new Table(row);
                list.Add(table);
            }
            return list;

        }
        public DataTable GetListTableAdmin()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên bàn', status as N'Trạng thái' FROM dbo.TableFood");
        }
        public void SwitchTable(int id1,int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTable2 ",new object[] {id1,id2});
        }
        public Table GetTableByID(int tableId)
        {
            Table listTable = null;
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.TableFood WHERE id = " + tableId);
            foreach (DataRow item in data.Rows)
            {
                listTable = new Table(item);
                return listTable;
            }
            return listTable;
        }
        public bool InsertTable(string name, string status)
        {
            string query = string.Format("INSERT dbo.TableFood (name,status) VALUES ( N'{0}' , N'{1}' )", name, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateTable(int id, string name, string status)
        {
            string query = string.Format("UPDATE dbo.TableFood SET name = N'{0}', status = {1} WHERE id = {2} ", name, status, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int idTable)
        {
            List<Bill> listBill = BillDAO.Instance.GetListBillByTableId(idTable);
            foreach (Bill item in listBill)
            {
                BillInfoDAO.Instance.DeleteBillInfoByBillId(item.Id);
            }
            BillDAO.Instance.DeleteBillByTableId(idTable);
            string query = string.Format("DELETE dbo.TableFood WHERE id= {0}", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public DataTable SearchTableByname(string nameTable)
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên bàn', status as N'Trạng thái' FROM dbo.TableFood WHERE name COLLATE Latin1_General_CI_AI LIKE N'%" + nameTable + "%'");
        }
    }
}
