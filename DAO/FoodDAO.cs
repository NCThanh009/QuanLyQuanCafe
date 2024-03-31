using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    internal class FoodDAO
    {
        private static FoodDAO instance;//ctrl + r + e
        public static FoodDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FoodDAO();
                }
                return FoodDAO.instance;
            }
            private set => instance = value;
        }
        private FoodDAO() { }
        public List<Food> GetListFoodByCateId(int idCate)
        {
            List<Food> listCate = new List<Food>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Food\tWHERE idCategory=" + idCate);
            foreach (DataRow item in data.Rows)
            {
                Food info = new Food(item);
                listCate.Add(info);
            }
            return listCate;
        }
            
        public DataTable SearchFoodByname(string nameFood)
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên thức uống', idCategory as N'ID danh mục', price as N'Giá' FROM dbo.Food WHERE name COLLATE Latin1_General_CI_AI LIKE N'%" + nameFood + "%'");
        }
        /*
        public List<Food> GetListFood()
        {
            List<Food> listCate = new List<Food>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Food");
            foreach (DataRow item in data.Rows)
            {
                Food info = new Food(item);
                listCate.Add(info);
            }
            return listCate;
        }
        */
        public DataTable GetListFood()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên thức uống', idCategory as N'ID danh mục', price as N'Giá' FROM dbo.Food");

        }
        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT dbo.Food (name,idCategory,price) VALUES ( N'{0}' , {1} , {2} )", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFood(int idFood, string name, int idcate, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET name = N'{0}', idCategory = {1}, price = {2} WHERE id = {3} ", name, idcate, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);
            string query = string.Format("DELETE dbo.Food WHERE id= {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public void DeleteFoodByCategoryId(int idCate)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCateId(idCate);
            foreach (Food item in listFood)
            {
                DeleteFood(item.Id); 
            }
            DataProvider.Instance.ExecuteQuery("DELETE dbo.Food WHERE idCategory =" + idCate);
        }


    }
}
