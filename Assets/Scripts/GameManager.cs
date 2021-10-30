using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int checkpoint = 1; //Store the checkpoint number
    
    [Header("Movement Variables")]
    
    [SerializeField] public float movementMultiplier = 10f;
    [SerializeField] public float airMovementMultiplier = 0.8f;
    [SerializeField] public float jumpForce = 20f; // When force is 20f, Jump Height is calculated as 7.5f~
    [SerializeField] public float fallMultiplier = 2.5f;
    [SerializeField] public float lowJumpMultiplier = 2f;
    [SerializeField] public float jumpDuration = 1f;
    [SerializeField] public float minWallJumpAngle = 0.5f;
    [SerializeField] public float wallJumpHorizontalForce;
    [SerializeField] public float wallJumpVerticalForce;
    
    [Header("Cinematics")]
    
    [SerializeField] public float levelTransitionDuration = 5f;
    [SerializeField] public float WaitBeforeRestart = 2f;
    [SerializeField] public float vfxDuration = 2f;

    [Header("Drag Values")]
    
    [SerializeField] public float horizontalDrag = 0.1f;
    [SerializeField] public float verticalDrag = 0.0075f;

    [Header("Ground Detection")]

    [SerializeField] public float groundedCheckDistance = 0.3f;
    [SerializeField] public LayerMask environmentMask;
    [SerializeField] public LayerMask floorMask;

    [Header("Spawn Points")] 
    
    [SerializeField] public Transform firstSpawnPoint;
    [SerializeField] public Transform secondSpawnPoint;
    [SerializeField] public Transform thirdSpawnPoint;

    [Header("Drag and Drop Objects")] 
    
    [SerializeField] public Transform player;
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] public CinemachineVirtualCamera vcam;
    
    public void ResetAllObstaclePositions()
    {
        Obstacle[] allObstacles = (Obstacle[]) FindObjectsOfType(typeof(Obstacle));

        foreach (var obstacle in allObstacles)
        {
            obstacle.ResetPosition();
        }
    }
    
    public void CheckpointUpdate(int roomNumber)
    {
        checkpoint = roomNumber;
    }
}
