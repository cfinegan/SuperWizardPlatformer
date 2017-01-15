namespace SuperWizardPlatformer
{
    /// <summary>
    /// Represents an object which can be marked for removal, so it can be cleaned up by 
    /// the class that owns it at a later time. This is distrinct from an IDisposable, which 
    /// is in charge of destroying its own resources.
    /// </summary>
    interface IRemovable
    {
        bool IsMarkedForRemoval { get; set; }
    }
}
