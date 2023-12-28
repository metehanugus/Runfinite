using UnityEngine;

public class PlusTime : MonoBehaviour
{
    [SerializeField] int moneyAmount = 10; // Eklenecek para
    public AudioSource crashSoundSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            // Ses çalma kontrolü
            if (crashSoundSource != null && crashSoundSource.clip != null)
            {
                crashSoundSource.Play();
            }
            else
            {
                Debug.LogError("CrashSoundSource or its clip is missing on PlusTime!");
            }

            // Diğer işlevler
            GameManager.inst.AddTime();
            GameManager.inst.AddMoney(moneyAmount);
            Destroy(gameObject);
        }
    }
}
