using System.ComponentModel.DataAnnotations;

namespace X509ObserverAdmin.Models
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = "Адрес удаленного сервера регистрации пуст")]
        public string RemoteRegistrationServiceAddress { get; set; }

        [Required(ErrorMessage = "Адрес удаленного сервера аутентификации пуст")]
        public string RemoteAuthenticationServiceAddress { get; set; }

        [Required(ErrorMessage = "Адрес источника базы сертификатов пуст")]
        public string RemoteX509VaultStoreService { get; set; }

        [Required(ErrorMessage = "Укажите логин для подключения к серверу")]
        public string RemoteServiceLogin { get; set; }
        
        [Required(ErrorMessage = "Укажите пароль для подключения к серверу")]
        [DataType(DataType.Password)]
        public string RemoteServicePassword { get; set; }

        [Required(ErrorMessage = "Интервал проверки не указан")]
        public int MonitoringInterval { get; }

        public string ApiKey { get; set; }
    }
}
