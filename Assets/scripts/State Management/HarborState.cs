using System.Collections;
using UnityEngine;

public class HarborState : State
{
    // Canvas to show the points
    // PowerUpMenu powerUpMenu;
    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        // powerUpMenu.OnSelect.AddListener(StartNextLevel);
    }

    public override void AfterActivate()
    {
        // enable canvas
        // show "selling" of fish, counting of points
        // enable power up menu buttons

        audioManager.PlayOnce(AudioManager.SoundName.LoungeMusic);
        audioManager.PlayOnce(AudioManager.SoundName.HarborAmbient);
    }

    public override void BeforeDeactivate()
    {
        // hide/disable power up buttons
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
