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

        public async Task<ResponseModel<RequestListDto>> ListRequest(string userId, string? search, int page,int pageSize)
        {
            ResponseModel<RequestListDto> response = new ResponseModel<RequestListDto>();

            try
            {
                var consultaPorUsuario = _context.Request.Where(u => u.UserId == userId);
                
                var query = consultaPorUsuario;
               
                DateTime? searchDate = null;
                bool isDate = DateTime.TryParse(search, out var parsedDate);
                if (isDate)
                {
                    searchDate = parsedDate.Date;
                }

                if (!string.IsNullOrEmpty(search))
                {
                    query = consultaPorUsuario
                        .Where(r =>
                            r.Ip.Contains(search) ||
                            r.Agent.Contains(search) ||
                            r.Body.Contains(search) ||
                            r.Path.Contains(search) ||
                            (isDate && r.Date.Date == searchDate.Value)
                        );
                }

                RequestListDto resposta = new RequestListDto(query.Skip((page - 1) * pageSize).Take(pageSize).ToList(), query.Count());

                   
                
                
                response.Data = resposta;
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

        public async Task<ResponseModel<KpisDto>> ListKpis(string userId)
        {
            ResponseModel<KpisDto> response = new ResponseModel<KpisDto>();
            try
            {
                var ultimas24h = DateTime.UtcNow.AddHours(-24);
                var query_from_this_user = _context.Request.Where(u => u.UserId == userId);
                KpisDto kpis = new KpisDto
                {
                    RequisicoesBenignas = query_from_this_user.Count(r => r.IsMalicious == 0),
                    RequisicoesMaliciosas = query_from_this_user.Count(r => r.IsMalicious == 1),
                    RequisicoesNasUltimasVinteQuatroHoras = query_from_this_user.Count(r => r.Date >= ultimas24h)
                };
                
                
                response.Data = kpis;
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
