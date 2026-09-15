using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SelectSceneManager : MonoBehaviour
{
    // SO
    [SerializeField] private OuterShell m_selectShell;
    [SerializeField] private SelectedMaterials m_selectMaterials;

    // ボタンの一覧
    ShellButton m_selectShellButton;
    [SerializeField] private List<ShellButton> m_shellButtons = new List<ShellButton>();

    // 選択カーソル
    [SerializeField] private RectTransform m_selectCursor;

    // 星選択管理
    [SerializeField] private StarButtonManager m_starButtonManager;

    // ループカウンタ
    [SerializeField] private LoopCounter m_loopCounter;

    // 回数テキスト
    [SerializeField] private TextMeshProUGUI m_text;

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
                        m_selectCursor.anchoredPosition = new Vector2(-60, 60);
                        m_selectCursor.localScale = Vector2.one;

                        m_selectShellButton = shellButton;
                    }
                );
            }
        }

        if (m_text)
        {
            string str = (m_loopCounter.m_loopCount + 1).ToString() + " / " + LoopCounter.MAX_COUNT.ToString();

            m_text.text = str;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 外殻決定関数
    public void ShellDecide()
    {
        if (!m_selectShellButton) return;

        m_selectShell = m_selectShellButton.GetShell();
        m_selectShell.size = m_selectShellButton.GetSize();
    }

    // 全体決定関数
    public void Decide()
    {
        // 初期化
        m_selectMaterials.stars.Clear();

        foreach (var star in m_starButtonManager.GetManagedButtons())
        {
            if (!star.Select) continue;

            m_selectMaterials.stars.Add(star.GetMyStar());
        }

        ShellDecide();

        m_selectMaterials.outerShell = m_selectShell;

        // 作成シーンへ移動する
        TransitionManager.Instance.LoadScene("WorkshopScene", TransitionType.Fade);
    }
}
