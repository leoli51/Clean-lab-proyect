using System.Collections;
using UnityEngine;

public class StartmenuState : State
{
    // public Canvas startMenuCanvas
    GameStartManager gameStartManager;
    AudioManager audioManager;
    public FollowPointer redBoat;
    public FollowPointer blueBoat;

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
        


        // show start text canvas (credits, explanation)
        // show start button element

        audioManager.Play(AudioManager.SoundName.MainMusic);
    }

    public override void BeforeDeactivate()
    {
        // hide/disable start menu canvas
        // hide/disable start menu buttons
    }

    public void StartGame()
    {
        audioManager.Play(AudioManager.SoundName.Select);
        stateMachine.GoTo<FishingState>();
    }
}
