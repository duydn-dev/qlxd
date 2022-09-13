using Common.Dtos;
using Common.Dtos.DocumentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IDocumentRepository
    {
        Task<Response<GetListResponseModel<List<DocumentViewDto>>>> GetListAsync(DocumentGetListRequestDto request);
        Task<Response<DocumentViewDto>> GetByIdAsync(long id);
        Task<Response<DocumentViewDto>> CreateAsync(DocumentViewDto request);
        Task<Response<DocumentViewDto>> UpdateAsync(DocumentViewDto request);
        Task<Response<DocumentViewDto>> DeleteAsync(long id);
    }
}
