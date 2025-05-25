using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public float startTimeInMinutes = 3f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    [Header("UI Texts")]
    public TextMeshProUGUI timerText1;
    public TextMeshProUGUI timerText2;
    public TextMeshProUGUI timesUpText1;
    public TextMeshProUGUI timesUpText2;

    [Header("Testing")]
    public float testSpeedMultiplier = 1f;

    void Start()
    {
        timeRemaining = startTimeInMinutes * 60f;

        if (timesUpText1 != null) timesUpText1.gameObject.SetActive(false);
        if (timesUpText2 != null) timesUpText2.gameObject.SetActive(false);

        // Do not start automatically
        timerIsRunning = false;
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime * testSpeedMultiplier;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);

                if (timesUpText1 != null) timesUpText1.gameObject.SetActive(true);
                if (timesUpText2 != null) timesUpText2.gameObject.SetActive(true);

                //Block all interactions
                //Set active panel 
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timerText1 != null) timerText1.text = formattedTime;
        if (timerText2 != null) timerText2.text = formattedTime;
    }
}
