using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public string playerTag = "Player"; // Ekledik: Takip edilecek objenin etiketi
    private Transform player;
    Vector3 offset;

    private void Start()
    {
        // Oyun baþladýðýnda, etiketi "Player" olan objeyi bul ve onun transform'unu kullan
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        if (player == null)
        {
            Debug.LogError("Player objesi bulunamadý! Lütfen etiketinizi kontrol edin.");
            return;
        }

        offset = transform.position - player.position;
    }

    private void Update()
    {
        // Player null deðilse takip et
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
            Vector3 targetPos = player.position + offset;
            targetPos.x = 0;
            transform.position = targetPos;
        }
    }
}
