using UnityEngine;

public class EnemyMovement12 : MonoBehaviour
{
	public float moveSpeed = 2.0f;
	public Transform topBox; // Define a top box for vertical movement
	public Transform bottomBox; // Define a bottom box for vertical movement

	private bool moveUp = true;

	private void Update()
	{
		// Calculate movement direction based on the current direction
		float verticalInput = moveUp ? 1.0f : -1.0f;

		// Calculate the new position
		Vector3 newPosition = transform.position + new Vector3(0, verticalInput * moveSpeed * Time.deltaTime, 0);

		// Move the object
		transform.position = newPosition;

		// Check if the object is within the topBox or bottomBox trigger zones
		if (topBox != null && bottomBox != null)
		{
			if (transform.position.y >= topBox.position.y)
			{
				moveUp = false;
			}
			else if (transform.position.y <= bottomBox.position.y)
			{
				moveUp = true;
			}
		}
	}
}
