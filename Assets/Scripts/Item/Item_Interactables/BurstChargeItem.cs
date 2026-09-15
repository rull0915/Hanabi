using UnityEngine;

public class BurstChargeItem : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteraction playerInteraction)
    {
        CraftingState state = CraftingManager.Instance.CurrentState;

        if (state != CraftingState.CompleteLayer1 && state != CraftingState.CompleteLayer2 && state != CraftingState.CompleteLayer3)
        {
            return;
        }

        CraftingManager.Instance.AddBurstCharge();

        gameObject.SetActive(false);

        CraftingManager.Instance.CompleteBurstCharge();
    }

    public string GetInteractionText()
    {
        CraftingState state = CraftingManager.Instance.CurrentState;

        if (state == CraftingState.CompleteLayer1 || state == CraftingState.CompleteLayer2 || state == CraftingState.CompleteLayer3)
        {
            return "Use Burst Charge";
        }

        return "";
    }
}