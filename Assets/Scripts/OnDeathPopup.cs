using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnDeathPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text WaveText;
    [SerializeField] private TMP_Text ZombieText;
    [SerializeField] private TMP_Text SpawnerText;
    [SerializeField] private TMP_Text FinalScoreText;
    [SerializeField] private GameObject Popup;


    // Start is called before the first frame update
    void Start()
    {
        Popup.SetActive(false);
    }

    public void PlayerDied()
    {
        WaveText.text = LevelRules.Instance.WaveNumber.ToString();
        ZombieText.text = LevelRules.Instance.KillCount.ToString();
        SpawnerText.text = LevelRules.Instance.SpawnerDestroyCount.ToString();
        FinalScoreText.text = Mathf.RoundToInt(LevelRules.Instance.Score).ToString();
        Popup.SetActive(true);
    }

    public void Quit()
    {
        AudioManager.Instance.ToggleSound("Music", false);
        AudioManager.Instance.PlaySound("Title Music");
        SceneManager.LoadScene("Menu");
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
