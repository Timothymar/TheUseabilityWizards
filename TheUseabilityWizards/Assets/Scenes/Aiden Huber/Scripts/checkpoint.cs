using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class checkpoint : MonoBehaviour
{
    [SerializeField] List<checkpoint> checkPtList;
    checkpoint currCheckPt;
    GameObject player;
    Vector3 checkPtPos;
    Vector3 playerStartPos;

    public bool isActiveCheckPt = false;
    public bool hasPrevCheckPt = false;

    
    private void Start()
    {  
        player = gameManager.instance.player;
        playerStartPos = gameManager.instance.GetPlayerPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPrevCheckPt && !isActiveCheckPt)
        {
            isActiveCheckPt = true;
            hasPrevCheckPt = true;
            currCheckPt = this;
            checkPtList.Add(currCheckPt);
        }
        else if (hasPrevCheckPt && currCheckPt != this && currCheckPt.isActiveCheckPt)
        {
            currCheckPt.isActiveCheckPt = false;
            isActiveCheckPt = true;
            hasPrevCheckPt = true;
            currCheckPt = this;
            checkPtList.Add(currCheckPt);
        }
        else if (hasPrevCheckPt && !isActiveCheckPt)
        {
            return;
        }
    }

    public bool GetActiveValue()
    {
        return isActiveCheckPt;
    }

    public checkpoint GetCheckpoint()
    {
        return currCheckPt;
    }

    public void RespawnLocation()
    {
        player.transform.position = currCheckPt.transform.position;
    }











    //bool cpActive;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //private void ActivateCheckPt()
    //{
    //    foreach(GameObject chkPt in lastCheckPt)
    //    {
    //        chkPt.GetComponent<checkpoint>().cpActive = false;
    //    }

    //    cpActive = true;
    //}

    //public Vector3 GetActiveCheckPt()
    //{
    //    Vector3 lastCheckPt = new Vector3();
    //    if (this.lastCheckPt != null)
    //    {
    //        foreach (GameObject chkPt in this.lastCheckPt)
    //        {
    //            if (chkPt.GetComponent<checkpoint>().cpActive)
    //            {
    //                lastCheckPt = chkPt.transform.position;
    //            }
    //        }
    //    }
    //    return lastCheckPt;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        ActivateCheckPt();
    //    }
    //}
}
