using UnityEngine;
using System.Collections.Generic;

public class CraftingTableSpawner : MonoBehaviour
{
    [SerializeField] private SelectedMaterials _selectedMaterials;

    [Header("Crafting Materials")]
    [SerializeField] private FireworkStarItem _fireworkStarPrefab;
    [SerializeField] private Transform _outerShellSpawnPoint;
    [SerializeField] private Transform[] _fireworkStarSpawnPoints;

    [Header("Completed Shell")]
    [SerializeField] private GameObject _completedShellHalfPrefab;
    [SerializeField] private Transform _completedHalfSpawnPoint1;
    [SerializeField] private Transform _completedHalfSpawnPoint2;

    private OuterShellItem _spawnedOuterShell;
    private List<FireworkStarItem> _spawnedStars = new List<FireworkStarItem>();

    private void Start()
    {
        SpawnSelectedMaterials();
    }

    private void SpawnSelectedMaterials()
    {
        SpawnOuterShell();
        SpawnStars();
    }

    private void SpawnOuterShell()
    {
        OuterShell selectedShell = _selectedMaterials.outerShell;

        if (selectedShell == null) return;
        if (selectedShell.prefab == null) return;

        _spawnedOuterShell = Instantiate(selectedShell.prefab, _outerShellSpawnPoint.position, _outerShellSpawnPoint.rotation);
        _spawnedOuterShell.Initialize(selectedShell);
    }

    private void SpawnStars()
    {
        _spawnedStars.Clear();

        for (int i = 0; i < _selectedMaterials.stars.Count; i++)
        {
            if (i >= _fireworkStarSpawnPoints.Length) break;

            FireworkStar starData = _selectedMaterials.stars[i];

            if (starData == null) continue;

            FireworkStarItem star = Instantiate(_fireworkStarPrefab, _fireworkStarSpawnPoints[i].position, _fireworkStarSpawnPoints[i].rotation);

            star.Initialize(starData);

            _spawnedStars.Add(star);
        }

        CraftingManager.Instance.SetSpawnedStarItems(_spawnedStars);
    }

    public void SpawnCompletedShellHalves()
    {
        Instantiate(_completedShellHalfPrefab, _completedHalfSpawnPoint1.position, _completedHalfSpawnPoint1.rotation);
        Instantiate(_completedShellHalfPrefab, _completedHalfSpawnPoint2.position, _completedHalfSpawnPoint2.rotation);
    }

    public void HideCraftingMaterials()
    {
        if (_spawnedOuterShell != null)
        {
            _spawnedOuterShell.gameObject.SetActive(false);
        }

        foreach (FireworkStarItem star in _spawnedStars)
        {
            if (star != null)
            {
                star.gameObject.SetActive(false);
            }
        }
    }
}
