using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoesController : MonoBehaviour
{
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Grow()
    {
        _anim.SetTrigger("grow");
    }
}
