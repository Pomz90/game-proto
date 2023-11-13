using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserLength;
    private LineRenderer line;
    public LayerMask _border;
    private PlayerLife life;
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 endPosition = (Vector3.down * laserLength) + transform.position;

        RaycastHit2D[] hit = new RaycastHit2D[1];

        line.SetPositions(new Vector3[] { transform.position, endPosition }); // Adjust the length as needed
    }


}
    


