using System.Collections.Generic;

namespace X509Observer.Primitives.Base
{
    public interface IX509Subject
    {
        int ID { get; }
        string Name { get; }
        string Phone { get; set; }
        IEnumerable<IX509CertificateInfo> Certificates { get; set; }
    }
}
