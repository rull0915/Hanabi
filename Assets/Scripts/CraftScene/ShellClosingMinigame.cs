using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ShellClosingMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CraftingManager _craftingManager;

    [SerializeField] private Canvas _shellClosingCanvas;
    [SerializeField] private Slider _accuracySlider;
    [SerializeField] private TMP_Text _qualityText;

    [Header("Settings")]
    [SerializeField] private float _sliderSpeed = 0.75f;

    private bool _isMoving;
    private bool _movingRight = true;

    private void Update()
    {
        if (!_isMoving) return;

        MoveSlider();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StopSlider();
        }
    }

    public void StartMinigame()
    {
        _shellClosingCanvas.enabled = true;

        _accuracySlider.gameObject.SetActive(true);
        _qualityText.gameObject.SetActive(false);

        _accuracySlider.value = _accuracySlider.minValue;

        _movingRight = true;
        _isMoving = true;
    }

    private void MoveSlider()
    {
        float direction = _movingRight ? 1f : -1f;

        _accuracySlider.value += direction * _sliderSpeed * Time.deltaTime;

        if (_accuracySlider.value >= _accuracySlider.maxValue)
        {
            _accuracySlider.value = _accuracySlider.maxValue;
            _movingRight = false;
        }
        else if (_accuracySlider.value <= _accuracySlider.minValue)
        {
            _accuracySlider.value = _accuracySlider.minValue;
            _movingRight = true;
        }
    }

    private void StopSlider()
    {
        _isMoving = false;

        float center = 0.5f;

        float distanceFromCenter = Mathf.Abs(_accuracySlider.value - center);

        float accuracy = 1f - (distanceFromCenter * 2f);

        accuracy = Mathf.Clamp01(accuracy);

        // Show the achieved quality
        _qualityText.text = $"{accuracy * 100f:F1}% Quality Achieved!";

        _qualityText.gameObject.SetActive(true);

        // Tell CraftingManager that closing is complete
        _craftingManager.CompleteShellClosing(accuracy);
    }
}