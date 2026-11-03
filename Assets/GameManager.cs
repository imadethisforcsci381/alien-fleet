using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Inscribed")]
    public GameObject startScreen;
    public GameObject endScreen;
    public TextMeshProUGUI endTitle;
    public TextMeshProUGUI endMessage;
    public Spaceship ship;

    enum State { Title, Playing, Over }
    State state;

    //survives the scene reload so "play again" skips the title screen
    static bool restarting = false;

    //this project's Enter Play Mode settings skip the domain reload, so statics
    //would otherwise carry over from the previous Play session
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        restarting = false;
        Instance = null;
    }

    //set up in Awake so the game is frozen before other objects' Start runs
    void Awake()
    {
        Instance = this;
        endScreen.SetActive(false);
        if (restarting)
        {
            restarting = false;
            StartCoroutine(BeginPlay());
        }
        else
        {
            //freeze the game behind the title screen until the player starts
            state = State.Title;
            startScreen.SetActive(true);
            ship.enabled = false;
            Time.timeScale = 0;
        }
    }

    void Update()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            return;
        }
        if (state == State.Title)
        {
            StartCoroutine(BeginPlay());
        }
        else if (state == State.Over)
        {
            restarting = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator BeginPlay()
    {
        state = State.Playing;
        startScreen.SetActive(false);
        Time.timeScale = 1;
        //wait a frame so the Space press that started the game doesn't also fire a dart
        yield return null;
        if (ship != null)
        {
            ship.enabled = true;
        }
    }

    public void EndGame(bool won, string message)
    {
        if (state == State.Over)
        {
            return;
        }
        state = State.Over;
        Time.timeScale = 0;
        if (ship != null)
        {
            ship.enabled = false;
        }
        endTitle.text = won ? "YOU WIN!" : "GAME OVER";
        endTitle.color = won ? new Color(0.55f, 1f, 0.6f) : new Color(1f, 0.4f, 0.35f);
        endMessage.text = message;
        endScreen.SetActive(true);
    }
}
