using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject minuscoinPrefab;
    [SerializeField] GameObject plustimePrefab;

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();

    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject, 2);

    }


    public void SpawnObstacle()
    {
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity,transform);

    }

   

    public void SpawnCoins()
    {
        int coinsToSpawn = 10;
        GameObject temp;
        for (int i = 0; i < (coinsToSpawn/2)+3; i++)
        {
            temp = Instantiate(coinPrefab,transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());

        }
        for (int i = (coinsToSpawn / 2) + 3; i < coinsToSpawn; i++)
        {
            temp = Instantiate(minuscoinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
        temp = Instantiate(plustimePrefab, transform);
        temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

        if ( point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }
        point.y = 1;
        return point;

    }


}
