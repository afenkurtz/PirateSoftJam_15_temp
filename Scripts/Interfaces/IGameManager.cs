public interface IGameManager
{
    ManagerStatus Status { get; }

    void Startup();
}

public enum ManagerStatus
{
    Shutdown,
    Initializing,
    Started
}