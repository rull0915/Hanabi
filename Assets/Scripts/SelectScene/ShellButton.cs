using UnityEngine;
using UnityEngine.EventSystems;

public class ShellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // –îˆó‚ÌRectTransform
    [SerializeField] private RectTransform m_arrowRect;

    // ƒTƒCƒY
    private uint m_size;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_arrowRect != null)
        {
            m_arrowRect.anchoredPosition = new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x, m_arrowRect.anchoredPosition.y);
        }
        Debug.Log("EnterOnButton");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ExitOnButton");
    }
}
