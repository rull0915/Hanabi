using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    //設定画面の呼び出し
    //Accessing the settings screen
    public void ChangeSettingFromTitle()
    {

        PauseManager.Instance.m_fromPause = false;

        TransitionManager.Instance.LoadScene("TestSetting",TransitionType.Fade);

    }

    public void ChangeSettingFromPause()
    {
        PauseManager.Instance.m_fromPause = true;

        //フェード・シーン遷移を動かすため一時的に再開
        Time.timeScale = 1f;

        TransitionManager.Instance.LoadSceneAdditive("TestSetting", TransitionType.Fade);
    }

    //設定から戻る処理
    public void ChangeBack()
    {
        if (PauseManager.Instance.m_fromPause)
        {
            //ポーズから設定を開いた場合
            //シーンを削除
            SceneManager.UnloadSceneAsync("TestSetting");

            PauseManager.Instance.m_pausePanel.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            //タイトルから設定を開いた場合
            TransitionManager.Instance.LoadScene("YuTestScene", TransitionType.Fade);
        }
    }

    //タイトル画面の呼び出し
    //Accessing the title screen
    public void ChanegeTitle()
    {
        //動作の再開
        Time.timeScale = 1f;

        //ポーズUIの非表示
        PauseManager.Instance.m_pausePanel.SetActive(false);

        TransitionManager.Instance.LoadScene("YuTestScene", TransitionType.Fade);
    }

    //セレクトシーンの呼び出し
    public void ChangeSelect()
    {   
        Time.timeScale = 1f;
        TransitionManager.Instance.LoadScene("selectScene", TransitionType.Fade);
    }

    //ゲームの終了 End Game
    public void ChangeExit()
    {
#if UNITY_EDITOR
        // Unity Editor上では再生を停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // ビルド版ではゲーム終了
    Application.Quit();
#endif
    }
}
