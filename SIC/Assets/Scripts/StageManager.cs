using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public LaserSpawner spawner;

    public TMP_Text stagePopupText;
    public GameObject popupPanel;

    public float stageDuration = 25f;

    int stage = 1;
    float timer;

    void Start()
    {
        ApplyStage();
        ShowPopup();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stageDuration)
        {
            stage++;
            timer = 0f;

            ApplyStage();
            ShowPopup();
        }
    }

    void ApplyStage()
    {
        // tabel difficulty sederhana
        float laserInterval = Mathf.Max(0.7f, 3f - stage * 0.4f);
        float tileInterval  = Mathf.Max(1.2f, 4f - stage * 0.5f);

        spawner.ApplyDifficulty(laserInterval, tileInterval);
    }

    void ShowPopup()
    {
        popupPanel.SetActive(true);
        stagePopupText.text = "STAGE " + stage;

        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), 2f);
    }

    void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}