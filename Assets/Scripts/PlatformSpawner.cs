using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner instance; // Singleton instance
    //[SerializeField] private GameObject platformPrefab; // Reference to the platform prefab
    //[SerializeField] private Transform platformParent; // Parent object for spawned platforms
    private PlatformPooler objectPool; // Reference to the object pool
    private float spawnDistance = 6f; // Standard distance between platforms
    private float lastZPosition = 0f; // Tracks the last spawned platform's Z position
    [SerializeField]
    private int platformCount = 6; // Initial platform pool size
    private float randomXOffset = 2f; // Random X offset range
    private float fixedYPosition = -0.4f; // Fixed Y position for platforms
    public List<GameObject> platforms;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        objectPool = PlatformPooler.instance;
        SpawnInitialPlatforms(platformCount);
    }

    public void SpawnInitialPlatforms(int count)
    {
        // Spawn initial set of platforms for pooling
        for (int i = 0; i < count; i++)
        {
            GameObject platform = objectPool.SpawnPlatform();
            float randomX = GetRandomXOffset(); // Get random X offset
            platform.transform.position = new Vector3(randomX, fixedYPosition, lastZPosition + spawnDistance);
            platform.SetActive(true);
            lastZPosition = platform.transform.position.z; // Update the lastZPosition after each spawn
            platforms.Add(platform);
        }
    }

    public void SpawnPlatform()
    {
        // Get platform from pool
        GameObject platform = objectPool.SpawnPlatform();

        if (platform != null)
        {
            // Set a random X offset and fixed Y position for the new platform
            float randomX = GetRandomXOffset(); // Get random X offset
            platform.transform.position = new Vector3(randomX, fixedYPosition, lastZPosition + spawnDistance);
            platform.SetActive(true);

            // Update the lastZPosition after platform spawn to ensure correct distance
            lastZPosition = platform.transform.position.z;
            platforms.Add(platform);
        }
    }

    // Returns a random X offset (-randomXOffset, 0, or randomXOffset)
    private float GetRandomXOffset()
    {
        float[] options = {-randomXOffset, 0f, randomXOffset};
        return options[Random.Range(0, options.Length)];
    }
}
