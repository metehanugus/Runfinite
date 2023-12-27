using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject minuscoinPrefab;
    [SerializeField] GameObject plustimePrefab;
    public GameObject[] buildingPrefabs;
    public float buildingOffset = 15f;
    public float roadWidth = 5f;

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnBuildingsAroundTile();

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

    public void SpawnBuildingsAroundTile()
    {
        float tileLength = GetComponent<BoxCollider>().size.z;
        float startPosZ = transform.position.z - tileLength / 2;
        float endPosZ = transform.position.z + tileLength / 2;

        for (int side = 0; side < 2; side++)
        {
            Vector3 positionOffset = (side == 0 ? Vector3.right : Vector3.left) * (roadWidth + buildingOffset);

            // Bu liste, zaten spawn edilmiş bina pozisyonlarını takip edecek
            List<float> usedPositions = new List<float>();

            for (float posZ = startPosZ; posZ < endPosZ;)
            {
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                float buildingLength = buildingPrefab.GetComponent<Renderer>().bounds.size.z;

                // Rastgele bir pozisyon seç, fakat önceden kullanılan pozisyonları kontrol et
                float spawnPosZ;
                do
                {
                    spawnPosZ = Random.Range(startPosZ, endPosZ);
                } while (usedPositions.Any(usedPos => Mathf.Abs(usedPos - spawnPosZ) < buildingLength));

                // Kullanılan pozisyon listesine bu pozisyonu ekle
                usedPositions.Add(spawnPosZ);

                // Spawn pozisyonunu ayarla
                Vector3 buildingPosition = new Vector3(positionOffset.x, 0f, spawnPosZ);

                // Bina objesini spawn et
                Instantiate(buildingPrefab, buildingPosition, Quaternion.identity, transform);

                // Sonraki spawn için posZ'yi güncelle
                posZ += buildingLength + buildingOffset;
            }
        }
    }




    // Belirli bir kenar için bina pozisyonu hesaplama
    private Vector3 GetPositionForBuilding(int side)
    {
        float offset = 5f; // Bina ile düzlem arasındaki mesafe
        Vector3 position = transform.position;

        switch (side)
        {
            case 0: // Üst kenar
                position += Vector3.forward * offset;
                break;
            case 1: // Sağ kenar
                position += Vector3.right * offset;
                break;
            case 2: // Alt kenar
                position -= Vector3.forward * offset;
                break;
            case 3: // Sol kenar
                position -= Vector3.right * offset;
                break;
        }

        return position;
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
        Vector3 point;
        do
        {
            point = new Vector3(
                Random.Range(-roadWidth / 2, roadWidth / 2), // Assume the road is centered at x = 0
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        } while (point != collider.ClosestPoint(point));

        point.y = 1; // Set the y position to 1 to lift the coins above the ground
        return point;
    }


}
