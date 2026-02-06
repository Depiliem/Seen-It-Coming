using UnityEngine;
using TMPro;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float stageDuration = 45f;

    [Header("References")]
    public LaserSpawner laserSpawner;

    [Header("UI Objects")]
    public GameObject uiPanel;
    public GameObject introBoxObj;

    [Header("UI Texts")]
    public TMP_Text stageNumberText;
    public TMP_Text minigameNameText;
    public TMP_Text countdownText;
    public TMP_Text timerText;

    int currentStage = 1;

    void Start()
    {
        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        while (true)
        {
            ResetSystems();

            // ===== Tentukan nama mode =====
            string gameName = "";
            if (currentStage == 1) gameName = "LASER TAG";
            else if (currentStage == 2) gameName = "FALLING TILES";
            else gameName = "CHAOS MODE";

            // ===== INTRO UI =====
            uiPanel.SetActive(true);
            introBoxObj.SetActive(true);

            stageNumberText.text = "STAGE " + currentStage;
            yield return StartCoroutine(AnimatePopUp(stageNumberText));

            yield return new WaitForSeconds(0.5f);

            minigameNameText.text = gameName;
            yield return StartCoroutine(AnimatePopUp(minigameNameText));

            yield return new WaitForSeconds(2f);

            stageNumberText.gameObject.SetActive(false);
            minigameNameText.gameObject.SetActive(false);
            introBoxObj.SetActive(false);

            // ===== COUNTDOWN =====
            countdownText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                StartCoroutine(AnimatePopUp(countdownText));
                yield return new WaitForSeconds(1f);
            }

            countdownText.text = "GO";
            StartCoroutine(AnimatePopUp(countdownText));
            yield return new WaitForSeconds(0.5f);
            countdownText.gameObject.SetActive(false);

            // ===== AKTIFKAN STAGE =====
            ActivateStage(currentStage);

            // ===== TIMER STAGE =====
            float t = stageDuration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                timerText.text = Mathf.CeilToInt(t).ToString();
                yield return null;
            }

            currentStage++;
        }
    }

    // =========================
    // STAGE LOGIC
    // =========================

    void ActivateStage(int stage)
    {
        if (!laserSpawner) return;

        if (stage == 1)
        {
            // Laser only
            laserSpawner.ApplyDifficulty(2f);
            laserSpawner.StartLaser();
            laserSpawner.SetTileSystem(false, 0f);
        }
        else if (stage == 2)
        {
            // Tile only
            laserSpawner.StopLaser();
            laserSpawner.SetTileSystem(true, 3f);
        }
        else
        {
            // Laser + Tile
            laserSpawner.ApplyDifficulty(1.0f);
            laserSpawner.StartLaser();
            laserSpawner.SetTileSystem(true, 2.5f);
        }
    }

    void ResetSystems()
    {
        if (!laserSpawner) return;

        laserSpawner.StopLaser();
        laserSpawner.SetTileSystem(false, 0f);

        timerText.text = "";
        introBoxObj.SetActive(false);
        stageNumberText.gameObject.SetActive(false);
        minigameNameText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
    }

    // =========================
    // UI ANIMATION
    // =========================

    IEnumerator AnimatePopUp(TMP_Text txt)
    {
        txt.gameObject.SetActive(true);
        txt.transform.localScale = Vector3.zero;

        float t = 0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(0f, 1.2f, t / 0.2f);
            txt.transform.localScale = Vector3.one * s;
            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(1.2f, 1f, t / 0.1f);
            txt.transform.localScale = Vector3.one * s;
            yield return null;
        }

        txt.transform.localScale = Vector3.one;
    }
}
