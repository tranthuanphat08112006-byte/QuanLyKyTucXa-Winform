using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QuanLyKyTucXa.DAL
{
    public class DatabaseHelper
    {
        // Tự động lấy chuỗi kết nối có tên "ConnStr" từ App.config
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        // Hàm 1: Dành cho các câu lệnh SELECT (Lấy dữ liệu trả về DataTable)
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi truy vấn dữ liệu: " + ex.Message);
                    }
                }
            }
            return dataTable;
        }

        // Hàm 2: Dành cho các câu lệnh INSERT, UPDATE, DELETE (Trả về số dòng bị ảnh hưởng)
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int affectedRows = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi thao tác dữ liệu: " + ex.Message);
                    }
                }
            }
            return affectedRows;
        }
    }
}