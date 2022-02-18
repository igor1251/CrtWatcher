using ElectronicDigitalSignatire.Models.Classes;
using System.Collections.Generic;

namespace WA4D0GWebPanel.ViewModels
{
    public class UsersListViewModel
    {
        public List<User> Users { get; set; }

        public UsersListViewModel(List<User> users)
        {
            Users = users;
        }
    }
}
