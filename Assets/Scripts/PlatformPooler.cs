using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class PlatformPooler : MonoBehaviour
{
    public static PlatformPooler instance;

    public GameObject platformPrefab; // The platform prefab to pool
    public int poolSize = 6; // Number of platforms in the pool
    private Queue<GameObject> platformPool = new Queue<GameObject>();
    private ObjectPool<GameObject> _platformPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        // Initialize pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject platform = Instantiate(platformPrefab);
            platform.SetActive(false);
            platformPool.Enqueue(platform);
        }
        _platformPool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(platformPrefab);
        }, platform =>
        {
            platform.gameObject.SetActive(true);
        }, platform =>
        {
            platform.gameObject.SetActive(false);
        }, platform =>
        {
            Destroy(platform.gameObject);
        }, false, 200, 100);
    }
    public GameObject SpawnPlatform()
    {
        return _platformPool.Get();
        //platform.transform.position = spawnPosition.position;
    }
    public void ReleasePlatforms(GameObject platform)
    {
        //PlatformSpawner.instance.platforms.Remove(platform);
        _platformPool.Release(platform);
    }

}
