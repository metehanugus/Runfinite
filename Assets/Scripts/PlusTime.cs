using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusTime : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] int moneyAmount = 10; // Eklenecek para

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.inst.AddTime();
            GameManager.inst.AddMoney(moneyAmount); // Oyuncuya para ekliyor

            Destroy(gameObject);
        }
    }

}