using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingManager : MonoBehaviour
{
    //スライダー Slider
    public Slider m_masterVolumeSlider;   //MasterVolume
    public Slider m_bgmSlider;            //BGM
    public Slider m_seSlider;             //SE
    public Slider m_brightness;           //Brightness

    //チェックボックス Toggle
    public Toggle m_fullScreen;  //スクリーン
    public Toggle m_english;     //英語
    public Toggle m_japanese;    //日本語

    //テキスト Text
    public TextMeshProUGUI m_masterVolumeValue;
    public TextMeshProUGUI m_bgmValue;
    public TextMeshProUGUI m_seValue;
    public TextMeshProUGUI m_brightnessValue;

    //Volume
    public Volume globalVolume;
    private LiftGammaGain liftGammaGain;

    //Save
    private static float m_saveBrightness = 70.0f;  //明るさ
    private static bool m_saveFullScreen = true;    //フルスクリーン
    private static bool m_saveEnglish = true;       //英語
    private static bool m_saveJapanese = false;     //日本語

    private void Start()
    {
        globalVolume = GlobalVolumeManager.Instance.gameObject.GetComponent<Volume>();

        //各音量のセット
        m_masterVolumeSlider.value = SoundManager.Instance.GetMasterVolume() * 100;  //MasterVolume
        m_bgmSlider.value          = SoundManager.Instance.GetBGM() * 100;           //BGM
        m_seSlider.value           = SoundManager.Instance.GetSE() * 100;            //SE

        //globalVolumeからliftGammaGainを取得
        globalVolume.profile.TryGet<LiftGammaGain>(out liftGammaGain);

        //Brightnessの初期値を設定
        m_brightness.value = m_saveBrightness;

        //Brightnessを起動
        SetBrightness(m_brightness.value);

        //FullScreenの初期化
        m_fullScreen.isOn = m_saveFullScreen;

        //言語の初期化
        m_english.isOn = m_saveEnglish;
        m_japanese.isOn = m_saveJapanese;

    }

    public void Update()
    {
        //Set
        SoundManager.Instance.ChangeMasterVolume(m_masterVolumeSlider.value / 100);  //MasterVolume
        SoundManager.Instance.ChangeBGMVolume(m_bgmSlider.value / 100);              //BGM
        SoundManager.Instance.ChangeSEVolume(m_seSlider.value / 100);                //SE

        //Brightness
        SetBrightness(m_brightness.value);
        m_saveBrightness = m_brightness.value;

        //Text
        m_masterVolumeValue.text = m_masterVolumeSlider.value.ToString();  //MasterVolume
        m_bgmValue.text = m_bgmSlider.value.ToString();                    //BGM
        m_seValue.text = m_seSlider.value.ToString();                      //SE
        m_brightnessValue.text = m_brightness.value.ToString();            //Brightness

        //チェックヒットボックス
        //スクリーン
        if (m_fullScreen.isOn)
        {
            Screen.fullScreen = true;
            m_saveFullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
            m_saveFullScreen = false;
        }

        //言語設定
        if (m_english.isOn)
        {
            m_saveEnglish = true;
            m_saveJapanese = false;
        }
        else if (m_japanese.isOn)
        {
            m_saveEnglish = false;
            m_saveJapanese = true;
        }

    }

    private void SetBrightness(float value)
    {
        //liftGammaGainが空だったら終了x
        if (liftGammaGain == null) return;

        //現在のGammaを取得
        Vector4 currentGamma = liftGammaGain.gamma.value;

        //GammaのWに明るさの値を設定
        currentGamma.w = (value - 50.0f) / 60.0f;

        //Gammaの設定を有効にする
        liftGammaGain.gamma.overrideState = true;

        //変更したGammaを適用
        liftGammaGain.gamma.value = currentGamma;
    }
}