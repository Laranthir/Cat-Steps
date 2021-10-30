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
            other.gameObject.GetComponentInParent<PlayerMovement>().StartLevelTransition(doorNumber);
            StartCoroutine(CheckIfLevelPassed());
        }
    }
    
    public IEnumerator CheckIfLevelPassed()
    {
        yield return new WaitForSeconds(GameManager.Instance.WaitBeforeRestart);
        
        if (GameManager.Instance.checkpoint > doorNumber)
        {
            levelPassed = true;
        }
    }
}
