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
                    IsMalicious = requestCreationDto.IsMalicious,
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

        public async Task<ResponseModel<List<IpCountDto>>> ListTopIp(string userId)
        {
            ResponseModel<List<IpCountDto>> response = new ResponseModel<List<IpCountDto>>();
            try
            {
                var request = await _context.Request.
                    Where(u => u.UserId == userId && u.IsMalicious == 1).GroupBy(x => x.Ip).
                    Select(g => new IpCountDto 
                    {
                        Ip = g.Key,
                        Quantidade = g.Count()
                    }).
                    OrderByDescending(g => g.Quantidade).
                    ToListAsync();
                
                response.Data = request;
                response.Message = "Success";
                
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;

                return response;
            }
        }
        public async Task<ResponseModel<List<PathCountDto>>> ListTopPath(string userId)
        {
            ResponseModel<List<PathCountDto>> response = new ResponseModel<List<PathCountDto>>();
            try
            {
                var request = await _context.Request.
                    Where(u => u.UserId == userId && u.IsMalicious == 1).
                    GroupBy(x => x.Path).
                    Select(g => new PathCountDto 
                    {
                        Path = g.Key,
                        Quantidade = g.Count()
                    }).
                    OrderByDescending(g => g.Quantidade).
                    ToListAsync();
                
                response.Data = request;
                response.Message = "Success";
                
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;

                return response;
            }
        }
    }
}
