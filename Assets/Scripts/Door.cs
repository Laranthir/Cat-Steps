using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int doorNumber;
    [SerializeField] private bool levelPassed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !levelPassed)
        {
            Debug.Log("Player entered the trigger");
            other.gameObject.GetComponentInChildren<PlayerMovement>().StartLevelTransition(doorNumber);
            levelPassed = true;
            GameManager.Instance.checkpoint += 1;
        }
    }
}
