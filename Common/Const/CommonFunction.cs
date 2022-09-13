using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Common.Const.CommonConstant;

namespace Common.Const
{
    public static class CommonFunction
    {
        public static DataTable RenameHeaderAndConvertToDatatable<T>(this IEnumerable<T> data, List<string> header) where T: class
        {
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            DataTable table = new DataTable();
            if (header?.Count > 0)
            {
                header.ForEach(n => table.Columns.Add(n));
            }
            else
            {
                foreach (PropertyInfo prop in Props)
                {
                    table.Columns.Add(prop.Name);
                }
            }
               
            foreach (T item in data)  
            {  
                var values = new object[Props.Length];  
                for (int i = 0; i < Props.Length; i++)  
                {  
                    values[i] = Props[i].GetValue(item, null);  
                }
                table.Rows.Add(values);  
            }  
              
            return table;  
        }
        public static string DocumentStateName(int? state)
        {
            if (!state.HasValue)
                return "";

            string name;
            switch (state)
            {
                case (int)DocumentState.NotHad:
                    name = "Chưa có";
                    break;
                case (int)DocumentState.Had:
                    name = "Đã có";
                    break;
                case (int)DocumentState.Edited:
                    name = "Đã chỉnh sửa";
                    break;
                default:
                    name = "";
                    break;
            }
            return name;
        }
        public static double ValidRatio(double? divisor, double? dividend)
        {
            if((!dividend.HasValue && !dividend.HasValue) || (divisor != 0 && dividend == 0))
            {
                return 0;
            }
            var result = (divisor / dividend) * 100;
            return (result > 100) ? 100 : (result < 0) ? 0 : Math.Round(result.Value, 2);
        }
        public static string GetApprovedStatusName(int status)
        {
            switch (status)
            {
                case ApprovedStatus.Approve:
                    return "Đã duyệt";
                case ApprovedStatus.Inputed:
                    return "Chờ duyệt";
                case ApprovedStatus.Reject:
                    return "Từ chối";
                default:
                    return "";
            }
        }
        public static string GetLoaiSoHuu(int loaiSoHuu)
        {
            switch (loaiSoHuu)
            {
                case (int)LoaiSoHuuEnum.SoHuu:
                    return "Sở hữu";
                case (int)LoaiSoHuuEnum.DongSoHuu:
                    return "Đồng sở hữu";
                default:
                    return "";
            }
        }
        public static string GetLoaiBan(int loaiBan)
        {
            switch (loaiBan)
            {
                case (int)LoaiBanEnum.TrongNuoc:
                    return "Trong nước";
                case (int)LoaiBanEnum.XuatKhau:
                    return "Xuất khẩu";
                default:
                    return "";
            }
        }
        public static string GetLoaiNhap(int loaiNhap)
        {
            switch (loaiNhap)
            {
                case (int)LoaiNhapEnum.TrongNuoc:
                    return "Trong nước";
                case (int)LoaiNhapEnum.XuatKhau:
                    return "Xuất khẩu";
                default:
                    return "";
            }
        } 
        
        public static string GetLoaiDoiTuong(int loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case (int)LoaiDoiTuongEnum.Dmsx:
                    return "Thương nhân đầu mối sản xuất";
                case (int)LoaiDoiTuongEnum.Dmkd:
                    return "Thương nhân đầu mối kinh doanh";
                case (int)LoaiDoiTuongEnum.Tnpp:
                    return "Thương nhân phân phối và tổng lại lý kinh doanh";
                case (int)LoaiDoiTuongEnum.Dlbl:
                    return "Đại lý bán lẻ và thương nhân nhượng quyền bán lẻ";
                case (int)LoaiDoiTuongEnum.Ch:
                    return "Cửa hàng";
                default:
                    return "";
            }
        }

        public static string GetVungMien(int loaiDoiTuong)
        {
            switch (loaiDoiTuong)
            {
                case (int)VungMienEnum.Bac:
                    return "Miền bắc";
                case (int)VungMienEnum.Trung:
                    return "Miền trung";
                case (int)VungMienEnum.Nam:
                    return "Miền nam";
                default:
                    return "";
            }
        }
        public static string GetDonViTinh(int donvi)
        {
            switch (donvi)
            {
                case (int)DonViTinhEnum.M3:
                    return "Mét khối";
                case (int)DonViTinhEnum.Tan:
                    return "Tấn";
                default:
                    return "";
            }
        }
    }
}
