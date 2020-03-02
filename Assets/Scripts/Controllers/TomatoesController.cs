using UnityEngine;

public class TomatoesController : MonoBehaviour {
    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void Grow() {
        _animator.SetTrigger("grow");
    }
}
