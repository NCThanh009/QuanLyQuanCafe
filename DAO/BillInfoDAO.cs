using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;//ctrl + r + e
        public static BillInfoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillInfoDAO();
                }
                return BillInfoDAO.instance;
            }
            private set => instance = value;
        }
        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int billId)
        {
            List<BillInfo> billInfos = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill=" + billId );
            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                billInfos.Add(info);
            }
            return billInfos;
        }
        public void InsertBillInfo(int idBill, int idFood,int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count ", new object[] {idBill,idFood,count});
        }
        public void DeleteBillInfoByFoodId(int idFood)
        {
            DataProvider.Instance.ExecuteQuery("DELETE dbo.BillInfo WHERE idFood =" + idFood);
        }
        public void DeleteBillInfoByBillId(int idBill)
        {
            DataProvider.Instance.ExecuteQuery("DELETE dbo.BillInfo WHERE idBill =" + idBill);
        }
    }
}
