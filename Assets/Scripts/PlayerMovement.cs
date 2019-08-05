using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;

    public MasterController moveJoystick;
    Rigidbody2D rigid;
    

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        rigid.velocity = ( new Vector2(moveJoystick.Horizontal,moveJoystick.Vertical)* movementSpeed * Time.deltaTime);
        
    }

}
