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

    [SerializeField] GameObject[] treePrefabs; // Ağaç prefabları
    public float treeOffset = 5f; // Ağaçlar için yolun kenarından ne kadar uzakta spawn olacaklar

    private GameManager gameManager;  // GameManager referansı

    private void Awake()
    {
        gameManager = GameManager.inst;  // GameManager'ın örneğini al
    }

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();

        // Rastgele şansla bina spawn etme veya kesinlikle ağaç spawn etme
        if (Random.value > 0.8f) // %50 şans
        {
            SpawnBuildingsAroundTile();
        }
        else // Bina spawn edilmiyorsa, kesinlikle ağaçları spawn et
        {
            SpawnTreesAroundTile();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject, 2);

    }


    public void SpawnObstacle()
    {
        if (gameManager != null)
        {
            // Oyuncunun puanını GameManager'dan al
            int playerScore = gameManager.score;

            // Puan 10 arttıkça bir engel ekle, maksimum 5'e kadar
            int obstaclesToSpawn = Mathf.Clamp(playerScore / 10, 1, 5);

            for (int i = 0; i < obstaclesToSpawn; i++)
            {
                int obstacleSpawnIndex = Random.Range(2, 5);
                Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
                Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
            }
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
        }
    }

    public void SpawnTreesAroundTile()
    {
        int minTreesToSpawn = 10; // Bu tile için en az 10 ağaç spawn edilecek
        HashSet<Vector2> usedPositions = new HashSet<Vector2>(); // Kullanılan pozisyonları takip et (X ve Z)

        for (int i = 0; i < minTreesToSpawn; i++)
        {
            GameObject treePrefab = GetRandomTreePrefab();
            if (treePrefab != null)
            {
                float side = Random.Range(-1f, 1f); // Her iki taraf için rastgele seçim
                Vector2 spawnPos;
                do
                {
                    // Rastgele bir X ve Z pozisyonu seç, fakat önceden kullanılan pozisyonları kontrol et
                    float spawnPosX = side * (roadWidth + Random.Range(1f, treeOffset));
                    float spawnPosZ = Random.Range(-treeOffset, treeOffset) + transform.position.z;
                    spawnPos = new Vector2(spawnPosX, spawnPosZ);
                } while (usedPositions.Contains(spawnPos) || Mathf.Abs(spawnPos.x) < roadWidth / 2);

                // Kullanılan pozisyon listesine bu pozisyonu ekle
                usedPositions.Add(spawnPos);

                Vector3 treePosition = new Vector3(spawnPos.x + transform.position.x, 0f, spawnPos.y);
                Instantiate(treePrefab, treePosition, Quaternion.identity, transform);
            }
        }
    }


    private GameObject GetRandomTreePrefab()
    {
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("No tree prefabs are assigned!");
            return null;
        }
        return treePrefabs[Random.Range(0, treePrefabs.Length)];
    }

    public void SpawnBuildingsAroundTile()
    {
        float tileLength = GetComponent<BoxCollider>().size.z;
        float startPosZ = transform.position.z - tileLength / 2;
        float endPosZ = transform.position.z + tileLength / 2;

        HashSet<float> usedPositions = new HashSet<float>();

        for (int side = 0; side < 2; side++)
        {
            Vector3 positionOffset = (side == 0 ? Vector3.right : Vector3.left) * (roadWidth + buildingOffset);

            float posZ = startPosZ;
            while (posZ < endPosZ)
            {
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                float buildingLength = buildingPrefab.GetComponent<Renderer>().bounds.size.z;

                // Generate a random position that hasn't been used
                float spawnPosZ = Random.Range(posZ, endPosZ);
                if (!usedPositions.Contains(spawnPosZ))
                {
                    Vector3 buildingPosition = new Vector3(positionOffset.x, 0f, spawnPosZ);
                    Instantiate(buildingPrefab, buildingPosition, Quaternion.identity, transform);
                    usedPositions.Add(spawnPosZ);
                    posZ = spawnPosZ + buildingLength + buildingOffset; // Move past the current building
                }
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
        // Rastgele sayıda objeler spawn etmek için sayıları belirle
        int coinsToSpawn = Random.Range(0, 6); // 0-5 arası coin
        int minusCoinsToSpawn = Random.Range(0, 4); // 0-3 arası minuscoin
        int plusCoinsToSpawn = Random.Range(0, 3); // 0-2 arası plus coin

        GameObject temp;

        // Coin spawn etme
        for (int i = 0; i < coinsToSpawn; i++)
        {
            temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }

        // MinusCoin spawn etme
        for (int i = 0; i < minusCoinsToSpawn; i++)
        {
            temp = Instantiate(minuscoinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }

        // PlusCoin spawn etme
        for (int i = 0; i < plusCoinsToSpawn; i++)
        {
            temp = Instantiate(plustimePrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
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
