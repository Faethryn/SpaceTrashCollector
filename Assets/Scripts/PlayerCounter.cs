
using UnityEngine;
using UnityEngine.Events;

public class PlayerCounter : MonoBehaviour
{
    public int _players = 0;

    private static PlayerCounter instance = null;

    private PlayerCounter()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static PlayerCounter Instance
    {
        get
        {
            //if (instance == null)
            //{
            //    instance = new PlayerCounter();
            //}
            return instance;
        }
    }


    public void OnPlayerJoined()
    {
        _players++;
        PlayerJoined?.Invoke();
    }

    public UnityEvent PlayerJoined;
}
