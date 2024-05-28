using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleArrow : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.IcycleArrow;
    }

    public override void OnChangedSkillData()
    {
    }

    protected override void DoSkillJob()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            Vector3 startPos = Managers.Game.Player.PlayerCenterPos;
            Vector3 dir = Managers.Game.Player.PlayerDirection;
            for (int i = 0; i < SkillData.NumProjectiles; i++)
            {
                float angle = SkillData.AngleBetweenProj * (i - (SkillData.NumProjectiles - 1) / 2f);
                Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
                GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
            }
        }
    }
}
