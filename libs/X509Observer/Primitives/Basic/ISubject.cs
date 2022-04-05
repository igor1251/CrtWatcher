using System.Collections.Generic;

namespace X509Observer.Primitives.Basic
{
    public interface ISubject
    {
        int ID { get; }
        string Name { get; }
        string Phone { get; set; }
        List<DigitalFingerprint> Fingerprints { get; set; }
    }
}
