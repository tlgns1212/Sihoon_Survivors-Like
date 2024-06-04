using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class UI_ParticlePlay : MonoBehaviour
{
    [SerializeField]
    UIParticleSystem _particleSystem;


    public void OnEnable()
    {
        _particleSystem.DOPlay();
    }
}
