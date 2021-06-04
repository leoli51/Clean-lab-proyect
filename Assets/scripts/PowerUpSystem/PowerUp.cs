using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float decision_time;
    float elapsed_time;

    bool player_1_in;
    bool player_2_in;

    // Start is called before the first frame update
    void Start()
    {
        player_1_in = false;
        player_2_in = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_1_in && player_2_in)
            elapsed_time += Time.deltaTime;
        else
            elapsed_time = 0;

        if (elapsed_time > decision_time)
        {
            Consume();
            transform.parent.parent.GetComponent<PowerUpManager>().Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1")
            player_1_in = true;
        if (other.tag == "Player2")
            player_2_in = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
            player_1_in = false;
        if (other.tag == "Player2")
            player_2_in = false;
    }

    void Consume() 
    {
        Debug.Log("Consumed power up");
    }
}
