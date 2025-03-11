using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudGeneratorScript : MonoBehaviour
{
    [SerializeField] GameObject clouds;
    [SerializeField] Sprite[] cloudImages;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;

    [SerializeField]
    GameObject startPoint;
    [SerializeField] int preSpawnCount =10;
    [SerializeField] bool doSacling = true;
    
    [SerializeField] float doMaxSacling = 0.15f;
    [SerializeField] float doMinSacling = 0.1f;

    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Prewarm();
        AttemptSpawn();
    }

    void SpawnCloud(Vector3 startPos)
    {
        if (startPoint.transform.childCount > preSpawnCount)
            return;

        int randomIndex = UnityEngine.Random.Range(0, cloudImages.Length);

        clouds.GetComponent<SpriteRenderer>().sprite = cloudImages[randomIndex];

        GameObject cloud = Instantiate(clouds, startPoint.transform);
        cloud.name = randomIndex.ToString();

        float startY = UnityEngine.Random.Range(startPos.y - 1f, startPos.y + 1f);

        cloud.transform.position = new Vector3(startPos.x, startY, startPos.z);
        
        if (doSacling)
        {
            float scale = UnityEngine.Random.Range(doMinSacling, doMaxSacling);
            cloud.transform.localScale = new Vector2(scale, scale);   
        }
        
        float speed = UnityEngine.Random.Range(0.05f, 0.1f);
        cloud.GetComponent<CloudScript>().StartFloating(speed, endPoint.transform.position.x);


    }

    void AttemptSpawn()
    {
        //check some things.
        SpawnCloud(startPos);

        Invoke("AttemptSpawn", spawnInterval);
    }

    void Prewarm()
    {
        for (int i = 0; i < preSpawnCount; i++)
        {
            Vector3 spawnPos = startPos + Vector3.right * (i * 3);
            SpawnCloud(spawnPos);
        }
    }
}
