using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StatsManager : MonoBehaviour
{
    [Inject] private IPlayer _player;

    [SerializeField] TextMeshProUGUI _totalWrong;
    [SerializeField] TextMeshProUGUI _totalCorrect;
    [SerializeField] TextMeshProUGUI _avgAttemptsPerRound;
    [SerializeField] TextMeshProUGUI _avgTimePerRound;
    [SerializeField] List<TextMeshProUGUI> _numberSets;
    [SerializeField] TextMeshProUGUI _randomized;

    [SerializeField] Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseStats);

        _player.Init();

        UpdateUI();
    }

    private void UpdateUI()
    {
        ApplyGeneralUIData();

        ApplyRandomizedUIData();

        _numberSets.ForEach(set =>
        {
            var index = _numberSets.IndexOf(set);

            if (_player.PlayerData.NumberSetScore.ContainsKey(index))
            {
                _player.PlayerData.NumberSetScore.TryGetValue(index, out var sessionData);
                var percentage = (sessionData.PerfectSessions / (sessionData.TotalSessions * 1.0f)) * 100;

                set.text = $"{sessionData.PerfectSessions}/{sessionData.TotalSessions} ({percentage}%)";
            }
            else
            {
                set.text = $"0/0 (0%)";
            }
        });
    }

    private void ApplyGeneralUIData()
    {
        _totalWrong.text = _player.PlayerData.TotalWrongAnswers.ToString();
        _totalCorrect.text = _player.PlayerData.TotalRightAnswers.ToString();
        _avgAttemptsPerRound.text = _player.PlayerData.AverageAttemptsPerRound.ToString();
        _avgTimePerRound.text = _player.PlayerData.AverageTimePerRound.ToString();
    }

    private void ApplyRandomizedUIData()
    {
        // random data
        var randomPerfect = _player.PlayerData.RandomizedScore.PerfectSessions;
        var totalSessions = _player.PlayerData.RandomizedScore.TotalSessions;

        if (totalSessions == 0)
        {
            _randomized.text = $"0/0 (0%)";
        }
        else
        {
            var percentage = (randomPerfect / (totalSessions * 1.0f)) * 100;
            _randomized.text = $"{randomPerfect}/{totalSessions} ({percentage}%)";
        }
    }

    private void CloseStats() => SceneManager.LoadScene(0);
}
