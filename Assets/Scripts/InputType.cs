using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputType : MonoBehaviour
{
    void OnTouchDown()
    {
        Debug.Log("Down");
    }
    void OnTouchStay()
    {
        Debug.Log("Stay");
    }
    void OnTouchUp()
    {
        Debug.Log("Up");
    }
    void OnTouchExit()
    {
        Debug.Log("Exit");
    }
}
