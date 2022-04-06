namespace X509Observer.Primitives.Service
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
