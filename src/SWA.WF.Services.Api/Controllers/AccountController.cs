using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SWA.WF.Services.Api.Models.AccountViewModels;
using SWA.WF.Services.Api.Models;
using Microsoft.AspNetCore.Identity;
using SWA.WF.Services.Api.Authorization;

namespace SWA.WF.Services.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;      
        private readonly TokenDescriptor _tokenDescriptor;

        public AccountController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    ILoggerFactory loggerFactory,
                    TokenDescriptor tokenDescriptor) 
        {
            _userManager = userManager;
            _signInManager = signInManager;            
            _logger = loggerFactory.CreateLogger<AccountController>();
            _tokenDescriptor = tokenDescriptor;
        }

        private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        [HttpPost]
        [AllowAnonymous]
        [Route("nova-conta")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, int version)
        {
            if (version == 2)
            {
                return Response(false,"API V2 não disponível");
            }

            if (!ModelState.IsValid)
            {                
                return Response(false, "Usuario invalido");
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            
            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {
                    await _userManager.AddClaimAsync(user, new Claim("Personagem", "Ler"));
                    await _userManager.AddClaimAsync(user, new Claim("Especie", "Ler"));
                    _logger.LogInformation(1, "Usuario criado com sucesso!");
                    var response = await GerarTokenUsuario(new LoginViewModel { Email = model.Email, Senha = model.Senha });
                    return Response(true, "Usuario criado com sucesso!", response);
            }       
           
            return Response(false,"Não foi possivel cadastrar o usuário");          
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("conta")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {                
                return Response(false,"Login Invalido");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "Usuario logado com sucesso");
                var response = await GerarTokenUsuario(model);
                return Response(true,"Logado com sucesso",response);
            }
            
            return Response(false,"Falha ao realizar o login");
        }

        private async Task<object> GerarTokenUsuario(LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var userClaims = await _userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Necessário converver para IdentityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            var handler = new JwtSecurityTokenHandler();
            var signingConf = new SigningCredentialsConfiguration();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenDescriptor.Issuer,
                Audience = _tokenDescriptor.Audience,
                SigningCredentials = signingConf.SigningCredentials,
                Subject = identityClaims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid)
            });

            var encodedJwt = handler.WriteToken(securityToken);
            var User = await _userManager.FindByIdAsync(user.Id); 

            var response = new
            {
                access_token = encodedJwt,
                expires_in = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                user = new
                {
                    id = user.Id,                  
                    email = User.Email,
                    claims = userClaims.Select(c => new {c.Type, c.Value})
                }
            };

            return response;
        }
    }
}