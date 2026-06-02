using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecordingManager : MonoBehaviour
{
    private static RecordingManager instance = null;

    private RecordingManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static RecordingManager Instance
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

    private Transform _player1;

    const string _controlScheme = "Controller";

    private void Awake()
    {
        _inputManager = this.gameObject.GetComponent<PlayerInputManager>();

        InputDevice controller1 = Gamepad.all[0];

        _inputManager.JoinPlayer(0, 0, _controlScheme, controller1);
    }
}
