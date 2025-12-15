using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarBossEvent : MonoBehaviour
{   
    public BossEvent bossEvent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sonic"))
        {
            bossEvent.IniciarBossEvent();
        }
    }
    
}
