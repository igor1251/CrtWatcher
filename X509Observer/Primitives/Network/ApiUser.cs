namespace X509Observer.Primitives.Network
{
    public class ApiUser
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ApiKey Token { get; set; }
    }
}
