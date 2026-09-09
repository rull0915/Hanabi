using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade")]
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    private bool _isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.alpha = 1f;
            _fadeCanvasGroup.blocksRaycasts = true;
        }
    }

    private IEnumerator Start()
    {
        yield return Fade(0f);
    }

    public void LoadScene(string sceneName)
    {
        if (_isTransitioning) return;
        StartCoroutine(TransitionToScene(sceneName));
    }

    public void ReloadCurrentScene()
    {
        if (_isTransitioning) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(TransitionToScene(currentSceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        _isTransitioning = true;
        yield return Fade(1f);

        // Load new scene
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        if (loadOperation == null)
        {
            Debug.LogError($"Unable to load {sceneName}");
            yield return Fade(0f);
            _isTransitioning = false;
            yield break;
        }

        // Wait until loading is finished
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return Fade(0f);
        _isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (_fadeCanvasGroup == null)
        {
            Debug.LogError("Fade Canvas Group is not assigned.");
            yield break;
        }

        float startAlpha = _fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        _fadeCanvasGroup.blocksRaycasts = true;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentage = elapsedTime / _fadeDuration;
            _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, percentage);

            yield return null;
        }

        _fadeCanvasGroup.alpha = targetAlpha;
        if (targetAlpha <= 0f)
        {
            _fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}
