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
        //実行時に動作を止める Stop execution
        UnityEditor.EditorApplication.isPlaying = false;

        //ゲームの終了 End Game
        Application.Quit();
    }

}
