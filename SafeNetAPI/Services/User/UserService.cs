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
        private readonly UserManager<Models.User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Models.User> _passwordHasher;

        public UserService(AppDbContext context, UserManager<Models.User> userManager, IConfiguration configuration)
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

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userCreationDto.Email);

            if (existingUser != null)
            {
                response.Status = false;
                response.Message = "Usuário já existe com este e-mail.";
                return response;
            }

            var newUser = new Models.User
            {
                Email = userCreationDto.Email,
                UserName = userCreationDto.Email
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userCreationDto.Senha);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Agora sim: gerando e salvando o token
            var tokenValue = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            // Você precisa ter acesso ao UserManager aqui
            await _userManager.SetAuthenticationTokenAsync(
                newUser,
                "SafeNetAPI",           // Nome do provedor personalizado
                "PersonalAccessToken",  // Nome do token
                tokenValue
            );

            response.Status = true;
            response.Message = "Usuário registrado com sucesso e token gerado.";
            response.Data = tokenValue;

            return response;
        }
    }

    
    
}
