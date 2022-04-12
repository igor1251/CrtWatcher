using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X509Observer.Service.Entities
{
    public class ObserverServiceConfiguration
    { 
        private ObserverServiceCondition _Condition;
        //private ConnectionInfo _ConnectionInfo;

        public ObserverServiceConfiguration()
        {
            _Condition = ObserverServiceCondition.None;
            //_ConnectionInfo = new ConnectionInfo();
        }

        [Required]
        [JsonPropertyName("condition")]
        public ObserverServiceCondition Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        //[Required]
        //[JsonPropertyName("connection-info")]
        //public ConnectionInfo ConnectionInfo
        //{
        //    get { return _ConnectionInfo; }
        //    set { _ConnectionInfo = value; }
        //}
    }
}
