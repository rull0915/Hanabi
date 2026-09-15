using UnityEngine;

public class FireworksParticleController : MonoBehaviour
{
    // 完成品のScriptableObject
    [SerializeField] private CompletedFireworks m_fireworks;

    // 操作対象のParticleSystem
    [SerializeField] private ParticleSystem[] m_particleSystems = new ParticleSystem[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 外殻のサイズを反映
        uint shellSize = m_fireworks.shell.size;
    }


}
