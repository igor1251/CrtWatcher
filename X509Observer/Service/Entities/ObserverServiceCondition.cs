namespace X509Observer.Service.Entities
{
    public enum ObserverServiceCondition
    {
        None,
        FirstLaunch,
        RegularLaunch,
        ConnectionError,
        CriticalError
    }
}
