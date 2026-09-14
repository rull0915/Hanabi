using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField] private OuterShell m_selectShell;

    // ボタンの一覧
    ShellButton m_selectShellButton;
    [SerializeField] private List<ShellButton> m_shellButtons = new List<ShellButton>();

    // 選択カーソル
    [SerializeField] private RectTransform m_selectCursor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // クリック時関数を追加
        foreach (var shellButton in m_shellButtons)
        {
            var button = shellButton.GetComponent<Button>();

            if (button)
            {
                button.onClick.AddListener(
                    () => 
                    {
                        var rect = shellButton.GetComponent<RectTransform>();

                        // カーソルの位置を移動
                        m_selectCursor.SetParent(rect);
                        m_selectCursor.anchoredPosition = Vector2.zero;
                        m_selectCursor.localScale = Vector2.one;

                        m_selectShellButton = shellButton;
                    }
                );
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 決定関数
    public void Decide()
    {
        if (!m_selectShell || !m_selectShellButton) return;

        m_selectShell.size = m_selectShellButton.GetSize();
        m_selectShell.material = m_selectShellButton.GetMaterial();
    }
}
