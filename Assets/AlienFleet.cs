using UnityEngine;
using System.Collections.Generic;

public class AlienFleet : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject alienPrefab;

    public AlienCounter alienCounter;

    public List<Alien> aliens = new List<Alien> { };

    float speed = .1f;
    float baseSpeed = .1f;
    float maxSpeed = .4f;
    float dropSpeed = .12f;
    int startingCount;
    Spaceship ship;
    bool gameOver = false;
    float leftBound = -16;
    float rightBound = 16;
    float downTime = .1f;
    bool movingDown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Alien alien = Instantiate<GameObject>(alienPrefab).GetComponent<Alien>();
                alien.transform.position = new Vector3(2 * i - 12, 10 - 2 * j, 0);
                alien.fleet = this;
                aliens.Add(alien);
            }
        }
        startingCount = aliens.Count;
        alienCounter.SetCount(aliens.Count);
        ship = FindAnyObjectByType<Spaceship>();

    }

    public void RemoveAlien(Alien alien)
    {
        aliens.Remove(alien);
        alienCounter.SetCount(aliens.Count);
        if (aliens.Count == 0)
        {
            gameOver = true;
            GameManager.Instance.EndGame(true, "You destroyed the entire fleet");
        }
    }

    void FixedUpdate()
    {
        if (gameOver)
        {
            return;
        }

        //speed up as aliens are destroyed, keeping the current direction
        float fractionDestroyed = 1 - (float)aliens.Count / startingCount;
        speed = Mathf.Sign(speed) * Mathf.Lerp(baseSpeed, maxSpeed, fractionDestroyed);

        //move all aliens, tracking the fleet's left and right edges as we go
        float left = float.MaxValue, right = float.MinValue;
        foreach (Alien alien in aliens)
        {
            Vector3 position = alien.transform.position;
            if (!movingDown)
            {
                position.x += speed;
            }
            else
            {
                position.y += -dropSpeed;
            }
            alien.transform.position = position;
            left = Mathf.Min(left, position.x);
            right = Mathf.Max(right, position.x);
        }

        //change alien direction
        if (speed > 0 && right >= rightBound || speed < 0 && left <= leftBound)
        {
            speed = -speed;
            movingDown = true;
            Invoke(nameof(StartSidewaysMotion), downTime);
        }

        string reason = UnwinnableReason();
        if (reason != null)
        {
            gameOver = true;
            GameManager.Instance.EndGame(false, reason);
        }
    }

    //the game can't be won once the ship is gone, or an alien has dropped
    //below the line darts are fired from (darts only travel upward).
    //returns why, or null if the game can still be won
    string UnwinnableReason()
    {
        if (aliens.Count == 0)
        {
            return null;
        }
        if (ship == null)
        {
            return "Your ship was destroyed";
        }
        float fireLine = ship.transform.position.y;
        foreach (Alien alien in aliens)
        {
            if (alien.Hitbox.bounds.max.y < fireLine)
            {
                return "The aliens got past your guns";
            }
        }
        return null;
    }

    void StartSidewaysMotion()
    {
        movingDown = false;
    }
}
