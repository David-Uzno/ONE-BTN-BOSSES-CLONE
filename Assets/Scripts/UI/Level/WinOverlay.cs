using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinOverlay : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Timer _timer;

    [Header("Stats Panel UI")]
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private TextMeshProUGUI _statsTitle;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject _leaderBoardPanel;
    [SerializeField] private TextMeshProUGUI _leaderBoardTitle;
    
    public void StatsPanel()
    {
        _leaderBoardPanel.SetActive(false);
        _leaderBoardTitle.text = $"{_leaderBoardTitle.text.Replace("<u>", "").Replace("</u>", "")}";

        _statsPanel.SetActive(true);
        if (_statsTitle.text.Contains("<u>") == false)
        {
            _statsTitle.text = $"<u>{_statsTitle.text}</u>";
        }
    }

    public void LeaderBoardPanel()
    {
        _statsPanel.SetActive(false);
        _statsTitle.text = $"{_statsTitle.text.Replace("<u>", "").Replace("</u>", "")}";

        _leaderBoardPanel.SetActive(true);
        if (_leaderBoardTitle.text.Contains("<u>") == false)
        {
            _leaderBoardTitle.text = $"<u>{_leaderBoardTitle.text}</u>";
        }
    }

    public void OnVictory()
    {
        _timer.StopGameTimer();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Map()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
