namespace SuperWizardPlatformer.Input
{
    /// <summary>
    /// Abstracts the concept of a user action from the underlying input device used to signal
    /// the action. Any action that can be performed both through the mouse/keyboard and a game
    /// pad should go here.
    /// </summary>
    enum UserAction
    {
        MoveLeft = 0,
        MoveRight,
        Jump,
        Duck,
        MenuDown,
        MenuUp,
        MenuLeft,
        MenuRight,
        MenuAccept,
        MenuCancel
    }
}
