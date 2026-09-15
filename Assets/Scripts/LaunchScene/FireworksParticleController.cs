using System;
using System.Collections.Generic;
using UnityEngine;

public class FireworksParticleController : MonoBehaviour
{
    // 完成品のScriptableObject
    [SerializeField] private CompletedFireworks m_fireworks;

    // 操作対象のParticleSystem
    [SerializeField] private ParticleSystem[] m_particleSystems = new ParticleSystem[3];

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

        // 優先度 合体タイミング > 量 > 素材

        // 合体タイミング判定


        // 外殻の素材によって成功確率を変える
        int randNum = UnityEngine.Random.Range(0, 100);

        switch (m_fireworks.shell.material)
        {
            case ShellMaterial.Paper:
                success = randNum >= 75; break;
            case ShellMaterial.Wood:
                success = randNum >= 0; break;
            case ShellMaterial.Metal:
                success = randNum >= 90; break;
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
            // 失敗パターンを確定する



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

    enum FallPattern
    {
        Unexploded,
        Accidental,
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

                break;
        }
    }
}
