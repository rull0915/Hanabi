using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LoopCounter", menuName = "Scriptable Objects/LoopCounter")]
public class LoopCounter : ScriptableObject
{
    public const uint MAX_COUNT = 5;

    public uint m_loopCount = 0;

    // ì¬‚µ‚½‰Ô‰Î‹Ê‚ÌƒŠƒXƒg
    public List<CompletedFireworks> m_fireworkses = new List<CompletedFireworks>();

    // ¡‚Ì‰Ô‰Î‹Ê‚ğæ“¾‚·‚éŠÖ”
    public CompletedFireworks GetCurrentFirework()
    {
        int index = (int)m_loopCount;
        return m_fireworkses[index];
    }

    // Ÿ‚Ì‰Ô‰Î‹Ê‚Ös‚­ŠÖ”
    public bool ToNextFireworks()
    {
        m_loopCount++;

        if (m_loopCount >= MAX_COUNT)
        {
            m_loopCount = MAX_COUNT - 1;
            return true;
        }

        return false;
    }
}
