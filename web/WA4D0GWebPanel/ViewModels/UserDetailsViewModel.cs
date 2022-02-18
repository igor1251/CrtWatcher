using ElectronicDigitalSignatire.Models.Classes;

namespace WA4D0GWebPanel.ViewModels
{
    public class UserDetailsViewModel
    {
        private User _user;

        public UserDetailsViewModel()
        {
            _user = new User();
        }

        public UserDetailsViewModel(User user)
        {
            _user = user;
        }

        public User User 
        {
            get => _user;
            set => _user = value;
        }
    }
}
