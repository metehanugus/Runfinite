using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public string playerTag = "Player";
    private Transform player;
    Vector3 offset;
    Vector3 lastPlayerPosition;
    public float rotationSpeed = 5.0f; // Kamera rotasyon h�z�n� ayarlamak i�in
    public float downwardAngle = 15.0f; // Kameran�n a�a��ya bak�� a��s�n� ayarlamak i�in

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        if (player == null)
        {
            Debug.LogError("Player objesi bulunamad�! L�tfen etiketinizi kontrol edin.");
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

            // Oyuncunun mevcut pozisyonunu g�ncelle, y eksenini sabit tut
            Vector3 targetPos = currentPlayerPosition + offset;
            targetPos.y = transform.position.y; // Kameran�n y eksenini sabit tut
            transform.position = targetPos;

            // Oyuncunun hareket y�n�n� hesapla, y eksenindeki de�i�imi yok say
            Vector3 movementDirection = new Vector3(currentPlayerPosition.x, 0, currentPlayerPosition.z) - new Vector3(lastPlayerPosition.x, 0, lastPlayerPosition.z);
            if (movementDirection != Vector3.zero)
            {
                // Kameran�n rotasyonunu oyuncunun hareket y�n�ne g�re ayarla
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                Quaternion downwardRotation = Quaternion.Euler(downwardAngle, 0, 0); // Kameray� a�a��ya e�
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation * downwardRotation, rotationSpeed * Time.deltaTime);
            }

            lastPlayerPosition = currentPlayerPosition; // Son oyuncu pozisyonunu g�ncelle
        }
    }
}
