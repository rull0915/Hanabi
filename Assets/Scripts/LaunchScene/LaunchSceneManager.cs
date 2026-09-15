using UnityEngine;

public class LaunchSceneManager : MonoBehaviour
{
    // ループカウンター
    [SerializeField] private LoopCounter m_loopCounter;

    [SerializeField] private FireworksParticleController[] m_particleControllers = new FireworksParticleController[LoopCounter.MAX_COUNT];

    // 発射の間隔
    [SerializeField] private float m_launchDistance = 1;

    private uint m_currentIndex = 0;
    private float m_elapsedTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < LoopCounter.MAX_COUNT; i++)
        {
            CompletedFireworks fw = m_loopCounter.m_fireworkses[i];

            m_particleControllers[i].SetCompletedFirewokrs(fw);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentIndex < m_particleControllers.Length)
        {
            // 時間の計測
            m_elapsedTime += Time.deltaTime;

            if (m_elapsedTime > m_launchDistance)
            {
                m_elapsedTime = 0;

                // アクティブに
                m_particleControllers[m_currentIndex].gameObject.SetActive(true);

                // 次へ
                m_currentIndex++;
            }
        }
    }
}
