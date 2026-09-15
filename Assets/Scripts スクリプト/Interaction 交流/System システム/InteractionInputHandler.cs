using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionInputHandler : PersistentSingleton<InteractionInputHandler>
{
    public bool CheckClickInteractPressed()
    {
        return InputSystem.actions["Click Interact"].WasPressedThisFrame();
    }
}
