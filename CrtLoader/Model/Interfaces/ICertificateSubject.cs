namespace CrtLoader.Model.Interfaces
{
    public interface ICertificateSubject
    {
        int ID { get; set; }
        string SubjectName { get; set; }
        string SubjectPhone { get; set; }
        string SubjectComment { get; set; }
    }
}
