using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{

    public float time_to_disappear = .75f;

    PlayerStartPosition p1, p2;
    bool game_started = false;

    // Start is called before the first frame update
    void Start()
    {
        p1 = transform.GetChild(0).GetComponent<PlayerStartPosition>();
        p2 = transform.GetChild(1).GetComponent<PlayerStartPosition>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.player_ready && p2.player_ready && !game_started)
        {
            game_started = true;
            p1.disappear(time_to_disappear);
            p2.disappear(time_to_disappear);

            StartGame();
        }
        if (game_started)
        {
            time_to_disappear -= Time.deltaTime;
            if (time_to_disappear <= 0)
                Destroy(this.gameObject);
        }
    }

    void StartGame()
    {
        Debug.Log("Game is starting what should we do?");
    }
}
