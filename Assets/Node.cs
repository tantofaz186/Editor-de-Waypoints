using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public enum NodeState
{
    Open,
    Closed
}
public class Node : MonoBehaviour
{
    List<Node> neighbors;
    public List<Node> Neighbors => neighbors;
    public NodeState State = NodeState.Open;
    private static Material LNMaterial;
    public Node Parent { get; set; }

    private void Awake()
    {
        if (LNMaterial != null) return;
        LNMaterial = new Material(Shader.Find("Sprites/Default"));
    }

    private void Start()
    {
        neighbors = new List<Node>();
    }

    public void AddNeighbor (Node neighbor)
    {
        if (neighbors.Contains(neighbor)) return;
        neighbors.Add(neighbor);
        neighbor.AddNeighbor(this);
        AddVisualConnection(neighbor);
    }

    private void AddVisualConnection(Node neighbor)
    {
        var neighbourPos = neighbor.transform.position;
        if (!TryGetComponent<LineRenderer>(out LineRenderer lineRenderer))
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, neighbourPos);
            lineRenderer.material = LNMaterial;
            lineRenderer.startColor = lineRenderer.endColor = Color.cyan;
            lineRenderer.startWidth = lineRenderer.endWidth = 0.2f;
        }
        else
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount += 2;
            lineRenderer.SetPosition(lineRenderer.positionCount-2, transform.position);
            lineRenderer.SetPosition(lineRenderer.positionCount-1, neighbourPos);
        }
    }

    public void MoveNode(Vector3 position)
    {
        transform.position = position;
        if (TryGetComponent(out LineRenderer lineRenderer))
        {
            for (int i = 0; i < lineRenderer.positionCount; i+=2)
            {
                lineRenderer.SetPosition(i, transform.position);

            }
        }

        foreach (var neighbor in neighbors)
        {
            if (TryGetComponent(out LineRenderer ln))
            {
                for (int i = 1; i < ln.positionCount; i+=2)
                {
                    ln.SetPosition(i, transform.position);

                }
            }
        }

    }
}
