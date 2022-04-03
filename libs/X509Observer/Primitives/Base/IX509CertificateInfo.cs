using System;

namespace X509Observer.Primitives.Base
{
    public interface IX509CertificateInfo
    {
        int ID { get; }
        string Hash { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }
}
