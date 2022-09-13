using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BusinessLogic.BaseRepository;
using BusinessLogic.Config;
using BusinessLogic.Contracts;
using BusinessLogic.Repository;
using BusinessLogic.UnitOfWork;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Authenticator;

namespace Api.Installer
{
    public static class InstallerService
    {
        [Obsolete]
        public static void Installer(this IServiceCollection services, IConfiguration Configuration)
        {
            // add dbcontext
            services.AddHttpContextAccessor();
            services.AddDbContext<DuLieuXangDauContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));
            //add cache service
            services.AddMemoryCache();

            //add authen
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            // add base service
            services.AddTransient<Func<DuLieuXangDauContext>>((provider) => () => provider.GetService<DuLieuXangDauContext>());
            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
            services.AddSingleton<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IStoreProcedureRepository<>), typeof(StoreProcedureRepository<>));
            // add user-defined service

            services.AddSingleton<TwoFactorAuthenticator>();
            services.AddTransient<IActionLogsRepository, ActionLogsRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IDataManagerRepository, DataManagerRepository>();
            services.AddTransient<IDoiTuongQuanLyRepository, DoiTuongQuanLyRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IGiaXangDauRepository, GiaXangDauRepository>();
            services.AddTransient<IStoreHouseRepository, StoreHouseRepository>();
            services.AddTransient<IWarningSystemRepository, WarningSystemRepository>();
            services.AddTransient<ITongNguonPhanGiaoRepository, TongNguonPhanGiaoRepository>();
        }
    }
}
