using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Animator _ac = null;
    [SerializeField] float _destroyDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(_ac == null)
        {
            _ac = GetComponent<Animator>();
        }
    }
    public void Hit()
    {
        _ac.Play("hit");
        GetComponent<AudioSource>()?.Play();
        Destroy(gameObject, _destroyDuration);
    }

    public void AnimationEnded()
    {
        while(transform.childCount > 0)
        {
            var child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}
