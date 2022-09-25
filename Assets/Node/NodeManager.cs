using System;
using System.Collections;
using System.Collections.Generic;
using NiceIO.Sysroot;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeManager : Singleton<NodeManager>
{
    private Node inicial;
    public Node Inicial => inicial;
    private Node final;
    public Node Final => final;
    private Node atual;
    public List<Node> Nodes;
    [SerializeField] private GameObject nodePrefab;
    private Camera mainCamera;
    [SerializeField] private Agente agente;
    Vector3 mousePos => new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11 + 65);

    private void Start()
    {
        Nodes = new List<Node>();
        inicial = Instantiate(nodePrefab).GetComponent<Node>();
        inicial.transform.position = new Vector3(inicial.transform.position.x, -65, inicial.transform.position.z);
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (final != null) final.GetComponent<MeshRenderer>().material.color = Color.green;

            //se clicar em um node, ele vira o atual e vira vizinho do anterior
            //se clicar fora de um node, cria um novo node que vira o atual e vira vizinho do anterior
            //se clicar no mesmo node, o node é desmarcado
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hitData, 200) &&
                hitData.collider.gameObject.GetComponent<Node>() != null)
            {
                var node = hitData.collider.gameObject.GetComponent<Node>();
                if (node == atual)
                {
                    SetAtual(null);
                }
                else if (atual != null)
                {
                    atual.AddNeighbor(node);
                    SetAtual(node);
                }
                else
                {
                    SetAtual(node);
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
            SetFinal();
        }

        if (Input.GetKeyDown(KeyCode.Space) && final != null)
        {
            agente.GetPath();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteNode(atual);
        }

        inicial.GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    private void DeleteNode(Node node)
    {
        if (node != null)
        {
            Nodes.Remove(node);
            node.Delete();
        }
    }

    private void SetAtual(Node node)
    {
        if (atual != null) atual.GetComponent<MeshRenderer>().material.color = Color.white;
        atual = node;
        if (atual != null) atual.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void SetFinal(Node node)
    {
        SetAtual(null);
        if (final != null) final.GetComponent<MeshRenderer>().material.color = Color.white;
        final = node;
        if (final != null) final.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    private void SetFinal()
    {
        SetAtual(null);
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitData, 200) &&
            hitData.collider.gameObject.GetComponent<Node>() != null)
        {
            if (final != null) final.GetComponent<MeshRenderer>().material.color = Color.white;
            final = hitData.collider.gameObject.GetComponent<Node>();
            final.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    void MoveNode(Node node)
    {
        if (node != null) node.MoveNode(mainCamera.ScreenToWorldPoint(mousePos));
    }

    public void ResetPath()
    {
        foreach (var node in Nodes)
        {
            node.Reset();
        }

        SetInicial(final);
        SetAtual(inicial);
        SetFinal(null);
    }

    void SetInicial(Node node)
    {
        if (inicial != null) inicial.GetComponent<MeshRenderer>().material.color = Color.white;
        inicial = node;
    }
}