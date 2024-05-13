using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AnimationEventDetector : MonoBehaviour
{
    public event Action OnEvent;

    public void OnAnimEvent()
    {
        OnEvent?.Invoke();  
    }


}
