using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //どこからでもアクセス可能
    public static PauseManager Instance { get; private set; }

    //ポーズ可能か
    private bool m_canPause;

    //ポーズ中か
    public bool m_isPaose;

    //ポーズ画面
    public GameObject m_pausePanel;

    //現在のシーン名
    private string m_currentSceneName;

    //設定画面をポーズから開いたか
    public bool m_fromPause;

    //生成されたときに自動で呼ぶ
    private void Awake()
    {
        //既に存在するか
        if (Instance != null && Instance != this)
        {
            //今作ったPauseManagerを削除
            Destroy(gameObject);
            return;
        }

        //ゲーム全体に登録
        Instance = this;

        //シーンが変わっても削除しない
        DontDestroyOnLoad(gameObject);


        //現在のシーン名を保存
        m_currentSceneName = SceneManager.GetActiveScene().name;

        //ポーズが使えるか
        CheckCanPause();

    }

    private void Update()
    {
        //現在のシーン名を取得
        string sceneName = SceneManager.GetActiveScene().name;

        //シーンが変わったか
        if (sceneName !=  m_currentSceneName)
        {
            //現在のシーン名を更新
            m_currentSceneName = sceneName;

            //ポーズが使えるか確認
            CheckCanPause();
        }

        //シーンごとのTrueFalse判定確認
        //Debug.Log(SceneManager.GetActiveScene().name + " : " + m_canPause);

        //ポーズ不可だったら
        if (!m_canPause) return;

        //Escキーが押されたら
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }
    }

    //現在のシーンがポーズ可能か
    public void CheckCanPause()
    {
        //現在のシーン名を取得
        string sceneName = SceneManager.GetActiveScene().name;

        //タイトルと設定はポーズ不可
        if (sceneName == "TestSetting" || sceneName == "YuTestScene") m_canPause = false;
        else                                                          m_canPause = true;

    }

    //ポーズ
    private void Pause()
    {
        //ポーズ呼ばれたか確認
        Debug.Log("Pause()が呼ばれた");

        //ポーズ中
        m_isPaose = true;

        //動作の停止
        Time.timeScale = 0f;

        //ポーズ画面を表示
        m_pausePanel.SetActive(true);

    }

    //ポーズ解除
    public void Resume()
    {
        //ポーズ解除
        m_isPaose = false;

        //動作の再開
        Time.timeScale = 1f;

        //ポーズ画面を非表示
        m_pausePanel.SetActive(false);

    }

}
