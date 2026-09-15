using UnityEngine;
using System.Collections.Generic;

public enum CraftingState
{
    PrepareShell,

    PlaceLayer1,
    CompleteLayer1,

    PlaceLayer2,
    CompleteLayer2,

    PlaceLayer3,
    CompleteLayer3,

    AddBurstCharge,

    PrepareCloseShell,
    CloseShell,

    Completed
}

public class CraftingManager : SingletonMonoBehaviour<CraftingManager>
{
    [SerializeField] private StarLoadingMinigame _starLoadingMinigame;
    [SerializeField] private ShellClosingMinigame _shellClosingMinigame;

    [SerializeField] private CraftingCameraController _craftingCameraController;
    [SerializeField] private Transform _closeShellCameraPoint;
    [SerializeField] private ClockWipeTransition _clockWipeTransition;

    [SerializeField] private SelectedMaterials _selectedMaterials;

    [SerializeField] private List<StarLayerData> _starLayers = new List<StarLayerData>();
    private int _currentStarIndex;

    [SerializeField] private CraftingState _currentState;
    public CraftingState CurrentState => _currentState;

    [SerializeField] private float _shellClosingAccuracy;
    public float ShellClosingAccuracy => _shellClosingAccuracy;

    private List<FireworkStarItem> _spawnedStarItems = new List<FireworkStarItem>();

    protected override void OnInitialize()
    {
        ChangeState(CraftingState.PrepareShell);
    }

    public void ChangeState(CraftingState newState)
    {
        _currentState = newState;
    }

    public bool IsPlacingStars()
    {
        return _currentState == CraftingState.PlaceLayer1 || _currentState == CraftingState.PlaceLayer2 || _currentState == CraftingState.PlaceLayer3;
    }

    public int GetCurrentLayer()
    {
        switch (_currentState)
        {
            case CraftingState.PlaceLayer1:
                return 1;

            case CraftingState.PlaceLayer2:
                return 2;

            case CraftingState.PlaceLayer3:
                return 3;

            default:
                return 0;
        }
    }

    public void StartStarPlacement(Transform cameraPoint)
    {
        if (_currentState != CraftingState.PrepareShell) return;

        _currentStarIndex = 0;
        _starLayers.Clear();

        ChangeState(CraftingState.PlaceLayer1);

        _craftingCameraController.MoveToCraftingView(cameraPoint);

        FireworkStar star = _selectedMaterials.stars[_currentStarIndex];

        _starLoadingMinigame.StartMinigame(star, 1);
    }

    public void CompleteCurrentLayer()
    {
        switch (_currentState)
        {
            case CraftingState.PlaceLayer1:
                ChangeState(CraftingState.CompleteLayer1);
                break;

            case CraftingState.PlaceLayer2:
                ChangeState(CraftingState.CompleteLayer2);
                break;

            case CraftingState.PlaceLayer3:
                ChangeState(CraftingState.CompleteLayer3);
                break;
        }
    }

    public void CompleteStarLoading(int amount)
    {
        if (!IsPlacingStars()) return;

        int layer = GetCurrentLayer();
        FireworkStar star = _selectedMaterials.stars[_currentStarIndex];

        StarLayerData layerData = new StarLayerData(star, layer, amount);

        _starLayers.Add(layerData);

        Debug.Log($"Saved Layer {layer}: {star.color}, Amount: {amount}");

        // The physical star material has now been used.
        if (_currentStarIndex < _spawnedStarItems.Count)
        {
            FireworkStarItem starItem = _spawnedStarItems[_currentStarIndex];

            if (starItem != null)
            {
                starItem.gameObject.SetActive(false);
            }
        }

        CompleteCurrentLayer();

        _starLoadingMinigame.HideMinigame();

        _currentStarIndex++;
    }

    public void TryStartNextStarLayer(FireworkStar star)
    {
        if (_currentState != CraftingState.CompleteLayer1 && _currentState != CraftingState.CompleteLayer2) return;

        if (_currentStarIndex >= _selectedMaterials.stars.Count) return;

        FireworkStar nextStar = _selectedMaterials.stars[_currentStarIndex];

        if (star != nextStar) return;

        StartNextLayer();
    }
    private void StartNextLayer()
    {
        int layer = _currentStarIndex + 1;

        switch (layer)
        {
            case 2:
                ChangeState(CraftingState.PlaceLayer2);
                break;

            case 3:
                ChangeState(CraftingState.PlaceLayer3);
                break;
        }

        FireworkStar star = _selectedMaterials.stars[_currentStarIndex];

        _starLoadingMinigame.StartMinigame(star, layer);
    }

    public void AddAnotherLayer()
    {
        switch(_currentState)
        {
            case CraftingState.CompleteLayer1:
                ChangeState(CraftingState.PlaceLayer2);
                break;

            case CraftingState.CompleteLayer2:
                ChangeState(CraftingState.PlaceLayer3);
                break;
        }
    }

    public void AddBurstCharge()
    {
        if (_currentState != CraftingState.CompleteLayer1 && _currentState != CraftingState.CompleteLayer2 && _currentState != CraftingState.CompleteLayer3) return;
        ChangeState(CraftingState.AddBurstCharge);
    }

    public void CompleteBurstCharge()
    {
        if (_currentState != CraftingState.AddBurstCharge) return;
        ChangeState(CraftingState.PrepareCloseShell);

        _clockWipeTransition.PlayWipe();
    }

    public void StartCloseShell()
    {
        if (_currentState != CraftingState.PrepareCloseShell) return;
        ChangeState(CraftingState.CloseShell);

        _craftingCameraController.MoveToCraftingView(_closeShellCameraPoint);

        _shellClosingMinigame.StartMinigame();
    }

    public void CompleteShellClosing(float accuracy)
    {
        if (_currentState != CraftingState.CloseShell) return;

        _shellClosingAccuracy = accuracy;

        ChangeState(CraftingState.Completed);
    }

    public void Complete()
    {
        if (_currentState != CraftingState.CloseShell) return;
        ChangeState(CraftingState.Completed);
    }

    public void SetSpawnedStarItems(List<FireworkStarItem> starItems)
    {
        _spawnedStarItems = new List<FireworkStarItem>(starItems);
    }
}
