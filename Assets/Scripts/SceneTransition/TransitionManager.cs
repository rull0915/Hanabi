using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
{
    // シーン遷移演出のリスト
    // List of scene transition effects
    [SerializeField] private List<TransitionBase> m_transitions;

    private Dictionary<TransitionType, TransitionBase> m_transitionDictionary = new();

    private bool m_isTransitioning = false;

    // ゲーム開始時に自動生成
    // Automatically generated at the start of the game.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        InitializeBeforeSceneLoad("TransitionManager");
    }

    protected override void OnInitialize()
    {
        InitializeTransitions();
    }

    private void InitializeTransitions()
    {
        foreach (var transition in m_transitions)
        {
            // transitionが存在しまだdictに存在しなければ
            // If the transition exists but is not yet in the dict
            if (transition != null && !m_transitionDictionary.ContainsKey(transition.GetTransitionType()))
            {
                // 新たに追加
                // Newly added
                m_transitionDictionary.Add(transition.GetTransitionType(), transition);
            }
        }
    }

    public void LoadScene(string sceneName, TransitionType type)
    {
        if (m_isTransitioning) return;

        StartCoroutine(TransitionRoutine(sceneName, type));
    }

    // 遷移を行うコルーチン
    // Coroutine that performs a transition
    private IEnumerator TransitionRoutine(string sceneName, TransitionType type)
    {
        m_isTransitioning = true;

        // 指定された演出を取得
        // Retrieve the specified presentation effect.
        if (!m_transitionDictionary.TryGetValue(type, out var effect))
        {
            Debug.LogError($"TransitionEffect for {type} is not registered!");

            // 遷移なしでシーンを変更
            // Change scene without transition
            SceneManager.LoadScene(sceneName);

            m_isTransitioning = false;

            yield break;
        }

        // 画面を隠す演出を開始
        // Start the screen-hiding effect.
        yield return StartCoroutine(effect.PlayInRoutine());

        SceneManager.LoadScene(sceneName);

        // 画面を開く演出を開始
        // Start the screen-opening effect.
        yield return StartCoroutine(effect.PlayOutRoutine());

        m_isTransitioning = false;
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
