using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DelayAnimation : MonoBehaviour
{
    [SerializeField] private float maxDelay = 0.5f;
    [SerializeField] private bool startAtRandomPosition; 
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void Start()
    {
        StartCoroutine(DelayAndAnimate());
    }

    // Update is called once per frame
    IEnumerator DelayAndAnimate()
    {
        yield return new WaitForSeconds(Random.Range(0f, maxDelay));

        _animator.enabled = true;
        _animator.playbackTime = startAtRandomPosition ? Random.Range(0f, _animator.GetCurrentAnimatorClipInfo(0).Length) : 0;
    }
}
