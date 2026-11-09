using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Animal : MonoBehaviour
{
    public static event Action<Animal, Animal> MergeRequested;
    private static readonly List<Animal> active = new List<Animal>();
    public static IReadOnlyList<Animal> Active => active;
    public static event System.Action<int,float,Vector3> Hit;


    private float droppedTime;
    private int level;
    private bool isDropped;
    private bool isMerged;
    private bool hasHit;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D col;

    public int Level => level;
    public bool IsDropped => isDropped;
    public bool IsMerged => isMerged;
    public float TimeSinceDropped => isDropped ? Time.time - droppedTime : 0f;
    public float TopY => col.bounds.max.y;

    private void OnEnable()
    {
        active.Add(this);
    }

    private void OnDisable()
    {
        active.Remove(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    //gán và chỉnh size thú
    public void Init(AnimalDatabase db, int newLevel)
    {
        level = newLevel;
        AnimalDatabase.AnimalInfo info = db.Get(level);

        sr.sprite = info.Sprite;

        float spriteWidth = info.Sprite.bounds.size.x;
        transform.localScale = Vector3.one*(info.Diameter/spriteWidth);

        col.offset = Vector2.zero;
        col.radius = spriteWidth*0.5f*db.ColliderRatio;
    }

   
    public void Hold()
    {
        isDropped=false;
        rb.simulated=false;
    }

   
    public void Drop()
    {
        isDropped = true;
        hasHit= false;
        droppedTime = Time.time;
        rb.simulated = true;
    }

    public void MarkMerged()
    {
        isMerged=true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReportHit(collision);
        if(isMerged || !isDropped) return;

        if(!collision.gameObject.TryGetComponent(out Animal other)) return;
        if(other.isMerged||!other.isDropped) return;
        if(other.level != level) return;

        if(GetInstanceID() > other.GetInstanceID()) return;

        MergeRequested?.Invoke(this, other);
    }

    public void PlayPop()
    {
        StartCoroutine(PopRoutine());
    }

    private IEnumerator PopRoutine()
    {
        Vector3 targetScale = transform.localScale;
        float duration = 0.25f;
        float t = 0f;

        while(t<duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t/duration);
            //vuot 1 chut r ve dung size
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            float ease = 1f + c3*Mathf.Pow(k-1f,3f) + c1*Mathf.Pow(k-1f,2f);

            transform.localScale = targetScale*Mathf.LerpUnclamped(0.4f,1f,ease);
            yield return null;
        }

        transform.localScale = targetScale;
    }
    
    private void ReportHit(Collision2D collision)
    {
        if(!isDropped||isMerged|| hasHit) return;
        hasHit = true;

        float speed = collision.relativeVelocity.magnitude;


        Vector2 point = collision.contactCount>0?collision.GetContact(0).point:(Vector2)transform.position;

        Hit?.Invoke(level,speed,point);
    }

    public void SuppressHitSound()
    {
        hasHit=true;
    }
}
