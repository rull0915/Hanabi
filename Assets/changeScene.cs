using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    //設定画面の呼び出し
    //Accessing the settings screen
    public void ChangeSetting()
    {
        SceneManager.LoadScene("TestSetting");
    }

    //タイトル画面の呼び出し
    //Accessing the title screen
    public void ChanegeTitle()
    {
        TransitionManager.Instance.LoadScene("YuTestScene", TransitionType.Fade);
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
