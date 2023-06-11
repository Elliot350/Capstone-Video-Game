using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayEffect(string animationName)
    {
        animator.SetTrigger(animationName);
    }

    public void Done()
    {
        GetComponentInParent<Fighter>().EffectDone(this);
    }
}
