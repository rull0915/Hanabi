using UnityEngine;

public class FireworkStarItem : MonoBehaviour, IInteractable
{
    [SerializeField] private FireworkStar _starData;

    public FireworkStar StarData => _starData;

    public void Interact(PlayerInteraction playerInteraction)
    {
        Debug.Log($"Interacted with {_starData.color} Star");
    }

    public string GetInteractionText()
    {
        return "Pick Up";
    }
}