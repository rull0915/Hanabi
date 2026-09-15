using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    //現在のボタンの値
    public int m_selectButton = 0;

    public GameObject m_Audio;
    public GameObject m_graphic;
    public GameObject m_language;


    public void Update()
    {
        //選択中のボタンに応じて描画を変える
        switch (m_selectButton) {
            case 0:
                m_Audio.   SetActive(true);
                m_graphic. SetActive(false);
                m_language.SetActive(false);
                break;
            case 1:
                m_Audio.   SetActive(false);
                m_graphic. SetActive(true);
                m_language.SetActive(false); 
                break;
            case 2:
                m_Audio.   SetActive(false);
                m_graphic. SetActive(false);
                m_language.SetActive(true); 
                break;
        }
    }

    //ボタンを押したら
    public void PushAudio(){ m_selectButton = 0; }      //オーディオ
    public void PushGraphic() { m_selectButton = 1; }   //グラフィック
    public void PushLanguage() { m_selectButton = 2; }  //言語


}
