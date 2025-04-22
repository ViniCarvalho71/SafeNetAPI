using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafeNetAPI.Data;
using SafeNetAPI.Dto;
using SafeNetAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SafeNetAPI.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;


        public UserService(AppDbContext context, UserManager<UserModel> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;

        }


        public async Task<ResponseModel<string>> RegisterUser(UserCreationDto userCreationDto)
        {
            var response = new ResponseModel<string>();

            if (userCreationDto.Senha != userCreationDto.ConfirmarSenha)
            {
                response.Status = false;
                response.Message = "As senhas não coincidem.";
                return response;
            }

            var existingUser = await _userManager.FindByEmailAsync(userCreationDto.Email);
            if (existingUser != null)
            {
                response.Status = false;
                response.Message = "Usuário já existe com este e-mail.";
                return response;
            }

            var newUser = new UserModel
            {
                Email = userCreationDto.Email,
                UserName = userCreationDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, userCreationDto.Senha);
            if (!result.Succeeded)
            {
                response.Status = false;
                response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            // Gerar token pessoal e salvar na AspNetUserTokens
            var tokenValue = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            await _userManager.SetAuthenticationTokenAsync(
                newUser,
                "SafeNetAPI",
                "PersonalAccessToken",
                tokenValue
            );

            response.Status = true;
            response.Message = "Usuário registrado com sucesso e token gerado.";
            response.Data = tokenValue;

            return response;
        }
    }

    
    
}
