using SafeNetAPI.Dto;
using SafeNetAPI.Models;

namespace SafeNetAPI.Services.Request
{
    public interface IRequestInterface
    {
        Task<ResponseModel<List<RequestModel>>> ListRequest(string userId, string search);
        Task<ResponseModel<List<RequestModel>>> CreateRequest(RequestCreationDto requestCreationDto, string userId);
        Task<ResponseModel<List<IpCountDto>>> ListTopIp(string userId);
        
        Task<ResponseModel<List<PathCountDto>>> ListTopPath(string userId);
        Task<ResponseModel<KpisDto>> ListKpis(string userId);

    }
}
