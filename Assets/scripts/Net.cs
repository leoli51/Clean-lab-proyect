using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Net : MonoBehaviour
{
    public Transform red_boat;
    public Transform blue_boat;

    public float slow_down_distance;
    public float max_speed;

    public float max_stretch;
    public float changeLineWidthFrom = 0.8f;

    private Vector3 vertexPosition;
    public bool ripped = false;
      
    public float strength = 10f;
    public int damage = 0;

    public int fishCount;
    public int trashCount;

    AudioManager audioManager;
    public GameObject repairText;

    LineRenderer lineRenderer;
    MeshCollider meshCollider;
    Mesh mesh;

    float initialWidthFactor;


    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();

        vertexPosition = red_boat.position + ((blue_boat.position - red_boat.position) / 2);
        initialWidthFactor = lineRenderer.widthMultiplier;
    }


    void Update()
    {
        if (!ripped)
        {
            if (damage >= strength) Rip();

            float boat_distance = Vector3.Distance(blue_boat.position, red_boat.position);
            if (boat_distance > max_stretch) Rip();

            // change line width when boat distances is high
            float stretch = boat_distance / max_stretch;
            if (stretch > changeLineWidthFrom)
            {
                float widthFactor = Mathf.Clamp(((stretch - 1) / (1-changeLineWidthFrom)) * -10, 0.01f, 10);
                lineRenderer.widthMultiplier = widthFactor;

                // change color based on stretch
                Color lineColor = new Color(1, 1-stretch, 1-stretch, 1);
                lineRenderer.startColor = lineColor;
                lineRenderer.endColor = lineColor;
            }
            else
            {
                // change color based on damage
                float colorFactor = damage / (float)strength * changeLineWidthFrom;
                Color lineColor = new Color(1, 1 - colorFactor, 1 - colorFactor, 1);
                lineRenderer.startColor = lineColor;
                lineRenderer.endColor = lineColor;
            }
            

            // calculate position of central curve vertex
            Vector3 boatsMidPoint = red_boat.position + ((blue_boat.position - red_boat.position) / 2);
            Vector3 direction = Vector3.Normalize(boatsMidPoint - vertexPosition);
            float speed = Mathf.Lerp(0, max_speed, Mathf.Clamp01(Vector3.Distance(vertexPosition, boatsMidPoint) / slow_down_distance));
            vertexPosition += direction * Time.deltaTime * speed;

            // draw line
            DrawQuadraticBezierCurve(red_boat.position, vertexPosition, blue_boat.position);

            // update collider
            lineRenderer.BakeMesh(mesh, true);
            meshCollider.sharedMesh = mesh;
        }
    }

    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        lineRenderer.positionCount = 40;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[ NET ] Collision with " + collision.gameObject.name);

        if (collision.gameObject.tag == "Island")
        { 
            Debug.Log("[ NET ] Crashed into Island");
            Rip();
        }
        else if (collision.gameObject.tag == "Fish")
        {
            Debug.Log("[ NET ] Collected Fish");
            CollectFish(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Trash")
        {
            Debug.Log("[ NET ] Collected Trash");
            CollectTrash(collision.gameObject);
        }
    }

    void CollectFish(GameObject fish)
    {
        fish.SetActive(false);
        Destroy(fish);
        fishCount++;
        audioManager.Play(AudioManager.SoundName.CollectFish);
    }

    void CollectTrash(GameObject trash)
    {
        trash.SetActive(false);
        Destroy(trash);
        trashCount++;
        damage++;
        audioManager.Play(AudioManager.SoundName.CollectTrash);
    }

    public void Rip()
    {
        ripped = true;
        mesh.Clear();
        fishCount = 0;
        trashCount = 0;
        meshCollider.sharedMesh = null;
        meshCollider.enabled = false;
        lineRenderer.enabled = false;
        audioManager.Play(AudioManager.SoundName.NetRip);
        repairText.SetActive(true);
    }

    public void Restore()
    {
        damage = 0;
        ripped = false;
        fishCount = 0;
        trashCount = 0;
        meshCollider.enabled = true;
        lineRenderer.enabled = true;
        repairText.SetActive(false);
        lineRenderer.widthMultiplier = initialWidthFactor;
        vertexPosition = red_boat.position + ((blue_boat.position - red_boat.position) / 2);
    }

}
