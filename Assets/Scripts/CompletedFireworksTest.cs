using UnityEngine;

public class CompletedFireworkTest : MonoBehaviour
{
    [SerializeField] private CompletedFireworks _completedFireworks;

    [ContextMenu("Print Completed Firework")]
    public void PrintCompletedFirework()
    {
        if (_completedFireworks == null)
        {
            Debug.LogWarning("No CompletedFirework assigned.");
            return;
        }

        if (_completedFireworks.shell == null)
        {
            Debug.LogWarning("Completed firework has no shell.");
            return;
        }

        Debug.Log("===== COMPLETED FIREWORK =====");

        Debug.Log(
            $"Shell: {_completedFireworks.shell.name} | " +
            $"Material: {_completedFireworks.shell.material} | " +
            $"Size: {_completedFireworks.shell.size}"
        );

        Debug.Log($"Star Layers: {_completedFireworks.stars.Count}");

        foreach (CompletedStar completedStar in _completedFireworks.stars)
        {
            if (completedStar.star == null)
                continue;

            Debug.Log(
                $"Layer {completedStar.layer} | " +
                $"Star: {completedStar.star.color} | " +
                $"Amount: {completedStar.amount}"
            );
        }

        Debug.Log(
            $"Shell Closing Accuracy: " +
            $"{_completedFireworks._shellClosingAccuracy * 100f:F1}%"
        );

        Debug.Log("==============================");
    }
}