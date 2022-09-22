using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{
    List<Node> path = new List<Node>();

    private void OnEnable()
    {
        transform.position = NodeManager.Instance.Inicial.transform.position;
        path = PathFinding.Instance.FindPath();
    }

    private void FixedUpdate()
    {
        if (path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, 0.1f);
            if (transform.position == path[0].transform.position)
            {
                path.RemoveAt(0);
            }
        }
    }
}