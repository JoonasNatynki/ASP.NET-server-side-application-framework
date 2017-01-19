using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using GameWebApi.Data;

namespace GameWebApi.Common.Security
{
    public interface IBasicSecurityService
    {
        bool SetPrincipal(string playerId, string password);
        Player AddSaltAndPasswordHash(Player player, string password);
    }
}
