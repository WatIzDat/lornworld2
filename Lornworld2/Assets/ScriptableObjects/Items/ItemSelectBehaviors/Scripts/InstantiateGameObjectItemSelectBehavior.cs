using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiateGameObjectItemSelectBehavior", menuName = "Scriptable Objects/Items/SelectBehaviors/InstantiateGameObject")]
public class InstantiateGameObjectItemSelectBehavior : ItemSelectBehaviorScriptableObject
{
    [SerializeField]
    private GameObject prefab;

    public event Action<GameObject> ItemSelected;
    public event Action ItemDeselected;

    public override void SelectItem()
    {
        ItemSelected?.Invoke(prefab);
    }

    public override void DeselectItem()
    {
        ItemDeselected?.Invoke();
    }
}
