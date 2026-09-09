using UnityEngine;
using UnityEngine.InputSystem;

public class SceneTransTest : MonoBehaviour
{
    [SerializeField] string targetSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TransitionManager.Instance.LoadScene(targetSceneName, TransitionType.Fade);
        }
    }
}
