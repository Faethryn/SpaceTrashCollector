using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnvironmentChangeBase : MonoBehaviour
{

    public virtual void DoChange(float percentage)
    {
       
    }


    public virtual void TweenChange(float percentage, float duration)
    {
        
    }

    private void OnDestroy()
    {
        DOTween.KillAll(this);
    }
}



