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

    private void Update()
    {
        PerformRaycast();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    public void PerformRaycast()
    {
        _currentInteractable = null;

        Vector3 origin = _playerCamera.transform.position;
        Vector3 direction = _playerCamera.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, _range, _interactionLayer))
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


}