using System;

namespace X509Observer.Primitives.Network
{
    public class ApiKey
    {
        string Value { get; init; }
        DateTime ExpirationTime { get; init; }
    }
}
