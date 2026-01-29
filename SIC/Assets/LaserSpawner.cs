using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject warningPrefab;

    public float spawnInterval = 3f;
    public float warningTime = 1.5f;

    public float minX = -5f;
    public float maxX = 5f;
    public float spawnY = 1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnLaser), 1f, spawnInterval);
        InvokeRepeating(nameof(IncreaseDifficulty), 10f, 10f);
    }

    void SpawnLaser()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(randomX, spawnY, 0f);

        StartCoroutine(SpawnWithWarning(pos));
    }

    IEnumerator SpawnWithWarning(Vector3 pos)
    {
        GameObject warning = Instantiate(warningPrefab, pos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);
        Instantiate(laserPrefab, pos, Quaternion.identity);
    }

    void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(0.8f, spawnInterval - 0.3f);
        CancelInvoke(nameof(SpawnLaser));
        InvokeRepeating(nameof(SpawnLaser), 0f, spawnInterval);
    }
}
