using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OuterShellItem : MonoBehaviour, IInteractable
{
    [SerializeField] private OuterShell _shellData;
    public OuterShell ShellData => _shellData;


    [SerializeField] private Transform _craftingCameraPoint;
    public Transform CraftingCameraPoint => _craftingCameraPoint;

    public void Initialize(OuterShell shellData)
    {
        _shellData = shellData;
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (CraftingManager.Instance.CurrentState != CraftingState.PrepareShell) return;

        CraftingManager.Instance.StartStarPlacement(_craftingCameraPoint);
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