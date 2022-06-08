using System;
using System.Collections;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        StartScreen,
        Gameplay,
        EndScreen
    }

    public SmallPersonsPlacer.EntityColorCode playerColorCode;
    private CharacterForwardMove _charFwdMove;
    [SerializeField] private Renderer playerRenderer;

    [SerializeField] private int playerHealth;

    [SerializeField] private float scaleStep = 1.1f;
    [SerializeField] private float timeToLerp = 0.25f;
    [SerializeField] private float scaleModifier = 1f;

    [SerializeField] private CinemachineVirtualCamera playerFollowCam;
    private Cinemachine3rdPersonFollow _cinemachine3RdPersonFollow;

    public static Transform PlayerTransform;

    // [SerializeField] private float growAmount;
    public GameState state;
    private ThirdPersonController _thirdPersonController;

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        _charFwdMove = PlayerTransform.GetComponent<CharacterForwardMove>();

        UpdateGameState(GameState.StartScreen);

        _thirdPersonController = PlayerTransform.GetComponent<ThirdPersonController>();
        _cinemachine3RdPersonFollow = playerFollowCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        // StartCoroutine(LerpScale(targetScale, timeToLerp));
        var colorsLength = System.Enum.GetValues(typeof(SmallPersonsPlacer.EntityColorCode)).Length;
        var rColorCode = Random.Range(0, colorsLength);

        SetColor((SmallPersonsPlacer.EntityColorCode)rColorCode);
        LittlePerson.OnColorTrigger += RunIntoPerson;

        UpdateGameState(GameState.StartScreen);
    }

    private void SetColor(SmallPersonsPlacer.EntityColorCode colCode)
    {
        playerColorCode = colCode;
        switch (colCode)
        {
            case SmallPersonsPlacer.EntityColorCode.Red:
                foreach (var mat in playerRenderer.materials)
                    mat.color = Color.red;
                break;
            case SmallPersonsPlacer.EntityColorCode.Green:
                foreach (var mat in playerRenderer.materials)
                    mat.color = Color.green;
                break;
            case SmallPersonsPlacer.EntityColorCode.Blue:
                foreach (var mat in playerRenderer.materials)
                    mat.color = Color.blue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(colCode), colCode, null);
        }
    }

    private void SizeChanger(bool increase)
    {
        if (increase)
        {
            PlayerTransform.localScale *= scaleStep;
            _cinemachine3RdPersonFollow.CameraDistance *= scaleStep;
            _thirdPersonController.SprintSpeed *= scaleStep;
        }
        else
        {
            PlayerTransform.localScale /= scaleStep;
            _cinemachine3RdPersonFollow.CameraDistance /= scaleStep;
            _thirdPersonController.SprintSpeed /= scaleStep;
        }
    }

    private IEnumerator SizeChanger(float endValue, float duration)
    {
        var ogCamDist = _cinemachine3RdPersonFollow.CameraDistance;
        var ogSpeed = _thirdPersonController.SprintSpeed;
        var time = 0f;
        var startValue = scaleModifier;
        var startScale = PlayerTransform.localScale;
        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            PlayerTransform.localScale = startScale * scaleModifier;

            _cinemachine3RdPersonFollow.CameraDistance = ogCamDist * scaleModifier;
            _thirdPersonController.SprintSpeed = ogSpeed * scaleModifier * 2f;

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale * endValue;
        scaleModifier = endValue;
    }

    private void RunIntoPerson(SmallPersonsPlacer.EntityColorCode colorCode)
    {
        if (playerColorCode == colorCode)
        {
            playerHealth++;
            if (playerHealth > 10)
            {
                playerHealth = 10;
                print("too fat!");
            }

            if (playerHealth == 10)
                return;
            SizeChanger(true);

            // StartCoroutine(SizeChanger(targetScale, timeToLerp));
        }
        else
        {
            playerHealth--;
            if (playerHealth < 0)
            {
                playerHealth = 0;
                print("too skinny!");

                UpdateGameState(GameState.EndScreen);
            }

            if (playerHealth == 0)
                return;

            SizeChanger(false);

            // StartCoroutine(SizeChanger(1 / targetScale, timeToLerp));
        }

        // if (playerHealth < 1)
        // {
        //     print("You're dead, nerd!");
        //     UpdateGameState(GameState.EndScreen);
        // }
        // else if (playerHealth >= 10)
        // {
        //     playerHealth = 10;
        //     print("Maximum Power!");
        // }
    }

    private void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.StartScreen:
                HandleStartScreen();
                break;
            case GameState.Gameplay:
                HandleGameplay();
                break;
            case GameState.EndScreen:
                HandleEndScreen();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleStartScreen()
    {
        // _charFwdMove.enabled = false;
    }

    private void HandleGameplay()
    {
        //_charFwdMove.enabled = true;
    }

    private void HandleEndScreen()
    {
        print("Called End Screen");
        //_charFwdMove.enabled = false;
    }
}
