using System.Collections;
using UnityEngine;

public class FishingState : State
{
    AudioManager audioManager;
    public SeaManager seaManager;
    public Harbor harbor;
    public FollowPointer redBoat;
    public FollowPointer blueBoat;
    public Net net;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindObjectOfType<AudioManager>();
        // TODO ideally count points/fish/trash here 
        // -> the Net can tell FishingState to count, FishManager can get information from State
        // -> or better to have a point counting script (score) and connect that with the net?
    }

    public override void AfterActivate()
    {
        net.Restore();
        harbor.OnBoatsInHarbor.AddListener(EndLevel);

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

    public void EndLevel()
    {
        audioManager.Play(AudioManager.SoundName.EnterHarbor);
        stateMachine.GoTo<HarborState>();
    }
}
