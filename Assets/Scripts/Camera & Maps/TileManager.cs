using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    [SerializeField] public GameObject[] tilePrefabs;
    
    private Transform PlayerTransform;
    private float spawnZ = 0;
    private float tileLenght = 310.0f;
    private float safeZone = 320.0f;
    private int amnTilesOnScreen = 3;
    private int lastPrefabIndex = 0;

    private List<GameObject> activateTiles;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        activateTiles = new List<GameObject>();
        
        for (int i = 0 ; i < amnTilesOnScreen ; i++)
        {
            if (i<1)
            {
                SpawnTile(0);
            } 
            else 
            {
                SpawnTile();
            }
           
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLenght)){
            SpawnTile();
            DeleteTile();
        }
    }

    private void SpawnTile(int prefabIndex = -1)
        {
            GameObject go;
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
            go.transform.position= Vector3.forward * spawnZ;
            spawnZ += tileLenght;
            activateTiles.Add(go);
        }

    private void DeleteTile()
    {

        Destroy (activateTiles[0]);
        activateTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (tilePrefabs.Length <=1) {
            return 0;
        }

        int randomIndex = lastPrefabIndex;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0,tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

}
