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

    // SOのリスト
    [SerializeField] private List<FireworkStar> m_fireworkStars;

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
            if (!star.Select) continue;

            foreach (var fireworkStar in m_fireworkStars)
            {
                if (fireworkStar.color == star.GetMyColor())
                {
                    m_selectMaterials.stars.Add(fireworkStar);

                    break;
                }
            }
        }

        ShellDecide();

        m_selectMaterials.outerShell = m_selectShell;

        // 作成シーンへ移動する
        TransitionManager.Instance.LoadScene("WorkshopScene", TransitionType.Fade);
    }
}
