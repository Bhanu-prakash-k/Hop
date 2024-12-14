using System.Collections;
using UnityEngine;

public class BounceMovement : MonoBehaviour
{
    public static BounceMovement instance;
    [SerializeField] private float bounceHeight = 2f;  
    [SerializeField] private float travelTime = 1f;    
    [SerializeField] private float swerveSpeed = 5f;   
    [SerializeField] private float swerveLimit = 2f;   
    [SerializeField] private float smoothTime = 0.1f;  
    private Vector3 startPosition;                    
    private Vector3 targetPosition;                   
    private float elapsedTime;                                     
    private float targetX;                            
    private float currentXVelocity;                   

    private void Awake() {
        if(instance == null)
            instance = this;
    }
    
    private void Start()
    {
        // Initialize the starting position and first target
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(0, 0, 6);
        elapsedTime = 0f;
        targetX = transform.position.x; // Start with the current X position as the target
    }

    private void Update()
    {
        if (GameManager.instance.isGameOver) return;

        HandleSwerveInput();
        MoveParabolically();

        // Check for platform when reaching near the target position
        if (elapsedTime / travelTime >= 0.99f) // Close to target Z position
        {
            CheckForPlatformBelow();
        }
    }

    private void HandleSwerveInput()
    {
        // Mouse input for Editor
        if (Input.GetMouseButton(0))
        {
            float percentageX = (Input.mousePosition.x - Screen.width / 2) / (Screen.width * 0.5f) * 2;
            percentageX = Mathf.Clamp(percentageX, -1.0f, 1.0f);
            targetX = percentageX * 3; // Calculate target position based on swipe distance
        }

        // Touch input for Mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float touchDeltaX = touch.deltaPosition.x;
                targetX += touchDeltaX * swerveSpeed * Time.deltaTime;
            }
        }

        targetX = Mathf.Clamp(targetX, -swerveLimit, swerveLimit);

        // Smoothly transition to the target X position using SmoothDamp
        float smoothX = Mathf.SmoothDamp(transform.position.x, targetX, ref currentXVelocity, smoothTime);
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);
    }

    private void MoveParabolically()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / travelTime;
        float z = Mathf.Lerp(startPosition.z, targetPosition.z, t);
        float y = Mathf.Sin(t * Mathf.PI) * bounceHeight;
        transform.position = new Vector3(transform.position.x, y, z);

        if (t >= 1f)
        {
            elapsedTime = 0f;
            startPosition = targetPosition;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + 6); 
            PlatformSpawner.instance.SpawnPlatform();
        }
    }

    private void CheckForPlatformBelow()
    {
        // Raycast to detect platform below
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("Platform"))
            {
                GameManager.instance.AddScore(2);
                // Platform found, return platform to pool
                StartCoroutine(ReturnPlatformToPool(hit.collider.gameObject));
                return;
            }
        }

        // If no platform is detected below
        Debug.Log("Game Over: No platform below!");
        GameManager.instance.isGameOver = true; // Stop the game
        GameManager.instance.OnGameOver();
    }

    private IEnumerator ReturnPlatformToPool(GameObject platform)
    {
        // Wait a few seconds before returning platform to pool
        yield return new WaitForSeconds(1f);  // Adjust this delay as needed
        PlatformSpawner.instance.platforms.Remove(platform);
        PlatformPooler.instance.ReleasePlatforms(platform); // Return the platform to the pool
    }
    public void ResetBallPosition(){
        transform.position = new Vector3(0, 0, 0);
    }
}
