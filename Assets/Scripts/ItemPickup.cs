using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickup(collision.gameObject);
        }
    }

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().AddRadius();
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<PlayerController>().AddSpeed();
                break;
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
        }
        Destroy(gameObject);
    }
}
public enum ItemType
{
    BlastRadius,
    ExtraBomb,
    SpeedIncrease
}