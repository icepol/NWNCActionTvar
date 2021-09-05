using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        
        if (player == null)
            return;

        player.IsCloseToLadder = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        
        if (player == null)
            return;

        player.IsCloseToLadder = false;
        player.IsOnLadder = false;
    }
}
