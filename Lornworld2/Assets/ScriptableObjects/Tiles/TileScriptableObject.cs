using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileScriptableObject", menuName = "Scriptable Objects/Tile")]
public class TileScriptableObject : RegistryEntry
{
    public Tile placeholderTile;
    public int order;
    public bool isWalkable;
}
