using System.Text.Json.Serialization;
using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Models
{
    public class AuthenticateResponse
    {
        private Usuario user;
        private string token;
        private Usuario usuario;

        public int Id { get; set; }
        public bool OK { get; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string JWtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponse( Usuario user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            OK = true;
            Nombre = user.Email;
            Email = user.Email;
            JWtToken = jwtToken;
            RefreshToken = refreshToken;
        }

        public AuthenticateResponse(Usuario user, string jwtToken)
        {
            Id = user.Id;
            Nombre = user.Email;
            Email = user.Email;
            JWtToken = jwtToken;
        }
    }
}