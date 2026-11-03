using UnityEngine;
using TMPro;

public class AlienCounter : MonoBehaviour
{
    private TextMeshProUGUI uiText;

    // Awake runs before any Start, so the fleet can set the count from its own Start
    void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    //only rebuild the string when the count changes, not every frame
    public void SetCount(int count)
    {
        uiText.text = "Remaining Aliens: " + count.ToString("#,0");
    }
}
