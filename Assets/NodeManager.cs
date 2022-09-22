using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeManager : Singleton<NodeManager>
{
    private Node inicial;
    public Node Inicial => inicial;
    private Node final;
    public Node Final  => final;
    private Node atual;
    private Node Selected;
    public List<Node> Nodes;
    [SerializeField] private GameObject nodePrefab;
    private Camera mainCamera;
    Vector3 mousePos => new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11 );

    private void Start()
    {
        Nodes = new List<Node>();
        inicial = Instantiate(nodePrefab).GetComponent<Node>();
        Nodes.Add(inicial);
        atual = inicial;
        mainCamera = Camera.main;
    }


    public void MakeNewVertex(Vector3 position)
    {
        var newNode = Instantiate(nodePrefab, position, Quaternion.identity).GetComponent<Node>();
        Nodes.Add(newNode);
        if (atual != null)
        {
            atual.AddNeighbor(newNode);
        }
        SetAtual(newNode);
    }
    
    public void SetAtual(Node node)
    {
        if (atual != null)
        {
            atual.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        atual = node;
        if (atual != null)
        {
            atual.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {

            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if(Physics.Raycast(ray, out RaycastHit hitData, 15))
            {
                if(atual != null)
                {
                    if(hitData.collider.gameObject.GetComponent<Node>() != null)
                    {
                        var node = hitData.collider.gameObject.GetComponent<Node>();
                        if(node != atual)
                        {
                            atual.AddNeighbor(node);
                            SetAtual(node);
                        }
                    }
                }
                else if(hitData.collider.gameObject.GetComponent<Node>() != null)
                {
                    SetAtual(hitData.collider.gameObject.GetComponent<Node>());
                }
            }
            else
            {
                MakeNewVertex(mainCamera.ScreenToWorldPoint(mousePos));
            }
        }
        if (Input.GetMouseButton(0))
        {
            MoveNode(atual);
        }
        else if (Input.GetMouseButtonDown(1))
        {

            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hitData, 15) && hitData.collider.gameObject.GetComponent<Node>() != null)
                SetFinal(hitData.collider.gameObject.GetComponent<Node>());
            else
                SetAtual(null);
        }
        inicial.GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    private void SetFinal(Node node)
    {
        if(final != null) final.GetComponent<MeshRenderer>().material.color = Color.white;
        final = node;
        final.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void MoveNode(Node node)
    {
        node.MoveNode(mainCamera.ScreenToWorldPoint(mousePos));
    }
}
