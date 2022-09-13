using Common.Dtos;
using Common.Dtos.ConfigDtos;
using Common.Dtos.LocalityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IConfigRepository
    {
        Task<Response<DataAccess.Config>> SaveExpireAsync(DataAccess.Config request);
        Task<Response<DataAccess.Config>> GetExpireAsync();
        Task<Response<GetExpireDayDto>> GetExpireDayAsync();
    }
}
