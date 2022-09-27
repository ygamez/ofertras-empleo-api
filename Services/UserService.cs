                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplicationAPI.Data;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;
using WebApplicationAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace WebApplicationAPI.Services
{
    public interface IUserService
        {
            AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
            Usuario Authenticate(string username, string password);
            AuthenticateResponse RefreshToken(string refreshToken, string ipAddress);
            AuthenticateResponse ValidarToken(string refreshToken);
            IEnumerable<Usuario> GetAll();
            Usuario GetById(int id);
            //User Create(User user, string password);
            void Update(Usuario user, string password = null);
            void Delete(int id);
            Usuario Create(Usuario user, string password);
    }

        public class UserService : IUserService
        {
            private ApplicationDbContext _context;

            private readonly AppSettings _appSettings;

            public UserService(ApplicationDbContext context, IOptions<AppSettings> appSettings )
                {
                    _context = context;
                    _appSettings = appSettings.Value ;
                }

            public Usuario Authenticate(string username, string password)
            {
                throw new NotImplementedException();
            }

            //public User Create(User usser, string password)
            //{
            //    //Validacion
            //    if (string.IsNullOrWhiteSpace(password))
            //        throw new Exception("Contraseña Obligatoria");

            //    if (_context.Users.Any( x=> x.Email == usser.Email ))
            //        throw new Exception("El usuario \"" + usser.Email + "\" ya existe");

            //    byte[] passwordHash, passwordSalt;
            //    CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //    usser.PasswordHash = passwordHash;
            //    usser.PasswordSalt = passwordSalt; 

            //    _context.Users.Add(usser);
            //    _context.SaveChanges();

            //    return usser;
            //}

            private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            public void Delete(int id)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Usuario> GetAll()
            {
                return (IEnumerable<Usuario>)_context.Usuarios;
            }

            public Usuario GetById(int id)
            {
                return _context.Usuarios.Find( id );
            }

            public void Update(Usuario user, string password = null)
            {
                throw new NotImplementedException();
            }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            //verific if the usser exits
            var usuario = _context.Usuarios.SingleOrDefault(x => x.Email  == model.Email );

            // check if username exists
            if (usuario == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(model.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(usuario);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            usuario.RefreshTokens.Add(refreshToken);
            _context.Update(usuario);
            _context.SaveChanges();

            //method whit token reflresh
            return new AuthenticateResponse(usuario, jwtToken, refreshToken.Token);

            ////method no token reflresh
            //return new AuthenticateResponse( usuario, jwtToken);
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        private RefreshToken generateRefreshToken(string ipAddress )
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        private string generateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Usuario Create(Usuario user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Usuarios.Any(x => x.Email == user.Email))
                throw new AppException("Username \"" + user.Nombre + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Usuarios.Add(user);
            _context.SaveChanges();

            return user;
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _context.Usuarios.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public AuthenticateResponse ValidarToken(string refreshToken)
        {
            var user = _context.Usuarios.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            // return null if no user found with token
            if (user == null) return null;

            var refreshTokenUser = user.RefreshTokens.Single(x => x.Token == refreshToken);

            // return null if token is no longer active
            if (!refreshTokenUser.IsActive) return null;

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, refreshTokenUser.Token);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       