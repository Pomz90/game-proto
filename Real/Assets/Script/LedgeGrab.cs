using System.Collections;
using UnityEditor;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{

    [SerializeField]
    private Transform grabPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "ledgeGrab")
        {
            Movement move = other.transform.GetComponentInParent<Movement>();
            if (move != null)
            {
                if(grabPos != null)
                {
                    move.Grab(grabPos.position,move.side);
                }
            }
        }
    }




}
