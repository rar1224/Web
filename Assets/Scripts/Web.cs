using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Web : MonoBehaviour
{
    public Node nodePrefab;
    public Line linePrefab;

    public GameObject circle;
    private Collider2D circleCollider;

    private List<Node> nodes = new List<Node>();
    private Line activeLine;
    float timestamp;

    private bool isValid = false;
    private bool isOverNode = false;
    public bool IsValid { get => isValid; set => isValid = value; }
    public bool IsOverNode { get => isOverNode; set => isOverNode = value; }

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = circle.GetComponent<Collider2D>();
        nodePrefab.Web = this;
        linePrefab.Web = this;

        List<Vector2> directions = new List<Vector2>();

        // starting position

        for (int i = 0; i<4; i++)
        {
            Vector2 direction = Vector2.one;
            bool valid = false;

            while (!valid)
            {
                direction = Random.insideUnitCircle.normalized * 0.55f;

                if (i == 0)
                {
                    break;
                }

                for (int j = 0; j < i; j++)
                {


                    if (Vector2.Angle(direction, directions[j]) < 15)
                    {
                        valid = false;
                        break;
                    }

                    valid = true;
                }
            }

            directions.Add(direction);


            Node node = Instantiate(nodePrefab);
            node.transform.position = direction;
            node.IsEssential = true;
            AddNode(node);

            Node outsideNode = Instantiate(nodePrefab);
            outsideNode.transform.position = direction * 30;
            AddNode(outsideNode);

            Line line = Instantiate(linePrefab);
            line.StartLine(node);
            line.EndLine(outsideNode);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isValid = true;

        foreach (Node node in nodes)
        {
            float distance = ((Vector2)node.transform.position - mousePos).magnitude;

            if (distance < 0.2)
            {
                isValid = false;
                break;
            }
        }




        if (activeLine != null) {
            Vector2 rayOrigin = activeLine.Origin.transform.position;
            bool redraw = true;

            if (activeLine.Origin.IsEssential)
            {
                redraw = true;
            }

            else  {
                RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, mousePos - rayOrigin);

                foreach (RaycastHit2D hit in hits )
                {
                    if (hit.collider == circleCollider)
                    {
                        redraw = false;
                        isValid = false;
                        break;
                    }
                }
            }

            if (redraw) activeLine.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        } 
        if (Time.time > timestamp + 0.1f && Input.GetMouseButtonDown(0) && activeLine != null && isValid == true) { 
            activeLine.Origin.RemoveLine(activeLine);
            Destroy(activeLine.gameObject);
        };
    }

    public Line GetActiveLine()
    {
        return activeLine;
    }

    public void StartActiveLine(Node node)
    {
        Line line = Instantiate(linePrefab);
        line.StartLine(node);
        activeLine = line;
        timestamp = Time.time;
    }

    public void EndActiveLine(Node node)
    {
        activeLine.EndLine(node);
        activeLine = null;
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
        node.Web = this;
    }

    public void RemoveNode(Node node)
    {
        nodes.Remove(node);
    }
}
