using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;
using WebApplicationAPI.Models.Users;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        private IUserService _userService;
        private IMapper _mapper;


        public AccountController(
            IUserService userService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
            
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
        }

        //[Route("create")]
        //[HttpPost]
        //public async Task<IActionResult> CreateUser([FromBody] Usuario model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return BuildToken(model);
        //        }
        //        else
        //        {
        //            return BadRequest("Usuario o contraseña inválida");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }

        //}

        private IActionResult BuildToken(Usuario model)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
                new Claim(JwtRegisteredClaimNames.Email, model.Email.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Llave_secreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var _expiration = DateTime.UtcNow.AddHours(1);
            
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "miusuario.com",
               audience: "midomain.com",
               claims: claims,
               expires: _expiration,
               signingCredentials: creds);

            return Ok(new
            {
                email = model.Email,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = _expiration,
                ok = "true"
                
            }) ;
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] Usuario userInfo)
        //{
        //    //var rolUser = await _userManager.GetRolesAsync(userInfo.Email).ConfigureAwait(false);
        //    var usserEmail = await _userManager.FindByEmailAsync(userInfo.Email);

        //    if (usserEmail== null) 
        //    {
        //        throw new Exception("No existe una cuenta registrada con este email");
        //        //throw new Exception($"No hay una cuenta registrada con el email ${usserEmail}.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            return BuildToken(userInfo);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError( "ok", "false" );
        //            ModelState.AddModelError("msj", "credenciales no  validas");
        //            return BadRequest(ModelState);
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("register")]
        //public IActionResult Register([FromBody] RegisterModel model)
        //{
        //    // map model to entity
        //    var user = _mapper.Map<User>(model);

        //    try
        //    {
        //        // create user
        //        _userService.Create(user, model.Password);
        //        //return Ok();
        //        if (user.Email!=null)
        //        {
        //            return BuildToken(model);
        //        }
        //        else
        //        {
        //            return BadRequest("Usuario o contraseña inválida");
        //        }
        //    }
        //    catch (AppException ex) 
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        //-------------------------------------new register con token refresh------------------------
        //-------------------------------------------------------------------------------------------

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model, ipAddress());

            if (response == null)
                return BadRequest(new { ok = "false",  message = "error en las credenciales" });

            setTokenCookie(response.RefreshToken);

            return Ok(new
            {
                ok = "true",
                uid = response.Id,
                nombre = response.Nombre,
                email = response.Email,
                token = response.JWtToken,
                RefreshToken = response.RefreshToken,
            }) ;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<Entities.Usuario>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //[AllowAnonymous]
        [HttpPost("refresh")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("validartoken")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RefreshTokenUser()
        {
            var refreshToken = Request.Headers["x-token"];
            var response = _userService.ValidarToken(refreshToken);

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }



        //-------------------helpers methods-----------------
        //---------------------------------------------------
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
