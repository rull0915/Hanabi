using UnityEngine;

public class SelectUI : MonoBehaviour
{
    // 1ウィンドウの幅
    [SerializeField] float m_windowWidth;

    [SerializeField] RectTransform m_parentRect;    // 親のRectTransform

    // スクロール
    [Header("Scroll")]
    [SerializeField] float m_scrollTime;    // スクロールにかける時間
    [SerializeField] EasingConfig m_scrollEasing;    // スクロールに使うイージング

    // 時間
    private float m_sumTime;

    // スクロール中フラグ
    private bool m_isScrolling;
    // 開始と終了の位置
    private float m_startXPosition;
    private float m_goalXPosition;
    // スクロール開始時間
    private float m_scrollStartTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // リセット
        m_isScrolling = false;
        m_goalXPosition = 0;
        m_scrollStartTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間の加算
        m_sumTime += Time.deltaTime;

        // スクロールのアニメーション
        ScrollAnimation();
    }

    // 1つ右にスクロールする関数
    public void ScrollRightStart()
    {
        // スクロール中なら何もしない
        if (m_isScrolling) return;

        // ゴールのX座標を設定
        m_startXPosition = m_parentRect.anchoredPosition.x;
        m_goalXPosition = m_parentRect.anchoredPosition.x - m_windowWidth;

        // 開始時間を設定
        m_scrollStartTime = m_sumTime;

        // フラグを立てる
        m_isScrolling = true;
    }

    // 1つ右にスクロールする関数
    public void ScrollLeftStart()
    {
        // スクロール中なら何もしない
        if (m_isScrolling) return;

        // ゴールのX座標を設定
        m_startXPosition = m_parentRect.anchoredPosition.x;
        m_goalXPosition = m_parentRect.anchoredPosition.x + m_windowWidth;

        // 開始時間を設定
        m_scrollStartTime = m_sumTime;

        // フラグを立てる
        m_isScrolling = true;
    }

    // スクロールのアニメーション
    void ScrollAnimation()
    {
        // スクロール中でなければ何もしない
        if (!m_isScrolling) return;

        // 割合を算出
        float t = (m_sumTime - m_scrollStartTime) / m_scrollTime;

        // 位置を算出
        float nextX = m_startXPosition + (m_goalXPosition - m_startXPosition) * m_scrollEasing.Get(t);

        // 変更
        m_parentRect.anchoredPosition = new Vector2(nextX, m_parentRect.anchoredPosition.y);

        // 1を超えたら
        if (t > 1)
        {
            m_isScrolling = false;
        }
    }
}
