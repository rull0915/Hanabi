using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(PlayerInteraction))]
public class CraftingCameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _transitionDuration = 0.75f;

    [Header("Player Controller")]
    [SerializeField] private PlayerController _playerController;

    [Header("Player Interaction")]
    [SerializeField] private PlayerInteraction _playerInteraction;

    [Header("UI")]
    [SerializeField] private Canvas _crosshairCanvas;

    private Coroutine _transitionCoroutine;

    private Vector3 _normalCameraLocalPosition;
    private Quaternion _normalCameraLocalRotation;

    private void Start()
    {
        _normalCameraLocalPosition = _playerCamera.transform.localPosition;
        _normalCameraLocalRotation = _playerCamera.transform.localRotation;
    }

    public void MoveToCraftingView(Transform cameraPoint)
    {
        EnterCraftingMode();

        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(MoveCamera(cameraPoint));
    }

    private void EnterCraftingMode()
    {
        _playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _crosshairCanvas.enabled = false;

        _playerInteraction.SetCraftingMode(true);
    }

    public void ReturnToPlayerView()
    {
        Transform cameraTransform = _playerCamera.transform;

        cameraTransform.localPosition = _normalCameraLocalPosition;
        cameraTransform.localRotation = _normalCameraLocalRotation;

        _playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _crosshairCanvas.enabled = true;

        _playerInteraction.SetCraftingMode(false);
    }

    private IEnumerator MoveCamera(Transform target)
    {
        Transform cameraTransform = _playerCamera.transform;

        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / _transitionDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            cameraTransform.position = Vector3.Lerp(startPosition, target.position, t);

            cameraTransform.rotation = Quaternion.Slerp(startRotation, target.rotation, t);

            yield return null;
        }

        cameraTransform.position = target.position;
        cameraTransform.rotation = target.rotation;

        _transitionCoroutine = null;
    }
}