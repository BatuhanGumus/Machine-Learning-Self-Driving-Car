using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackSaver : MonoBehaviour
{
    public TrackBuilder tm;

    public GameObject defaultTrack;

    private void Start()
    {
        OpenTrack(defaultTrack);
    }

    public void OpenTrack(GameObject track)
    {
        if (track == null)
        {
            return;
        }

        Transform parent = Instantiate(track).transform;

        foreach (Transform child in parent)
        {
            Line add = new Line();
            add.holderObject = child.gameObject;
            add.lineType = (LineType)Enum.Parse(typeof(LineType), child.tag);
            add.edges = child.GetComponent<EdgeCollider2D>().points.ToList();

            if (add.lineType == LineType.SpawnLane)
            {
                tm.SetSpawnPos(add);
            }

            add.col = child.GetComponent<EdgeCollider2D>();
            add.lr = child.GetComponent<LineRenderer>();
            tm.lines.Add(add);
        }
    }
}
