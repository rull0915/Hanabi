using UnityEngine;

public class CompletedOuterShellItem : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteraction playerInteraction)
    {
        if (CraftingManager.Instance.CurrentState != CraftingState.PrepareCloseShell) return;

        CraftingManager.Instance.StartCloseShell();
    }

    public string GetInteractionText()
    {
        if (CraftingManager.Instance.CurrentState == CraftingState.PrepareCloseShell) return "Close Shell";

        return "";
    }
}