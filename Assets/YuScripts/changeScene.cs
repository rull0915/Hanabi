using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    //設定画面の呼び出し
    //Accessing the settings screen
    public void ChangeSetting()
    {
        TransitionManager.Instance.LoadScene("TestSetting", TransitionType.Fade);
    }

    //タイトル画面の呼び出し
    //Accessing the title screen
    public void ChanegeTitle()
    {
        TransitionManager.Instance.LoadScene("YuTestScene", TransitionType.Fade);
    }

    //セレクトシーンの呼び出し
    public void ChangeSelect()
    {
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
