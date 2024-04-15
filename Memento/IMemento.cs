namespace Memento
{
    /// <summary>
    /// An interface for every type of Memento, each action (like adding/deleting GraphicObjects, adding/deleting
    /// Layer,...) should have its own Memento derived from this interface
    /// </summary>
    public interface IMemento
    {
        string GetName();
        object GetState();
    }
}
