using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public string playerTag = "Player";
    private Transform player;
    Vector3 offset;
    Vector3 lastPlayerPosition;
    public float rotationSpeed = 5.0f; // Kamera rotasyon hýzýný ayarlamak için
    public float downwardAngle = 15.0f; // Kameranýn aþaðýya bakýþ açýsýný ayarlamak için

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        if (player == null)
        {
            Debug.LogError("Player objesi bulunamadý! Lütfen etiketinizi kontrol edin.");
            return;
        }

        offset = transform.position - player.position;
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
            Vector3 currentPlayerPosition = player.position;

            // Oyuncunun mevcut pozisyonunu güncelle, y eksenini sabit tut
            Vector3 targetPos = currentPlayerPosition + offset;
            targetPos.y = transform.position.y; // Kameranýn y eksenini sabit tut
            transform.position = targetPos;

            // Oyuncunun hareket yönünü hesapla, y eksenindeki deðiþimi yok say
            Vector3 movementDirection = new Vector3(currentPlayerPosition.x, 0, currentPlayerPosition.z) - new Vector3(lastPlayerPosition.x, 0, lastPlayerPosition.z);
            if (movementDirection != Vector3.zero)
            {
                // Kameranýn rotasyonunu oyuncunun hareket yönüne göre ayarla
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                Quaternion downwardRotation = Quaternion.Euler(downwardAngle, 0, 0); // Kamerayý aþaðýya eð
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation * downwardRotation, rotationSpeed * Time.deltaTime);
            }

            lastPlayerPosition = currentPlayerPosition; // Son oyuncu pozisyonunu güncelle
        }
    }
}
