using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile(bool spawnItems)
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        // Burada, sonraki spawn noktasını ayarlıyoruz. 
        // Eğer GroundTile prefab'ınızın çocukları arasında bir referans noktası varsa (örneğin, bir sonraki tile'ın başlangıç noktası),
        // bu referans noktasının indeksini kullanın.
        nextSpawnPoint = temp.transform.GetChild(0).transform.position; // Bu indeks tile prefabınıza bağlı olarak değişebilir.

        if (spawnItems)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle();
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

