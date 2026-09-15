using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class StarLoadingMinigame : MonoBehaviour
{
    [SerializeField] private Canvas _starLoadingCanvas;

    [SerializeField] private Slider _amountSlider;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _resultText;

    [SerializeField] private float _sliderSpeed = 0.75f;

    [SerializeField] private int _minimumAmount = 5;
    [SerializeField] private int _maximumAmount = 15;

    private bool _movingRight = true;
    private bool _isMoving;

    private void Update()
    {
        if (!_isMoving) return;

        MoveSlider();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StopSlider();
        }
    }

    public void StartMinigame(FireworkStar star, int layer)
    {
        _starLoadingCanvas.enabled = true;

        _amountSlider.value = _amountSlider.minValue;

        _movingRight = true;
        _isMoving = true;

        _titleText.text = $"Layer {layer} - {star.color}";
        _resultText.text = "Amount: ?";
    }

    public void HideMinigame()
    {
        _isMoving = false;
        _starLoadingCanvas.enabled = false;
    }

    private void MoveSlider()
    {
        float direction = _movingRight ? 1f : -1f;

        _amountSlider.value += direction * _sliderSpeed * Time.deltaTime;

        if (_amountSlider.value >= _amountSlider.maxValue)
        {
            _amountSlider.value = _amountSlider.maxValue;
            _movingRight = false;
        }
        else if (_amountSlider.value <= _amountSlider.minValue)
        {
            _amountSlider.value = _amountSlider.minValue;
            _movingRight = true;
        }
    }

    private void StopSlider()
    {
        _isMoving = false;

        float normalizedValue = Mathf.InverseLerp(_amountSlider.minValue, _amountSlider.maxValue, _amountSlider.value);

        int amount = Mathf.RoundToInt(Mathf.Lerp(_minimumAmount, _maximumAmount, normalizedValue));

        _resultText.text = $"Amount: {amount}";

        CraftingManager.Instance.CompleteStarLoading(amount);
    }
}