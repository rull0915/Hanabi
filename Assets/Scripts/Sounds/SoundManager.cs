//====================================================//
// ファイル名   : SoundManager.cs
// 作成者       : Hoshino Ryunosuke
// 作成日       : 2026/07/29
//
// 概要 : 音管理システム
//        シングルトンにプロジェクト全体で唯一のインスタンスを所持します
//
// 更新履歴 :
// 2026/07/29 新規作成
//====================================================//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//====================================================//
// クラス宣言
//====================================================//

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // 音情報
    [Serializable]
    public class AudioData
    {
        public string key; // サウンドのキー名
        public AudioClip clip; // オーディオクリップ
        [Range(0f, 1f)] public float volume = 1f; // 個別音量
    }

    private class AudioDataImpl
    {
        public AudioClip clip; // オーディオクリップ
        [Range(0f, 1f)] public float volume = 1f; // 個別音量

        // コンストラクタ
        public AudioDataImpl(AudioClip clip, float volume)
        {
            this.clip = clip;
            this.volume = volume;
        }
    }

    // 使用する音のリスト
    [SerializeField] SoundList m_soundList;

    // BGMリストの本体
    private Dictionary<string, AudioDataImpl> m_bgmSoundListImpl = new Dictionary<string, AudioDataImpl>();
    // SEリストの本体
    private Dictionary<string, AudioDataImpl> m_seSoundListImpl = new Dictionary<string, AudioDataImpl>();

    // BGM用のAudioSource
    // 切り替えのために2つ保持します
    AudioSource m_bgmSourceA;
    AudioSource m_bgmSourceB;

    // 使用中のAudioSourceの参照
    AudioSource m_currentSource;

    // SE用のAudioSourceプール
    Pool<AudioSource> m_sePool;

    // 切り替えのコルーチン
    Coroutine m_fadeCoroutine;

    // ボリューム
    [Range(0f, 1f)] public float m_masterVolume = 1f;
    [Range(0f, 1f)] public float m_bgmVolume = 1f;
    [Range(0f, 1f)] public float m_seVolume = 1f;

    // 変更関数
    public void ChangeMasterVolume(float volume)
    {
        m_masterVolume = Mathf.Clamp01(volume);
    }
    public void ChangeBGMVolume(float volume)
    {
        m_bgmVolume = Mathf.Clamp01(volume);
    }
    public void ChangeSEVolume(float volume)
    {
        m_seVolume = Mathf.Clamp01(volume);
    }

    // 開始時処理
    protected override void OnInitialize()
    {
        // BGMのリストをマップに変換
        foreach (var sound in m_soundList.m_bgmList)
        {
            m_bgmSoundListImpl.Add(sound.key, new AudioDataImpl(sound.clip, sound.volume));
        }

        // SEのリストをマップに変換
        foreach (var sound in m_soundList.m_seList)
        {
            m_seSoundListImpl.Add(sound.key, new AudioDataImpl(sound.clip, sound.volume));
        }

        // BGM用Sourceの作成
        m_bgmSourceA = gameObject.AddComponent<AudioSource>();
        m_bgmSourceB = gameObject.AddComponent<AudioSource>();

        // ループをオン
        m_bgmSourceA.loop = true;
        m_bgmSourceB.loop = true;
        // 開始時再生をオフ
        m_bgmSourceA.playOnAwake = false;
        m_bgmSourceB.playOnAwake = false;

        // 最初はAを使用する
        m_currentSource = m_bgmSourceA;

        // SE用プールの初期化
        m_sePool = new Pool<AudioSource>(() =>   // AudioSourceを追加するラムダ式
        {
            // AudioSourceを自分に追加
            AudioSource source = gameObject.AddComponent<AudioSource>();

            // 開始時に再生しないように設定
            source.playOnAwake = false;

            // 追加したAudioSourceを返す
            return source;
        });
    }

    // ゲーム開始時に自動生成
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        InitializeBeforeSceneLoad("SoundManager");
    }

    // SEを再生する関数
    public void PlaySE(string key)
    {
        // キーに対応するClipを取得
        AudioDataImpl impl = null;
        m_seSoundListImpl.TryGetValue(key, out impl);

        // 取得できなければ何もしない
        if (impl == null) return;

        // 未使用のAudioSourceを取得
        AudioSource source = m_sePool.GetUnUsedObject();

        // 使用するクリップを変更
        source.clip = impl.clip;

        // 音量を変更
        source.volume = impl.volume * m_seVolume * m_masterVolume;

        // 再生
        source.Play();

        // 再生後AudioSourceを返すコルーチンを開始
        StartCoroutine(ReturnToPoolRoutine(source, impl.clip.length));
    }

    // BGMを再生する関数
    public void PlayBGM(string key, float fadeTime = -1f)
    {
        // キーの存在チェック
        if (!m_bgmSoundListImpl.TryGetValue(key, out var bgmData)) return;

        // 既に同じBGMが流れている場合は何もしない
        if (m_currentSource.isPlaying && m_currentSource.clip == bgmData.clip) return;

        // 使用していないSourceを次に使う
        AudioSource nextSource = (m_currentSource == m_bgmSourceA) ? m_bgmSourceB : m_bgmSourceA;

        // 進行中のフェード処理があれば停止
        if (m_fadeCoroutine != null)
        {
            StopCoroutine(m_fadeCoroutine);
        }

        // クロスフェード実行
        m_fadeCoroutine = StartCoroutine(CrossFadeRoutine(m_currentSource, nextSource, bgmData, fadeTime));

        // メインのAudioSource参照を更新
        m_currentSource = nextSource;
    }

    // BGMの停止を行う関数
    public void StopBGM(float fadeTime = -1f)
    {
        // フェード中なら
        if (m_fadeCoroutine != null)
        {
            // コルーチンを停止
            StopCoroutine(m_fadeCoroutine);
        }

        // フェードアウトコルーチンを開始
        m_fadeCoroutine = StartCoroutine(FadeOutRoutine(m_currentSource, fadeTime));
    }

    //--------------------------------------------
    // コルーチン
    //--------------------------------------------

    // SEの再生が終わったらプールにAudioSourceを返すコルーチン
    private System.Collections.IEnumerator ReturnToPoolRoutine(AudioSource source, float delay)
    {
        // 再生時間の間待つ
        yield return new WaitForSeconds(delay);

        // 返却してもらう
        m_sePool.Return(source);
    }

    // クロスフェードでBGMの切り替えを行うコルーチン
    private IEnumerator CrossFadeRoutine(AudioSource activeSource, AudioSource nextSource, AudioDataImpl nextClip, float fadeTime)
    {
        // 切り替え先のAudioSourceを設定
        nextSource.clip = nextClip.clip;    // クリップ
        nextSource.volume = 0f;             // ボリュームの初期値を0に
        nextSource.Play();                  // 再生開始

        // 目標の音量を設定
        float targetVolume = nextClip.volume * m_bgmVolume * m_masterVolume;

        // 切り替え前のBGMの音量を取得
        float startActiveVolume = activeSource.volume;

        // タイマーの初期化
        float timer = 0f;

        // 切り替えに時間を掛けるなら
        if (fadeTime > 0f)
        {
            // タイマーが切り替え時間を超えるまでループ
            while (timer < fadeTime)
            {
                // 時間の加算
                timer += Time.deltaTime;

                // 現在の経過時間割合を算出
                float rate = timer / fadeTime;

                // 切り替え元の音量を下げる
                activeSource.volume = Mathf.Lerp(startActiveVolume, 0f, rate);

                // 切り替え先の音量を上げる
                nextSource.volume = Mathf.Lerp(0f, targetVolume, rate);

                yield return null;
            }
        }

        // 切り替え先のBGMを確定
        nextSource.volume = targetVolume;

        // 切り替え元のBGMの停止
        activeSource.volume = 0f;
        activeSource.Stop();

        m_fadeCoroutine = null;
    }

    // フェードアウトを行うコルーチン
    private IEnumerator FadeOutRoutine(AudioSource source, float fadeTime)
    {
        // フェード開始前の音量を取得
        float startVolume = source.volume;

        // タイマーの初期化
        float timer = 0f;

        // フェードにに時間をかけるなら
        if (fadeTime > 0f)
        {
            // フェード時間を超えるまでループ
            while (timer < fadeTime)
            {
                // タイマーの加算
                timer += Time.deltaTime;

                // 0~開始時のボリュームに線形補間
                source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeTime);

                yield return null;
            }
        }

        // ボリュームを0で固定
        source.volume = 0f;
        
        // 再生を終了
        source.Stop();

        m_fadeCoroutine = null;
    }
}
