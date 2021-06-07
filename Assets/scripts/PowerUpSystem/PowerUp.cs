using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{

    public string gameobject_name;
    public string component_name;
    public string property_name;
    public float increase_amount;

    public float decision_time;
    float elapsed_decision_time;

    public Color default_color;
    public Color decision_color;

    [Range(1, 2)]
    public float size_increment = 1.2f;
    public float grow_time = .5f;
    float elapsed_grow_time = 0;

    public Image circle_fill;

    bool player_1_in;
    bool player_2_in;

    Func<bool, int> toInt = x => x ? 1 : 0;

    // Start is called before the first frame update
    void Start()
    {
        player_1_in = false;
        player_2_in = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (player_1_in || player_2_in) {
            elapsed_grow_time += Time.deltaTime;
            elapsed_decision_time += Time.deltaTime;
        }
        else
        {
            elapsed_decision_time = 0;
            elapsed_grow_time = 0;
        }

        if (elapsed_decision_time > decision_time)
        {
            Consume();
            transform.parent.parent.GetComponent<PowerUpManager>().Clear();
        }

        float new_scale = Mathf.Lerp(1, size_increment, Mathf.Clamp01(elapsed_grow_time / grow_time));
        transform.localScale = new Vector3(new_scale, 0, new_scale);

        float player_weight = (toInt(player_1_in) + toInt(player_2_in)) / 2.0f;

        elapsed_decision_time = Mathf.Clamp(elapsed_decision_time, 0, decision_time * player_weight);
        circle_fill.fillAmount = Mathf.Clamp01(elapsed_decision_time / decision_time);
        circle_fill.color = Color.Lerp(default_color, decision_color, circle_fill.fillAmount);
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
        GameObject gameobject = GameObject.Find(gameobject_name);
        Component component = gameobject.GetComponent(component_name);
        component.GetType().GetField(property_name).SetValue(component, ((float)component.GetType().GetField(property_name).GetValue(component)) + increase_amount);
    }
}
