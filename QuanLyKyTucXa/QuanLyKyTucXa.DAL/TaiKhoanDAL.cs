using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class TaiKhoanDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private static bool _schemaEnsured = false;

        public TaiKhoanDAL()
        {
            if (!_schemaEnsured)
            {
                EnsureSchema();
                _schemaEnsured = true;
            }
        }

        public void EnsureSchema()
        {
            try
            {
                string sqlSetup = @"
-- 1. Bảng VaiTro
IF OBJECT_ID('VaiTro', 'U') IS NULL
BEGIN
    CREATE TABLE VaiTro (
        MaVaiTro NVARCHAR(20) PRIMARY KEY,
        TenVaiTro NVARCHAR(50) NOT NULL,
        MoTa NVARCHAR(255) NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE MaVaiTro = 'Admin')
    INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa) VALUES ('Admin', N'Quản trị viên', N'Toàn quyền quản trị hệ thống, tài khoản và phân quyền');
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE MaVaiTro = 'NhanVien')
    INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa) VALUES ('NhanVien', N'Nhân viên', N'Quản lý sinh viên, phòng, hợp đồng và dịch vụ tài chính');

-- 2. Bảng TaiKhoan (Khởi tạo nếu chưa tồn tại trên CSDL mới)
IF OBJECT_ID('TaiKhoan', 'U') IS NULL
BEGIN
    CREATE TABLE TaiKhoan (
        MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
        MatKhau VARCHAR(255) NOT NULL,
        HoTen NVARCHAR(100) NULL,
        VaiTro NVARCHAR(20) NOT NULL DEFAULT 'NhanVien',
        TrangThai BIT NOT NULL DEFAULT 1,
        NgayTao DATETIME NOT NULL DEFAULT GETDATE()
    );
END

-- 3. Gỡ Check Constraint cứng cũ (nếu có) để hỗ trợ vai trò động
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_TaiKhoan_VaiTro')
BEGIN
    ALTER TABLE TaiKhoan DROP CONSTRAINT CK_TaiKhoan_VaiTro;
END

-- Đồng bộ các mã vai trò lạ (nếu có trên dữ liệu cũ) vào bảng VaiTro trước khi gắn khóa ngoại
INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa)
SELECT DISTINCT tk.VaiTro, tk.VaiTro, N'Vai trò tự động đồng bộ từ dữ liệu hiện có'
FROM TaiKhoan tk
WHERE tk.VaiTro IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM VaiTro vt WHERE vt.MaVaiTro = tk.VaiTro);

-- Gắn khóa ngoại FK_TaiKhoan_VaiTro nếu chưa tồn tại
IF OBJECT_ID('FK_TaiKhoan_VaiTro', 'F') IS NULL
BEGIN
    ALTER TABLE TaiKhoan ADD CONSTRAINT FK_TaiKhoan_VaiTro
        FOREIGN KEY (VaiTro) REFERENCES VaiTro(MaVaiTro);
END

-- 4. Bảng PhanQuyen (Ma trận Module x Xem/Thêm/Sửa/Xóa)
IF OBJECT_ID('PhanQuyen', 'U') IS NULL
BEGIN
    CREATE TABLE PhanQuyen (
        MaVaiTro NVARCHAR(20) NOT NULL,
        MaModule NVARCHAR(30) NOT NULL,
        CoQuyenXem BIT NOT NULL DEFAULT 0,
        CoQuyenThem BIT NOT NULL DEFAULT 0,
        CoQuyenSua BIT NOT NULL DEFAULT 0,
        CoQuyenXoa BIT NOT NULL DEFAULT 0,
        CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaVaiTro, MaModule),
        CONSTRAINT FK_PhanQuyen_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro) ON DELETE CASCADE
    );
END

-- Khởi tạo quyền mặc định cho Admin (Toàn quyền 6 module)
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'SinhVien')
    INSERT INTO PhanQuyen VALUES ('Admin', 'SinhVien', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'Phong')
    INSERT INTO PhanQuyen VALUES ('Admin', 'Phong', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'HopDong')
    INSERT INTO PhanQuyen VALUES ('Admin', 'HopDong', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'DichVuTaiChinh')
    INSERT INTO PhanQuyen VALUES ('Admin', 'DichVuTaiChinh', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'ThongKe')
    INSERT INTO PhanQuyen VALUES ('Admin', 'ThongKe', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'Admin' AND MaModule = 'TaiKhoan')
    INSERT INTO PhanQuyen VALUES ('Admin', 'TaiKhoan', 1, 1, 1, 1);

-- Khởi tạo quyền mặc định cho NhanVien (Toàn quyền các module nghiệp vụ & thống kê, không vào Tài khoản)
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'SinhVien')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'SinhVien', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'Phong')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'Phong', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'HopDong')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'HopDong', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'DichVuTaiChinh')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'DichVuTaiChinh', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'ThongKe')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'ThongKe', 1, 1, 1, 1);
IF NOT EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = 'NhanVien' AND MaModule = 'TaiKhoan')
    INSERT INTO PhanQuyen VALUES ('NhanVien', 'TaiKhoan', 0, 0, 0, 0);

-- 5. Khởi tạo tài khoản Admin mặc định nếu bảng TaiKhoan đang trống (Mật khẩu: Admin@123)
IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE VaiTro = 'Admin')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai, NgayTao)
    VALUES ('admin', '$2a$11$CsOkEZNOwZni10pp4LvLJOKk2Wg6jjFH546nU6YoodbWJmBUXdOPW', N'Quản trị viên', 'Admin', 1, GETDATE());
END
";
                dbHelper.ExecuteNonQuery(sqlSetup);
            }
            catch
            {
                // Bỏ qua nếu đã cấu hình hoặc không có quyền DDL đặc thù
            }
        }

        public TaiKhoanDTO GetTaiKhoanByUsername(string tenDangNhap)
        {
            string query = @"
                SELECT tk.MaTaiKhoan, tk.TenDangNhap, tk.MatKhau, tk.HoTen, tk.VaiTro, vt.TenVaiTro, tk.TrangThai, tk.NgayTao 
                FROM TaiKhoan tk
                LEFT JOIN VaiTro vt ON vt.MaVaiTro = tk.VaiTro
                WHERE tk.TenDangNhap = @TenDangNhap";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tenDangNhap ?? "")
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                var dto = new TaiKhoanDTO(
                    Convert.ToInt32(r["MaTaiKhoan"]),
                    r["TenDangNhap"]?.ToString(),
                    r["MatKhau"]?.ToString(),
                    r["HoTen"]?.ToString(),
                    r["VaiTro"]?.ToString(),
                    r["TrangThai"] != DBNull.Value && Convert.ToBoolean(r["TrangThai"]),
                    r["NgayTao"] != DBNull.Value ? Convert.ToDateTime(r["NgayTao"]) : DateTime.Now
                );
                dto.TenVaiTro = r["TenVaiTro"] != DBNull.Value ? r["TenVaiTro"].ToString() : null;
                return dto;
            }
            return null;
        }

        public TaiKhoanDTO GetTaiKhoanById(int maTaiKhoan)
        {
            string query = @"
                SELECT tk.MaTaiKhoan, tk.TenDangNhap, tk.MatKhau, tk.HoTen, tk.VaiTro, vt.TenVaiTro, tk.TrangThai, tk.NgayTao 
                FROM TaiKhoan tk
                LEFT JOIN VaiTro vt ON vt.MaVaiTro = tk.VaiTro
                WHERE tk.MaTaiKhoan = @MaTaiKhoan";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTaiKhoan", maTaiKhoan)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                var dto = new TaiKhoanDTO(
                    Convert.ToInt32(r["MaTaiKhoan"]),
                    r["TenDangNhap"]?.ToString(),
                    r["MatKhau"]?.ToString(),
                    r["HoTen"]?.ToString(),
                    r["VaiTro"]?.ToString(),
                    r["TrangThai"] != DBNull.Value && Convert.ToBoolean(r["TrangThai"]),
                    r["NgayTao"] != DBNull.Value ? Convert.ToDateTime(r["NgayTao"]) : DateTime.Now
                );
                dto.TenVaiTro = r["TenVaiTro"] != DBNull.Value ? r["TenVaiTro"].ToString() : null;
                return dto;
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách tài khoản phục vụ hiển thị (KHÔNG trả về mật khẩu mã băm để bảo mật)
        /// </summary>
        public DataTable GetAllTaiKhoan()
        {
            string query = @"
                SELECT 
                    tk.MaTaiKhoan, 
                    tk.TenDangNhap, 
                    tk.HoTen, 
                    tk.VaiTro, 
                    tk.TrangThai, 
                    tk.NgayTao,
                    CASE WHEN tk.TrangThai = 1 THEN N'Hoạt động' ELSE N'Bị khóa' END AS TrangThaiHienThi,
                    ISNULL(vt.TenVaiTro, tk.VaiTro) AS VaiTroHienThi
                FROM TaiKhoan tk
                LEFT JOIN VaiTro vt ON vt.MaVaiTro = tk.VaiTro
                ORDER BY tk.MaTaiKhoan DESC";

            return dbHelper.ExecuteQuery(query);
        }

        public DataTable FilterTaiKhoan(string keyword, string vaiTro, string trangThai)
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add("(tk.TenDangNhap LIKE @Keyword OR tk.HoTen LIKE @Keyword OR vt.TenVaiTro LIKE @Keyword)");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(vaiTro) && vaiTro != "Tất cả vai trò")
            {
                conditions.Add("(tk.VaiTro = @VaiTro OR vt.TenVaiTro = @VaiTro)");
                parameters.Add(new SqlParameter("@VaiTro", vaiTro.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả trạng thái")
            {
                if (trangThai == "Đang hoạt động" || trangThai == "Hoạt động")
                {
                    conditions.Add("tk.TrangThai = 1");
                }
                else if (trangThai == "Bị khóa" || trangThai == "Đã khóa")
                {
                    conditions.Add("tk.TrangThai = 0");
                }
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";
            string query = @"
                SELECT 
                    tk.MaTaiKhoan, 
                    tk.TenDangNhap, 
                    tk.HoTen, 
                    tk.VaiTro, 
                    tk.TrangThai, 
                    tk.NgayTao,
                    CASE WHEN tk.TrangThai = 1 THEN N'Hoạt động' ELSE N'Bị khóa' END AS TrangThaiHienThi,
                    ISNULL(vt.TenVaiTro, tk.VaiTro) AS VaiTroHienThi
                FROM TaiKhoan tk
                LEFT JOIN VaiTro vt ON vt.MaVaiTro = tk.VaiTro"
                + whereClause + " ORDER BY tk.MaTaiKhoan DESC";

            return dbHelper.ExecuteQuery(query, parameters.Count > 0 ? parameters.ToArray() : null);
        }

        public bool InsertTaiKhoan(TaiKhoanDTO tk)
        {
            string query = @"
                INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai, NgayTao)
                VALUES (@TenDangNhap, @MatKhau, @HoTen, @VaiTro, @TrangThai, @NgayTao)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tk.TenDangNhap.Trim()),
                new SqlParameter("@MatKhau", tk.MatKhauHash),
                new SqlParameter("@HoTen", (object)tk.HoTen?.Trim() ?? DBNull.Value),
                new SqlParameter("@VaiTro", tk.VaiTro),
                new SqlParameter("@TrangThai", tk.TrangThai ? 1 : 0),
                new SqlParameter("@NgayTao", tk.NgayTao == DateTime.MinValue ? DateTime.Now : tk.NgayTao)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateTaiKhoan(TaiKhoanDTO tk)
        {
            string query = @"
                UPDATE TaiKhoan 
                SET HoTen = @HoTen, 
                    VaiTro = @VaiTro, 
                    TrangThai = @TrangThai 
                WHERE MaTaiKhoan = @MaTaiKhoan";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@HoTen", (object)tk.HoTen?.Trim() ?? DBNull.Value),
                new SqlParameter("@VaiTro", tk.VaiTro),
                new SqlParameter("@TrangThai", tk.TrangThai ? 1 : 0),
                new SqlParameter("@MaTaiKhoan", tk.MaTaiKhoan)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateMatKhau(int maTaiKhoan, string matKhauHash)
        {
            string query = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE MaTaiKhoan = @MaTaiKhoan";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MatKhau", matKhauHash),
                new SqlParameter("@MaTaiKhoan", maTaiKhoan)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateTrangThai(int maTaiKhoan, bool trangThai)
        {
            string query = "UPDATE TaiKhoan SET TrangThai = @TrangThai WHERE MaTaiKhoan = @MaTaiKhoan";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TrangThai", trangThai ? 1 : 0),
                new SqlParameter("@MaTaiKhoan", maTaiKhoan)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteTaiKhoan(int maTaiKhoan)
        {
            string query = "DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTaiKhoan", maTaiKhoan)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public int CountActiveAdmin(int excludeMaTaiKhoan = 0)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE VaiTro = 'Admin' AND TrangThai = 1 AND MaTaiKhoan <> @ExcludeId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ExcludeId", excludeMaTaiKhoan)
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public bool CheckUsernameExists(string tenDangNhap, int excludeMaTaiKhoan = 0)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MaTaiKhoan <> @ExcludeId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tenDangNhap.Trim()),
                new SqlParameter("@ExcludeId", excludeMaTaiKhoan)
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }

        public bool BackupDatabase(string backupFilePath)
        {
            // Thư mục công cộng chia sẻ quyền với SQL Server Service
            string publicSharedFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "QuanLyKyTucXa_Backup");
            if (!Directory.Exists(publicSharedFolder))
            {
                try { Directory.CreateDirectory(publicSharedFolder); } catch { }
            }

            // Thử sao lưu trực tiếp vào đường dẫn đích
            try
            {
                string query = @"BACKUP DATABASE [QuanLyKyTucXa] TO DISK = @BackupPath WITH FORMAT, INIT, NAME = N'QuanLyKyTucXa-Full Backup'";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BackupPath", backupFilePath)
                };
                dbHelper.ExecuteNonQuery(query, parameters);
                return true;
            }
            catch (Exception ex) when (ex.Message.Contains("Operating system error 5") || ex.Message.Contains("Access is denied"))
            {
                // Nếu SQL Server không có quyền ghi vào thư mục riêng của người dùng (Error 5),
                // sao lưu tạm vào C:\Users\Public rồi dùng tiến trình ứng dụng sao chép về đích
                string publicDir = @"C:\Users\Public\QuanLyKyTucXa_Backup";
                if (!Directory.Exists(publicDir))
                {
                    Directory.CreateDirectory(publicDir);
                }

                string tempBak = Path.Combine(publicDir, $"temp_backup_{Guid.NewGuid():N}.bak");
                string query = @"BACKUP DATABASE [QuanLyKyTucXa] TO DISK = @BackupPath WITH FORMAT, INIT, NAME = N'QuanLyKyTucXa-Full Backup'";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BackupPath", tempBak)
                };
                dbHelper.ExecuteNonQuery(query, parameters);

                if (File.Exists(tempBak))
                {
                    string targetDir = Path.GetDirectoryName(backupFilePath);
                    if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    File.Copy(tempBak, backupFilePath, true);
                    try { File.Delete(tempBak); } catch { }
                    return true;
                }
                throw;
            }
        }

        public bool RestoreDatabase(string restoreFilePath)
        {
            string actualRestorePath = restoreFilePath;
            string tempRestoreFile = null;

            // Nếu file sao lưu nằm ở thư mục cá nhân người dùng, SQL Server service có thể bị từ chối truy cập.
            // Ta sao chép tạm vào C:\Users\Public để SQL Server đọc an toàn.
            try
            {
                string publicDir = @"C:\Users\Public\QuanLyKyTucXa_Backup";
                if (!Directory.Exists(publicDir))
                {
                    Directory.CreateDirectory(publicDir);
                }

                tempRestoreFile = Path.Combine(publicDir, $"temp_restore_{Guid.NewGuid():N}.bak");
                File.Copy(restoreFilePath, tempRestoreFile, true);
                actualRestorePath = tempRestoreFile;
            }
            catch
            {
                actualRestorePath = restoreFilePath;
            }

            try
            {
                SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(dbHelper.ConnectionString);
                cb.InitialCatalog = "master";

                string restoreSql = @"
                    ALTER DATABASE [QuanLyKyTucXa] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [QuanLyKyTucXa] FROM DISK = @RestorePath WITH REPLACE;
                    ALTER DATABASE [QuanLyKyTucXa] SET MULTI_USER;";

                using (SqlConnection conn = new SqlConnection(cb.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(restoreSql, conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add(new SqlParameter("@RestorePath", actualRestorePath));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            finally
            {
                if (tempRestoreFile != null && File.Exists(tempRestoreFile))
                {
                    try { File.Delete(tempRestoreFile); } catch { }
                }
            }
        }
    }
}
