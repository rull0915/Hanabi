using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CompletedStar
{
    public FireworkStar star;
    public int layer;
    public float amount;
}

[CreateAssetMenu(fileName = "CompletedFireworks", menuName = "Scriptable Objects/CompletedFireworks")]
public class CompletedFireworks : ScriptableObject
{
    public OuterShell shell;
    public List<CompletedStar> stars;
    public float _shellClosingAccuracy;
}
