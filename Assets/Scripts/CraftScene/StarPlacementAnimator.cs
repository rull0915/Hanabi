using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPlacementAnimator : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private Transform _placementCenter;
    [SerializeField] private Transform _starParent;

    [Header("Layer Radius")]
    [SerializeField] private float _layer1Radius = 0.05f;
    [SerializeField] private float _layer2Radius = 0.08f;
    [SerializeField] private float _layer3Radius = 0.11f;

    [Header("Animation")]
    [SerializeField] private float _moveDuration = 0.15f;
    [SerializeField] private float _delayBetweenStars = 0.03f;

    private readonly List<GameObject> _placedStars = new();

    public void PlayPlacement(FireworkStar star, int layer, int amount, Vector3 startPosition)
    {
        StartCoroutine(PlaceStarsRoutine(star, layer, amount, startPosition));
    }

    private IEnumerator PlaceStarsRoutine(FireworkStar star, int layer, int amount, Vector3 startPosition)
    {
        float radius = GetLayerRadius(layer);

        for (int i = 0; i < amount; i++)
        {
            float angle = (360f / amount) * i * Mathf.Deg2Rad;

            Vector3 localOffset = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            Vector3 targetPosition = _placementCenter.TransformPoint(localOffset);

            FireworkStarItem starItem = Instantiate(star.prefab, startPosition, _placementCenter.rotation, _starParent);

            DisableInteraction(starItem);

            _placedStars.Add(starItem.gameObject);

            yield return MoveStar(starItem.transform, startPosition, targetPosition);

            yield return new WaitForSeconds(_delayBetweenStars);
        }
    }

    private IEnumerator MoveStar(Transform star, Vector3 startPosition, Vector3 targetPosition)
    {
        float timer = 0f;

        while (timer < _moveDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / _moveDuration);

            star.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        star.position = targetPosition;
    }

    private void DisableInteraction(FireworkStarItem starItem)
    {
        starItem.enabled = false;

        Collider[] colliders = starItem.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private float GetLayerRadius(int layer)
    {
        switch (layer)
        {
            case 1:
                return _layer1Radius;

            case 2:
                return _layer2Radius;

            case 3:
                return _layer3Radius;

            default:
                return _layer1Radius;
        }
    }
}