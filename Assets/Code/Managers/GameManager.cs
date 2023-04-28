using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    // TODO: Make the problem zoom in big and then back out a bit smaller.
    // TODO: Implement the timer.

    [Inject] private IPlayer _player;

    [SerializeField] private TextMeshProUGUI _firstProblemNumber;
    [SerializeField] private TextMeshProUGUI _secondProblemNumber;

    [SerializeField] private Button AnswerOneButton;
    [SerializeField] private Button AnswerTwoButton;
    [SerializeField] private Button AnswerThreeButton;

    [SerializeField] private TextMeshProUGUI _AnswerOne;
    [SerializeField] private TextMeshProUGUI _AnswerTwo;
    [SerializeField] private TextMeshProUGUI _AnswerThree;

    [SerializeField] private Image _countdownFill;
    [SerializeField] private TextMeshProUGUI _timerCountdownText;

    [SerializeField] private GameObject _gameFinished;
    [SerializeField] private TextMeshProUGUI _gameFinishedEncouragement;

    [SerializeField] private Button _finishGameButton;
    [SerializeField] private Button _quitGame;

    [SerializeField] private AudioClip _answerCorrectClip;
    [SerializeField] private AudioClip _answerWrongClip;
    [SerializeField] private AudioClip _neutralButtonClip;
    [SerializeField] private AudioClip _finishClip;


    private Round _roundData;
    private int _numberOfRounds = 0;

    private int _roundTracker = 1;

    private float _elapsedTime = 0;
    private bool _timerStarted = false;
    private int _timerCountdown = 10;

    // Round info for stats
    private int _roundTimeCounter = 0;
    private int _roundWrongAnswers = 0;
    private int _roundRightAnswers = 0;
    private bool _perfectSession = true;

    private byte _redFill = 0;
    private byte _greenFill = 255;
    private byte _fillValue = 51;

    //private Player _player;

    private List<int> _numberSets = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    private List<int> _rounds = new List<int> { 10, 25, 50, 100 };

    private void Start()
    {
        _finishGameButton.onClick.AddListener(ReturnToMenu);
        _quitGame.onClick.AddListener(EarlyQuit);

        _player.Init();

        if (_roundData == null)
        {
            _roundData = new Round();
        }

        _roundData.NumberSet = _numberSets[_player.PlayerData.NumberSetChoice];
        _numberOfRounds = _rounds[_player.PlayerData.RoundChoice];

        NewRound();

        AnswerOneButton.onClick.AddListener(delegate { CheckAnswer(1, AnswerOneButton); });
        AnswerTwoButton.onClick.AddListener(delegate { CheckAnswer(2, AnswerTwoButton); });
        AnswerThreeButton.onClick.AddListener(delegate { CheckAnswer(3, AnswerThreeButton); });
    }

    private void Update()
    {
        if (_timerStarted)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 1)
            {
                UpdateTimer();
                _elapsedTime = 0f;

                _roundTimeCounter++;
            }
        }
    }

    public void NewRound()
    {

        if (_roundTracker == _numberOfRounds)
        {
            EndGame();
            return;
        }

        _roundTracker++;
        _roundData.GetNewRound();

        UpdateUI();

        RestartTimer();
    }

    private void EarlyQuit()
    {
        PlayNeutralButtonSound();
        EndGame();
    }

    private void EndGame()
    {
        _gameFinished.SetActive(true);
        SetEncouragementText();

        _player.PlayerData.AverageTimePerRound = _roundTimeCounter / _roundTracker;
        _player.PlayerData.AverageAttemptsPerRound = (_roundRightAnswers + _roundWrongAnswers) / _roundTracker;

        _player.PlayerData.TotalRightAnswers += _roundRightAnswers;
        _player.PlayerData.TotalWrongAnswers += _roundWrongAnswers;

        UpdateSessionScore();

        _player.SavePlayerData();
    }

    private void UpdateSessionScore()
    {
        var numberSetIndex = _roundData.NumberSet - 1;

        if (numberSetIndex == -1)  // if randomized game
        {
            _player.PlayerData.RandomizedScore.PerfectSessions += GetPerfectSessionScore();
            _player.PlayerData.RandomizedScore.TotalSessions += 1;
        }
        else
        {
            if (!_player.PlayerData.NumberSetScore.ContainsKey(numberSetIndex))
            {
                _player.PlayerData.NumberSetScore.Add(
                    numberSetIndex,
                    new SessionData
                    {
                        PerfectSessions = GetPerfectSessionScore(),
                        TotalSessions = 1
                    });
            }
            else
            {
                var updatedSessionData = _player.PlayerData.NumberSetScore;

                updatedSessionData[numberSetIndex].PerfectSessions += GetPerfectSessionScore();
                updatedSessionData[numberSetIndex].TotalSessions += 1;

                _player.PlayerData.NumberSetScore = updatedSessionData;
            }
        }
    }

    private int GetPerfectSessionScore() => _perfectSession ? 1 : 0;

private void SetEncouragementText()
    {
        int num = UnityEngine.Random.Range(0, 10);

        string message = "";

        switch (num)
        {
            case 0:
                message = "FANTASTIC JOB!";
                break;
            case 1:
                message = "YOU DID GREAT!";
                break;
            case 2:
                message = "MATHTASTIC!";
                break;
            case 3:
                message = "AMAZING JOB!";
                break;
            case 4:
                message = "WOW! SO GOOD!";
                break;
            case 5:
                message = "SUPER EFFORT!";
                break;
            case 6:
                message = "WONDERFUL!";
                break;
            case 7:
                message = "EXCELLENT WORK!";
                break;
            case 8:
                message = "INCREDIBLE!";
                break;
            case 9:
                message = "AWESOME KEEP IT UP!";
                break;
        }

        _gameFinishedEncouragement.text = message;
    }

    private async void ReturnToMenu()
    {
        AudioSource.PlayClipAtPoint(_finishClip, Camera.main.transform.position);

        await Task.Delay(1400);
        
        SceneManager.LoadScene(0);
    }
        

    private void RestartTimer()
    {
        _redFill = 0;
        _greenFill = 255;
        _timerCountdown = 10;

        _timerCountdownText.text = _timerCountdown.ToString();

        _countdownFill.color = new Color32(0, 255, 0, 255);
        _countdownFill.fillAmount = 1f;
        _timerStarted = true;
    }

    private void UpdateTimer()
    {
        _countdownFill.fillAmount -= .1f;

        _timerCountdown--;
        _timerCountdownText.text = _timerCountdown.ToString();

        var timerValue = _countdownFill.fillAmount;

        if (timerValue >= .6f)
        {
            _greenFill = 255;
            _redFill += _fillValue;

            _countdownFill.color = new Color32(_redFill, _greenFill, 0, 255);
        }
        else if (timerValue > Mathf.Epsilon)
        {
            _redFill = 255;
            _greenFill -= _fillValue;

            if (_greenFill <= 0)
            {
                _countdownFill.color = Color.red;
            }

            if (_countdownFill.color != Color.red)
            {
                _countdownFill.color = new Color32(_redFill, _greenFill, 0, 255);
            }
        }
        else
        {
            _timerStarted = false;

            _perfectSession = false;
        }
    }

    private void UpdateUI()
    {
        _firstProblemNumber.text = _roundData.GetFirstNumber();
        _secondProblemNumber.text = _roundData.GetSecondNumber();

        _AnswerOne.text = _roundData.GetAnswerOne();
        _AnswerTwo.text = _roundData.GetAnswerTwo();
        _AnswerThree.text = _roundData.GetAnswerThree();

        AnswerOneButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        AnswerTwoButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        AnswerThreeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

    }

    public void CheckAnswer(int answerNumber, Button button)
    {
        if (_roundData.CheckAnswer(answerNumber))
        {
            PlayCorrectAnswerSound();

            _roundRightAnswers++;
            button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            WinRound();
        }
        else
        {
            PlayWrongAnswerSound();

            _perfectSession = false;
            _roundWrongAnswers++;
            button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
    }

    private void PlayCorrectAnswerSound() => AudioSource.PlayClipAtPoint(_answerCorrectClip, Camera.main.transform.position);
    private void PlayWrongAnswerSound() => AudioSource.PlayClipAtPoint(_answerWrongClip, Camera.main.transform.position);
    private void PlayNeutralButtonSound() => AudioSource.PlayClipAtPoint(_neutralButtonClip, Camera.main.transform.position);

    private void WinRound()
    {
        _timerStarted = false;

        Debug.Log("YOU WIN!");
        NewRound();
    }
}
