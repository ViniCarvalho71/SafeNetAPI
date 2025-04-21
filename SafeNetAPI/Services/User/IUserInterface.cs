using SafeNetAPI.Dto;
using SafeNetAPI.Models;

namespace SafeNetAPI.Services.User
{
    public interface IUserInterface
    {
        public Task<ResponseModel<string>> RegisterUser (UserCreationDto userCreationDto);
    }
}
