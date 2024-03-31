using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        
        private static DataProvider instance;//ctrl + r + e
        public static DataProvider Instance 
        {
            get { 
                if (instance == null) { 
                    instance = new DataProvider(); 
                }
                return DataProvider.instance;
            }
            private set => instance = value; 
        }
        private DataProvider() { }

        private String connectionStr = "Data Source=DELL-VOSTRO3580\\NCITHANH;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public DataTable ExecuteQuery(String que, Object[] para = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))//tự giải phóng bộ nhớ tránh lỗi data
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(que, sqlConnection);
                if(para != null)
                {
                    String[] listPara= que.Split(' ');
                    int i= 0;
                    foreach(String s in listPara)
                    {
                        if (s.Contains("@"))
                        {
                            sqlCommand.Parameters.AddWithValue(s, para[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);//trung gian thực hiện câu truy vấn
                adapter.Fill(data);//đổ dữ liệu vào data

                sqlConnection.Close();
            }

            return data;
        }
        //số dòng truy xuất thành công
        public int ExecuteNonQuery(String que, Object[] para = null)
        {
            int data = 0;
            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))//tự giải phóng bộ nhớ tránh lỗi data
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(que, sqlConnection);
                if (para != null)
                {
                    String[] listPara = que.Split(' ');
                    int i = 0;
                    foreach (String s in listPara)
                    {
                        if (s.Contains("@"))
                        {
                            sqlCommand.Parameters.AddWithValue(s, para[i]);
                            i++;
                        }
                    }
                }
                //SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);//trung gian thực hiện câu truy vấn
                //adapter.Fill(data);//đổ dữ liệu vào data
                data = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

            return data;
        }
        //đếm số dòng
        public Object ExecuteScalar(String que, Object[] para = null)
        {
            Object data = 0;
            using (SqlConnection sqlConnection = new SqlConnection(connectionStr))//tự giải phóng bộ nhớ tránh lỗi data
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(que, sqlConnection);
                if (para != null)
                {
                    String[] listPara = que.Split(' ');
                    int i = 0;
                    foreach (String s in listPara)
                    {
                        if (s.Contains("@"))
                        {
                            sqlCommand.Parameters.AddWithValue(s, para[i]);
                            i++;
                        }
                    }
                }
                //SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);//trung gian thực hiện câu truy vấn
                //adapter.Fill(data);//đổ dữ liệu vào data
                data = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
            }

            return data;
        }

    }
}
