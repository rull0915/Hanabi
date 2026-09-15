using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;

    [Header("Interaction Settings")]
    [SerializeField] private float _range = 5.0f;
    [SerializeField] private LayerMask _interactionLayer;

    private IInteractable _currentInteractable;

    private bool _isCraftingMode;

    private void Update()
    {
        PerformRaycast();

        if (_isCraftingMode)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Interact();
            }
        }
        else
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }

    public void PerformRaycast()
    {
        _currentInteractable = null;

        Ray ray;

        if (_isCraftingMode)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            ray = _playerCamera.ScreenPointToRay(mousePosition);
        }
        else
        {
            ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, _range, _interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                _currentInteractable = interactable;
            }
        }
    }

    private void Interact()
    {
        if (_currentInteractable == null) return;
        _currentInteractable.Interact(this);
    }

    public void SetCraftingMode(bool isCraftingMode)
    {
        _isCraftingMode = isCraftingMode;
    }
}