using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    // 現在操作中のシェル
    [SerializeField] private ShellButton m_shellButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 上クリック時
    public void OnUpButtonClicked()
    {
        // サイズを上げる
        if (m_shellButton)
        {
            m_shellButton.ChangeSize(true);
        }
    }

    // 下クリック時
    public void OnDownButtonClicked()
    {
        // サイズを上げる
        if (m_shellButton)
        {
            m_shellButton.ChangeSize(false);
        }
    }

    // 操作対象を変える関数
    public void ChangeTarget(ShellButton shell)
    {
        m_shellButton = shell;
    }
}
