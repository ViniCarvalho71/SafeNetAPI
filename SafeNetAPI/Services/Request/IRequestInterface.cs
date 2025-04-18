using SafeNetAPI.Dto;
using SafeNetAPI.Models;

namespace SafeNetAPI.Services.Request
{
    public interface IRequestInterface
    {
        Task<ResponseModel<List<RequestModel>>> ListRequest();
        Task<ResponseModel<List<RequestModel>>> CreateRequest(RequestCreationDto requestCreationDto);

    }
}
