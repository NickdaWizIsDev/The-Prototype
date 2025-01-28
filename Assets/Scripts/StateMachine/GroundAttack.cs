using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class GroundAttack : GroundStates
{
    public AnimationClip[] attacks;
    public int attackIndex;
    public bool isInCombo;
    public bool canAttack = true;
    public float completion;
    public float completionRequirement = 0.8f;
    float _time;

    public override void Enter()
    {
        attackIndex = 0;
        Animator.Play(attacks[attackIndex].name);
        _time = 0f;
        isInCombo = true;
        canAttack = false;
        core.canMove = false;
    }
    public override void Do()
    {
        core.animator.SetBool(AnimationStrings.canMove, false);
        if(isInCombo)
        {
            if (_time < attacks[attackIndex].length)
            {
                _time += Time.deltaTime;
            }
            if (_time >= attacks[attackIndex].length * completionRequirement && completion < 1)
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
    public override void Exit()
    {
        core.animator.SetBool(AnimationStrings.canMove, true);
        core.canMove = true;
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
    bool buffer = false;
    IEnumerator AttackBuffer()
    {
        if (completion < 0.5f || buffer) yield break;
        buffer = true;
        yield return new WaitUntil(() => canAttack);
        BasicAttack();
        buffer = false;
    }
}