using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeNetAPI.Dto;
using SafeNetAPI.Models;
using SafeNetAPI.Services.Request;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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
        [Authorize]
        [HttpPost("CreateRequest")]
        public async Task<ActionResult<ResponseModel<List<RequestModel>>>> CreateRequest(RequestCreationDto requestCreationDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }
            var artists = await _requestInterface.CreateRequest(requestCreationDto , userId);
            return Ok(artists);
        }

        [Authorize]
        [HttpGet("ListRequest")]
        public async Task<ActionResult<ResponseModel<List<RequestModel>>>> ListRequest()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var requests = await _requestInterface.ListRequest(userId);
            return Ok(requests);
        }
    }
}