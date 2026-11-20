using System;

namespace QuanLyKyTucXa.DTO
{
    public class PhanQuyenDTO
    {
        public string MaVaiTro { get; set; }
        public string MaModule { get; set; }
        public string TenModule { get; set; }
        public bool CoQuyenXem { get; set; }
        public bool CoQuyenThem { get; set; }
        public bool CoQuyenSua { get; set; }
        public bool CoQuyenXoa { get; set; }

        public PhanQuyenDTO() { }

        public PhanQuyenDTO(string maVaiTro, string maModule, bool xem, bool them, bool sua, bool xoa)
        {
            MaVaiTro = maVaiTro;
            MaModule = maModule;
            TenModule = ModuleSystem.GetDisplayName(maModule);
            CoQuyenXem = xem;
            CoQuyenThem = them;
            CoQuyenSua = sua;
            CoQuyenXoa = xoa;
        }
    }
}
