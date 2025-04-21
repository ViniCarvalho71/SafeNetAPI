using Microsoft.AspNetCore.Mvc;
using SafeNetAPI.Dto;
using SafeNetAPI.Models;

using SafeNetAPI.Services.User;

namespace SafeNetAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;

        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }
        [HttpPost("registrar")]
        public async Task<ActionResult<ResponseModel<object>>> Registrar([FromBody] UserCreationDto userCreationDto)
        {
            var resultado = await _userInterface.RegisterUser(userCreationDto);

            if (!resultado.Status)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}
