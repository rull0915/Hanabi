using UnityEngine;
using UnityEngine.XR;

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
    AddFuse,
    CloseShell,

    Completed
}

public class CraftingManager : SingletonMonoBehaviour<CraftingManager>
{
    [SerializeField] private CraftingState _currentState;

    public CraftingState CurrentState => _currentState;

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

    public void StartStarPlacement()
    {
        if (_currentState == CraftingState.PrepareShell)
        {
            ChangeState(CraftingState.PlaceLayer1);
        }
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

    public void AddAnotherLayer()
    {
        switch(_currentState)
        {
            case CraftingState.PlaceLayer1:
                ChangeState(CraftingState.PlaceLayer2);
                break;

            case CraftingState.PlaceLayer2:
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
        if (_currentState != CraftingState.AddFuse) return;
        ChangeState(CraftingState.AddFuse);
    }

    public void CompleteFuse()
    {
        if (_currentState != CraftingState.CloseShell) return;
        ChangeState(CraftingState.CloseShell);
    }

    public void Complete()
    {
        if (_currentState != CraftingState.CloseShell) return;
        ChangeState(CraftingState.Completed);
    }
}
