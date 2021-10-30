using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    void Awake()
    {
        Destroy(this, GameManager.Instance.vfxDuration);
    }
}
