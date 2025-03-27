using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeckScript : MonoBehaviour
{
    private static HeckScript instance;

    private Animator animator;

    private Vector3 delta;
    private float distance;

    [Header("Heck moving")]
    public float speed;

    [Header("Appearance settings")]
    public bool appearOnStart = true;
    public bool appeared { get; private set; }
    [SerializeField] private ParticleSystem appearanceEffect;
    [SerializeField] private float secondsToAppear;

    [Header("Innkeeper position")]
    [SerializeField] private Transform innkeeperPosition;

    [Header("Following the Innkeeper")]
    [SerializeField] private float minDistanceToFollowInnkeeper;
    public bool activeFollowing;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one \"HeckScript\" class");
        }
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (!appearOnStart) animator.SetBool("Appear", false);
        appeared = appearOnStart;
    }

    void Update()
    {
        delta = innkeeperPosition.position - this.transform.position;

        distance = Vector2.Distance(innkeeperPosition.position, transform.position);

        if (appeared)
        {
            LookAtInnkeeper();

            if (activeFollowing && !DialogueManager.instance.dialogueIsPlaying) FollowTheInnkeeper();
        }
    }

    public static HeckScript GetInstance()
    {
        return instance;
    }

    void LookAtInnkeeper()
    {
        animator.SetFloat("Horizontal", delta.x);
        animator.SetFloat("Vertical", delta.y);
    }

    void FollowTheInnkeeper()
    {
        if (distance > minDistanceToFollowInnkeeper)
        {
            transform.position = Vector2.MoveTowards(transform.position, innkeeperPosition.position, speed * Time.deltaTime);
        }
    }

    public void Appear()
    {
        if (!appeared)
        {
            StartCoroutine(SetAppear(true));
        }
    }

    public void Disappear()
    {
        if (appeared)
        {
            StartCoroutine(SetAppear(false));
        }
    }

    private IEnumerator SetAppear(bool _appeared)
    {
        appearanceEffect.Play();
        yield return new WaitForSeconds(secondsToAppear);
        animator.SetBool("Appear", _appeared);
        appeared = _appeared;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(innkeeperPosition.position, minDistanceToFollowInnkeeper);
    }
}
