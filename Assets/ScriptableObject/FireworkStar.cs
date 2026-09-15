using UnityEngine;

public enum StarColor
{
    Red, Green, Blue, Yellow, White
}

[CreateAssetMenu(fileName = "FireworkStar", menuName = "Scriptable Objects/FireworkStar")]
public class FireworkStar : ScriptableObject
{
    public StarColor color;
}
