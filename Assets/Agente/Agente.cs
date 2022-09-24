using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AgentState
{
    Start,
    Moving,
    Finished
}

public class Agente : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    List<Node> path = new List<Node>();
    AgentState state = AgentState.Start;
    private void FixedUpdate()
    {
        switch (state)
        {
            case AgentState.Start:
                WaitForPath();
                break;
            case AgentState.Moving:
                Move();
                break;
            case AgentState.Finished:
                ResetPath();
                break;
        }
    }

    private void ResetPath()
    {
        NodeManager.Instance.ResetPath();
        state = AgentState.Start;
    }
    private void WaitForPath()
    {
        transform.position = NodeManager.Instance.Inicial.transform.position;
    }
    public void GetPath()
    {
        if (state != AgentState.Moving)
        {
            path = PathFinding.Instance.FindPath();
            state = AgentState.Moving;
        }
    }
    private void Move()
    {
        if (path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, speed);
            if (Vector3.Distance(transform.position, path[0].transform.position) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
        else
        {
            state = AgentState.Finished;
        }
    }
}