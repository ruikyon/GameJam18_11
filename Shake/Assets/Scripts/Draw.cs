using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {
    [SerializeField]
    private GameObject point_prefab;
    UdpReceiver receiver;
    private int size = 31;
    private GameObject[] points;
    private List<float> positions = new List<float>();

	// Use this for initialization
	void Start () {
        points = new GameObject[size];
        for(int i=0;i<size;i++)
        {
            var temp = Instantiate(point_prefab, new Vector3(-7.5f + (float)15/(size-1)*i, 0), new Quaternion(0, 0, 0, 0));
            points[i] = temp;
        }
        receiver = new UdpReceiver(AccelAction);
        receiver.UdpStart();
	}
	
	// Update is called once per frame
	void Update () {
        if (positions.Count == size)
        {
            for (int i = 0; i < size; i++)
            {
                points[i].transform.position = new Vector3(points[i].transform.position.x, positions[i]);
            }
        }		
	}

    public void AccelAction(float x, float y, float z)
    {
        positions.Add(y*0.5f);
        if(positions.Count > size)positions.RemoveAt(0);
        if (x > y && x > z && x>1) Debug.Log("max: x");
        else if (y > z && y>1) Debug.Log("max: y");
        else if(z>1) Debug.Log("max: z");
    }

    private void OnApplicationQuit()
    {
        receiver.UdpFinish();
    }
}
