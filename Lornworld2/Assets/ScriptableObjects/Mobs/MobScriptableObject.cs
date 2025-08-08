using UnityEngine;

[CreateAssetMenu(fileName = "MobScriptableObject", menuName = "Scriptable Objects/Mob")]
public class MobScriptableObject : ScriptableObject
{
    public string mobName;
    public float maxHealth;
}
