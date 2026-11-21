using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class VaiTroDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        public VaiTroDAL()
        {
            // Đảm bảo schema VaiTro & PhanQuyen đã được khởi tạo
            new TaiKhoanDAL();
        }

        public DataTable GetAllVaiTro()
        {
            string query = @"
                SELECT 
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                    ISNULL(vt.MoTa, N'') AS MoTa,
                    COUNT(tk.MaTaiKhoan) AS SoTaiKhoan
                FROM VaiTro vt
                LEFT JOIN TaiKhoan tk ON tk.VaiTro = vt.MaVaiTro
                GROUP BY vt.MaVaiTro, vt.TenVaiTro, vt.MoTa
                ORDER BY 
                    CASE WHEN vt.MaVaiTro = 'Admin' THEN 0 ELSE 1 END,
                    vt.TenVaiTro ASC";

            return dbHelper.ExecuteQuery(query);
        }

        public DataTable FilterVaiTro(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAllVaiTro();

            string query = @"
                SELECT 
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                    ISNULL(vt.MoTa, N'') AS MoTa,
                    COUNT(tk.MaTaiKhoan) AS SoTaiKhoan
                FROM VaiTro vt
                LEFT JOIN TaiKhoan tk ON tk.VaiTro = vt.MaVaiTro
                WHERE vt.MaVaiTro LIKE @Keyword OR vt.TenVaiTro LIKE @Keyword OR vt.MoTa LIKE @Keyword
                GROUP BY vt.MaVaiTro, vt.TenVaiTro, vt.MoTa
                ORDER BY 
                    CASE WHEN vt.MaVaiTro = 'Admin' THEN 0 ELSE 1 END,
                    vt.TenVaiTro ASC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Keyword", "%" + keyword.Trim() + "%")
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }

        public VaiTroDTO GetVaiTroById(string maVaiTro)
        {
            string query = @"
                SELECT 
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                    ISNULL(vt.MoTa, N'') AS MoTa,
                    COUNT(tk.MaTaiKhoan) AS SoTaiKhoan
                FROM VaiTro vt
                LEFT JOIN TaiKhoan tk ON tk.VaiTro = vt.MaVaiTro
                WHERE vt.MaVaiTro = @MaVaiTro
                GROUP BY vt.MaVaiTro, vt.TenVaiTro, vt.MoTa";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaVaiTro", maVaiTro ?? "")
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new VaiTroDTO(
                    r["MaVaiTro"]?.ToString(),
                    r["TenVaiTro"]?.ToString(),
                    r["MoTa"]?.ToString(),
                    r["SoTaiKhoan"] != DBNull.Value ? Convert.ToInt32(r["SoTaiKhoan"]) : 0
                );
            }
            return null;
        }

        public bool CheckMaVaiTroExists(string maVaiTro)
        {
            string query = "SELECT COUNT(*) FROM VaiTro WHERE MaVaiTro = @MaVaiTro";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaVaiTro", maVaiTro.Trim())
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }

        public int CountTaiKhoanByVaiTro(string maVaiTro)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE VaiTro = @MaVaiTro";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaVaiTro", maVaiTro.Trim())
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public bool InsertVaiTro(VaiTroDTO vt, List<PhanQuyenDTO> permissions)
        {
            using (SqlConnection conn = new SqlConnection(dbHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string insertRoleSql = @"
                            INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa)
                            VALUES (@MaVaiTro, @TenVaiTro, @MoTa)";

                        using (SqlCommand cmd = new SqlCommand(insertRoleSql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaVaiTro", vt.MaVaiTro.Trim());
                            cmd.Parameters.AddWithValue("@TenVaiTro", vt.TenVaiTro.Trim());
                            cmd.Parameters.AddWithValue("@MoTa", (object)vt.MoTa?.Trim() ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }

                        if (permissions != null)
                        {
                            foreach (var p in permissions)
                            {
                                UpsertPhanQuyenInTransaction(conn, trans, vt.MaVaiTro.Trim(), p);
                            }
                        }

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdateVaiTro(VaiTroDTO vt)
        {
            string query = @"
                UPDATE VaiTro
                SET TenVaiTro = @TenVaiTro,
                    MoTa = @MoTa
                WHERE MaVaiTro = @MaVaiTro";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenVaiTro", vt.TenVaiTro.Trim()),
                new SqlParameter("@MoTa", (object)vt.MoTa?.Trim() ?? DBNull.Value),
                new SqlParameter("@MaVaiTro", vt.MaVaiTro.Trim())
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteVaiTro(string maVaiTro)
        {
            string query = "DELETE FROM VaiTro WHERE MaVaiTro = @MaVaiTro";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaVaiTro", maVaiTro.Trim())
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<PhanQuyenDTO> GetPhanQuyenByVaiTro(string maVaiTro)
        {
            var result = new List<PhanQuyenDTO>();
            var map = new Dictionary<string, PhanQuyenDTO>(StringComparer.OrdinalIgnoreCase);

            string query = @"
                SELECT MaVaiTro, MaModule, CoQuyenXem, CoQuyenThem, CoQuyenSua, CoQuyenXoa
                FROM PhanQuyen
                WHERE MaVaiTro = @MaVaiTro";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaVaiTro", maVaiTro ?? "")
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    string mod = r["MaModule"]?.ToString() ?? "";
                    if (!string.IsNullOrWhiteSpace(mod))
                    {
                        map[mod] = new PhanQuyenDTO(
                            maVaiTro,
                            mod,
                            r["CoQuyenXem"] != DBNull.Value && Convert.ToBoolean(r["CoQuyenXem"]),
                            r["CoQuyenThem"] != DBNull.Value && Convert.ToBoolean(r["CoQuyenThem"]),
                            r["CoQuyenSua"] != DBNull.Value && Convert.ToBoolean(r["CoQuyenSua"]),
                            r["CoQuyenXoa"] != DBNull.Value && Convert.ToBoolean(r["CoQuyenXoa"])
                        );
                    }
                }
            }

            // Luôn trả đủ 5 module chuẩn theo đúng thứ tự
            bool isAdmin = string.Equals(maVaiTro, "Admin", StringComparison.OrdinalIgnoreCase);
            foreach (var mod in ModuleSystem.AllModules)
            {
                if (isAdmin)
                {
                    result.Add(new PhanQuyenDTO("Admin", mod.Key, true, true, true, true));
                }
                else if (map.TryGetValue(mod.Key, out PhanQuyenDTO existing))
                {
                    result.Add(existing);
                }
                else
                {
                    result.Add(new PhanQuyenDTO(maVaiTro, mod.Key, false, false, false, false));
                }
            }

            return result;
        }

        public bool SavePhanQuyen(string maVaiTro, List<PhanQuyenDTO> permissions)
        {
            using (SqlConnection conn = new SqlConnection(dbHelper.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var p in permissions)
                        {
                            UpsertPhanQuyenInTransaction(conn, trans, maVaiTro.Trim(), p);
                        }

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        private void UpsertPhanQuyenInTransaction(SqlConnection conn, SqlTransaction trans, string maVaiTro, PhanQuyenDTO p)
        {
            string sql = @"
                IF EXISTS (SELECT 1 FROM PhanQuyen WHERE MaVaiTro = @MaVaiTro AND MaModule = @MaModule)
                BEGIN
                    UPDATE PhanQuyen
                    SET CoQuyenXem = @Xem,
                        CoQuyenThem = @Them,
                        CoQuyenSua = @Sua,
                        CoQuyenXoa = @Xoa
                    WHERE MaVaiTro = @MaVaiTro AND MaModule = @MaModule;
                END
                ELSE
                BEGIN
                    INSERT INTO PhanQuyen (MaVaiTro, MaModule, CoQuyenXem, CoQuyenThem, CoQuyenSua, CoQuyenXoa)
                    VALUES (@MaVaiTro, @MaModule, @Xem, @Them, @Sua, @Xoa);
                END";

            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaVaiTro", maVaiTro);
                cmd.Parameters.AddWithValue("@MaModule", p.MaModule);
                cmd.Parameters.AddWithValue("@Xem", p.CoQuyenXem ? 1 : 0);
                cmd.Parameters.AddWithValue("@Them", p.CoQuyenThem ? 1 : 0);
                cmd.Parameters.AddWithValue("@Sua", p.CoQuyenSua ? 1 : 0);
                cmd.Parameters.AddWithValue("@Xoa", p.CoQuyenXoa ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
