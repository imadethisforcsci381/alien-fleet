using UnityEngine;

public class Alien : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject bulletPrefab;
    public AlienFleet fleet;

    static float bulletSpawnRate = .002f;
    public float lastShot = 0;
    public static float shotCooldown = .1f;

    public Collider Hitbox { get; private set; }

    void Awake()
    {
        Hitbox = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if(UnityEngine.Random.value < bulletSpawnRate && Time.time - lastShot > shotCooldown){
            GameObject bullet = Instantiate<GameObject>(bulletPrefab);
            bullet.transform.position = transform.position;
            lastShot = Time.time;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        fleet.RemoveAlien(this);
        Destroy(gameObject);
        Destroy(coll.gameObject);
    }
}
