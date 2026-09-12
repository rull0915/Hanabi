using UnityEngine;

public enum ShellMaterial
{
    Paper,
    Wood,
    Metal,
}

[CreateAssetMenu(fileName = "OuterShell", menuName = "Scriptable Objects/OuterShell")]
public class OuterShell : ScriptableObject
{
    public uint size;

    public ShellMaterial material;
}
