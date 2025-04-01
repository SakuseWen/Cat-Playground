using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gate : MonoBehaviour
{
    public Transform to;
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = to.position;
    }
}
