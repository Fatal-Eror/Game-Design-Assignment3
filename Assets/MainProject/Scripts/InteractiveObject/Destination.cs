using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Destination : MonoBehaviour
{
    [SerializeField] private string playerTag;    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(playerTag))
        LevelState_Level1.GameEnd();
    }
}
