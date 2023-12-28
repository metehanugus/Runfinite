using UnityEngine;

public class PlusCoin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
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
                Debug.LogError("CrashSoundSource or its clip is missing on PlusCoin!");
            }

            // Diğer işlevler
            GameManager.inst.IncrementScore();
            GameManager.inst.IncrementIceCount();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
