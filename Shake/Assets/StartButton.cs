using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartButton : MonoBehaviour
{
    public Button[] difficulty = new Button[3];

    private ColorBlock colors = ColorBlock.defaultColorBlock;
    private int line = 0;
    private float preUd = 0;

    // Use this for initialization
    void Start()
    {
        colors.normalColor = Color.blue;
        difficulty[0].colors = colors;
        difficulty[1].colors = ColorBlock.defaultColorBlock;
        difficulty[2].colors = ColorBlock.defaultColorBlock;
    }

    // Update is called once per frame
    void Update()
    {
        float ud = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("load scene");
            GameManager.level = line+1;
            SceneManager.LoadScene("ExperimentScene");
        }
        if (ud == preUd) return;
        if (ud == -1 && line != 2)
        {
            difficulty[line + 1].colors = colors;
            difficulty[line].colors = ColorBlock.defaultColorBlock;
            line++;
        }
        else if (ud == 1 && line != 0)
        {
            difficulty[line - 1].colors = colors;
            difficulty[line].colors = ColorBlock.defaultColorBlock;
            line--;
        }
        preUd = ud;
    }
}
