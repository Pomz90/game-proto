using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public Transform leftBox;
    public Transform rightBox;
    public Transform player;

    private bool moveRight = true;

    private void Update()
    {
        // Calculate movement direction based on the current direction
        float horizontalInput = moveRight ? 1.0f : -1.0f;

        // Calculate the new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

        // Move the object
        transform.position = newPosition;

        // Rotate the object to face the player's position
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Check if the object is within the leftBox or rightBox trigger zones
        if (leftBox != null && rightBox != null)
        {
            if (transform.position.x <= leftBox.position.x)
            {
                moveRight = true;
            }
            else if (transform.position.x >= rightBox.position.x)
            {
                moveRight = false;
            }
        }
    }
}

