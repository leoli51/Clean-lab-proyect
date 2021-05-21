using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    public int start_round_fishes = 12;
    public int start_round_trash = 0;
    public GameObject[] fishes_prefabs;

    public float reproduction_rate = .75f;
    public float environmental_impact = .3f;
    public int overpopulation_limit = 120;
    public int trash_per_round = 2;

    int fishes_catched_current_round;
    int trash_catched_current_round;
    int fishes_catched = 0;
    int trash_catched = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void populateSea(int fish_count, int trash_count)
    {
        for (int i = 0; i < fish_count; i++)
            spawnFish(Vector3.random);
        for (int i = 0; i < trash_count; i++)
            spawnTrash(Vector3.random);
    }

    void spawnFish(Vector3 position)
    {
        index = Random.Range(0, fishes_prefabs.Length);
        Instantiate(fishes_prefabs[index], position, Quaternion.Identity);
        // todo spawn fiahes in child of FishManager to control their behaviour better
    }

    void spawnTrash(Vector3 position)
    {
        // TODO implement
    }

    int computeFishesForNextRound()
    {
        int remaining_fishes = 0; // retrieve fishes remaining
        int trash = 0; // retrieve trash that is remaining
        int next_fishes = remaining_fishes * (reproduction_rate + 1); // reproduction of fishes
        next_fishes -= (float)trash * environmental_impact; // environmental impact on fishes
        next_fishes = Mathf.clamp(next_fishes, 0, overpopulation_limit); // limit fishes number due to overpopulation/ too much trash
        return next_fishes;
    }

    int computeTrashForNextRound()
    {
        int trash = 0;
        return trash + trash_per_round;
    }
}
