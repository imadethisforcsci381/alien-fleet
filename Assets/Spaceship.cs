using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject dartPrefab;
    public LivesDisplay livesDisplay;
    public int startingLives = 3;
    public float invulnerableTime = 2f;

    public static float shotCooldown = .05f;
    public float lastShot = 0;
    public float speed = 10f;

    GameObject currentDart;
    float leftExtent;
    float rightExtent;
    int lives;
    float invulnerableUntil = 0;
    Renderer[] renderers;

    //lives are set up in Awake because the GameManager keeps this component
    //disabled (so Start doesn't run) until the player leaves the title screen
    void Awake()
    {
        lives = startingLives;
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        livesDisplay.SetLives(lives);

        //measure the ship so its edges, not its centre, stop at the screen edge
        //(the pivot isn't centred, so each side is measured separately)
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        leftExtent = transform.position.x - bounds.min.x;
        rightExtent = bounds.max.x - transform.position.x;
    }

    //never leave the ship invisible if the game ends mid-blink
    void OnDisable()
    {
        SetVisible(true);
    }

    // Update is called once per frame
    void Update()
    {
        //blink while invulnerable after being hit
        bool invulnerable = Time.time < invulnerableUntil;
        SetVisible(!invulnerable || Mathf.FloorToInt(Time.time * 10) % 2 == 0);

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x += speed * Time.deltaTime;
            transform.position = position;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
        }
        KeepOnScreen();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    //only one dart can be on screen at a time, so missed shots cost time
    public void TryShoot()
    {
        if (currentDart != null || Time.time - lastShot <= shotCooldown)
        {
            return;
        }
        currentDart = Instantiate<GameObject>(dartPrefab);
        currentDart.transform.position = transform.position;
        lastShot = Time.time;
    }

    //called when an alien bullet hits the ship
    public void Hit()
    {
        if (Time.time < invulnerableUntil)
        {
            return;
        }
        lives--;
        livesDisplay.SetLives(lives);
        if (lives <= 0)
        {
            //the fleet sees the ship is gone and ends the game
            Destroy(gameObject);
            return;
        }
        invulnerableUntil = Time.time + invulnerableTime;
    }

    void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }

    //clamp the ship between the left and right edges of the camera view
    void KeepOnScreen()
    {
        Camera cam = Camera.main;
        float depth = transform.position.z - cam.transform.position.z;
        float left = cam.ViewportToWorldPoint(new Vector3(0, 0, depth)).x + leftExtent;
        float right = cam.ViewportToWorldPoint(new Vector3(1, 0, depth)).x - rightExtent;
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, left, right);
        transform.position = position;
    }

}


