using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class checkpoint : MonoBehaviour
{
    [SerializeField] List<GameObject> checkPts;

    bool cpActive;

    // Start is called before the first frame update
    void Start()
    {
        checkPts = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
    }

    private void ActivateCheckPt()
    {
        foreach(GameObject chkPt in checkPts)
        {
            chkPt.GetComponent<checkpoint>().cpActive = false;
        }

        cpActive = true;
    }

    public Vector3 GetActiveCheckPt()
    {
        Vector3 lastCheckPt = new Vector3();
        if (checkPts != null)
        {
            foreach (GameObject chkPt in checkPts)
            {
                if (chkPt.GetComponent<checkpoint>().cpActive)
                {
                    lastCheckPt = chkPt.transform.position;
                }
            }
        }
        return lastCheckPt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ActivateCheckPt();
        }
    }
}
