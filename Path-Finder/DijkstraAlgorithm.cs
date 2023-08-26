using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm : MonoBehaviour
{
    public Node startNode;           // Starting node
    public Node endNode;             // Destination node
    public LineRenderer lineRenderer; // LineRenderer for visualization
    public GameObject Trigger;
    private List<Node> shortestPath = new List<Node>();

    private void Start()
    {
        setNewPath(startNode, RandomNode());
    }

    public void setNewPath(Node startPoint, Node EndPoint)
    {
        startNode = startPoint;
        endNode = EndPoint;
        Instantiate(Trigger, endNode.transform.position, endNode.transform.rotation);
        FindShortestPath();
        VisualizePath();
    }

    // testing purpose only
    public void SetRandomPath()
    {
        Node[] allNodes = FindObjectsOfType<Node>();

        setNewPath(allNodes[Random.Range(0, allNodes.Length)], allNodes[Random.Range(0, allNodes.Length)]);
    }

    public Node RandomNode()
    {
        Node[] allNodes = FindObjectsOfType<Node>();
        return allNodes[Random.Range(0, allNodes.Length)];
    }

    private void FindShortestPath()
    {
        Dictionary<Node, float> distances = new Dictionary<Node, float>();
        Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();

        List<Node> unvisitedNodes = new List<Node>();

        foreach (Node node in FindObjectsOfType<Node>())
        {
            distances[node] = Mathf.Infinity;
            previousNodes[node] = null;
            unvisitedNodes.Add(node);
        }

        distances[startNode] = 0f;

        while (unvisitedNodes.Count > 0)
        {
            Node closestNode = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Node node in unvisitedNodes)
            {
                if (distances[node] < shortestDistance)
                {
                    shortestDistance = distances[node];
                    closestNode = node;
                }
            }

            if (closestNode == endNode)
                break;

            unvisitedNodes.Remove(closestNode);

            foreach (Node neighbor in GetNeighbors(closestNode))
            {
                float tentativeDistance = distances[closestNode] + Vector3.Distance(closestNode.transform.position, neighbor.transform.position);
                if (tentativeDistance < distances[neighbor])
                {
                    distances[neighbor] = tentativeDistance;
                    previousNodes[neighbor] = closestNode;
                }
            }
        }

        shortestPath.Clear();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            shortestPath.Insert(0, currentNode);
            currentNode = previousNodes[currentNode];
        }
    }

    private List<Node> GetNeighbors(Node node)
    {
        return node.neighbors;
    }

    private void VisualizePath()
    {
        if (lineRenderer != null && shortestPath.Count > 0)
        {
            lineRenderer.positionCount = shortestPath.Count;

            for (int i = 0; i < shortestPath.Count; i++)
            {
                lineRenderer.SetPosition(i, shortestPath[i].transform.position);
            }
        }
    }
}
