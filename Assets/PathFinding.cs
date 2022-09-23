using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    private List<Node> path = new List<Node>();
    private List<Node> openList;
    private List<Node> closedList;
    private Vector3 startPos;
    private Vector3 finalPos;
    public List<Node> FindPath()
    {
        startPos = NodeManager.Instance.Inicial.transform.position;
        finalPos = NodeManager.Instance.Final.transform.position;
        path = new List<Node>();
        openList = NodeManager.Instance.Nodes.FindAll(node => node.State == NodeState.Open);
        closedList = NodeManager.Instance.Nodes.FindAll(node => node.State == NodeState.Closed);
        while (openList.Count > 0)
        {
            if (Search(NodeManager.Instance.Inicial))
            {
                var node = NodeManager.Instance.Final;
                while (node != null)
                {
                    path.Add(node);
                    node = node.Parent;
                }
            }
            openList = NodeManager.Instance.Nodes.FindAll(node => node.State == NodeState.Open);
            closedList = NodeManager.Instance.Nodes.FindAll(node => node.State == NodeState.Closed);    
        }
        
        path.Reverse();
        DrawPath(path);
        return path;
    }
    
    public void DrawPath(List<Node> Path)
    {
        foreach (var node in Path)
        {
            var position = node.transform.position;
            Debug.DrawLine(position, position + Vector3.up * 2, Color.red, 10);
        }
    }

    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        List<Node> nextNodes = GetNextOpenNodes(currentNode);
        /*if(nextNodes.Count == 0)
        {
            return false;
        }*/
        nextNodes.Sort((node1, node2) => CalculateDistanceCost(currentNode, node1)
            .CompareTo(CalculateDistanceCost(currentNode, node2)));
        foreach (var nextNode in nextNodes)
        {
            if (nextNode == NodeManager.Instance.Final)
            {
                nextNode.Parent = currentNode;
                return true;
            }
            if (Search(nextNode))
            {
                nextNode.Parent = currentNode;
                return true;
            }
        }
        return false;
    }

    private List<Node> GetNextOpenNodes(Node node)
    {
        return node.Neighbors.FindAll(neighbour => neighbour.State == NodeState.Open);
    }
    private float CalculateDistanceCost(Node current, Node next)
    {
        Vector3 currentPosition = current.transform.position;
        Vector3 nodePosition = next.transform.position;
        return Vector3.Distance(currentPosition, nodePosition) + Vector3.Distance(nodePosition, finalPos);
    }
    
}