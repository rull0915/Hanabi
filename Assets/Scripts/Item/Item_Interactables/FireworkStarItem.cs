using UnityEngine;

public class FireworkStarItem : MonoBehaviour, IInteractable
{
    [SerializeField] private FireworkStar _starData;
    public CraftingManager _craftingManager;

    public FireworkStar StarData => _starData;

    public void Initialize(FireworkStar starData)
    {
        _starData = starData;
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        _craftingManager.TryStartNextStarLayer(_starData);
    }

    public string GetInteractionText()
    {
        return "Use Stars";
    }
}