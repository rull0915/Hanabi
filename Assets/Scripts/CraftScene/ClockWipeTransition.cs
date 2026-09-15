using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockWipeTransition : MonoBehaviour
{
    [SerializeField] private Image _clockWipeImage;

    [SerializeField] private CraftingTableSpawner _craftingTableSpawner;

    [SerializeField] private float _wipeDuration = 0.6f;
    [SerializeField] private float _coveredDuration = 0.2f;

    [SerializeField] private CraftingCameraController _craftingCameraController;

    private bool _isTransitioning;
    private bool _hasSwappedMaterials;

    public void PlayWipe()
    {
        if (_isTransitioning) return;

        StartCoroutine(WipeRoutine());
    }

    private IEnumerator WipeRoutine()
    {
        _isTransitioning = true;

        yield return AnimateFill(0f, 1f);
        SwapCraftingTable();
        yield return new WaitForSecondsRealtime(_coveredDuration);
        yield return AnimateFill(1f, 0f);

        _isTransitioning = false;
    }

    private void SwapCraftingTable()
    {
        if (_hasSwappedMaterials) return;

        _craftingTableSpawner.HideCraftingMaterials();

        _craftingTableSpawner.SpawnCompletedShellHalves();

        _craftingCameraController.ReturnToPlayerView();

        _hasSwappedMaterials = true;
    }

    private IEnumerator AnimateFill(float start, float end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _wipeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = elapsedTime / _wipeDuration;

            _clockWipeImage.fillAmount = Mathf.Lerp(start, end, t);

            yield return null;
        }

        _clockWipeImage.fillAmount = end;
    }
}