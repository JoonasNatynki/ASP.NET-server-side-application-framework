using System.Collections.Generic;

namespace GameWebApi.Data
{
    public class Player
    {
        public Player()
        {
            ItemsList = new List<Item>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Item> ItemsList { get; set; }
        public List<string> RolesList { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool Banned { get; set; }
    }
}