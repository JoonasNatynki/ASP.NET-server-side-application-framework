using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GameWebApi.Data;

namespace GameWebApi.Common.Security
{
    public class BasicSecurityService : IBasicSecurityService
    {
        private readonly IRepository _repository;

        public BasicSecurityService(IRepository repository)
        {
            _repository = repository;
        }

        // This is where we check for the validity of the password hash against the given password and if they match
        public bool SetPrincipal(string playerId, string password)
        {
            //Get player from database
            var player = _repository.GetPlayer(playerId);

            //Validate password
            if (player.PasswordHash == GetPasswordHash(password, player.PasswordSalt))
            {
                //This somehow authenticates the claimer
                HttpContext.Current.User = GetPrincipal(player);
                return true;
            }
            else
            {
                return false;
            }
        }

        private IPrincipal GetPrincipal(Player player)
        {
            var identity = new GenericIdentity(player.Name, "basic");
            identity.AddClaim(new Claim(ClaimTypes.Name, player.Name));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, player.Id));

            foreach (var item in player.RolesList)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }
            return new ClaimsPrincipal(identity);
        }

        private string GetSalt()
        {
            var cryptoServiceProvider = new RNGCryptoServiceProvider();
            int maxLength = 32;
            byte[] salt = new byte[maxLength];
            cryptoServiceProvider.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private string GetPasswordHash(string password, string salt)
        {
            string stringToHash = salt + password;
            byte[] bytes = Encoding.UTF8.GetBytes(stringToHash);
            var sha256 = new SHA256Managed();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            string hashString = Convert.ToBase64String(hashBytes);

            return hashString;
        }

        public Data.Player AddSaltAndPasswordHash(Data.Player player, string password)
        {
            var salt = GetSalt();
            player.PasswordHash = GetPasswordHash(password, salt);
            player.PasswordSalt = salt;

            return player;
        }
    }
}
