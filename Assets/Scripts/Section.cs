using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{

    private static int lastRandomIndex = -1;

    public List<GameObject> obstacles;

    public float speed;

    private int sectionsCount = 0;

    public int sectionSize = 44;

    void Start()
    {

        sectionsCount = GameObject.FindGameObjectsWithTag("Section").Length;

        obstacles = new List<GameObject>();

        foreach (Transform child in transform)
        {

            if (child.tag == "Obstacle")
            {

                obstacles.Add(child.gameObject);

            }

        }

        EnableRandomObstacle();

    }

    public void EnableRandomObstacle()
    {

        foreach (GameObject obstacle in obstacles)
        {

            obstacle.SetActive(false);
            
        }

        int randomIndex = lastRandomIndex;

        while (randomIndex == lastRandomIndex)
        {

            randomIndex = Random.Range(0, obstacles.Count);

        }

        lastRandomIndex = randomIndex;

        obstacles[5].SetActive(true);

    }

    void Update()
    {

        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z <= -sectionSize)
        {

            transform.Translate(Vector3.forward * sectionSize * sectionsCount);

            EnableRandomObstacle();

        }

    }
}
