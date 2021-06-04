using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    public List<GameObject> power_ups;

    Transform power_up1_transform;
    Transform power_up2_transform;

    // Start is called before the first frame update
    void Start()
    {
        power_up1_transform = transform.GetChild(0);
        power_up2_transform = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Show()
    {
        // chooses 2 power ups 
        // instantiate them
        GameObject p1 = Instantiate(power_ups[Random.Range(0, power_ups.Count)]) as GameObject;
        GameObject p2 = Instantiate(power_ups[Random.Range(0, power_ups.Count)]) as GameObject;

        p1.transform.parent = power_up1_transform;
        p2.transform.parent = power_up2_transform;
    }

    public void Clear()
    {
        // destroy children 
        Destroy(power_up1_transform.GetChild(0));
        Destroy(power_up2_transform.GetChild(0));
    }

}
