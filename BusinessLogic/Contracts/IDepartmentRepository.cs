using Common.Dtos;
using Common.Dtos.DepartmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IDepartmentRepository
    {
        Task<Response<GetListResponseModel<List<DepartmentViewDto>>>> GetDepartments(DepartmentGetPageDto request);
        Task<Response<DepartmentViewDto>> GetByIdAsync(Guid id);
        Task<Response<DepartmentViewDto>> CreateAsync(DepartmentViewDto request);
        Task<Response<DepartmentViewDto>> EditAsync(DepartmentViewDto request);
        Task<Response<DepartmentViewDto>> DeleteAsync(Guid id);
    }
}
