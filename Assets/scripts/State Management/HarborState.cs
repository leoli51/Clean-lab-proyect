using System.Collections;
using TMPro;
using UnityEngine;

public class HarborState : State
{
    public int neededFishCount = 10;

    public PowerUpManager powerUpManager;
    public Net net;
    AudioManager audioManager;
    SeaManager seaManager;
    public TextMeshPro captainText;
    public GameObject captain;
    public TextMeshPro timerText;
    public GameObject timer;

    public GameObject counter;
    public TextMeshPro fishCountText;
    public TextMeshPro trashCountText;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        seaManager = FindObjectOfType<SeaManager>();
        powerUpManager.OnSelect.AddListener(StartNextLevel);

        counter.SetActive(false);
        timer.SetActive(false);
    }

    public override void AfterActivate()
    {
        captain.SetActive(true);
        counter.SetActive(true);

        // show fish + trash count
        fishCountText.text = net.fishCount.ToString("F0");
        trashCountText.text = net.trashCount.ToString("F0");

        // powerups / timer + captain's message
        if (net.fishCount >= neededFishCount)
        {
            powerUpManager.Show();

            if (net.fishCount > neededFishCount+6)
            {
                captainText.text = "You caught way too much fish. Try to keep the ecosystem in balance!";
            }
            else if (net.trashCount >= 3)
            {
                captainText.text = "The sea got cleaner. Thanks for taking care of that trash!";
            }
            else if (seaManager.GetTrashPopulation() > 8)
            {
                captainText.text = "The sea is very poluted. We should do something to change this.";
            }
            else if (net.fishCount <= neededFishCount+3)
            {
                captainText.text = "Perfect catch! Choose your gift.";
            }
            else
            {
                captainText.text = "Fish for dinner. Fish for lunch. Fish for breakfast. That's life :)";
            }
        }
        else
        {
            if (net.ripped)
            {
                captainText.text = "Take care of the net!";
            }
            else if (net.trashCount >= 3)
            {
                captainText.text = "The sea got cleaner. Thanks for taking care of that trash!";
            }
            else if (seaManager.GetTrashPopulation() > 8)
            {
                captainText.text = "The sea is very poluted. We should do something to change this.";
            }
            else
            {
                captainText.text = "We didn't catch enough fish today :(";
            }


            StartCoroutine(TimerToNextLevel(5f)); 
        }
        

        audioManager.PlayOnce(AudioManager.SoundName.LoungeMusic);
        audioManager.PlayOnce(AudioManager.SoundName.HarborAmbient);
    }

    public override void BeforeDeactivate()
    {
        // hide ui
        captain.SetActive(false);
        counter.SetActive(false);
        timer.SetActive(false);

        audioManager.Stop(AudioManager.SoundName.LoungeMusic);
        audioManager.Stop(AudioManager.SoundName.HarborAmbient);
    }

    public IEnumerator TimerToNextLevel(float seconds)
    {
        timer.SetActive(true);
        while (seconds > 0)
        {
            timerText.text = seconds.ToString("F0");
            yield return new WaitForSeconds(1f);
            seconds -= 1;
        }
        StartNextLevel();
    }

    public void StartNextLevel()
    {
        stateMachine.GoTo<FishingState>();
    }
}
