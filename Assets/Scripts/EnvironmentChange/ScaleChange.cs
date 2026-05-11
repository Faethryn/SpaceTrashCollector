using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : EnvironmentChangeBase
{

    private Vector3 _originalScale;

  
    private Vector3 _endScale = new Vector3(1,1,1);

    [SerializeField]
    private Vector3 _relativeScale = new Vector3(1,1,1);


    private void Awake()
    {
        _originalScale = transform.localScale;

        _endScale = new Vector3( _originalScale.x * _relativeScale.x, _originalScale.y * _relativeScale.y, _originalScale.z * _relativeScale.z) ;
        
    }


    // Start is called before the first frame update
    public override void DoChange(float percentage)
    {
        Vector3 currentScale  = Vector3.Lerp(_originalScale, _endScale, percentage);

        transform.localScale = currentScale;
    }

    public override void TweenChange(float percentage, float duration)
    {
      
        Vector3 currentScale = Vector3.Lerp(_originalScale, _endScale, percentage);

        transform.DOScale(currentScale, duration);

    }
}
