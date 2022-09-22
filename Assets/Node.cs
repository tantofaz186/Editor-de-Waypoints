using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum NodeState
{
    Open,
    Closed
}
public class Node : MonoBehaviour
{
    [SerializeField]List<Node> neighbors;
    public List<Node> Neighbors => neighbors;
    public NodeState State = NodeState.Open;
    private static Material LNMaterial;
    public Node Parent { get; set; }

    private void Awake()
    {
        if (LNMaterial != null) return;
        LNMaterial = new Material(Shader.Find("Sprites/Default"));
        neighbors = new List<Node>();
    }

    public void AddNeighbor (Node neighbor)
    {
        if (neighbors.Contains(neighbor)) return;
        neighbors.Add(neighbor);
        AddVisualConnection(neighbor);
        neighbor.AddNeighbor(this);
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
            lineRenderer.positionCount = neighbors.Count * 2;
            lineRenderer.SetPosition(lineRenderer.positionCount-2, transform.position);
            lineRenderer.SetPosition(lineRenderer.positionCount-1, neighbourPos);
        }
    }

    public void MoveNode(Vector3 position)
    {
        transform.position = position;
        if (TryGetComponent(out LineRenderer lineRenderer))
        {
            for (int i = 0; i < lineRenderer.positionCount; i += 2)
            {
                lineRenderer.SetPosition(i, transform.position);
            }

            foreach (var neighbor in Neighbors)
            {
                int index = neighbor.neighbors.FindIndex(node => node == this);
                Debug.Log("index: " + index);
                if (index >= 0)
                {
                    try{neighbor.GetComponent<LineRenderer>().SetPosition(index*2 + 1, transform.position);}
                    catch{}
                }
            }
        }
    }
}
