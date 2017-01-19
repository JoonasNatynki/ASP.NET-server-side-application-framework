using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace GameWebApi.Models
{
    /// <summary>
    /// This model is used for creating new players, it has the properties that player is allowed to define
    /// </summary>
    public class NewPlayer
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public int Score { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(16), MinLength(6)]
        public string PassWord { get; set; }
        public string[] Roles { get; set; }

        NewPlayer()
        {
            Roles = new string[] {};
        }
    }
}