using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.Shuriken;
    }

    IEnumerator SetShuriken()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            List<MonsterController> targets = Managers.Object.GetMonsterWithinCamera(SkillData.NumProjectiles);

            if (targets == null)
                yield break;
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets != null)
                {
                    if (targets[i].IsValid() == false)
                        continue;
                    Vector3 dir = targets[i].CenterPosition - Managers.Game.Player.CenterPosition;
                    Vector3 startPos = Managers.Game.Player.CenterPosition;
                    GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir.normalized, Vector3.zero, this);
                }

                yield return new WaitForSeconds(SkillData.ProjectileSpacing);
            }
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetShuriken());
    }
}
