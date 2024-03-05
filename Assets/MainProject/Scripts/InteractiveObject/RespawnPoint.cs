using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static RespawnPoint respawnLocation { get; private set; }

    private void Awake()
    {
        if (respawnLocation == null)
        {
            respawnLocation = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
