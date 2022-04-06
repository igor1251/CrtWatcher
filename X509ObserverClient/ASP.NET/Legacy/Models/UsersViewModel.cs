using DataStructures;
using System.Collections.Generic;

namespace Site.Models
{
    public class UsersViewModel
    {
        public List<User> AvailableUsers { get; set; }

        public UsersViewModel(List<User> users)
        {
            AvailableUsers = users;
        }
    }
}
