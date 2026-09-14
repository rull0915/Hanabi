using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SelectedMaterials", menuName = "Scriptable Objects/SelectedMaterials")]
public class SelectedMaterials : ScriptableObject
{
    public OuterShell outerShell;

    public List<FireworkStar> stars;
}
