using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public int tilesSpawned = 0;  // Spawn edilen tile sayısı

    public void SpawnTile(bool spawnItems)
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(0).transform.position;
        tilesSpawned++;  // Her tile spawn edildiğinde sayaç artar

        if (spawnItems)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle(tilesSpawned);  // Tile sayısını parametre olarak geçir
            temp.GetComponent<GroundTile>().SpawnCoins();
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

