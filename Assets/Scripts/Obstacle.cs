using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Obstacle : MonoBehaviour
{
    public int revertToPreviousCheckpoint;
    
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    private GameManager gameManager;
    private GameObject explosionVFX;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            //Instantiate toon explosion
            explosionVFX = Instantiate(gameManager.explosionVFX, transform.position, new Quaternion(0,1,0,0));
            StartCoroutine(DisplayExplosion());
            
            //Camera follows the explosion
            

            //Return player back to LastCheckpoint
            StartCoroutine(MoveToCheckPoint());

            //Restore Obstacles back to their original positions.
            gameManager.ResetAllObstaclePositions();

            Debug.Log("Obstacle hit the floor!");
        }
    }

    private IEnumerator DisplayExplosion()
    {
        gameManager.vcam.Follow = explosionVFX.transform;
        gameManager.vcam.LookAt = explosionVFX.transform;
        yield return new WaitForSeconds(gameManager.WaitBeforeRestart);
        gameManager.vcam.Follow = gameManager.player.GetChild(0).GetChild(0);
        gameManager.vcam.LookAt = gameManager.player.GetChild(0).GetChild(0);
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private IEnumerator MoveToCheckPoint()
    {
        gameManager.player.GetComponentInChildren<PlayerMovement>().movementEnabled = false;
        gameManager.player.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        gameManager.player.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(gameManager.WaitBeforeRestart);
        UpdateCheckpoint(); //Incase an obstacle falls during transition.
        

        if (gameManager.checkpoint == 1)
        {
            gameManager.player.position = gameManager.firstSpawnPoint.position;
            ResetLocalPosition();
            GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().movementEnabled = true;
        }
        else if (gameManager.checkpoint == 2)
        {
            gameManager.player.position = gameManager.secondSpawnPoint.position;
            ResetLocalPosition();
            GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().movementEnabled = true;
        }
        else if (gameManager.checkpoint == 3)
        {
            gameManager.player.position = gameManager.thirdSpawnPoint.position;
            ResetLocalPosition();
            GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().movementEnabled = true;
        }
    }

    private void ResetLocalPosition()
    {
        gameManager.player.GetChild(0).transform.localPosition = new Vector3(0, 0.5f, 0);
        gameManager.player.GetChild(0).transform.localRotation = new Quaternion(0, 0, 0,1);
        gameManager.player.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameManager.player.GetChild(0).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    
    public void UpdateCheckpoint()
    {
        gameManager.CheckpointUpdate(revertToPreviousCheckpoint);
    }
}
