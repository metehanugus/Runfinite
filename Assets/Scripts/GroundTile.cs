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
        gameManager = GameManager.inst;
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


    public void SpawnObstacle(int tilesSpawned)
    {
        int spawnRate;  // Her kaç tile'da bir spawn olacak

        // Tile sayısına göre spawn oranını belirle
        if (tilesSpawned <= 20)
        {
            spawnRate = 5;
        }
        else if (tilesSpawned <= 30)
        {
            spawnRate = 4;
        }
        else if (tilesSpawned <= 40)
        {
            spawnRate = 3;
        }
        else
        {
            spawnRate = 2;  // 40. tile'dan sonra her 2 tile'da bir
        }

        // Belirli tile'larda engel spawn et
        if (tilesSpawned % spawnRate == 0 && gameManager != null)
        {
            int playerScore = gameManager.score;

            // Puan 10 arttıkça bir engel ekle, maksimum 2'ye kadar
            int obstaclesToSpawn = Mathf.Clamp(playerScore / 10, 1, 2);

            HashSet<int> usedSpawnIndexes = new HashSet<int>();

            for (int i = 0; i < obstaclesToSpawn; i++)
            {
                int obstacleSpawnIndex;
                do
                {
                    obstacleSpawnIndex = Random.Range(2, 5);
                } while (usedSpawnIndexes.Contains(obstacleSpawnIndex));

                usedSpawnIndexes.Add(obstacleSpawnIndex);
                Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
                Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
            }
        }
    }




    public void SpawnTreesAroundTile()
    {
        int maxAttempts = 20;
        int minTreesToSpawn = 10;
        HashSet<Vector2> usedPositions = new HashSet<Vector2>();

        for (int i = 0; i < minTreesToSpawn; i++)
        {
            GameObject treePrefab = GetRandomTreePrefab();
            if (treePrefab != null)
            {
                int attempts = 0;
                Vector2 spawnPos;
                do
                {
                    // Her iki taraf için rastgele seçim yap ve yolu aşacak şekilde pozisyon belirle
                    float side = Random.Range(0, 2) * 2 - 1; // -1 veya 1 döner
                    float spawnPosX = side * (roadWidth / 2 + Random.Range(buildingOffset, buildingOffset + treeOffset));
                    float spawnPosZ = Random.Range(-treeOffset, treeOffset) + transform.position.z;
                    spawnPos = new Vector2(spawnPosX, spawnPosZ);

                    if (++attempts > maxAttempts)
                    {
                        Debug.LogWarning("Max attempts reached, skipping tree spawn.");
                        break;
                    }
                } while (usedPositions.Contains(spawnPos));

                usedPositions.Add(spawnPos);

                Vector3 treePosition = new Vector3(spawnPos.x + transform.position.x, 0f, spawnPos.y);
                Quaternion treeRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(treePrefab, treePosition, treeRotation, transform);
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
            int maxAttempts = 20;
            int attempts = 0;
            while (posZ < endPosZ)
            {
                if (++attempts > maxAttempts)
                {
                    Debug.LogWarning("Max attempts reached, skipping building spawn.");
                    break; // Maksimum deneme sayısına ulaşıldıysa döngüden çık
                }
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
