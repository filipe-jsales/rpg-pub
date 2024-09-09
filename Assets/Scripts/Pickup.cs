using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private int pointPerCoin = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindFirstObjectByType<GameManager>().AddToScore(pointPerCoin);
            AudioManager.instance.PlayAtPoint("Coin Pickup");
            Destroy(gameObject);
        }
    }
}
