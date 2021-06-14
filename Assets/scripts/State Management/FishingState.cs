using System.Collections;
using UnityEngine;

public class FishingState : State
{
    AudioManager audioManager;
    SeaManager seaManager;
    public Harbor harbor;
    public FollowPointer redBoat;
    public FollowPointer blueBoat;
    public Net net;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        seaManager = FindObjectOfType<SeaManager>();
    }

    public override void AfterActivate()
    {
        net.Restore();
        StartCoroutine(SetHarborListenerAfterSeconds(5f));

        // activate boat movement
        redBoat.GetComponent<FollowPointer>().enabled = true;
        redBoat.GetComponent<Rigidbody>().isKinematic = false;
        blueBoat.GetComponent<FollowPointer>().enabled = true;
        blueBoat.GetComponent<Rigidbody>().isKinematic = false;

        seaManager.nextRound();

        audioManager.PlayOnce(AudioManager.SoundName.StartLevel);
        audioManager.PlayOnce(AudioManager.SoundName.BlueBoatNoise);
        audioManager.PlayOnce(AudioManager.SoundName.RedBoatNoise);
        audioManager.PlayOnce(AudioManager.SoundName.MainMusic);
        audioManager.PlayOnce(AudioManager.SoundName.FishingAmbient);

    }

    public override void BeforeDeactivate()
    {
        harbor.OnBoatsInHarbor.RemoveListener(EndLevel);

        // deactivate boat movement
        redBoat.GetComponent<FollowPointer>().enabled = false;
        redBoat.GetComponent<Rigidbody>().isKinematic = true;
        blueBoat.GetComponent<FollowPointer>().enabled = false;
        blueBoat.GetComponent<Rigidbody>().isKinematic = true;


        audioManager.Stop(AudioManager.SoundName.BlueBoatNoise);
        audioManager.Stop(AudioManager.SoundName.RedBoatNoise);
        audioManager.Stop(AudioManager.SoundName.MainMusic);
        audioManager.Stop(AudioManager.SoundName.FishingAmbient);

    }

    public IEnumerator SetHarborListenerAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        harbor.OnBoatsInHarbor.AddListener(EndLevel);
    }

    public void EndLevel()
    {
        audioManager.Play(AudioManager.SoundName.EnterHarbor);
        stateMachine.GoTo<HarborState>();
    }
}
