using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    GameObject spawner;
    public GameObject door;
    GameObject doorInfo;
    public TMP_Text infoUI;

    public List<GameObject> enemiesThatCanSpawn;
    
    public List<GameObject> enemiesSpawned;

    public int amountAtStart;
    public float range;

    public float amountOfTimeBetweenSpawn;

    public int amountToKill;

    private void Start()
    {
        enemiesSpawned = new List<GameObject>();
        spawner = transform.GetChild(0).gameObject;
        doorInfo = transform.GetChild(1).gameObject;

        doorInfo.transform.position = door.transform.position;

        for(int i = 0; i < amountAtStart; i++)
        {
            SpawnEnemies();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InvokeRepeating("SpawnEnemies", 0f, amountOfTimeBetweenSpawn);
            spawner.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            infoUI.gameObject.SetActive(true);
            if (CheckIfHasMissingGO(enemiesSpawned))
                infoUI.text = (amountToKill- HowManyMissingGO(enemiesSpawned)).ToString() + " enemies left to kill";
            else
                infoUI.text = (amountToKill).ToString() + " enemies left to kill";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            infoUI.gameObject.SetActive(false);
        }
    }

    void SpawnEnemies()
    {
        enemiesSpawned.Add(Instantiate(enemiesThatCanSpawn[Random.Range(0, enemiesThatCanSpawn.Count)], new Vector3(Random.Range(spawner.transform.position.x - range, spawner.transform.position.x + range), spawner.transform.position.y, Random.Range(spawner.transform.position.z - range, spawner.transform.position.z + range)), Quaternion.identity));
    }

    private void Update()
    {

        if (enemiesSpawned.Count >= amountToKill)
        {
            CancelInvoke("SpawnEnemies");
        }
        if (CheckIfHasMissingGO(enemiesSpawned))
        {
            if (HowManyMissingGO(enemiesSpawned) >= amountToKill)
            {
                door.SetActive(false);
            }
        }
    }

    bool CheckIfHasMissingGO(List<GameObject> gameObjects)
    {
        foreach (GameObject go in gameObjects)
        {
            if(go == null)
            {
                return true;
            }
        }
        return false;
    }

    int HowManyMissingGO(List<GameObject> gameObjects)
    {
        int howMany=0;
        foreach (GameObject go in gameObjects)
        {
            if (go == null)
            {
                howMany++;
            }
        }
        return howMany;
    }
}
