using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class GameManager : MonoBehaviour
{

    private static GameManager instance = null;

    private GameManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private PlayerInputManager _inputManager;

    [SerializeField]
    private Transform _shipSpawn1;

    [SerializeField] 
    private Transform _shipSpawn2;

    [SerializeField]
    private Camera _endScreenCam;

    [SerializeField]
    private TextMeshProUGUI _player1WinText;

    [SerializeField]
    private TextMeshProUGUI _player2WinText;

    [SerializeField]
    private string _player1WinString = "Player 1 Wins!";

    [SerializeField]
    private string _player2WinString = "Player 2 Wins!";

    [SerializeField]
    private string _player1LossString = "Player 1 lost";

    [SerializeField]
    private string _player2LossString = "Player 2 lost";

    private Transform _player1;
    private Transform _player2;

    const string _controlScheme = "Controller";

    private IASpaceShip _inputs;

    private InputAction _startGame;

    private InputAction _stopGame;

    [SerializeField]
    private string _menuScene;

    bool _gameOver = false;

    private void Awake()
    {
        _inputs = new IASpaceShip();
        _inputManager = this.gameObject.GetComponent<PlayerInputManager>();

        InputDevice controller1 = Gamepad.all[0];

        InputDevice controller2 = Gamepad.all[1];

        _inputManager.JoinPlayer(0, 0, _controlScheme, controller1);

        _inputManager.JoinPlayer(1, 1, _controlScheme, controller2);

        StartCoroutine(CoDelaySpawnLocation());
    }

    private void OnEnable()
    {
        _startGame = _inputs.StartMenu.StartGame;
        _startGame.Enable();
        _startGame.performed += OnStartGame;

        _stopGame = _inputs.StartMenu.CloseGame;
        _stopGame.Enable();
        _stopGame.performed += OnStopGame;
    }

    private void OnDisable()
    {
        _startGame.Disable();
        _startGame.performed -= OnStartGame;

        _stopGame.Disable();
        _stopGame.performed -= OnStopGame;
    }

    public Transform GetOpponent(int playerId)
    {
        switch (playerId)
        {
            case 0:
                return _player2.gameObject.transform;

            case 1:
                return _player1.gameObject.transform;

        }

        return _player1.gameObject.transform;
    }

    public void PlayerHasJoined(int playerId, Ship ship) 
    {
        switch (playerId)
        {
            case 0:
                _player1 = ship.transform;
                //ship.transform.position = _shipSpawn1.position;
                break;
            case 1:
                _player2 = ship.transform;
                //ship.transform.position = _shipSpawn2.transform.position;
                break;

        }
    }

    public void OnFuelEmpty(int playerId)
    {
        _player1.gameObject.SetActive(false);
        _player2.gameObject.SetActive(false);
        _gameOver = true;

        _endScreenCam.gameObject.SetActive(true);
        switch (playerId)
        {
            case 0:
                _player1WinText.text = _player1LossString;
                _player2WinText.text = _player2WinString;
                break;
            case 1:
                _player1WinText.text = _player1WinString;
                _player2WinText.text = _player2LossString;
                break;

        }
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if (!_gameOver)
        {
            return;
        }
        StopAllCoroutines();
        SceneManager.LoadScene(_menuScene);
    }

    private void OnStopGame(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        Application.Quit();
    }

    private IEnumerator CoDelaySpawnLocation()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        yield return null;
        _player1.transform.position = _shipSpawn1.position;
        _player2.transform.position = _shipSpawn2.position;
    }
}
