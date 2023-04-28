using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    [Inject] private IPlayer _player;

    [SerializeField] Button _startButton;

    [SerializeField] List<Button> _numberSetButtons;
    [SerializeField] List<Button> _roundButtons;

    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] Button _ok;
    [SerializeField] Button _cancel;
    [SerializeField] GameObject _passwordPanel;
    [SerializeField] Button _openLogin;

    [SerializeField] AudioClip _neutralButtonClip;
    [SerializeField] AudioClip _startButtonClip;

    private void Start()
    {
        _player.Init();

        UpdateUI();

        _startButton.onClick.AddListener(StartGame);
        _ok.onClick.AddListener(CheckPassword);
        _cancel.onClick.AddListener(closePasswordPanel);
        _openLogin.onClick.AddListener(() => _passwordPanel.SetActive(true));
    }

    private async void StartGame()
    {

        AudioSource.PlayClipAtPoint(_startButtonClip, Camera.main.transform.position);

        await Task.Delay(1250);

        _player.SavePlayerData();
        LoadGame();
    }

    private void UpdateUI()
    {
        BuildSetButtons();
        BuildRoundButtons();

        UpdateButtonSelection();
    }

    private void LoadGame() => SceneManager.LoadScene(1);

    private void BuildSetButtons()
    {
        for (int i = 0; i < _numberSetButtons.Count; i++)
        {
            var setNumber = i;
            _numberSetButtons[i].onClick.AddListener(
                delegate { UpdateButtons(setNumber, _numberSetButtons); });
        }
    }

    private void BuildRoundButtons()
    {
        for (int i = 0; i < _roundButtons.Count; i++)
        {

            var roundNumber = i;
            _roundButtons[i].onClick.AddListener(delegate { UpdateButtons(roundNumber, _roundButtons); });
        }
    }

    private void UpdateButtons(int index, List<Button> buttonList)
    {
        AudioSource.PlayClipAtPoint(_neutralButtonClip, Camera.main.transform.position);

        int oldIndex;

        if (buttonList.Count > 4)
        {
            oldIndex = _player.PlayerData.NumberSetChoice;
            _player.PlayerData.NumberSetChoice = index;
        }
        else
        {
            oldIndex = _player.PlayerData.RoundChoice;
            _player.PlayerData.RoundChoice = index;
        }

        _player.SavePlayerData();

        // Update button colors
        buttonList[oldIndex].GetComponent<Image>().color = new Color32(0, 94, 255, 255);
        buttonList[index].GetComponent<Image>().color = Color.green;
    }

    private void UpdateButtonSelection()
    {
        var setIndex = _player.PlayerData.NumberSetChoice;
        var roundIndex = _player.PlayerData.RoundChoice;

        _numberSetButtons[setIndex].GetComponent<Image>().color = Color.green;
        _roundButtons[roundIndex].GetComponent<Image>().color = Color.green;
    }

    private void closePasswordPanel()
    {
        _passwordPanel.SetActive(false);
    }

    private void CheckPassword()
    {
        if (_passwordInput.text == "486245")
        {
            _passwordInput.text = "";

            _passwordPanel.SetActive(false);
            SceneManager.LoadScene(2);
        }
        else
        {
            _passwordInput.text = "";
        }
    }
}
