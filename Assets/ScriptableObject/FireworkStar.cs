using UnityEngine;

public enum StarColor
{
    Red, Green, Blue
}

[CreateAssetMenu(fileName = "FireworkStar", menuName = "Scriptable Objects/FireworkStar")]
public class FireworkStar : ScriptableObject
{
    public StarColor color;
}
