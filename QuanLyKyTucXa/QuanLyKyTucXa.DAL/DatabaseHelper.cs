using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QuanLyKyTucXa.DAL
{
    public class DatabaseHelper
    {
        private string connectionString;

        public string ConnectionString => connectionString;

        public DatabaseHelper()
        {
            try
            {
                var connSetting = ConfigurationManager.ConnectionStrings["ConnStr"];
                if (connSetting != null && !string.IsNullOrEmpty(connSetting.ConnectionString))
                {
                    connectionString = connSetting.ConnectionString;
                }
                else
                {
                    connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKyTucXa;Integrated Security=True";
                }
            }
            catch
            {
                connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyKyTucXa;Integrated Security=True";
            }
        }

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
                        throw new Exception("Lỗi thực thi lệnh dữ liệu: " + ex.Message);
                    }
                }
            }
            return affectedRows;
        }

        // Hàm 3: Lấy giá trị đơn (SELECT COUNT, SCOPE_IDENTITY, ...)
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;
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
                        result = cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi truy vấn giá trị: " + ex.Message);
                    }
                }
            }
            return result;
        }
    }
}