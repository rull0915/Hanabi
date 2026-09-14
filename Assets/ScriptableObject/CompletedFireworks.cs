using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CompletedStar
{
    public FireworkStar star;
    public float amount;
}

[CreateAssetMenu(fileName = "CompletedFireworks", menuName = "Scriptable Objects/CompletedFireworks")]
public class CompletedFireworks : ScriptableObject
{
    public OuterShell shell;
    public List<CompletedStar> stars;
}
