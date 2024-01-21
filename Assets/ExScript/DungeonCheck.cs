using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCheck : MonoBehaviour
{
    public int thisNum;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Player>(out Player player))
        {
            player.dungeonNum = thisNum;
        }
    }
}
