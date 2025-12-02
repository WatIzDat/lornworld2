using UnityEngine;

[CreateAssetMenu(fileName = "CoalUpgradedWeaponItemAttackBehavior", menuName = "Scriptable Objects/Items/AttackBehaviors/CoalUpgradedWeapon")]
public class CoalUpgradedWeaponItemAttackBehavior : ItemAttackBehaviorScriptableObject
{
    [SerializeField]
    private GameObject lightPrefab;

    public override void Attack(RaycastHit2D raycastHit, bool isEntity, Entity entity, System.Func<Object, Transform, Object> instantiate)
    {
        if (!isEntity)
        {
            return;
        }

        string objectName = nameof(CoalUpgradedWeaponItemAttackBehavior);

        if (raycastHit.transform.Find(objectName) != null)
        {
            return;
        }

        GameObject lightObject = (GameObject)instantiate(lightPrefab, raycastHit.transform);

        lightObject.name = objectName;
    }
}