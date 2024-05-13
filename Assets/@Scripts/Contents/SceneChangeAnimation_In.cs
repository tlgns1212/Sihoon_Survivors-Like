using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In
public class SceneChangeAnimation_In : UI_Popup
{
    Animator _anim;
    Action _action;
    Define.Scene _nextScene;

    private void Awake()
    {
        _anim= GetComponent<Animator>();   
    }

    public void SetInfo(Define.Scene nextScene, Action callback)
    {
        transform.localScale = Vector3.one;
        _action = callback;
        _nextScene = nextScene;
        StartCoroutine(OnAnimationComplete());
    }

    IEnumerator OnAnimationComplete()
    {
        yield return new WaitForSeconds(1f);
        _action.Invoke();
    }

}
