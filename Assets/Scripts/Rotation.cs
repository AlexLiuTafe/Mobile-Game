using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
    public MasterController masterController;
    public float rotateSpeed = 5f;
  
    void Update()
    {
        Rotate();
    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, masterController.Horizontal), rotateSpeed);
    }
}
