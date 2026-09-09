using UnityEngine;
using System.Collections;

public class FadeTransition : TransitionBase
{
    // 透明度を変化させるキャンバスグループ
    [SerializeField] private CanvasGroup m_canvasGroup;

    // かかる時間
    [SerializeField] private float m_duration = 0.5f;

    public void Awake()
    {
        // 初期状態は透明にしておく
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.blocksRaycasts = false;   // クリック判定が取られないように
    }

    public override TransitionType GetTransitionType()
    {
        return TransitionType.Fade;
    }

    // 画面を隠す処理
    public override IEnumerator PlayInRoutine()
    {
        // マウスが当たるように変更
        m_canvasGroup.blocksRaycasts = true;

        // 経過時間を計測
        float elapsed = 0f;

        // durationより小さい間繰り返す
        while (elapsed < m_duration)
        {
            // 時間の加算
            elapsed += Time.deltaTime;

            // 透明度を変更 (0 ~ 1でクランプ)
            m_canvasGroup.alpha = Mathf.Clamp(elapsed / m_duration, 0, 1);

            yield return null;
        }
    }

    // 画面を開く処理
    public override IEnumerator PlayOutRoutine()
    {
        // 警官時間を計測
        float elapsed = 0f;

        // durationより小さい間繰り返す
        while (elapsed < m_duration)
        {
            // 時間を加算
            elapsed += Time.deltaTime;

            // 透明度を変更
            m_canvasGroup.alpha = Mathf.Clamp(1f - (elapsed / m_duration), 0, 1);

            yield return null;
        }

        // 開き終わったらマウスの衝突判定を消す
        m_canvasGroup.blocksRaycasts = false;
    }
}
