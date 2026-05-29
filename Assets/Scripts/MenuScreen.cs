
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _player1TextBox;

    [SerializeField]
    private TextMeshProUGUI _player2TextBox;

    [SerializeField]
    private string _player1NotConnected = "Player 1 is not Connected";
    [SerializeField]
    private string _player2NotConnected = "Player 2 is not Connected";

    [SerializeField]
    private string _player1Connected = "Player 1 Connected";
    [SerializeField]
    private string _player2Connected = "Player 2 Connected";

    [SerializeField]
    private Color _connectedColor;
    [SerializeField]
    private Color _disconnectedColor;

    [SerializeField]
    private float _checkTime = 2.0f;

    [SerializeField]
    private TextMeshProUGUI _readyToPlayText;

    private bool _readyToPlay = false;

    private IASpaceShip _inputActions;

    private InputAction _startGame;

    [SerializeField]
    private string _nextScene;

    private void Awake()
    {
        _inputActions = new IASpaceShip();
        StartCoroutine(CoCheckControllers());

    }

    private void OnEnable()
    {
        _startGame = _inputActions.StartMenu.StartGame;
        _startGame.Enable();
        _startGame.performed += OnStartGame;
    }

    private void OnDisable()
    {
        _startGame.Disable();
        _startGame.performed -= OnStartGame;
    }

    private IEnumerator CoCheckControllers()
    {
        List<InputDevice> controllers = new List<InputDevice>();

        controllers.AddRange(Gamepad.all);

        if(controllers.Count > 0)
        {
            _player1TextBox.text = _player1Connected;
            _player1TextBox.color = _connectedColor;
            if(controllers.Count == 2)
            {
                _player2TextBox.text = _player2Connected;
                _player2TextBox.color = _connectedColor;
                _readyToPlay = true;
                _readyToPlayText.gameObject.SetActive(true);
            }
            else
            {
                _readyToPlay = false;
                _readyToPlayText.gameObject.SetActive(false);

            }
        }
        else
        {
            _player1TextBox.text = _player1NotConnected;
            _player1TextBox.color = _disconnectedColor;
            _player2TextBox.text = _player2NotConnected;
            _player2TextBox.color = _disconnectedColor;
            _readyToPlayText.gameObject.SetActive(false);
            _readyToPlay = false;

        }

        yield return new WaitForSeconds(_checkTime);

        StartCoroutine(CoCheckControllers());
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if(!_readyToPlay)
        {
            return;
        }
        StopAllCoroutines();
        SceneManager.LoadScene(_nextScene);
    }
}
