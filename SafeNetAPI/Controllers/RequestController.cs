using Microsoft.AspNetCore.Mvc;
using SafeNetAPI.Dto;
using SafeNetAPI.Models;
using SafeNetAPI.Services.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestInterface _requestInterface;

        public RequestController(IRequestInterface requestInterface)
        {
            _requestInterface = requestInterface;
        }

        [HttpPost("CreateRequest")]
        public async Task<ActionResult<ResponseModel<List<RequestModel>>>> CreateRequest(RequestCreationDto requestCreationDto)
        {
            var artists = await _requestInterface.CreateRequest(requestCreationDto);
            return Ok(artists);
        }

        [HttpGet("ListRequest")]

        public async Task<ActionResult<ResponseModel<List<RequestModel>>>> ListRequest()
        {
            var requests = await _requestInterface.ListRequest();
            return Ok(requests);
        }
    }
}