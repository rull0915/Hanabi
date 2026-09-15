using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //どこからでもアクセス可能
    public static PauseManager Instance { get; private set; }

    private void Aweke()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
