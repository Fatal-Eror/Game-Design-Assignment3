using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LavaScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.transform.GetComponent<PlayerCharacter>();

        if (player != null)
        {
            player.Respawn();
        }
    }
}
