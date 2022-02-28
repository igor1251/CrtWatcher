using DataStructures;
using System.Collections.Generic;

namespace Site.Models
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }

        public UserDetailsViewModel(User user)
        {
            User = user;
        }
    }
}
