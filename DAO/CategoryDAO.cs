using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;//ctrl + r + e
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }
                return CategoryDAO.instance;
            }
            private set => instance = value;
        }
        private CategoryDAO() { }
        public List<Category> GetListCategory()
        {
            List<Category> listCate = new List<Category>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.FoodCategory");
            foreach (DataRow item in data.Rows)
            {
                Category info = new Category(item);
                listCate.Add(info);
            }
            return listCate;
        }
        public DataTable SearchCategoryByname(string nameCategory)
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên danh mục' FROM dbo.FoodCategory WHERE name COLLATE Latin1_General_CI_AI LIKE N'%" + nameCategory + "%'");
        }
        public DataTable GetListCategoryFood()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as ID, name as N'Tên danh mục' FROM dbo.FoodCategory");
        }
        public Category GetCategoryByID(int cateId)
        {
            Category listCate = null;
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.FoodCategory WHERE id = "+cateId);
            foreach (DataRow item in data.Rows)
            {
                listCate = new Category(item);
                return listCate;
            }
            return listCate;
        }
        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT dbo.FoodCategory (name) VALUES ( N'{0}' )", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateCategory(string name,int id)
        {
            string query = string.Format("UPDATE dbo.FoodCategory SET name = N'{0}' WHERE id = {1} ", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteCategory(int idCate)
        {
            FoodDAO.Instance.DeleteFoodByCategoryId(idCate);
            string query = string.Format("DELETE dbo.FoodCategory WHERE id= {0}", idCate);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
