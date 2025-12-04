using System;
using UnityEngine;

public class ShootItemUseBehavior : ItemUseBehaviorScriptableObject
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float speed;

    public override void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate)
    {
        Vector2 direction = (mousePos - (Vector2)player.transform.position).normalized;

        GameObject projectileObject = (GameObject)instantiate(projectilePrefab, player.transform.position, Quaternion.identity, new InstantiateParameters());

        projectileObject.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }
}
