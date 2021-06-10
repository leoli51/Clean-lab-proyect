using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour
{

    public GameObject[] fishes_prefabs;
    public GameObject[] trash_prefabs;

    public int overpopulation_limit = 50;
    public float reproduction_rate = .75f;
    public float environmental_impact = .3f;
    public int trash_per_round = 2;
    public int start_fishes = 12;
    public int start_trash = 2;

    public MeshRenderer water_plane;
    public Color polluted_water;
    Color clean_water;

    //private List<GameObject> createdObjects = new List<GameObject>();
    private int fishPopulation = 0;
    private int trashPopulation = 0;

    // Start is called before the first frame update
    void Start()
    {
        //populateSea(start_fishes, start_trash);
        clean_water = water_plane.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void nextRound() {

        int addFishes = computeFishesForNextRound();
        int addTrash = computeTrashForNextRound();

        wipeSea(); //deletes previous round fishes, because in the populateSea the remaining are already added
        populateSea(addFishes, addTrash);

        // set water color
        // color based on percentage of trash in the sea
        water_plane.material.color = Color.Lerp(clean_water, polluted_water, ((float)addTrash) / (addFishes + addTrash));

    }

    public void populateSea(int fish_count, int trash_count)
    {
        SpawnFish(fish_count);
        SpawnTrash(trash_count);
    }


    public void SpawnFish(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, fishes_prefabs.Length);
            GameObject new_fish = Instantiate(fishes_prefabs[index]);
            new_fish.gameObject.transform.position = new Vector3(Random.Range(0, 100), -1, Random.Range(0, 100));
            new_fish.transform.parent = transform;
        }
    }

    void SpawnTrash(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, trash_prefabs.Length);
            GameObject new_trash = Instantiate(trash_prefabs[index]);
            new_trash.gameObject.transform.position = new Vector3(Random.Range(0, 100), -1, Random.Range(0, 100));
            new_trash.transform.parent = transform;
            new_trash.transform.rotation = Random.rotation;
        }
    }

    int computeFishesForNextRound()
    {
        updateFishTrashPopulation();

        int remaining_fishes = fishPopulation; // retrieve fishes remaining
        int trash = trashPopulation; // retrieve trash that is remaining
        int next_fishes = (int)(remaining_fishes * (reproduction_rate + 1)); // reproduction of fishes
        next_fishes -= (int)(trash * environmental_impact); // environmental impact on fishes
        next_fishes = Mathf.Clamp(next_fishes, 0, overpopulation_limit); // limit fishes number due to overpopulation/ too much trash
        return next_fishes;
    }

    int computeTrashForNextRound()
    {
        updateFishTrashPopulation();

        int trash = trashPopulation;
        return trash + trash_per_round;
    }

    void updateFishTrashPopulation()
    {

        int countFish = 0; int countTrash = 0;
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] != null) {
                if (children[i].tag == "Fish")
                {
                    countFish++;
                }
                else
                {
                    countTrash++;
                }
            }            
        }

        trashPopulation = countTrash;
        fishPopulation = countFish;
        Debug.Log("remaining fish = " + fishPopulation + " | remaining trash = " + trashPopulation);
    }

    void wipeSea() {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }


}
