using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // インスタンス
    // プロパティを使用し読み取りはpublic,書き込みはprivateに設定 自身のみが値を変更できます。
    // Uses a property with public read access and private write access; only the instance itself can modify the value.
    static public T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            // T型として自身を生成
            // Generates itself as a T-type.
            Instance = this as T;

            // シーンロード時に消えないように設定
            // Set to persist across scene loads
            DontDestroyOnLoad(gameObject);

            // 派生クラスの初期化処理を呼ぶ
            // Call the derived class's initialization logic.
            OnInitialize();
        }

        else if (Instance != this)
        {
            // 重複するので自分を消す
            // Delete self because of duplication
            Destroy(gameObject);
        }
    }

    // 派生クラスで実装するAwakeの代替関数
    // Alternative to Awake to be implemented in derived classes
    protected virtual void OnInitialize() { }

    /// <summary>
    /// シーンロード前にResourcesから自動生成する共通処理
    /// 派生クラスの[RuntimeInitializeOnLoadMethod]から呼び出す
    /// Common logic to automatically generate assets from Resources before the scene loads
    /// Called from [RuntimeInitializeOnLoadMethod] in derived classes
    /// </summary>
    protected static void InitializeBeforeSceneLoad(string prefabName)
    {
        if (Instance != null) return;

        // Resourcesフォルダからプレハブを読み込む
        // Load a prefab from the Resources folder.
        T prefab = Resources.Load<T>(prefabName);

        if (prefab == null)
        {
            Debug.LogError("The specified prefab does not exist.");
            return;
        }

        var instance = Instantiate(prefab);
        Instance = instance;

        DontDestroyOnLoad(instance.gameObject);
    }
}
