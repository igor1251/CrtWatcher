using System.Collections.Generic;

namespace X509Observer.Primitives.Base
{
    public interface ISubject
    {
        int ID { get; }
        string Name { get; }
        string Phone { get; set; }
        IEnumerable<DigitalFingerprint> Fingerprints { get; set; }
    }
}
