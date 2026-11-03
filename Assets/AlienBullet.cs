using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    static float speed = .1f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.y -= speed;
        transform.position = position;
        if(position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        Destroy(gameObject);
        //the ship has lives, so it handles its own hits; anything else (a dart) is destroyed
        Spaceship ship = coll.gameObject.GetComponentInParent<Spaceship>();
        if (ship != null)
        {
            ship.Hit();
        }
        else
        {
            Destroy(coll.gameObject);
        }
    }
}
