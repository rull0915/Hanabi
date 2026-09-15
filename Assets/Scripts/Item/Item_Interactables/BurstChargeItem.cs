using UnityEngine;

public class BurstChargeItem : MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingManager _craftingManager;

    public void Interact(PlayerInteraction playerInteraction)
    {
        CraftingState state = _craftingManager.CurrentState;

        if (state != CraftingState.CompleteLayer1 && state != CraftingState.CompleteLayer2 && state != CraftingState.CompleteLayer3)
        {
            return;
        }

        _craftingManager.AddBurstCharge();

        gameObject.SetActive(false);

        _craftingManager.CompleteBurstCharge();
    }

    public string GetInteractionText()
    {
        CraftingState state = _craftingManager.CurrentState;

        if (state == CraftingState.CompleteLayer1 || state == CraftingState.CompleteLayer2 || state == CraftingState.CompleteLayer3)
        {
            return "Use Burst Charge";
        }

        return "";
    }
}