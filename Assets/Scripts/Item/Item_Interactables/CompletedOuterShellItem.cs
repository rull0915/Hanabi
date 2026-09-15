using UnityEngine;

public class CompletedOuterShellItem : MonoBehaviour, IInteractable
{
    public CraftingManager _manager;

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (_manager.CurrentState != CraftingState.PrepareCloseShell) return;

        _manager.StartCloseShell();
    }

    public string GetInteractionText()
    {
        if (_manager.CurrentState == CraftingState.PrepareCloseShell) return "Close Shell";

        return "";
    }
}