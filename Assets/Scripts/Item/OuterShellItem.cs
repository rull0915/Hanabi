using UnityEngine;

public class OuterShellItem : MonoBehaviour, IInteractable
{
    [SerializeField] private OuterShell _shellData;

    public OuterShell ShellData => _shellData;

    public void Interact(PlayerInteraction playerInteraction)
    {
        Debug.Log($"Shell Size: {_shellData.size}, Material: {_shellData.material}");
    }

    public string GetInteractionText()
    {
        return "Pick Up";
    }
}