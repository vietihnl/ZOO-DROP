using System.Collections.Generic;
using UnityEngine;


public class GameOverLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer lineVisual;
    [SerializeField] private float lineY=4.8f;
    [SerializeField] private float graceTime = 1.5f;
    [SerializeField] private float timeToLose = 2.5f;

    private float dangerTimer;

    private void Start()
    {
        lineVisual.transform.position = new Vector3(0f, lineY, 0f);
        SetLineAlpha(0f);
    }

    private void Update()
{
    if (gameManager.IsGameOver) return;

    if (IsAnyAnimalAboveLine())
    {
        dangerTimer += Time.deltaTime;

        float blink = Mathf.PingPong(Time.time * 4f, 1f);
        SetLineAlpha(Mathf.Lerp(0.3f, 1f, blink));

        if (dangerTimer >= timeToLose)
        {
            gameManager.TriggerGameOver();
        }
    }
    else
    {
        dangerTimer = 0f;
        SetLineAlpha(0f);
    }
}

    private bool IsAnyAnimalAboveLine()
    {
        IReadOnlyList<Animal> animals = Animal.Active;

        for(int i=0; i<animals.Count; i++)
        {
            Animal a = animals[i];
            if(a.TimeSinceDropped>=graceTime&&a.TopY>=lineY)
            {
                return true;
            }
        }
        return false;

    }

    private void SetLineAlpha(float alpha)
    {
        Color c = lineVisual.color;
        c.a = alpha;
        lineVisual.color = c;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-4f, lineY, 0f), new Vector3(4f, lineY, 0f));
    }
}
