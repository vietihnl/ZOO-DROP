using System.Collections.Generic;
using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer lineVisual;
    [SerializeField] private float lineY = 3.5f;
    [SerializeField] private float warningDistance = 1.5f;
    [SerializeField] private float graceTime = 1.5f;
    [SerializeField] private float timeToLose = 2.5f;

    [Header("Visual Settings")]
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private Color dangerColor = Color.red;

    private float dangerTimer;

    private void Start()
    {
        lineVisual.transform.position = new Vector3(0f, lineY, 0f);
        SetLineAlpha(0f);
    }

    private void Update()
    {
        if (gameManager.IsGameOver) return;

        float highestTopY = GetHighestAnimalTopY();

        if (highestTopY >= lineY)
        {
           
            dangerTimer += Time.deltaTime;

            float blink = Mathf.PingPong(Time.time * 4f, 1f);
            SetLineVisual(dangerColor, Mathf.Lerp(0.4f, 1f, blink));

            if (dangerTimer >= timeToLose)
            {
                gameManager.TriggerGameOver();
            }
        }
        else if (highestTopY >= lineY - warningDistance)
        {
           
            dangerTimer = 0f;

            float blink = Mathf.PingPong(Time.time * 2f, 1f);
            SetLineVisual(warningColor, Mathf.Lerp(0.25f, 0.75f, blink));
        }
        else
        {
            
            dangerTimer = 0f;
            SetLineAlpha(0f);
        }
    }

    private float GetHighestAnimalTopY()
    {
        IReadOnlyList<Animal> animals = Animal.Active;
        float highest = float.MinValue;

        for (int i = 0; i < animals.Count; i++)
        {
            Animal a = animals[i];
            if (a.TimeSinceDropped >= graceTime && a.TopY > highest)
            {
                highest = a.TopY;
            }
        }

        return highest;
    }

    private void SetLineVisual(Color color, float alpha)
    {
        color.a = alpha;
        lineVisual.color = color;
    }

    private void SetLineAlpha(float alpha)
    {
        Color c = lineVisual.color;
        c.a = alpha;
        lineVisual.color = c;
    }

    private void OnDrawGizmos()
    {
        // Vạch Game Over (đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-4f, lineY, 0f), new Vector3(4f, lineY, 0f));

        // Vạch giới hạn cảnh báo (vàng)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-4f, lineY - warningDistance, 0f), new Vector3(4f, lineY - warningDistance, 0f));
    }
}
