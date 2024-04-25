using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : SingletonClass<ScoreManager>
{
    [Header("Score Manager")]
    [SerializeField] private int _kills;
    [SerializeField] private int _enemykills;
    [SerializeField] private Text _playerkillCount;
    [SerializeField] private Text _enemykillCount;
    [SerializeField] private Text _mainText;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("kills"))
        {
            _kills = PlayerPrefs.GetInt("0");
        } else if(PlayerPrefs.HasKey("enemykills"))
        {
            _enemykills = PlayerPrefs.GetInt("0");
        }
    }

    private void Update()
    {
        StartCoroutine(WinOrLose());
    }

    IEnumerator WinOrLose()
    {
        _playerkillCount.text = " " + _kills;
        _enemykillCount.text = " " + _enemykills;

        if(_kills >= 10)
        {
            _mainText.text = "Blue Team WIN";
            PlayerPrefs.SetInt("kills", _kills);
            Time.timeScale = 0.0f;
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene("Play");
        }
        else if (_enemykills >= 10)
        {
            _mainText.text = "Red Team WIN";
            PlayerPrefs.SetInt("enemykills", _enemykills);
            Time.timeScale = 0.0f;
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene("Play");
        }
    }

    public void SetEnemyKills(int value)
    {
        _enemykills += value;
    }

    public void SetPlayerKills(int value)
    {
        _kills += value;
    }
}
