using UnityEngine;
using UnityEngine.EventSystems;

public class ShellButton : MonoBehaviour, IPointerEnterHandler
{
    // サイズの最大最小
    private const uint MIN_SIZE = 1;
    private const uint MAX_SIZE = 3;

    // 矢印のRectTransform
    [SerializeField] private RectTransform m_arrowRect;

    // 自分のShell
    [SerializeField] private OuterShell m_myShell;

    // サイズ
    private uint m_size = 2;

    public OuterShell GetShell() { return m_myShell; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_arrowRect != null)
        {
            // x座標を合わせる
            m_arrowRect.anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, m_arrowRect.anchoredPosition.y);

            // Arrowを取得
            var arrow = m_arrowRect.GetComponent<ArrowButton>();

            if (arrow)
            {
                arrow.ChangeTarget(this);
            }
        }
    }

    // サイズ変更関数
    public void ChangeSize(bool up)
    {
        if (up)
        {
            // 既に最大ならスキップ
            if (m_size == MAX_SIZE) return;

            m_size++;
            this.GetComponent<RectTransform>().localScale *= 1.1f;
        }
        else
        {
            // 既に最小ならスキップ
            if (m_size == MIN_SIZE) return;

            m_size--;
            this.GetComponent<RectTransform>().localScale *= 0.9f;
        }
    }

    public uint GetSize()
    {
        return m_size;
    }
}
