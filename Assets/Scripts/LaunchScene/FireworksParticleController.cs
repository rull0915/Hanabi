using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksParticleController : MonoBehaviour
{
    // 完成品のScriptableObject
    [SerializeField] private CompletedFireworks m_fireworks;

    // 操作対象のParticleSystem
    [SerializeField] private ParticleSystem[] m_particleSystems = new ParticleSystem[3];

    // 発射パーティクル
    [SerializeField] private ParticleSystem m_launchParticle;

    // 失敗演出
    [SerializeField] private SpriteRenderer m_fallSprite;

    [SerializeField] private EasingConfig m_expEasing;
    [SerializeField] private float m_expLength;

    // 各パラメータの基準値
    private class EachParameters
    {
        public float initVel = 0.0f;
        public float lifeTimeMin = 0.0f, lifeTimeMax = 0.0f;
        public float count = 0.0f;
        public float drag = 0.0f;
        public Color color = Color.white;
    }

    private EachParameters[] m_parameters = new EachParameters[3];

    [Serializable]
    public class StarColorDictionary
    {
        public StarColor type;
        public Color color;
    }

    enum FallPattern
    {
        Unexploded,
        Accidental,
    }

    [SerializeField] private List<StarColorDictionary> m_colorDictionary = new List<StarColorDictionary>();
    private Dictionary<StarColor, Color> m_dictionaryBody = new Dictionary<StarColor, Color>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Dictionaryを作成
        foreach (var dic in m_colorDictionary)
        {
            m_dictionaryBody.Add(dic.type, dic.color);
        }

        // 初期状態のパラメータを格納
        for (int i = 0; i < m_particleSystems.Length; i++)
        {
            // 新規作成
            m_parameters[i] = new EachParameters();

            ParticleSystem particle = m_particleSystems[i];

            // 対応する花火玉がないパーティクルは非表示に
            if (i >= m_fireworks.stars.Count)
            {
                particle.gameObject.SetActive(false);

                continue;
            }

            // メインモジュール
            var mainModule = particle.main;
            m_parameters[i].initVel = mainModule.startSpeed.constant;
            m_parameters[i].lifeTimeMin = mainModule.startLifetime.constantMin;
            m_parameters[i].lifeTimeMax = mainModule.startLifetime.constantMax;

            // 放出
            var emission = particle.emission;
            m_parameters[i].count = emission.GetBurst(0).count.constant;

            // 速度
            var limitVelocity = particle.limitVelocityOverLifetime;
            m_parameters[i].drag = limitVelocity.drag.constant;

            // 色
            var colorOver = particle.colorOverLifetime;
            var gradient = colorOver.color.gradient;
            var colorKeys = gradient.colorKeys;
            m_parameters[i].color = colorKeys[1].color;
        }

        // 外殻がなければスキップ
        if (!m_fireworks.shell) return;

        uint shellSize = m_fireworks.shell.size;

        Vector3 newScale = Vector3.zero;

        // 外殻のサイズに応じてスケールを変更
        switch (shellSize)
        {
            case 1:
                newScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 2:
                newScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
            case 3:
                newScale = new Vector3(2.0f, 2.0f, 2.0f);
                break;
        }

        // 全パーティクルに適用
        foreach (var particle in m_particleSystems)
        {
            particle.gameObject.transform.localScale = newScale;
        }

        // 成功失敗判定
        bool success = true;

        // 失敗パターン
        FallPattern pattern = new FallPattern();

        // 優先度 合体タイミング > 量 > 素材
        float r = (m_fireworks._shellClosingAccuracy * 100 - 50) * 100 / (95 - 50);
        r = Mathf.Clamp(r, 0, 100);

        // 合体タイミング判定
        if (r < UnityEngine.Random.Range(0, 100))
        {
            success = false;
            pattern = FallPattern.Accidental;
        }

        // 外殻の素材によって成功確率を変える
        int randNum = UnityEngine.Random.Range(0, 100);

        switch (m_fireworks.shell.material)
        {
            case ShellMaterial.Paper:
                if (randNum < 75)
                {
                    pattern = FallPattern.Accidental;
                    success = false;
                }
                break;
            case ShellMaterial.Metal:
                if (randNum < 90)
                {
                    pattern = FallPattern.Unexploded;
                    success = false;
                }
                break;
        }

        // 成功の場合
        if (success)
        {
            // 外から順に設定を変えていく
            for (int i = 0; i < m_fireworks.stars.Count; i++)
            {
                CompletedStar star = m_fireworks.stars[i];

                // パラメータを作成
                EachParameters newParams = m_parameters[i];
                Color color;
                if (m_dictionaryBody.TryGetValue(star.star.color, out color))
                {
                    newParams.color = color;
                }

                // 入れた量を適用
                // 5~15を中心を1.0として0.6~1.4にします
                float value = 0.6f + (star.amount - 5) * (1.4f - 0.6f) / (15 - 5);
                float dis = value - 1.0f;

                // パラメータを乗算
                newParams.initVel *= (1.0f + dis * 0.5f);
                newParams.count *= (1.0f + dis * 2.0f); ;
                newParams.drag *= (1.0f - dis * 0.5f); ;

                ChangeParticleFromParameter(newParams, m_particleSystems[i]);
            }
        }

        // 失敗の場合
        else
        {
            // 失敗
            Fall(pattern);
        }
    }

    // パラメータからパーティクルの設定を変更する関数
    void ChangeParticleFromParameter(EachParameters param, ParticleSystem particle)
    {
        // メインモジュール
        var mainModule = particle.main;
        mainModule.startSpeed = param.initVel;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(param.lifeTimeMin, param.lifeTimeMax);

        // 放出
        var emission = particle.emission;
        var burst = emission.GetBurst(0);
        burst.count = param.count;
        emission.SetBurst(0, burst);

        // 速度
        var limitVelocity = particle.limitVelocityOverLifetime;
        limitVelocity.drag = param.drag;

        // 色
        var colorOver = particle.colorOverLifetime;
        var gradient = colorOver.color.gradient;

        var colorKeys = gradient.colorKeys;
        var alphaKeys = gradient.alphaKeys;

        colorKeys[1].color = param.color;

        gradient.SetKeys(colorKeys, alphaKeys);

        colorOver.color = gradient;
    }

    // 失敗
    private void Fall(FallPattern pattern)
    {
        switch (pattern)
        {
            // 花火を不発にするパターン (鉄で作った場合、火薬球が少なすぎた場合)
            case FallPattern.Unexploded:

                // 全てのパーティクルのCountを0に
                foreach (var p in m_particleSystems)
                {
                    EachParameters newParams = new EachParameters();
                    newParams.count = 0;

                    ChangeParticleFromParameter(newParams, p);
                }

                break;

            // 暴発パターン (紙で作った場合、火薬球を入れすぎた場合、合体のタイミングを間違えた場合)
            case FallPattern.Accidental:

                // 全てのパーティクルのCountを0に
                foreach (var p in m_particleSystems)
                {
                    EachParameters newParams = new EachParameters();
                    newParams.count = 0;

                    ChangeParticleFromParameter(newParams, p);
                }

                // 発射を非ループに
                var main = m_launchParticle.main;
                main.loop = false;

                // 失敗コルーチンの開始
                StartCoroutine(FallExplosion());

                break;
        }
    }

    private IEnumerator FallExplosion()
    {
        float elapsed = 0.0f;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        m_fallSprite.transform.localScale = startScale;
        m_fallSprite.gameObject.SetActive(true);

        while (elapsed < m_expLength)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / m_expLength);

            // イージングを適用
            float easeT = m_expEasing.Get(t);

            m_fallSprite.transform.localScale =
                Vector3.Lerp(startScale, endScale, easeT);

            yield return null;
        }

        elapsed = 0;

        while (elapsed < m_expLength)
        {
            elapsed += Time.deltaTime;

            var color = m_fallSprite.color;
            float t = (1 - elapsed / m_expLength);

            t = Mathf.Clamp01(t);

            Debug.Log(t);

            color.a = t;

            m_fallSprite.color = color;

            yield return null;
        }

        m_fallSprite.transform.localScale = endScale;
    }
}
