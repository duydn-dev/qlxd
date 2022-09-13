using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.UserDtos;
using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý tài khoản")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            try
            {
                var response = await _userRepository.Login(request);
                return StatusCode(response.StatusCode, response.ResponseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể đăng nhập, vui lòng thử lại !");
            }
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<UserCreateDto>> Register([FromBody] UserCreateDto request)
        {
            request.UserId = Guid.NewGuid();
            request.CreatedBy = request.UserId;
            return await _userRepository.Create(request);
        }

        [RoleDescription("Xem danh sách tài khoản")]
        [Route("")]
        [HttpGet]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> GetFilter([FromQuery] GetListUserRequestDto request)
        {
            try
            {
                var response = await _userRepository.GetListUser(request);
                return Ok(response.ResponseData);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu người dùng, vui lòng thử lại !");
            }
        }

        [AuthenticationOnly]
        [Route("{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid userId)
        {
            try
            {
                var response = await _userRepository.GetUserByUserId(userId);
                if(response.StatusCode != StatusCodes.Status200OK) return StatusCode(response.StatusCode, response.Message);
                return Ok(response.ResponseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu người dùng, vui lòng thử lại !");
            }
        }

        [RoleDescription("Thêm mới tài khoản")]
        [Route("create")]
        [HttpPost]
        public async Task<Response<UserCreateDto>> Create([FromBody] UserCreateDto request)
        {
            return await _userRepository.Create(request);
        }

        [RoleDescription("Cập nhật tài khoản")]
        [Route("update/{userId}")]
        [HttpPut]
        public async Task<Response<UserCreateDto>> Update(Guid userId, [FromBody] UserCreateDto request)
        {
            request.UserId = userId;
            return await _userRepository.Update(request);
        }

        [RoleDescription("Xóa tài khoản")]
        [Route("delete/{userId}")]
        [HttpDelete]
        public async Task<Response<bool>> Delete(Guid userId)
        {
            return await _userRepository.Delete(userId);
        }

        [RoleDescription("Xóa nhiều tài khoản")]
        [Route("delete")]
        [HttpDelete]
        public async Task<Response<bool>> DeleteMany([FromQuery]List<Guid> userIds)
        {
            return await _userRepository.DeleteMany(userIds);
        }
        
        [RoleDescription("Upload avatar người dùng")]
        [Route("upload-avatar")]
        [HttpPost]
        public async Task<Response<string>> UploadAvatar()
        {
            return await _userRepository.UploadAvatar(Request.Form.Files[0]);
        }

        [AuthenticationOnly]
        [Route("profile/{userId}")]
        [HttpGet]
        public async Task<Response<UserProfileViewDto>> GetUserProfileAsync(Guid userId)
        {
            return await _userRepository.GetUserProfileAsync(userId);
        }

        [AuthenticationOnly]
        [Route("update-avatar/{userId}")]
        [HttpPut]
        public async Task<Response<bool>> UpdateAvatarAsync([FromRoute] Guid userId)
        {
            return await _userRepository.UpdateAvatarAsync(Request.Form.Files[0], userId);
        }

        [AuthenticationOnly]
        [Route("change-password/{id}")]
        [HttpPut]
        public async Task<Response<object>> ChangePasswordAsync([FromRoute] Guid id,[FromBody] ChangePassWordDto request)
        {
            return await _userRepository.ChangePasswordAsync(id, request);
        }
        [AuthenticationOnly]
        [Route("forgot-password/{id}")]
        [HttpPut]
        public async Task<Response<object>> ForgotPasswordAsync([FromRoute] Guid id)
        {
            return await _userRepository.ForgotPasswordAsync(id);
        }
        [AuthenticationOnly]
        [Route("get-by-locality/{type}/{parenId}")]
        [HttpGet]
        public async Task<Response<List<Locality>>> GetByLocalityAsync([FromRoute]int type,[FromRoute] int parenId)
        {
            return await _userRepository.GetByLocalityAsync(type,parenId);
        }
        [Route("get-token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] UserLoginDto request)
        {
            try
            {
                var response = await _userRepository.GetToken(request);
                if(response.StatusCode == StatusCodes.Status200OK)
                {
                    return Ok(response.ResponseData);
                }
                return StatusCode(response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không lấy token, vui lòng  thử lại !");
            }
        }

        #region SystemBackup
        //  // Connect to the local, default instance of SQL Server.   
        //  Server srv = new Server();
        //  // Reference the AdventureWorks2012 database.   
        //  Database db = default(Database);
        //  db = srv.Databases["AdventureWorks2012"];  

        //// Store the current recovery model in a variable.   
        //int recoverymod;
        //  recoverymod = (int) db.DatabaseOptions.RecoveryModel;

        //  // Define a Backup object variable.   
        //  Backup bk = new Backup();

        //  // Specify the type of backup, the description, the name, and the database to be backed up.   
        //  bk.Action = BackupActionType.Database;  
        //bk.BackupSetDescription = "Full backup of Adventureworks2012";  
        //bk.BackupSetName = "AdventureWorks2012 Backup";  
        //bk.Database = "AdventureWorks2012";  

        //// Declare a BackupDeviceItem by supplying the backup device file name in the constructor, and the type of device is a file.   
        //BackupDeviceItem bdi = default(BackupDeviceItem);
        //  bdi = new BackupDeviceItem("Test_Full_Backup1", DeviceType.File);

        //  // Add the device to the Backup object.   
        //  bk.Devices.Add(bdi);  
        //// Set the Incremental property to False to specify that this is a full database backup.   
        //bk.Incremental = false;  

        //// Set the expiration date.   
        //System.DateTime backupdate = new System.DateTime();
        //  backupdate = new System.DateTime(2006, 10, 5);  
        //bk.ExpirationDate = backupdate;  

        //// Specify that the log must be truncated after the backup is complete.   
        //bk.LogTruncation = BackupTruncateLogType.Truncate;  

        //// Run SqlBackup to perform the full database backup on the instance of SQL Server.   
        //bk.SqlBackup(srv);  

        //// Inform the user that the backup has been completed.   
        //System.Console.WriteLine("Full Backup complete.");  

        //// Remove the backup device from the Backup object.   
        //bk.Devices.Remove(bdi);  

        //// Make a change to the database, in this case, add a table called test_table.   
        //Table t = default(Table);
        //  t = new Table(db, "test_table");
        //  Column c = default(Column);
        //  c = new Column(t, "col", DataType.Int);
        //  t.Columns.Add(c);  
        //t.Create();  

        //// Create another file device for the differential backup and add the Backup object.   
        //BackupDeviceItem bdid = default(BackupDeviceItem);
        //  bdid = new BackupDeviceItem("Test_Differential_Backup1", DeviceType.File);

        //  // Add the device to the Backup object.   
        //  bk.Devices.Add(bdid);  

        //// Set the Incremental property to True for a differential backup.   
        //bk.Incremental = true;  

        //// Run SqlBackup to perform the incremental database backup on the instance of SQL Server.   
        //bk.SqlBackup(srv);  

        //// Inform the user that the differential backup is complete.   
        //System.Console.WriteLine("Differential Backup complete.");  

        //// Remove the device from the Backup object.   
        //bk.Devices.Remove(bdid);  

        //// Delete the AdventureWorks2012 database before restoring it  
        //// db.Drop();  

        //// Define a Restore object variable.  
        //Restore rs = new Restore();

        //  // Set the NoRecovery property to true, so the transactions are not recovered.   
        //  rs.NoRecovery = true;  

        //// Add the device that contains the full database backup to the Restore object.   
        //rs.Devices.Add(bdi);  

        //// Specify the database name.   
        //rs.Database = "AdventureWorks2012";  

        //// Restore the full database backup with no recovery.   
        //rs.SqlRestore(srv);  

        //// Inform the user that the Full Database Restore is complete.   
        //Console.WriteLine("Full Database Restore complete.");  

        //// reacquire a reference to the database  
        //db = srv.Databases["AdventureWorks2012"];  

        //// Remove the device from the Restore object.  
        //rs.Devices.Remove(bdi);  

        //// Set the NoRecovery property to False.   
        //rs.NoRecovery = false;  

        //// Add the device that contains the differential backup to the Restore object.   
        //rs.Devices.Add(bdid);  

        //// Restore the differential database backup with recovery.   
        //rs.SqlRestore(srv);  

        //// Inform the user that the differential database restore is complete.   
        //System.Console.WriteLine("Differential Database Restore complete.");  

        //// Remove the device.   
        //rs.Devices.Remove(bdid);  

        //// Set the database recovery mode back to its original value.  
        //db.RecoveryModel = (RecoveryModel) recoverymod;

        //  // Drop the table that was added.   
        //  db.Tables["test_table"].Drop();
        //  db.Alter();  

        //// Remove the backup files from the hard disk.  
        //// This location is dependent on the installation of SQL Server  
        //System.IO.File.Delete("C:\\Program Files\\Microsoft SQL Server\\MSSQL12.MSSQLSERVER\\MSSQL\\Backup\\Test_Full_Backup1");  
        //System.IO.File.Delete("C:\\Program Files\\Microsoft SQL Server\\MSSQL12.MSSQLSERVER\\MSSQL\\Backup\\Test_Differential_Backup1");  
        #endregion
    }
}
