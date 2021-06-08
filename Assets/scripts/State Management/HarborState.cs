using System.Collections;
using UnityEngine;

public class HarborState : State
{
    // Canvas to show the points
    public PowerUpManager powerUpManager;
    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        powerUpManager.OnSelect.AddListener(StartNextLevel);
    }

    public override void AfterActivate()
    {
        // enable canvas
        // show "selling" of fish, counting of points
        powerUpManager.Show();

        audioManager.PlayOnce(AudioManager.SoundName.LoungeMusic);
        audioManager.PlayOnce(AudioManager.SoundName.HarborAmbient);
    }

    public override void BeforeDeactivate()
    {
        // hide/disable canvas

        audioManager.Stop(AudioManager.SoundName.LoungeMusic);
        audioManager.Stop(AudioManager.SoundName.HarborAmbient);
    }

    public void StartNextLevel()
    {
        // TODO call when powerup was selected
        stateMachine.GoTo<FishingState>();
    }
}
