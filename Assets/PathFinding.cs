using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    private List<Node> path = new List<Node>();

    public List<Node> FindPath()
    {
        Node startNode = NodeManager.Instance.Inicial;
        startNode.Cost = 0;
        path = new List<Node>();
        Search(startNode);
        Node node = NodeManager.Instance.Final;
        while (node.Parent != null)
        {
            path.Add(node);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }


    private void Search(Node node)
    {
        if(node == NodeManager.Instance.Final)
        {
            Debug.LogWarning(node.Cost);
            return;
        }
        foreach (var nodeNeighbor in node.Neighbors)
        {
            if (node.Parent != nodeNeighbor)
            {
                if (Node.GetDistance(node, nodeNeighbor) + node.Cost < nodeNeighbor.Cost)
                {
                    nodeNeighbor.Cost = Node.GetDistance(node, nodeNeighbor) + node.Cost;
                    nodeNeighbor.Parent = node;
                    Debug.Log(node.Cost);
                    Search(nodeNeighbor);
                }
                
            }
        }
    }
    
}