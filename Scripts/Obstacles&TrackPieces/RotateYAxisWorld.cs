using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYAxisWorld : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }
}
