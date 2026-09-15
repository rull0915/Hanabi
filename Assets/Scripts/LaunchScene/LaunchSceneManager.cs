using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class LaunchSceneManager : MonoBehaviour
{
    // ループカウンター
    [SerializeField] private LoopCounter m_loopCounter;

    [SerializeField] private FireworksParticleController[] m_particleControllers = new FireworksParticleController[LoopCounter.MAX_COUNT];

    // 発射の間隔
    [SerializeField] private float m_launchDistance = 1;

    private uint m_currentIndex = 0;
    private float m_elapsedTime = 0;

    private bool m_openPopup = false;
    [SerializeField] private MoveResult m_moveResult;

    // 成功数のカウント
    private uint m_successCount = 0;

    // ランクを表示するテキストUI
    [SerializeField] private TextMeshProUGUI m_text;

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
        // 時間の計測
        m_elapsedTime += Time.deltaTime;

        if (m_currentIndex < m_particleControllers.Length)
        {
            if (m_elapsedTime > m_launchDistance)
            {
                m_elapsedTime = 0;

                // アクティブに
                m_particleControllers[m_currentIndex].gameObject.SetActive(true);

                // 次へ
                m_currentIndex++;
            }
        }
        else if (m_elapsedTime > m_launchDistance * 1.5f)
        {
            if (!m_openPopup)
            {
                m_openPopup = true;
                m_moveResult.OpenPopup();

                // 成功数カウント
                foreach (var p in m_particleControllers)
                {
                    if (p.Success()) m_successCount++;
                }

                // 0,1 = C | 2,3 = B | 4 = A | 5 = S
                string rankStr = "";

                if (m_successCount <= 1) rankStr = "C";
                else if (m_successCount <= 3) rankStr = "B";
                else if (m_successCount <= 4) rankStr = "A";
                else rankStr = "S";

                m_text.text = rankStr;
            }
        }
    }
}
