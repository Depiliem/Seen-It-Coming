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

    GameObject[] pathTiles;

    public float tileInterval = 4f;
    public float tileWarningTime = 1.2f;
    public float tileRespawnTime = 5f;

    void Start()
    {
        // 🔥 AUTO AMBIL SEMUA TILE
        pathTiles = GameObject.FindGameObjectsWithTag("Lantai");

        InvokeRepeating(nameof(SpawnLaser), 1f, spawnInterval);
        InvokeRepeating(nameof(IncreaseDifficulty), 10f, 10f);
        InvokeRepeating(nameof(DespawnRandomTile), 3f, tileInterval);
    }

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

    void DespawnRandomTile()
    {
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

    void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(0.8f, spawnInterval - 0.3f);
        CancelInvoke(nameof(SpawnLaser));
        InvokeRepeating(nameof(SpawnLaser), 0f, spawnInterval);

        tileInterval = Mathf.Max(1.5f, tileInterval - 0.3f);
        CancelInvoke(nameof(DespawnRandomTile));
        InvokeRepeating(nameof(DespawnRandomTile), 1f, tileInterval);
    }
}