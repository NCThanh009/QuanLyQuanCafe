using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;//ctrl + r + e
        public static MenuDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuDAO();
                }
                return MenuDAO.instance;
            }
            private set => instance = value;
        }
        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> list = new List<Menu>();
            string query= "SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f\r\nWHERE bi.idBill=b.id AND bi.idFood=f.id AND b.status=0 AND b.idTable=" + id; 
            DataTable data= DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow dr in data.Rows) 
            {
                Menu menu = new Menu(dr);
                list.Add(menu);
            }
            return list;
        }
    }
}
