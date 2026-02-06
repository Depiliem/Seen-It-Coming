using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("Laser Settings")]
    public GameObject laserPrefab;
    public GameObject warningPrefab;

    public float spawnInterval = 3f;
    public float warningTime = 1.5f;

    public float minX = -5f;
    public float maxX = 5f;
    public float spawnY = 1f;

    bool laserActive = false;

    // ======================
    // TILE SYSTEM
    // ======================

    GameObject[] pathTiles;

    [Header("Tile Settings")]
    public float tileInterval = 4f;
    public float tileWarningTime = 1.2f;
    public float tileRespawnTime = 5f;

    bool tileSystemActive = false;

    void Awake()
    {
        pathTiles = GameObject.FindGameObjectsWithTag("Lantai");
    }

    void OnEnable()
    {
        CacheTilesIfNeeded();
    }

    void CacheTilesIfNeeded()
    {
        if (pathTiles == null || pathTiles.Length == 0)
            pathTiles = GameObject.FindGameObjectsWithTag("Lantai");
    }

    // ======================
    // LASER SYSTEM
    // ======================

    void SpawnLaser()
    {
        if (!laserActive) return;

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

        if (warning) Destroy(warning);

        GameObject laser = Instantiate(laserPrefab, pos, Quaternion.identity);
        laser.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        laser.transform.Rotate(Vector3.up, randomAngle, Space.World);

        Destroy(laser, 2f);
    }

    public void ApplyDifficulty(float newLaserInterval)
    {
        spawnInterval = newLaserInterval;

        if (laserActive)
        {
            CancelInvoke(nameof(SpawnLaser));
            InvokeRepeating(nameof(SpawnLaser), 0f, spawnInterval);
        }
    }

    public void StartLaser()
    {
        laserActive = true;
        CancelInvoke(nameof(SpawnLaser));
        InvokeRepeating(nameof(SpawnLaser), 0f, spawnInterval);
    }

    public void StopLaser()
    {
        laserActive = false;
        CancelInvoke(nameof(SpawnLaser));
    }

    // ======================
    // TILE SYSTEM
    // ======================

    void DespawnRandomTile()
    {
        if (!tileSystemActive) return;
        CacheTilesIfNeeded();
        if (pathTiles.Length == 0) return;

        GameObject tile = pathTiles[Random.Range(0, pathTiles.Length)];
        if (!tile.activeSelf) return;

        StartCoroutine(TileRoutine(tile));
    }

    IEnumerator TileRoutine(GameObject tile)
    {
        Renderer r = tile.GetComponent<Renderer>();
        Material m = r.material;

        Color original = m.color;

        // 🔥 aktifkan emission
        m.EnableKeyword("_EMISSION");

        float t = 0f;
        float blinkSpeed = 0.15f;

        while (t < tileWarningTime)
        {
            // nyala merah + glow
            m.color = Color.red;
            m.SetColor("_EmissionColor", Color.red * 4f);

            yield return new WaitForSeconds(blinkSpeed);

            // balik normal + glow mati
            m.color = original;
            m.SetColor("_EmissionColor", Color.black);

            yield return new WaitForSeconds(blinkSpeed);

            t += blinkSpeed * 2f;
        }

        // pastikan glow mati sebelum hilang
        m.SetColor("_EmissionColor", Color.black);

        tile.SetActive(false);

        yield return new WaitForSeconds(tileRespawnTime);

        tile.SetActive(true);

        // restore warna + emission off
        m.color = original;
        m.SetColor("_EmissionColor", Color.black);
    }


    public void SetTileSystem(bool active, float newTileInterval)
    {
        tileSystemActive = active;
        tileInterval = newTileInterval;

        CancelInvoke(nameof(DespawnRandomTile));

        if (tileSystemActive)
        {
            InvokeRepeating(nameof(DespawnRandomTile), 2f, tileInterval);
        }
    }
}
