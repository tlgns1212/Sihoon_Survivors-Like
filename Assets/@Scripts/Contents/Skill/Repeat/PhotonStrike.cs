using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStrike : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.PhotonStrike;
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator SetPhotonStrike()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            for (int i = 0; i < SkillData.NumProjectiles; i++)
            {
                Vector3 dir = Vector3.one;
                Vector3 startPos = Managers.Game.Player.CenterPosition;
                GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);

                yield return new WaitForSeconds(SkillData.ProjectileSpacing);
            }
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetPhotonStrike());
    }
}
