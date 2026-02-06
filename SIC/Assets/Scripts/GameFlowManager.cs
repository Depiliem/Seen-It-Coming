using UnityEngine;
using TMPro;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float stageDuration = 45f;

    [Header("References")]
    public LaserSpawner laserSpawner;
    // public TileManager tileManager; 

    [Header("UI Objects")]
    public GameObject uiPanel;
    public GameObject introBoxObj;       // Kotak Hitam Background

    [Header("UI Texts")]
    public TMP_Text stageNumberText;     // Text "STAGE 1"
    public TMP_Text minigameNameText;    // Text "LASER EVASION"
    public TMP_Text countdownText;       // Text "3.. 2.. 1.."
    public TMP_Text timerText;           // Text Timer Pojok

    private int currentStage = 1;

    void Start()
    {
        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        while (true) // Loop stage selamanya
        {
            // 1. RESET: Matikan semua game & UI
            ResetAllMinigames();
            introBoxObj.SetActive(false);
            stageNumberText.gameObject.SetActive(false);
            minigameNameText.gameObject.SetActive(false);
            countdownText.gameObject.SetActive(false);
            timerText.text = "";

            // Tentukan Nama Game
            string gameName = "";
            if (currentStage == 1) gameName = "LASER TAG!";
            else if (currentStage == 2) gameName = "FALLING TILES";
            else gameName = "SURVIVAL CHAOS";

            // ==================================================
            // SEQUENCE INTRO (REVISI: HILANG BARENG)
            // ==================================================

            // A. Munculkan Kotak Hitam
            uiPanel.SetActive(true);
            introBoxObj.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            // B. Pop Up "STAGE X"
            stageNumberText.text = "STAGE " + currentStage;
            // Munculkan Stage, tapi JANGAN dihilangkan dulu
            yield return StartCoroutine(AnimatePopUp(stageNumberText));

            yield return new WaitForSeconds(0.5f); // Jeda dikit biar gantian munculnya

            // C. Pop Up "NAMA GAME" (Muncul di bawah Stage)
            minigameNameText.text = gameName;
            yield return StartCoroutine(AnimatePopUp(minigameNameText));

            // D. TAHAN (Biarkan pemain baca dua-duanya)
            yield return new WaitForSeconds(2.0f);

            // E. HILANG BARENGAN!
            stageNumberText.gameObject.SetActive(false);
            minigameNameText.gameObject.SetActive(false);

            // Tutup Kotak Hitam
            introBoxObj.SetActive(false);

            // ==================================================
            // SEQUENCE COUNTDOWN (Lanjut seperti biasa)
            // ==================================================
            countdownText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                StartCoroutine(AnimatePopUp(countdownText));
                yield return new WaitForSeconds(1f);
            }

            countdownText.text = "GO!";
            StartCoroutine(AnimatePopUp(countdownText));
            yield return new WaitForSeconds(0.5f);
            countdownText.gameObject.SetActive(false);

            // ==================================================
            // GAMEPLAY MULAI
            // ==================================================
            ActivateMinigame(currentStage);

            float timeLeft = stageDuration;
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = Mathf.CeilToInt(timeLeft).ToString();
                yield return null;
            }

            currentStage++;
        }
    }

    // --- FUNGSI ANIMASI (Sama seperti sebelumnya) ---
    IEnumerator AnimatePopUp(TMP_Text targetText)
    {
        targetText.gameObject.SetActive(true);
        targetText.transform.localScale = Vector3.zero;

        // Tahap 1: Membesar (Pop)
        float timer = 0;
        float duration = 0.2f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 1.2f, timer / duration);
            targetText.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Tahap 2: Settling
        timer = 0;
        duration = 0.1f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1.2f, 1f, timer / duration);
            targetText.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        targetText.transform.localScale = Vector3.one;
    }

    void ResetAllMinigames()
    {
        if (laserSpawner) laserSpawner.enabled = false;
        // if (tileManager) tileManager.SetTileSystem(false);
    }

    void ActivateMinigame(int stage)
    {
        if (stage == 1)
        {
            if (laserSpawner) { laserSpawner.enabled = true; laserSpawner.ApplyDifficulty(2f); }
        }
        else if (stage == 2)
        {
            if (laserSpawner) { laserSpawner.enabled = true; laserSpawner.ApplyDifficulty(1.4f); }
        }
        else
        {
            if (laserSpawner) { laserSpawner.enabled = true; laserSpawner.ApplyDifficulty(1.0f); }
        }
    }
}