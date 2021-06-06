using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStartPosition : MonoBehaviour
{

    public string player_tag;

    [HideInInspector]
    public bool player_ready = false;

    bool disappearing = false;
    float disappear_time;
    float elapsed_disappear_time = 0;

    SpriteRenderer sprite;
    TextMeshPro text;

    Color sprite_start_color;
    Color text_start_color;

    Color transparent;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();

        sprite_start_color = sprite.color;
        text_start_color = text.color;
        transparent = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (disappearing)
        {
            elapsed_disappear_time += Time.deltaTime;
            float disappear_lerp_value = Mathf.Clamp01(elapsed_disappear_time / disappear_time);
            sprite.color = Color.Lerp(sprite_start_color, transparent, disappear_lerp_value);
            text.color = Color.Lerp(text_start_color, transparent, disappear_lerp_value);
        }
            
    }

    public void disappear(float disappear_time)
    {
        this.disappear_time = disappear_time;
        disappearing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == player_tag)
            player_ready = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == player_tag)
            player_ready = true;
    }
}
