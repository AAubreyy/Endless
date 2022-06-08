using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] public Button startButton;
    private TextMeshProUGUI _startButtonGUI;

    [SerializeField] public Button retryButton;

    // [SerializeField] public TextMeshProUGUI message;
    [SerializeField] private GameObject score;
    [SerializeField] private CharacterForwardMove _characterForward;

    private void Awake()
    {
        Instance = this;

        GameManager.OnGameStateChanged += GameManagerOnStateChanged;

        _startButtonGUI = startButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    public void OnStartButtonPressed(){
	    _characterForward.StartRunning();
	    startButton.gameObject.SetActive(false);
    }

    public void OnTryAgainPressed(){
	    SceneManager.LoadScene(0);
    }

	    private void GameManagerOnStateChanged(GameManager.GameState state)
	    {
        switch (state)
        {
            case GameManager.GameState.StartScreen:
                _startButtonGUI.text = "Play";
                if (retryButton.gameObject.activeSelf)
                    retryButton.gameObject.SetActive(false);
                if (score.activeSelf)
                    score.SetActive(false);
                // message.transform.parent.gameObject.SetActive(true);
                break;
            case GameManager.GameState.Gameplay:
                // message.transform.parent.gameObject.SetActive(false);
                break;
            case GameManager.GameState.EndScreen:
                // message.text = "Game Over";
                // message.transform.parent.gameObject.SetActive(true);
		
		_characterForward.Stop();
                retryButton.gameObject.SetActive(true);
                score.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
