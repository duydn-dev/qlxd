﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class ListApiDataResponseDto
    {
        public int MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public string TenDoanhNghiep { get; set; }
        public string SoDkkd { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public int? DiaChiXa { get; set; }
        public string TenXa { get; set; }
        public int? DiaChiHuyen { get; set; }
        public string TenHuyen { get; set; }
        public int? DiaChiTinh { get; set; }
        public string TenTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string NguoiDaiDienTen { get; set; }
        public string NguoiDaiDienDob { get; set; }
        public string NguoiDaiDienCccd { get; set; }
        public string NguoiDaiDienSdt { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public long Id { get; set; }
        public int Version { get; set; }
        public int AprrovedStatus { get; set; }
        public Guid? AprrovedBy { get; set; }
        public int MaPhienBan { get; set; }
        public string RejectMessage { get; set; }
        public DateTime? AprrovedDate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
