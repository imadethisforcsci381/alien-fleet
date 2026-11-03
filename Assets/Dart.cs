using UnityEngine;

public class Dart : MonoBehaviour
{
    static float speed = .5f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.y += speed;
        transform.position = position;
        if(position.y > 10)
        {
            Destroy(gameObject);
        }
    }

}
