using UnityEngine;

public class OuterShellItem : MonoBehaviour, IInteractable
{
    [SerializeField] private OuterShell _shellData;
    public OuterShell ShellData => _shellData;


    [SerializeField] private Transform _craftingCameraPoint;
    public Transform CraftingCameraPoint => _craftingCameraPoint;

    public CraftingManager _craftingManager;

    public void Initialize(OuterShell shellData)
    {
        _shellData = shellData;
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        if (_craftingManager.CurrentState != CraftingState.PrepareShell) return;

        _craftingManager.StartStarPlacement(_craftingCameraPoint);
    }

    public string GetInteractionText()
    {
        if (_craftingManager.CurrentState == CraftingState.PrepareShell)
        {
            return "Start Placing Stars";
        }

        return "";
    }
}