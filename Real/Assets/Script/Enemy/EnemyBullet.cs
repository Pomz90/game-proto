using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    public Transform player;
    public float shootingInterval = 2f;
    public float bulletSpeed = 30f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletLifetime = 3f; // Lifetime of each bullet

    private float lastShotTime;

    private void Start()
    {
        lastShotTime = Time.time;
    }

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time - lastShotTime >= shootingInterval)
        {
            ShootAtPlayer();
            lastShotTime = Time.time;
        }
    }

    private void ShootAtPlayer()
    {
        // Calculate the direction towards the player
        Vector2 direction = player.position - transform.position;

        // Normalize the direction vector
        direction.Normalize();

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate a bullet prefab at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Get the bullet's Rigidbody2D component and apply a force in the direction of the player
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed; // Adjust the speed as needed

        // Set the rotation of the bullet to face the direction
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Destroy the bullet after the specified lifetime
        Destroy(bullet, bulletLifetime);
    }


}
