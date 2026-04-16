using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera _mainCam;


    private void Awake()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainCam != null)
        {
            this.transform.forward = Camera.main.transform.forward;
            this.transform.up = Camera.main.transform.up;
        }
    }
}
