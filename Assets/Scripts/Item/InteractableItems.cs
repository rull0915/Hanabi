using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string _interactionText = "Pick Up";

    public virtual void Interact(PlayerInteraction playerInteraction)
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }

    public virtual string GetInteractionText()
    {
        return _interactionText;
    }
}
