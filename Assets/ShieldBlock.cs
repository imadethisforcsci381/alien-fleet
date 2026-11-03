using UnityEngine;

//one destructible piece of a shield. Blocks are triggers, so anything that
//touches them passes through instead of bouncing off
public class ShieldBlock : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //darts and bullets keep their colliders on child objects, so use the rigidbody's object
        GameObject hitter = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

        //a projectile is used up by the first block it touches, even if it overlaps two at once
        if (hitter.GetComponent<Dart>() != null || hitter.GetComponent<AlienBullet>() != null)
        {
            if (!hitter.activeSelf)
            {
                return;
            }
            hitter.SetActive(false);
            Destroy(hitter);
            Destroy(gameObject);
        }
        //aliens plough straight through shields
        else if (hitter.GetComponent<Alien>() != null)
        {
            Destroy(gameObject);
        }
    }
}
