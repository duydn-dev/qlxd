using AutoMapper;
using Common.Dtos;
using Common.Dtos.ActionLogs;
using Common.Dtos.CreateApiDtos;
using Common.Dtos.DataManagerDtos;
using Common.Dtos.DepartmentDtos;
using Common.Dtos.DocumentDtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using Common.Dtos.GiaXangDauDoanhNgiepDtos;
using Common.Dtos.GiaXangDauDtos;
using Common.Dtos.PositionDtos;
using Common.Dtos.RoleDtos;
using Common.Dtos.StoreHouseDtos;
using Common.Dtos.TongNguonPhanGiaoDtos;
using Common.Dtos.UserPositionDtos;
using Common.Dtos.WarningSystemDtos;
using DataAccess;
//using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserCreateDto, User>().ForMember(x => x.UserRoles, g => g.Ignore());
            CreateMap<User, UserCreateDto>().ForMember(x => x.IsAdministrator, g => g.Ignore());

            CreateMap<UserPosition, PositonGetDropdownViewDto>();
            CreateMap<PositonGetDropdownViewDto, UserPosition>().ForMember(x => x.Users, g => g.Ignore());

            CreateMap<Department, DepartmentViewDto>();
            CreateMap<DepartmentViewDto, Department>().ForMember(n => n.Users, g => g.Ignore());

            CreateMap<UserPosition, UserPositionDto>();
            CreateMap<UserPositionDto, UserPosition>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>()
                .ForMember(n => n.UserRoles, n => n.Ignore())
                .ForMember(n => n.GroupRole, n => n.Ignore());

            CreateMap<GroupRole, GroupRoleAndRoleDto>().ForMember(n => n.Roles, n => n.MapFrom(g => g.Roles));
            CreateMap<GroupRoleAndRoleDto, GroupRole>().ForMember(n => n.Roles, n => n.MapFrom(g => g.Roles));

            CreateMap<GiayDangKyKinhDoanh, ListApiDataResponseDto>()
                .ForMember(l => l.TenDoiTuong, r => r.Ignore())
                .ForMember(l => l.TenTinh, r => r.Ignore())
                .ForMember(l => l.TenHuyen, r => r.Ignore())
                .ForMember(l => l.TenXa, r => r.Ignore());
            CreateMap<ListApiDataResponseDto, GiayDangKyKinhDoanh>();


            CreateMap<HeThongPhanPhoi, HeThongPhanPhoiDto>();
            CreateMap<HeThongPhanPhoiDto, HeThongPhanPhoi>();


            CreateMap<SanXuat, SanXuatDto>();
            CreateMap<SanXuatDto, SanXuat>();


            CreateMap<TieuThu, TieuThuDto>();
            CreateMap<TieuThuDto, TieuThu>();


            CreateMap<Nhap, NhapDto>();
            CreateMap<NhapDto, Nhap>();

            CreateMap<TonKho, TonKhoDto>();
            CreateMap<TonKhoDto, TonKho>();

            CreateMap<NguyenLieu, NguyenLieuDto>();
            CreateMap<NguyenLieuDto, NguyenLieu>();

            CreateMap<QuyBinhOnGium, QuyBinhOnGiaDto>();
            CreateMap<QuyBinhOnGiaDto, QuyBinhOnGium>();

            CreateMap<Khac, KhacDto>();
            CreateMap<KhacDto, Khac>();

            CreateMap<DoiTuongQuanLy, DoiTuongQuanLyViewDto>();
            CreateMap<DoiTuongQuanLyViewDto, DoiTuongQuanLy>();

            CreateMap<CreateQuyBinhOnGiumDto, QuyBinhOnGium>();
            CreateMap<QuyBinhOnGium, CreateQuyBinhOnGiumDto>();

            CreateMap<CreateKhacDto, Khac>();
            CreateMap<Khac, CreateKhacDto>();

            CreateMap<CreateNguyenLieuDto, NguyenLieu>();
            CreateMap<NguyenLieu, CreateNguyenLieuDto>();

            CreateMap<CreateTonKhoDto, TonKho>();
            CreateMap<TonKho, CreateTonKhoDto>();

            CreateMap<CreateNhapDto, Nhap>();
            CreateMap<Nhap, CreateNhapDto>();

            CreateMap<CreateTieuThuDto, TieuThu>();
            CreateMap<TieuThu, CreateTieuThuDto>();

            CreateMap<CreateSanXuatDto, SanXuat>();
            CreateMap<SanXuat, CreateSanXuatDto>();

            CreateMap<CreateHeThongPhanPhoiDto, HeThongPhanPhoi>();
            CreateMap<HeThongPhanPhoi, CreateHeThongPhanPhoiDto>();

            CreateMap<CreateGiayDangKyKinhdoanhDto, GiayDangKyKinhDoanh>();
            CreateMap<GiayDangKyKinhDoanh, CreateGiayDangKyKinhdoanhDto>();

            CreateMap<CreateActionLogDto, ActionLogs>();
            CreateMap<ActionLogs, CreateActionLogDto>();
            
            CreateMap<Document, DocumentViewDto>();
            CreateMap<DocumentViewDto, Document>();

            CreateMap<CreateGiaXangDauDto, GiaXangDau>();
            CreateMap<GiaXangDau, CreateGiaXangDauDto>();

            CreateMap<CreateWarningSystemDto,WarningSystem>();
            CreateMap<WarningSystem, CreateWarningSystemDto>();

            CreateMap<CreateStoreHouseDto, StoreHouse>();
            CreateMap<StoreHouse, CreateStoreHouseDto>();

            CreateMap<StoreHouseGetListDto, StoreHouse>();
            CreateMap<StoreHouse, StoreHouseGetListDto>();

            CreateMap<CreateGiaXangDauDoanhNghiepDto, GiaBanXangDauDoanhNghiep>();
            CreateMap<GiaBanXangDauDoanhNghiep, CreateGiaXangDauDoanhNghiepDto>();

            CreateMap<CreateTongNguonPhanGiaoDto, TongNguonPhanGiao>();
            CreateMap<CreateTongNguonPhanGiaoDto, TongNguonPhanGiao>();

            CreateMap<TongNguonPhanGiao, TongNguonPhanGiaoViewsDto>();
            CreateMap<TongNguonPhanGiaoViewsDto, TongNguonPhanGiao>();

            CreateMap<DuKienNhap, CreateDuKienNhapDto>();
            CreateMap<CreateDuKienNhapDto, DuKienNhap>();

            CreateMap<DuKienNhap, DuKienNhapDto>();
            CreateMap<DuKienNhapDto, DuKienNhap>();
        }
    }
}
