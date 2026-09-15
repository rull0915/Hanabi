using UnityEngine;
using UnityEngine.UI;

public class StarButton : MonoBehaviour
{
    // 自分の星
    [SerializeField] private FireworkStar m_star;

    // 選択中フラグ
    bool m_selecting;

    // 自分を管理しているマネージャー
    StarButtonManager m_manager;

    // セッター
    public void SetManager(StarButtonManager manager)
    {
        m_manager = manager;
    }

    // 選択カーソル
    [SerializeField] private GameObject m_selectCursor;

    public bool Select
    {
        get => m_selecting; 
        set
        {
            m_selecting = value;
        }
    }

    public FireworkStar GetMyStar()
    {
        return m_star;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 自分のボタンに関数を追加
        Button button = GetComponent<Button>();

        if (button)
        {
            button.onClick.AddListener(
                () => 
                {
                    // 未選択の時
                    if (!m_selecting)
                    {      
                        // 選択可能チェック
                        bool canSelect = m_manager.CanCheck();

                        // 可能なら
                        if (canSelect)
                        {
                            m_selecting = true;
                        }
                    }

                    // 選択中の時
                    else
                    {
                        m_selecting = false;
                    }
                }
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selectCursor)
        {
            // 選択中の時
            m_selectCursor.SetActive(m_selecting);
        }
    }
}
