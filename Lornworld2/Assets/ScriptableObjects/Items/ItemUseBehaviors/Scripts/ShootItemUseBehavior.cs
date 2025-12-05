using System;
using UnityEngine;

public class ShootItemUseBehavior : ItemUseBehaviorScriptableObject
{
    [SerializeField]
    private Projectile projectile;

    public override void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate)
    {
        Vector2 direction = (mousePos - (Vector2)player.transform.position).normalized;

        //GameObject projectileObject = (GameObject)instantiate(projectilePrefab, player.transform.position, Quaternion.identity, new InstantiateParameters());

        //projectileObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        //projectileObject.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;

        Projectile.Create(projectile, player.transform.position, direction);
    }
}
