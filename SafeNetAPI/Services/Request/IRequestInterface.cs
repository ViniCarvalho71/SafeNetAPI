using SafeNetAPI.Dto;
using SafeNetAPI.Models;

namespace SafeNetAPI.Services.Request
{
    public interface IRequestInterface
    {
        Task<ResponseModel<List<RequestModel>>> ListRequest(string userId);
        Task<ResponseModel<List<RequestModel>>> CreateRequest(RequestCreationDto requestCreationDto, string userId);

    }
}
