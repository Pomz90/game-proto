using UnityEngine;

public class EnemyMovement12 : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public Transform leftBox;
    public Transform rightBox;

    private bool moveRight = true;

    private void Update()
    {
        // Calculate movement direction based on the current direction
        float horizontalInput = moveRight ? 1.0f : -1.0f;

        // Calculate the new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

        // Move the object
        transform.position = newPosition;

        if (leftBox != null && rightBox != null)
        {
            if (transform.position.x <= leftBox.position.x)
            {
                Destroy(gameObject);
            }
            else if (transform.position.x >= rightBox.position.x)
            {
                Destroy(gameObject);
            }
        }


    }
}
