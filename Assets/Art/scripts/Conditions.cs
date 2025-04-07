// ÷’µ„Ω≈±æ GoalTrigger.cs  (∑≈ Scripts/)
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.I.Win();
    }
}
