using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class projectship : MonoBehaviour
{
    public float speed; // Movement speed
    public float slowMult; // Slowdown multiplier
    public float grav; // Gravity scale for height control
    private Rigidbody2D _rb; // Rigidbody component
    private bool _checker = false; // Check if gravity should be applied
    private bool _canMove = true; // Whether the ship can move
    private float _collisionTime; // Timer for collision delay
    private float _collisionDelay = 0.5f; // Time delay after collision before enabling movement
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab for shooting
    [SerializeField] private Transform firingPoint; // The point from where the bullet will be fired
    public GameOverScreen gameOverScreen; // Reference to the game over screen
    public mpu6050 mpu6050; // Reference to the MPU6050 sensor for movement data
    private Vector3 sensorData; // Sensor data from MPU6050
    private Vector2 _inputDirection; // Direction of movement based on sensor data
    private bool _isMoving; // Whether the ship is moving

    [SerializeField] public Transform waterParticlesTransform; // Reference to water particle effects (optional)

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // Find the MPU6050 sensor reference
        }
    }

    private void Update()
    {
        HandleInput(); // Handle input from the MPU6050 sensor

        // Height check and gravity control
        if (gameObject.transform.position.y > 40)
        {
            _rb.gravityScale = grav; // Apply gravity if above the specified height
            _checker = true; // Set gravity check flag
        }
        else
        {
            _rb.gravityScale = 0; // Disable gravity below the threshold height
            _checker = false;
        }
    }

    private void HandleInput()
    {
        sensorData = mpu6050.sensorData; // Get sensor data from MPU6050
        _inputDirection = new Vector2(sensorData.y, sensorData.x); // Set the input direction based on the sensor data

        // Update movement state
        _isMoving = _inputDirection != Vector2.zero; // Check if there is movement

        // Apply movement and rotation if there's input
        if (_isMoving)
        {
            ChangePosition(_inputDirection); // Move the ship if there is input
        }
        else
        {
            _rb.velocity *= slowMult; // Slow down the ship if no movement
            if (Mathf.Approximately(_rb.velocity.magnitude, 0))
            {
                _rb.velocity = Vector2.zero; // Stop the ship if there is no velocity
            }
        }
    }

    private void ChangePosition(Vector2 direction)
    {
        if (!_canMove || GetComponent<MiniHealth>().currentHealth == 0 || !gameOverScreen.isPlayable)
            return;

        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0).normalized; // Normalize the movement direction
        float angleZ = 0; // Initial angle for rotation
        float angleY = 0; // Initial Y-axis rotation

        // Handle different movement directions and adjust rotation
        if (moveDirection.x < 0 && Mathf.Approximately(moveDirection.y, 0))
        {
            angleZ = 180;
            angleY = 0;
        }
        else if (moveDirection.x < 0 && moveDirection.y > 0)
        {
            angleZ = Mathf.Clamp(Mathf.Atan2(moveDirection.y, -moveDirection.x) * Mathf.Rad2Deg, 0, 88);
            angleY = 180;
        }
        else if (moveDirection.x < 0 && moveDirection.y < 0)
        {
            angleZ = Mathf.Clamp(Mathf.Atan2(moveDirection.y, -moveDirection.x) * Mathf.Rad2Deg, -88, 0);
            angleY = 180;
        }
        else if (moveDirection.x > 0 && Mathf.Approximately(moveDirection.y, 0))
        {
            angleZ = 0;
            angleY = 0;
        }
        else if (moveDirection.x > 0 && moveDirection.y > 0)
        {
            angleZ = Mathf.Clamp(Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg, 0, 88);
            angleY = 0;
        }
        else if (moveDirection.x > 0 && moveDirection.y < 0)
        {
            angleZ = Mathf.Clamp(Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg, -88, 0);
            angleY = 0;
        }

        // Update position if movement is allowed
        if (!_checker)
        {
            float throttleMult = direction.magnitude; // Throttle multiplier based on input strength
            if (throttleMult > 0.3f)
            {
                _rb.velocity = moveDirection * (speed * throttleMult); // Apply movement velocity
            }
        }

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, angleY, angleZ);
    }

    // Shoot a bullet
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation); // Create a new bullet
        bullet.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z - 90); // Rotate the bullet based on the ship's rotation
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Sand")) // If collided with an object tagged "Sand"
        {
            GetComponent<MiniHealth>().TakeDamage(1); // Take damage
            Vector2 collisionNormal = other.contacts[0].normal; // Get the collision normal
            _rb.AddForce(collisionNormal * 10000, ForceMode2D.Impulse); // Apply force to the ship
            _canMove = false; // Disable movement
            _collisionTime = Time.time;
            StartCoroutine(EnableMovement()); // Enable movement after a delay
        }
    }

    private IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(_collisionDelay); // Wait for the specified collision delay
        _canMove = true; // Re-enable movement
    }
}
