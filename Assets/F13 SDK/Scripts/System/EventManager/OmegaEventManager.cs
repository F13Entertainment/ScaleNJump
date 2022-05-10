using Assets.F13SDK.Scripts;

public class OmegaEventManager: OmegaSingletonManager<OmegaEventManager>
{
    /// <summary>
    /// These delegates are core delegates of the F13SDK. 
    /// Do not remove these delegates. 
    /// </summary>
    public delegate void GameStateHandler();
    public delegate void GameInputHandler();
    public delegate void GameLevelHandler();
    /// <summary>
    /// These delegates are the custom delegates for the game.
    /// You can use, remove or add. 
    /// </summary>
    
}
