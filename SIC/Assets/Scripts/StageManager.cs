using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public LaserSpawner spawner;

    public TMP_Text popupText;
    public GameObject popupPanel;

    public float stageDuration = 25f;

    int stage = 1;
    float timer;

    void Start()
    {
        ApplyStageSettings();
        ShowPopup();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stageDuration)
        {
            stage++;
            timer = 0f;
            ApplyStageSettings();
            ShowPopup();
        }
    }

    void ApplyStageSettings()
    {
        if (stage == 1)
        {
            spawner.ApplyDifficulty(2f);          // laser lambat
            spawner.SetTileSystem(false, 0f);     // tile aman
        }
        else if (stage == 2)
        {
            spawner.ApplyDifficulty(1.4f);
            spawner.SetTileSystem(false, 0f);
        }
        else if (stage >= 3)
        {
            spawner.ApplyDifficulty(1.0f);
            spawner.SetTileSystem(true, 3f);      // tile hilang aktif
        }
    }

    void ShowPopup()
    {
        popupPanel.SetActive(true);
        popupText.text = "STAGE " + stage;
        Invoke(nameof(HidePopup), 2f);
    }

    void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}