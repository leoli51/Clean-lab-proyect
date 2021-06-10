using System.Collections;
using TMPro;
using UnityEngine;

public class StartmenuState : State
{
    // public Canvas startMenuCanvas
    public GameStartManager gameStartManager;
    AudioManager audioManager;
    public FollowPointer redBoat;
    public FollowPointer blueBoat;
    public SeaManager seaManager;
    public TextMeshPro captainText;
    public GameObject captain;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        gameStartManager.OnSelect.AddListener(StartGame);
    }

    public override void AfterActivate()
    {
        // deactivate boat movement
        redBoat.GetComponent<FollowPointer>().enabled = false;
        redBoat.GetComponent<Rigidbody>().isKinematic = true;
        blueBoat.GetComponent<FollowPointer>().enabled = false;
        blueBoat.GetComponent<Rigidbody>().isKinematic = true;

        // show start text
        captain.SetActive(true);
        captainText.text = "Ahoy sailors! Let's set out to catch some fish.";

        audioManager.Play(AudioManager.SoundName.MainMusic);
    }

    public override void BeforeDeactivate()
    {
        // hide start text
        captain.SetActive(false);
    }

    public void StartGame()
    {
        audioManager.Play(AudioManager.SoundName.Select);
        seaManager.populateSea(seaManager.start_fishes, seaManager.start_trash);
        stateMachine.GoTo<FishingState>();
    }
}
