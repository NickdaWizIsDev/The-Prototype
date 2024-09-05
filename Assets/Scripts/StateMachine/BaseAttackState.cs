using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class BaseAttackState : GroundStates
{
    public AnimationClip[] attacks;
    public int attackIndex;
    public bool isInCombo;
    public bool canAttack = true;
    public float completion;
    float _time;

    public override void Enter()
    {
        attackIndex = -1;
        BasicAttack();
    }
    public override void Do()
    {
        if(isInCombo)
        {
            if (_time < attacks[attackIndex].length)
            {
                _time += Time.deltaTime;
            }
            if (_time >= attacks[attackIndex].length * .7f && completion < 1)
            {
                canAttack = true;
            }
            else if (completion >= 1)
            {
                canAttack = true;
                isInCombo = false;
                _time = 0f;
                IsComplete = true;
                parent.Set(idleState);
            }
        }
        completion = _time / attacks[attackIndex].length;
        Mathf.Clamp01(completion);
    }

    public void BasicAttack()
    {
        if (completion < 1 && !canAttack)
        {
            StartCoroutine(AttackBuffer());
            return;
        }
        attackIndex++;
        if (attackIndex == attacks.Length) attackIndex = 0;
        Animator.Play(attacks[attackIndex].name);
        _time = 0f;
        isInCombo = true;
        canAttack = false;
    }

    IEnumerator AttackBuffer()
    {
        yield return new WaitUntil(() => canAttack);
        BasicAttack();
    }
}