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

            ChangeParticleFromParameter(newParams, m_particleSystems[i]);
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
}
