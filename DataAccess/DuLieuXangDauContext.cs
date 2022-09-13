using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess
{
    public partial class DuLieuXangDauContext : DbContext
    {
        public DuLieuXangDauContext()
        {
        }

        public DuLieuXangDauContext(DbContextOptions<DuLieuXangDauContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CauHinh> CauHinhs { get; set; }
        public virtual DbSet<ChungLoai> ChungLoais { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DoiTuongQuanLy> DoiTuongQuanLies { get; set; }
        public virtual DbSet<GiayDangKyKinhDoanh> GiayDangKyKinhDoanhs { get; set; }
        public virtual DbSet<GiayDangKyKinhDoanhVer> GiayDangKyKinhDoanhVers { get; set; }
        public virtual DbSet<GroupRole> GroupRoles { get; set; }
        public virtual DbSet<GroupRoleUserPosition> GroupRoleUserPositions { get; set; }
        public virtual DbSet<HeThongPhanPhoi> HeThongPhanPhois { get; set; }
        public virtual DbSet<Khac> Khacs { get; set; }
        public virtual DbSet<LoaiMatHang> LoaiMatHangs { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<NguyenLieu> NguyenLieus { get; set; }
        public virtual DbSet<Nhap> Nhaps { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<QuyBinhOnGium> QuyBinhOnGia { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SanXuat> SanXuats { get; set; }
        public virtual DbSet<TieuThu> TieuThus { get; set; }
        public virtual DbSet<TinhHuyenXa> TinhHuyenXas { get; set; }
        public virtual DbSet<TonKho> TonKhos { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPosition> UserPositions { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<ZoneNews> ZoneNews { get; set; }
        public virtual DbSet<Locality> Localities { get; set; }
        public virtual DbSet<ActionLogs> ActionLogs { get; set; }
        public virtual DbSet<UsersReference> UsersReferences { get; set; }
        public virtual DbSet<GiaXangDau> GiaXangDaus { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<StoreHouseCategory> StoreHouseCategories { get; set; }
        public virtual DbSet<WarningSystem> WarningSystems { get; set; }
        public virtual DbSet<GiaBanXangDauDoanhNghiep> GiaBanXangDauDoanhNghieps { get; set; }
        public virtual DbSet<TongNguonPhanGiao> TongNguonPhanGiaos { get; set; }
        public virtual DbSet<DuKienNhap> DuKienNhaps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.74.122.164,26888;Database=DuLieuXangDau;User Id=o2tech.vn;Password=o2tech.vn123!!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CauHinh>(entity =>
            {
                entity.HasKey(e => e.MaChungLoai);

                entity.ToTable("CauHinh");

                entity.Property(e => e.MaChungLoai).ValueGeneratedNever();

                entity.Property(e => e.DonViTinh).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenDonVi).HasMaxLength(100);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<Locality>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Localities");
            });

            modelBuilder.Entity<ChungLoai>(entity =>
            {
                entity.HasKey(e => e.MaChungLoai);

                entity.ToTable("ChungLoai");

                entity.Property(e => e.MaChungLoai).ValueGeneratedNever();

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenChungLoai).HasMaxLength(500);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CmtOrCccdnguoiDaiDienPhapLuat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CmtOrCCCDNguoiDaiDienPhapLuat");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MaSoThue).HasMaxLength(200);

                entity.Property(e => e.NameCp).HasMaxLength(200);

                entity.Property(e => e.NgayThangNamSinhNguoiDaiDienPhapLuat)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SdtlienHe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SDTLienHe");

                entity.Property(e => e.SdtlienHeNguoiDaiDienPhapLuat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SDTLienHeNguoiDaiDienPhapLuat");

                entity.Property(e => e.SoGiayDangKyKinhDoanh).HasMaxLength(200);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenNguoiDaiDienPhapLuat).HasMaxLength(200);

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.Property(e => e.ConfigId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).ValueGeneratedNever();
            });

            modelBuilder.Entity<DoiTuongQuanLy>(entity =>
            {
                entity.HasKey(e => e.MaDoiTuong);

                entity.ToTable("DoiTuongQuanLy");

                entity.Property(e => e.TenDoiTuong).HasMaxLength(500);

                entity.Property(e => e.ToBen).HasMaxLength(500);

                entity.Property(e => e.WhiteListIp)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasColumnType("int")
                    .HasDefaultValueSql("(0)");
            });

            modelBuilder.Entity<GiayDangKyKinhDoanh>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("GiayDangKyKinhDoanh");

                entity.Property(e => e.MaDoiTuong).ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.DiaChiHuyen).HasColumnName("DiaChi_Huyen");

                entity.Property(e => e.DiaChiTinh).HasColumnName("DiaChi_Tinh");

                entity.Property(e => e.DiaChiXa).HasColumnName("DiaChi_Xa");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MaSoThue).HasMaxLength(500);

                entity.Property(e => e.NguoiDaiDienCccd)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_CCCD");

                entity.Property(e => e.NguoiDaiDienDob)
                    .HasMaxLength(50)
                    .HasColumnName("NguoiDaiDien_DOB");

                entity.Property(e => e.NguoiDaiDienSdt)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_SDT");

                entity.Property(e => e.NguoiDaiDienTen)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_Ten");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SoDkkd)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SoDKKD");

                entity.Property(e => e.TenDoanhNghiep).HasMaxLength(500);

                entity.Property(e => e.ThoiGianCapNhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GiayDangKyKinhDoanhVer>(entity =>
            {
                entity.HasKey(e => e.MaDoiTuong);

                entity.ToTable("GiayDangKyKinhDoanhVer");

                entity.Property(e => e.MaDoiTuong).ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.DiaChiHuyen).HasColumnName("DiaChi_Huyen");

                entity.Property(e => e.DiaChiTinh).HasColumnName("DiaChi_Tinh");

                entity.Property(e => e.DiaChiXa).HasColumnName("DiaChi_Xa");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MaSoThue).HasMaxLength(500);

                entity.Property(e => e.NguoiDaiDienCccd)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_CCCD");

                entity.Property(e => e.NguoiDaiDienDob)
                    .HasMaxLength(50)
                    .HasColumnName("NguoiDaiDien_DOB");

                entity.Property(e => e.NguoiDaiDienSdt)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_SDT");

                entity.Property(e => e.NguoiDaiDienTen)
                    .HasMaxLength(500)
                    .HasColumnName("NguoiDaiDien_Ten");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SoDkkd)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SoDKKD");

                entity.Property(e => e.TenDoanhNghiep).HasMaxLength(500);

                entity.Property(e => e.ThoiGianCapNhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ver).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.ToTable("GroupRole");

                entity.Property(e => e.GroupRoleId).ValueGeneratedNever();
            });

            modelBuilder.Entity<GroupRoleUserPosition>(entity =>
            {
                entity.Property(e => e.GroupRoleUserPositionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<HeThongPhanPhoi>(entity =>
            {
                entity.HasKey(e => new { e.Id});

                entity.ToTable("HeThongPhanPhoi");

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.DienThoai).HasMaxLength(500);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.TenHeThongPhanPhoi).HasMaxLength(500);

                entity.Property(e => e.ThoiGianCapNhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Khac>(entity =>
            {
                entity.ToTable("Khac");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<ActionLogs>(entity =>
            {
                entity.ToTable("ActionLogs");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ContentLog).HasMaxLength(1000);
                entity.Property(e => e.Url).HasMaxLength(1000);

                entity.Property(e => e.TimeLine)
                    .HasColumnType("datetime")
                    .HasColumnName("TimeLine")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<UsersReference>(entity =>
            {
                entity.ToTable("UsersReferences");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.EncryptUser).HasMaxLength(1000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDate")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<GiaXangDau>(entity =>
            {
                entity.ToTable("GiaXangDaus");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DotBienDong)
                  .HasColumnType("datetime")
                  .HasColumnName("DotBienDong")
                  .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgayCapNhat)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayCapNhat")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<LoaiMatHang>(entity =>
            {
                entity.ToTable("LoaiMatHang");

                entity.Property(e => e.LoaiMatHangId).HasColumnName("LoaiMatHangID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Acc);

                entity.Property(e => e.Acc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("acc");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnName("address");

                entity.Property(e => e.Birthday)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("birthday");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.InfomationOther)
                    .HasMaxLength(600)
                    .HasColumnName("infomationOther");

                entity.Property(e => e.Lever)
                    .HasColumnName("lever")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Pass)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.PassReset)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passReset");

                entity.Property(e => e.Point)
                    .HasColumnName("point")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Tel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<NguyenLieu>(entity =>
            {
                entity.ToTable("NguyenLieu");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.DonViTinh).HasMaxLength(500);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenNguyenLieu).HasMaxLength(500);
            });

            modelBuilder.Entity<Nhap>(entity =>
            {
                entity.ToTable("Nhap");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(2000);
            });

            modelBuilder.Entity<QuyBinhOnGium>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.HasOne(d => d.GroupRole)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.GroupRoleId);
            });

            modelBuilder.Entity<SanXuat>(entity =>
            {
                entity.ToTable("SanXuat");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TieuThu>(entity =>
            {
                entity.ToTable("TieuThu");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TinhHuyenXa>(entity =>
            {
                entity.ToTable("TinhHuyenXa");

                entity.Property(e => e.TinhHuyenXaId).HasColumnName("TinhHuyenXaID");

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TenTinhHuyenXa).HasMaxLength(1000);
            });

            modelBuilder.Entity<TonKho>(entity =>
            {
                entity.ToTable("TonKho");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NgayBc)
                    .HasColumnType("datetime")
                    .HasColumnName("NgayBC")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.DepartmentId, "IX_Users_DepartmentId");

                entity.HasIndex(e => e.UserPositionId, "IX_Users_UserPositionId");

                entity.Property(e => e.UserId).ValueGeneratedNever();
                entity.Property(e => e.IsBCT).HasDefaultValueSql("(0)");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DepartmentId);

                entity.HasOne(d => d.UserPosition)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserPositionId);
            });

            modelBuilder.Entity<UserPosition>(entity =>
            {
                entity.ToTable("UserPosition");

                entity.Property(e => e.UserPositionId).ValueGeneratedNever();

                entity.Property(e => e.LocalityAccept).HasMaxLength(20);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.UserRoleId).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<ZoneNews>(entity =>
            {
                entity.HasKey(e => e.ZoneId);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");
            });
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.ToTable("Documents");
                entity.Property(e => e.DateIssued)
                    .HasColumnType("datetime");
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");
            });
            modelBuilder.Entity<StoreHouseCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.ToTable("StoreHouseCategories");
                entity.Property(e => e.Name).HasMaxLength(500);
                entity.Property(e => e.NumNo).HasMaxLength(20);
            });
            modelBuilder.Entity<StoreHouse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.ToTable("StoreHouses");
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.UnitManager).HasMaxLength(200);
                entity.Property(e => e.StoreName).HasMaxLength(500);
                entity.Property(e => e.Nature).HasMaxLength(200);
                entity.Property(e => e.ZoneOfInfluence).HasMaxLength(500);
                entity.Property(e => e.Dwt).HasMaxLength(200);
            });
            modelBuilder.Entity<WarningSystem>(entity =>
            {
                entity.ToTable("WarningSystems");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Title).HasMaxLength(2000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDate")
                    .HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<GiaBanXangDauDoanhNghiep>(entity =>
            {
                entity.ToTable("GiaBanXangDauDoanhNghieps");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DonViTinh).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDate");
            });
            modelBuilder.Entity<TongNguonPhanGiao>(entity =>
            {
                entity.ToTable("TongNguonPhanGiaos");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDate");
            });
            modelBuilder.Entity<DuKienNhap>(entity =>
            {
                entity.ToTable("DuKienNhaps");
                entity.HasKey(n => n.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDate");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
