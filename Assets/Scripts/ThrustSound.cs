using MathUtil;
using UnityEngine;

public class ThrustSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource _ownSource;

    [SerializeField]
    private Vector2 _pitchRange = new Vector2(0.6f, 1.2f);

    [SerializeField]
    private Vector2 _volumeRange = new Vector2(0f, 1f);

    [SerializeField]
    private float _volumeThreshold = 0.05f;

    [SerializeField]
    private Vector2 _speedRange = new Vector2(0f, 50f);

    public void UpdateSpeed(float speed)
    {
        float speedFactor = MathUtilHelpers.RemapF(_speedRange.x, _speedRange.y, 0f, 1f, speed);

        float pitch = Mathf.Lerp(_pitchRange.x, _pitchRange.y, speedFactor);
        float volume = Mathf.Lerp(_volumeRange.x, _volumeRange.y, speedFactor);

        if(_ownSource.isPlaying == false && volume >= _volumeThreshold)
        {
            _ownSource.Play();
        }

        _ownSource.pitch = pitch;
        _ownSource.volume = volume;
    }
}
