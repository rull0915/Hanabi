using UnityEngine;

public class LoopResetter : MonoBehaviour
{
    [SerializeField] LoopCounter counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ƒ‹[ƒv‰ñ”‚Ì‰Šú‰»
        counter.m_loopCount = 0;
    }
}
