using UnityEngine;

public class CraftingTableSpawner : MonoBehaviour
{
    [SerializeField] private SelectedMaterials _selectedMaterials;

    [SerializeField] private OuterShellItem _outerShellPrefab;
    [SerializeField] private FireworkStarItem _fireworkStarPrefab;

    [SerializeField] private Transform _outerShellSpawnPoint;
    [SerializeField] private Transform[] _fireworkStarSpawnPoints;

    private void Start()
    {
        
    }

    private void SpawnSelectedMaterial()
    {
        SpawnOuterShell();
        SpawnStars();
    }

    private void SpawnOuterShell()
    {
        if (_selectedMaterials.outerShell == null) return;
        OuterShellItem shell = Instantiate(_outerShellPrefab, _outerShellSpawnPoint.position, _outerShellSpawnPoint.rotation);
        shell.Initialize(_selectedMaterials.outerShell);
    }

    private void SpawnStars()
    {
        for (int i = 0; i < _fireworkStarSpawnPoints.Length; i++)
        {
            if (i >=  _fireworkStarSpawnPoints.Length)
            {
                break;
            }

            FireworkStar starData = _selectedMaterials.stars[i];
            if (starData != null) continue;

            FireworkStarItem star = Instantiate(_fireworkStarPrefab, _fireworkStarSpawnPoints[i].position, _fireworkStarSpawnPoints[i].rotation);
            star.Initialize(_selectedMaterials.stars[i]);
        }
    }
}
