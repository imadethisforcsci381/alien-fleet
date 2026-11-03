using UnityEngine;

//shows one ship icon per remaining life
public class LivesDisplay : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject[] icons;

    public void SetLives(int lives)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActive(i < lives);
        }
    }
}
