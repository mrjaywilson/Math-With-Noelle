using Newtonsoft.Json;
using System.Collections.Generic;

public sealed class PlayerData
{

    [JsonProperty("total-wrong")]
    public int TotalWrongAnswers { get; set; }

    [JsonProperty("total-right")]
    public int TotalRightAnswers { get; set; }

    [JsonProperty("average-attempts-per-round")]
    public int AverageAttemptsPerRound { get; set; }

    [JsonProperty("average-time-per-round")]
    public int AverageTimePerRound { get; set; }

    [JsonProperty("number-set-score")]
    public Dictionary<int, SessionData> NumberSetScore { get; set; }

    [JsonProperty("randomized-score")]
    public SessionData RandomizedScore { get; set; }

    [JsonProperty("number-set-choice")]
    public int NumberSetChoice { get; set; }

    [JsonProperty("round-choice")]
    public int RoundChoice { get; set; }
}

[JsonObject]
public sealed class SessionData
{
    [JsonProperty("perfect-sessions")]
    public int PerfectSessions { get; set; }
    
    [JsonProperty("total-session-attempts")]
    public int TotalSessions { get; set; }
}