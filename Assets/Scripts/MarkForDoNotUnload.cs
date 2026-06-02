using UnityEngine;

public class MarkForDoNotUnload : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
