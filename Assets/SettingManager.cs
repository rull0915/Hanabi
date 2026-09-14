using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    //スライダー Slider
    public Slider m_masterVolumeSlider;   //MasterVolume
    public Slider m_bgmSlider;            //BGM
    public Slider m_seSlider;             //SE

    public void Update()
    {
        //MasterVolumeのセット
        //SetMasterVolume
        SoundManager.Instance.ChangeMasterVolume(m_masterVolumeSlider.value);

        //BGMのセット
        //SetBG
        SoundManager.Instance.ChangeBGMVolume(m_bgmSlider.value);

        //SEのセット
        //SetSE
        SoundManager.Instance.ChangeSEVolume(m_seSlider.value);

    }
}
