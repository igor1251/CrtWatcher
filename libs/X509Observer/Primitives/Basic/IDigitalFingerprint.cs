using System;

namespace X509Observer.Primitives.Basic
{
    public interface IDigitalFingerprint
    {
        int ID { get; }
        string Hash { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }
}
