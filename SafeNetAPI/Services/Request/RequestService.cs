using Microsoft.EntityFrameworkCore;
using SafeNetAPI.Data;
using SafeNetAPI.Dto;
using SafeNetAPI.Models;
using System.Security.Claims;

namespace SafeNetAPI.Services.Request
{
    public class RequestService : IRequestInterface
    {
        private readonly AppDbContext _context;
        public RequestService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<List<RequestModel>>> CreateRequest(RequestCreationDto requestCreationDto, string UserId)
        {
            ResponseModel<List<RequestModel>> response = new ResponseModel<List<RequestModel>>();
            try
            {
                var artist = new RequestModel()
                {
                    Agent = requestCreationDto.Agent,
                    Ip = requestCreationDto.Ip,
                    Path = requestCreationDto.Path,
                    Body = requestCreationDto.Body,
                    UserId = UserId,
                    Date = DateTime.Now

                };

                _context.Add(artist);
                await _context.SaveChangesAsync();

                response.Data = await _context.Request.ToListAsync();
                response.Message = $"Success";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<List<RequestModel>>> ListRequest(string userId)
        {
            ResponseModel<List<RequestModel>> response = new ResponseModel<List<RequestModel>>();

            try
            {
                var requests = await _context.Request.Where(u => u.UserId == userId).ToListAsync();
                response.Data = requests;
                response.Message = "Success";

                return response;
            } catch (Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;

                return response;
            }

        }
    }
}
