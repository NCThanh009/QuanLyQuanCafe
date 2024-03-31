using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;//ctrl + r + e
        public static BillDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillDAO();
                }
                return BillDAO.instance;
            }
            private set => instance = value;
        }
        private BillDAO() { }
        public int GetUncheckBillIDByTableID(int tableId)
        {
            DataTable data= DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + tableId + " AND status = 0");
            if(data.Rows.Count > 0 ) 
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            return -1;
        }
        public void InsertBill(int idTable)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable=" + idTable);
        }
        public void DeleteBillByTableId(int idTable)
        {
            DataProvider.Instance.ExecuteNonQuery("DELETE dbo.Bill WHERE idTable =" + idTable);
        }
        public int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }

        }
        public void CheckOut(int idBill, int discount, float totalPrice)
        {
            string query = "UPDATE dbo.Bill\tSET DateCheckOut = CONVERT(VARCHAR(19), GETDATE(), 120) , status = 1, discount = " + discount+" , totalPrice = "+totalPrice+" WHERE id = "+idBill;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut ", new object[] { checkIn, checkOut });
        }
        //test sơ sơ 
        public DataTable GetListBill()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]\r\nFROM dbo.Bill AS b,dbo.TableFood AS t\r\nWHERE b.status = 1\r\nAND t.id = b.idTable");
        }
        public List<Bill> GetListBillByTableId(int idTable)
        {
            List<Bill> listBill = new List<Bill>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill where idTable = " + idTable);
            foreach (DataRow item in data.Rows)
            {
                Bill info = new Bill(item);
                listBill.Add(info);
            }
            return listBill;
        }

    }
}
