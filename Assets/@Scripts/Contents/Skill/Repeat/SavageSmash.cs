using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavageSmash : RepeatSkill
{
    [SerializeField]
    private ParticleSystem[] _swingParticle;
    [SerializeField]
    private ParticleSystem[] _swingParticleFinal;
    float _radian;

    protected enum SwingType
    {
        First,
        Second,
        Third,
        Fourth,
    }

    private void Awake()
    {
        SkillType = Define.SkillType.SavageSmash;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    public override void OnLevelUp()
    {
        base.OnLevelUp();
        transform.localScale = Vector3.one * SkillData.ScaleMultiplier;
    }

    IEnumerator SwingSword()
    {
        if (Level == 6)
        {
            for (int i = 0; i < SkillData.NumProjectiles; i++)
            {
                _swingParticleFinal[i].gameObject.SetActive(true);
                SetParticles(i);
            }
        }
        else
        {
            for (int i = 0; i < SkillData.NumProjectiles; i++)
            {
                _swingParticle[i].gameObject.SetActive(true);
                SetParticles(i);
            }
        }
        yield return new WaitForSeconds(SkillData.CoolTime);
    }

    void SetParticles(int swingType)
    {
        Vector3 tempAngle = Managers.Game.Player.Indicator.transform.eulerAngles;
        transform.localEulerAngles = tempAngle;
        transform.position = Managers.Game.Player.CenterPosition;

        _radian = Mathf.Deg2Rad * tempAngle.z * -1;

        if (Level == 6)
        {
            var main = _swingParticleFinal[swingType].main;
            main.startRotation = _radian;
        }
        else
        {
            var main = _swingParticle[swingType].main;
            main.startRotation = _radian;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CreatureController creature = other.transform.GetComponent<CreatureController>();
        if (creature?.IsMonster() == true)
        {
            creature.OnDamaged(Managers.Game.Player, this);
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SwingSword());
    }

    public override void OnChangedSkillData()
    {
        transform.localScale = Vector3.one * SkillData.ScaleMultiplier;
    }
}
