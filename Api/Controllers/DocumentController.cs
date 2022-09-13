using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.DocumentDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý tài liệu")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        [Route("")]
        [RoleDescription("Xem danh sách tài liệu")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<DocumentViewDto>>>> GetListAsync([FromBody]DocumentGetListRequestDto request)
        {
           return await _documentRepository.GetListAsync(request);
        }
        [Route("{id}")]
        [RoleDescription("Xem chi tiết tài liệu")]
        [HttpGet]
        public async Task<Response<DocumentViewDto>> GetByIdAsync([FromRoute] long id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        [Route("create")]
        [RoleDescription("Thêm mới tài liệu")]
        [HttpPost]
        public async Task<Response<DocumentViewDto>> CreateAsync([FromBody] DocumentViewDto request)
        {
            return await _documentRepository.CreateAsync(request);
        }

        [Route("update/{id}")]
        [RoleDescription("Cập nhật tài liệu")]
        [HttpPut]
        public async Task<Response<DocumentViewDto>> UpdateAsync([FromRoute] long id, [FromBody] DocumentViewDto request)
        {
            request.Id = id;
            return await _documentRepository.UpdateAsync(request);
        }

        [Route("{id}")]
        [RoleDescription("Xóa tài liệu")]
        [HttpDelete]
        public async Task<Response<DocumentViewDto>> DeleteAsync([FromRoute] long id)
        {
            return await _documentRepository.DeleteAsync(id);
        }
    }
}
