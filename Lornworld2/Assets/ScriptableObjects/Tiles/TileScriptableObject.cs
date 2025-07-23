using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileScriptableObject", menuName = "Scriptable Objects/Tile")]
public class TileScriptableObject : ScriptableObject
{
    public string tileName;
    public Tile placeholderTile;
    public int order;
}
