using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OuterShellItem : MonoBehaviour, IInteractable
{
    [SerializeField] private OuterShell _shellData;

    public OuterShell ShellData => _shellData;

    public void Initialize(OuterShell shellData)
    {
        _shellData = shellData;
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (CraftingManager.Instance.CurrentState != CraftingState.PrepareShell) return;
        CraftingManager.Instance.StartStarPlacement();
    }

    public string GetInteractionText()
    {
        if (CraftingManager.Instance.CurrentState == CraftingState.PrepareShell)
        {
            return "Start Placing Stars";
        }

        return "";
    }
}