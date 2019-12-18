using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum LineType
{
    Track,
    RewardGate,
    SpawnLane
}

public class Line
{
    public LineType lineType;
    public List<Vector2> edges = new List<Vector2>();
    public LineRenderer lr;
    public EdgeCollider2D col;
    public GameObject holderObject;
}

public class TrackBuilder : MonoBehaviour
{
    [HideInInspector] public Vector3 spawnPos;

    public GameObject lastPointMarker;

    public GameObject linePrefab;

    public Color editingTrackColor;
    public Color doneTrackColor;
    public Color rewardGateColor;
    public Color spawnLaneColor;

    public Color tackLostPointCol;

    private Line lineInHand;
    public List<Line> lines = new List<Line>();

    private GameObject trackParent;

    public Camera cam;

    [HideInInspector] public bool trachBuilt = false;

    private void Start()
    {
        trackParent = new GameObject();
        trackParent.name = "track parent";
    }

    // Update is called once per frame
    void Update()
    {
        if (trachBuilt)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CreateNewTrackLine();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            DoneWithTrack();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoopTrack();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            CreateNewRewardGate();
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            CreateSpawnLane();
        }

        else if (Input.GetMouseButtonDown(0) && IsPointerOverUIObject() == false)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 edgePos = new Vector3(mousePos.x, mousePos.y, 0);
            AddEdgeToLine(edgePos);
        }

        else if (Input.GetMouseButtonDown(1))
        {
            DeleteClickedLine();
        }

        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
    }

    public void CreateNewTrackLine()
    {
        if (lineInHand != null)
        {
            return;
        }

        CreateNewLine();

        lineInHand.lineType = LineType.Track;
        lineInHand.lr.startColor = editingTrackColor;
        lineInHand.lr.endColor = editingTrackColor;
    }

    public void CreateNewRewardGate()
    {
        if (lineInHand != null)
        {
            return;
        }

        CreateNewLine();

        lineInHand.lineType = LineType.SpawnLane;
        lineInHand.lr.startColor = rewardGateColor;
        lineInHand.lr.endColor = rewardGateColor;
        lineInHand.col.isTrigger = true;
        lineInHand.holderObject.tag = "RewardGate";
        lineInHand.holderObject.layer = LayerMask.NameToLayer("RewardGate");

        
    }

    public void CreateSpawnLane()
    {
        if (lineInHand != null)
        {
            return;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].lineType == LineType.SpawnLane)
            {
                Destroy(lines[i].holderObject);
                lines.Remove(lines[i]);
            }
        }

        CreateNewLine();

        lineInHand.lineType = LineType.SpawnLane;
        lineInHand.lr.startColor = spawnLaneColor;
        lineInHand.lr.endColor = spawnLaneColor;
        lineInHand.col.isTrigger = true;
        lineInHand.holderObject.tag = "SpawnLane";
        lineInHand.holderObject.layer = LayerMask.NameToLayer("SpawnLane");
    }

    private void CreateNewLine()
    {
        lineInHand = new Line();

        lineInHand.holderObject = Instantiate(linePrefab);
        lineInHand.holderObject.transform.parent = trackParent.transform;

        lineInHand.lr = lineInHand.holderObject.GetComponent<LineRenderer>();
        lineInHand.col = lineInHand.holderObject.GetComponent<EdgeCollider2D>();
    }

    private void AddEdgeToLine(Vector3 pos)
    {
        if (lineInHand == null) return;

        lineInHand.edges.Add(pos);
        lastPointMarker.transform.position = pos;

        if (lineInHand.edges.Count == 1)
        {
            if (lineInHand.lineType == LineType.Track)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = tackLostPointCol;
            }
            else if (lineInHand.lineType == LineType.RewardGate)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = rewardGateColor;
            }
            else if (lineInHand.lineType == LineType.SpawnLane)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = spawnLaneColor;
            }
            
            lastPointMarker.SetActive(true);
        }

        lineInHand.lr.positionCount = lineInHand.edges.Count;
        lineInHand.lr.SetPosition(lineInHand.edges.Count - 1, pos);
        lineInHand.col.points = lineInHand.edges.ToArray();

        CheckDoneWithLineLimit();
    }

    public void CheckDoneWithLineLimit()
    {
        if (lineInHand.lineType != LineType.Track && lineInHand.edges.Count == 2)
        {
            if (lineInHand.lineType == LineType.SpawnLane)
            {
                SetSpawnPos(lineInHand);
            }

            DoneWithTrack();
        }
    }

    public void LoopTrack()
    {
        if (lineInHand == null || lineInHand.lineType == LineType.RewardGate) return;

        Vector2 pos = new Vector2(lineInHand.edges[0].x + 0.00001f, lineInHand.edges[0].y + 0.00001f);
        lineInHand.edges.Add(pos);
        lineInHand.lr.positionCount = lineInHand.edges.Count;
        lineInHand.lr.SetPosition(lineInHand.edges.Count - 1, pos);
        lineInHand.col.points = lineInHand.edges.ToArray();
        lastPointMarker.transform.position = pos;
    }

    public void Undo()
    {
        if (lineInHand != null)
        {
            if (lineInHand.edges.Count > 0)
            {
                lineInHand.edges.Remove(lineInHand.edges[lineInHand.edges.Count - 1]);
            }
            
            if (lineInHand.edges.Count == 0)
            {
                Destroy(lineInHand.holderObject);
                lineInHand = null;
                lastPointMarker.SetActive(false);
            }
            else
            {
                lineInHand.lr.positionCount = lineInHand.edges.Count;
                lineInHand.col.points = lineInHand.edges.ToArray();

                lastPointMarker.transform.position = lineInHand.edges[lineInHand.edges.Count - 1];
                lastPointMarker.SetActive(true);
            }

        }
        else if (lines.Count > 0)
        {
            lineInHand = lines[lines.Count - 1];
            lines.Remove(lines[lines.Count - 1]);

            if (lineInHand.lineType == LineType.Track)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = tackLostPointCol;
                lineInHand.lr.startColor = editingTrackColor;
                lineInHand.lr.endColor = editingTrackColor;
            }
            else if(lineInHand.lineType == LineType.RewardGate)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = rewardGateColor;
                lineInHand.lr.startColor = rewardGateColor;
                lineInHand.lr.endColor = rewardGateColor;

            }
            else if (lineInHand.lineType == LineType.SpawnLane)
            {
                lastPointMarker.GetComponent<MeshRenderer>().material.color = spawnLaneColor;
                lineInHand.lr.startColor = spawnLaneColor;
                lineInHand.lr.endColor = spawnLaneColor;

            }

            lastPointMarker.transform.position = lineInHand.edges[lineInHand.edges.Count - 1];
            
            lastPointMarker.SetActive(true);

        }
    }

    private void DeleteClickedLine()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(new Vector2(hit.point.x, hit.point.y), 0.1f);
            for (int i = 0; i < cols.Length; i++)
            {
                for (int j = 0; j < lines.Count; j++)
                {
                    if (lines[j].holderObject == cols[i].gameObject)
                    {
                        lines.Remove(lines[j]);
                        Destroy(cols[i].gameObject);
                    }
                }

            }
        }
    }

    public void DoneWithTrack()
    {
        if (lineInHand.edges.Count <= 1)
        {
            Destroy(lineInHand.holderObject);
        }
        else
        {
            if (lineInHand.lineType == LineType.Track)
            {
                lineInHand.lr.startColor = doneTrackColor;
                lineInHand.lr.endColor = doneTrackColor;
            }

            lines.Add(lineInHand);
        }

        lineInHand = null;
        lastPointMarker.SetActive(false);
    }

    public void SetSpawnPos(Line line)
    {
        spawnPos = new Vector2((line.edges[0].x + line.edges[1].x) / 2, (line.edges[0].y + line.edges[1].y) / 2);
    }

    public void ResetTrack()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].holderObject);
        }
        lines.Clear();

        if (lineInHand != null)
        {
            Destroy(lineInHand.holderObject);
            lineInHand = null;
        }

        lastPointMarker.SetActive(false);
    }
    
    private bool IsPointerOverUIObject()//When Touching UI
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void TrackBuilt()
    {
        trachBuilt = true;
    }
}
