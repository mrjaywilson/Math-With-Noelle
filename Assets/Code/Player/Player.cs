using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : IPlayer
{

    public PlayerData PlayerData { get; set; }

    public Player()
    {
        Init();

        Debug.Log(PlayerPrefs.GetString("player-data"));

        var test = PlayerData;
    }

    public void Init() => PlayerData = LoadPlayerData();

    private PlayerData LoadPlayerData()
    {
        var jsonString = PlayerPrefs.GetString("player-data");

        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.Log("No player data. Creating new player.");
            PlayerData = CreateNewPlayer();

            SavePlayerData();
            jsonString = PlayerPrefs.GetString("player-data");
        }

        return JsonConvert.DeserializeObject<PlayerData>(jsonString);
    }

    private PlayerData CreateNewPlayer()
    {
        return new PlayerData
        {
            TotalWrongAnswers = 0,
            TotalRightAnswers = 0,
            AverageAttemptsPerRound = 0,
            AverageTimePerRound = 0,
            NumberSetScore = new Dictionary<int, SessionData>(),
            RandomizedScore = new SessionData
            {
                PerfectSessions = 0,
                TotalSessions = 0
            },
            NumberSetChoice = 0,
            RoundChoice = 0
        };
    }

    public void SavePlayerData()
    {
        try
        {
            var jsonString = JsonConvert.SerializeObject(PlayerData);

            PlayerPrefs.SetString("player-data", jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void UpdateGameSettings(int numberSet, int rounds)
    {
        PlayerData.NumberSetChoice = numberSet;
        PlayerData.RoundChoice = rounds;
    }
}
