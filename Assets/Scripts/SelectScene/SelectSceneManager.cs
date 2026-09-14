using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    // SO
    [SerializeField] private OuterShell m_selectShell;
    [SerializeField] private SelectedMaterials m_selectMaterials;

    // ボタンの一覧
    ShellButton m_selectShellButton;
    [SerializeField] private List<ShellButton> m_shellButtons = new List<ShellButton>();

    // 星ボタンの一覧
    [SerializeField] private List<StarButton> m_starButtons = new List<StarButton>();

    // 選択カーソル
    [SerializeField] private RectTransform m_selectCursor;

    // 星選択管理
    [SerializeField] private StarButtonManager m_starButtonManager;
    
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

    // 外殻決定関数
    public void ShellDecide()
    {
        if (!m_selectShell || !m_selectShellButton) return;

        m_selectShell.size = m_selectShellButton.GetSize();
        m_selectShell.material = m_selectShellButton.GetMaterial();
    }

    // 全体決定関数
    public void Decide()
    {
        // 初期化
        m_selectMaterials.stars.Clear();

        foreach (var star in m_starButtonManager.GetManagedButtons())
        {
            m_selectMaterials.stars.Add(new FireworkStar());
        }
    }
}
