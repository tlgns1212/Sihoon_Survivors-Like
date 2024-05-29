using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBlade : RepeatSkill
{
    [SerializeField]
    private ParticleSystem[] _swingParticle;

    protected enum SwingType
    {
        First,
        Second,
        Third,
        Fourth,
    }

    private void Awake()
    {
        SkillType = Define.SkillType.StormBlade;
    }

    IEnumerator SwingSword()
    {
        if (Managers.Game.Player != null)
        {
            Vector3 dir = Managers.Game.Player.PlayerDirection;
            Shoot(dir);

            for (int i = 0; i < 7; i++)
            {
                dir = Quaternion.AngleAxis((45 + 45 * i) * -1, Vector3.forward) * dir;
                Shoot(dir);
                yield return new WaitForSeconds(SkillData.AttackInterval);
            }
        }
        yield return null;
    }

    private void Shoot(Vector3 dir)
    {
        string prefabName = SkillData.PrefabLabel;
        Vector3 startPos = Managers.Game.Player.CenterPosition;

        for (int i = 0; i < SkillData.NumProjectiles; i++)
        {
            float angle = SkillData.AngleBetweenProj * (i - (SkillData.NumProjectiles - 1) / 2f);
            Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
            GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsLearnedSkill == false)
            return;
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
}
