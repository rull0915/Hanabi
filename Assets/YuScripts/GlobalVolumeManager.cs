using UnityEngine;

public class GlobalVolumeManager : SingletonMonoBehaviour<GlobalVolumeManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ゲーム開始時に自動生成
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        InitializeBeforeSceneLoad("GlobalVolumeManager");
    }
}
