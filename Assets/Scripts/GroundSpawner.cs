using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile(bool spawnItems)
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

        // Always spawn buildings regardless of spawnItems
        

        if (spawnItems)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle();
            temp.GetComponent<GroundTile>().SpawnCoins();
            temp.GetComponent<GroundTile>().SpawnBuildingsAroundTile();
        }
    }

    private void Start()
    {

        for (int i = 0; i < 15; i++)
        {
            if(i<1)
            {SpawnTile(false);}
            else
            {SpawnTile(true);}
        }
    }
}

