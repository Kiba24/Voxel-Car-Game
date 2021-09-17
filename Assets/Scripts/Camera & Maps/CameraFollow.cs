using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private Transform Target; // Obect to follow
  [SerializeField] private Vector3 offset;

    private void Update() 
    {
        transform.position = Target.position + offset;
    }
}
