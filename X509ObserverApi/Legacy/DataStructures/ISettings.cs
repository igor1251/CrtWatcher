namespace DataStructures
{
    public interface ISettings
    {
        int VerificationFrequency { get; }
        string MainServerIP { get; }
        int MainServerPort { get; }
    }
}
