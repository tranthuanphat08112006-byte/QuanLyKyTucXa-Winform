using System;
using System.Data;
using System.Data.SqlClient; // Bắt buộc phải có thư viện này để dùng ADO.NET

namespace QuanLyKyTucXa.DAL
{
    public class DatabaseHelper
    {
        // LƯU Ý BẮT BUỘC: Sửa "ACER-NITRO\\SQLEXPRESS" thành tên Server SQL thực tế trên máy bạn.
        private string connectionString = @"Data Source=ADMIN;Initial Catalog=QuanLyKyTucXa;Integrated Security=True";

        // Hàm thực thi các lệnh SELECT (Tra cứu dữ liệu)
        // Trả về một DataTable chứa kết quả dạng bảng
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        // Hàm thực thi các lệnh INSERT, UPDATE, DELETE
        // Trả về số dòng bị ảnh hưởng (nếu > 0 nghĩa là đã thêm/sửa/xóa thành công)
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int affectedRows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }
    }
}