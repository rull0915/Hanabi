using UnityEngine;
using System.Collections.Generic;

public class StarButtonManager : MonoBehaviour
{
    // 選択できる最大数
    const uint MAX_STARS = 3;

    // 管理しているボタン
    [SerializeField] private List<StarButton> m_starButtons = new List<StarButton>();

    private void Start()
    {
        foreach (var button in m_starButtons)
        {
            button.SetManager(this);
        }
    }

    // 選択できるかを調べる関数
    public bool CanCheck()
    {
        // 3個以上選択されてたら選択不可
        return CheckSelectStarCount() < MAX_STARS;
    }

    // 選択している星の数を調べる関数
    public uint CheckSelectStarCount()
    {
        uint selectCount = 0;

        foreach (var button in m_starButtons)
        {
            if (button.Select) selectCount++;
        }

        // 3個以上選択されてたら選択不可
        return selectCount;
    }

    public List<StarButton> GetManagedButtons()
    {
        return m_starButtons;
    }
}
