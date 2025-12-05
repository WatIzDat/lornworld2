using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxLifetime;

    private float aliveTime;

    [SerializeField]
    private int pierceAmount = 1;

    private int currentPierceCount;

    //[SerializeField]
    //private int forkAmount;

    //[SerializeField]
    //private Projectile forkProjectile;

    //public static Projectile Create(Projectile projectile, Vector2 pos, Quaternion rotation)
    //{
    //    GameObject projectileObject = Instantiate(projectile.gameObject, pos, rotation);

    //    //projectileObject.GetComponent<Rigidbody2D>().linearVelocity = direction * projectile.speed;
    //    projectileObject.GetComponent<Rigidbody2D>().AddForce(Vector2.one * projectile.speed);

    //    return projectileObject.GetComponent<Projectile>();
    //}

    public static Projectile Create(Projectile projectile, Vector2 pos, Vector2 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        GameObject projectileObject = Instantiate(projectile.gameObject, pos, rotation);

        projectileObject.GetComponent<Rigidbody2D>().linearVelocity = direction * projectile.speed;

        return projectileObject.GetComponent<Projectile>();
    }

    private void Update()
    {
        if (aliveTime >= maxLifetime)
        {
            Destroy(gameObject);
        }
        else
        {
            aliveTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isMob = collision.TryGetComponent(out Mob mob);

        if (isMob)
        {
            mob.TakeDamage(damage);

            currentPierceCount++;

            if (currentPierceCount >= pierceAmount)
            {
                Destroy(gameObject);
            }
            //else
            //{
            //    Vector2 direction = GetComponent<Rigidbody2D>().linearVelocity.normalized 
            //    GameObject projectileLeft = Create(forkProjectile, collision.transform.position, ;
            //    GameObject projectileRight = Instantiate(forkProjectile, collision.transform.position, Quaternion.Euler(0f, 0f, -45f));


            //}
        }
    }
}
