using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int level;
    public bool run = true, clear = false;
    [SerializeField]
    private GameObject[] fields;
    [SerializeField]
    private GameObject final, goal;

	// Use this for initialization
	void Start () {
        instance = this;

        int j = 1;

        for(int i = 0; i < 3; i++)
        {
            Instantiate(fields[i], new Vector3(30 * (j + i), 0), new Quaternion(0, 0, 0, 0));
        }
        j += 3;

        if(GameManager.level >= 2)
        {

            for (int i = 0; i < 3; i++)
            {
                Instantiate(fields[i+3], new Vector3(30 * (j + i), 0), new Quaternion(0, 0, 0, 0));
            }
            j += 3;
        }
        if(GameManager.level == 3)
        {
            final.SetActive(true);
            j++;
        }
        goal.transform.position = new Vector3(j*30, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
