using UnityEngine;
using System.Collections;

public enum TransitionType
{
    Fade,
}

public abstract class TransitionBase : MonoBehaviour
{
    // ‰æ–Ê‚ğ‰B‚·ˆ—
    // Screen-hiding process
    public abstract IEnumerator PlayInRoutine();

    // ‰æ–Ê‚ğŠJ‚­ˆ—
    // Screen-opening process
    public abstract IEnumerator PlayOutRoutine();

    public abstract TransitionType GetTransitionType();
}
