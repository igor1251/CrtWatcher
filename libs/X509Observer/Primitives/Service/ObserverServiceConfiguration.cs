﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using X509Observer.Primitives.Network;

namespace X509Observer.Primitives.Service
{
    public class ObserverServiceConfiguration : IObserverServiceConfiguration
    {
        private ObserverServiceCondition _Condition;
        private ConnectionInfo _ConnectionInfo;

        [Required]
        [JsonPropertyName("condition")]
        public ObserverServiceCondition Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        [Required]
        [JsonPropertyName("connection-info")]
        public ConnectionInfo ConnectionInfo
        {
            get { return _ConnectionInfo; }
            set { _ConnectionInfo = value; }
        }
    }
}
