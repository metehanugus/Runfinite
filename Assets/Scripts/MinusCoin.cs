using UnityEngine;

public class MinusCoin : MonoBehaviour
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

        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Ses çalma kontrolü
        if (crashSoundSource != null && crashSoundSource.clip != null)
        {
            crashSoundSource.Play();
        }
        else
        {
            Debug.LogError("CrashSoundSource or its clip is missing on MinusCoin!");
        }

        // Diğer işlevler
        GameManager.inst.IncrementFireCount();
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
