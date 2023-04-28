using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public PlayerData PlayerData { get; set; }

    public void Init();
    public void SavePlayerData();
    public void UpdateGameSettings(int num, int rnd);
}
