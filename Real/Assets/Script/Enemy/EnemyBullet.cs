using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public Transform player;
    public float shootingInterval = 2f;
    public float CirInterval = 2f;
    public float bulletSpeed = 30f;
    public GameObject bulletPrefab;
    public GameObject AOEbulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletLifetime = 3f; // Lifetime of each bullet
    public int numberOfShots = 12;

    public int shotsFired;

    private float lastShotTime;
    private float lastLaser;
    public GameObject LaserPrefab;
    public Transform LaserPos;
    public int laserInterval = 10;
    private bool canShoot = true;
    private void Start()
    {
        lastShotTime = Time.time;
        lastLaser = Time.time;
    }

    void Update()
    {

        if (Time.time - lastShotTime >= shootingInterval)
        {
            if (shotsFired < 5)
            {
                ShootAtPlayer();
            }
            else
            {
                ShootInCircle();
                shotsFired = 0; // Reset the shot counter
            }

            lastShotTime = Time.time;
        }

        if (canShoot && Time.time - lastLaser >= laserInterval && Time.time >= 40f)
        {
            Laser();
            lastShotTime = Time.time;
            canShoot = false;
            StartCoroutine(WaitForNextShot());
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
        rb.velocity = direction * bulletSpeed * 1.5f; // Adjust the speed as needed

        // Set the rotation of the bullet to face the direction
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotsFired++;
        // Destroy the bullet after the specified lifetime
        Destroy(bullet, bulletLifetime);
    }

    private void ShootInCircle()
    {


        for (int i = 0; i < numberOfShots; i++)
        {
            // Calculate the angle for each shot in the circle
            float angle = i * (360f / numberOfShots);

            // Convert the angle to radians
            float radianAngle = Mathf.Deg2Rad * angle;

            // Calculate the direction of the bullet
            Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            // Spawn the bullet
            GameObject bullet = Instantiate(AOEbulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            // Set the bullet's direction and speed
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            float bulletRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(bulletRotation, Vector3.forward);

            Destroy(bullet, bulletLifetime);
        }

        
   }

    private void Laser()
    {

        GameObject Laser = Instantiate(LaserPrefab, LaserPos.position, Quaternion.identity);



    }

    private IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(40f);
        canShoot = true;
    }

}
