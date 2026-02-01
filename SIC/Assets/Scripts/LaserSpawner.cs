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

    // ======================
    // TILE SYSTEM
    // ======================
    GameObject[] pathTiles;

    public float tileInterval = 4f;
    public float tileWarningTime = 1.2f;
    public float tileRespawnTime = 5f;

    bool tileSystemActive = false;

    void Start()
    {
        pathTiles = GameObject.FindGameObjectsWithTag("Lantai");

        InvokeRepeating(nameof(SpawnLaser), 1f, spawnInterval);
    }

    // ======================
    // LASER
    // ======================

    void SpawnLaser()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(randomX, spawnY, 0f);
        StartCoroutine(SpawnWithWarning(pos));
    }

    IEnumerator SpawnWithWarning(Vector3 pos)
    {
        float randomAngle = Random.Range(0f, 360f);

        GameObject warning = Instantiate(warningPrefab, pos, Quaternion.identity);
        warning.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        warning.transform.Rotate(Vector3.up, randomAngle, Space.World);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);

        GameObject laser = Instantiate(laserPrefab, pos, Quaternion.identity);
        laser.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        laser.transform.Rotate(Vector3.up, randomAngle, Space.World);

        Destroy(laser, 2f);
    }

    // ======================
    // TILE DESPAWN
    // ======================

    void DespawnRandomTile()
    {
        if (!tileSystemActive) return;
        if (pathTiles.Length == 0) return;

        GameObject tile = pathTiles[Random.Range(0, pathTiles.Length)];
        if (!tile.activeSelf) return;

        StartCoroutine(TileRoutine(tile));
    }

    IEnumerator TileRoutine(GameObject tile)
    {
        Renderer r = tile.GetComponent<Renderer>();
        Color original = r.material.color;

        r.material.color = Color.red;
        yield return new WaitForSeconds(tileWarningTime);

        tile.SetActive(false);

        yield return new WaitForSeconds(tileRespawnTime);

        tile.SetActive(true);
        r.material.color = original;
    }

    // ======================
    // DIFFICULTY CONTROL
    // ======================

    public void ApplyDifficulty(float newLaserInterval)
    {
        spawnInterval = newLaserInterval;

        CancelInvoke(nameof(SpawnLaser));
        InvokeRepeating(nameof(SpawnLaser), 0f, spawnInterval);
    }

    public void SetTileSystem(bool active, float newTileInterval)
    {
        tileSystemActive = active;
        tileInterval = newTileInterval;

        CancelInvoke(nameof(DespawnRandomTile));

        if (tileSystemActive)
            InvokeRepeating(nameof(DespawnRandomTile), 2f, tileInterval);
    }
}