using System.Linq;
using TMPro;
using UnityEngine;
public class Debugger : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfield = null;
    [SerializeField] private Canvas canvas = null;
    private GameObject[] astar;
    private GameObject[] aIs;

    // Start is called before the first frame update
    void Start()
    {
        aIs = GameObject.FindGameObjectsWithTag("GhostEnemy");
        astar = GameObject.FindGameObjectsWithTag("GhostArea");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            canvas.gameObject.SetActive(true);
        }
    }
    public void CheckString()
    {
        string inp = inputfield.text.ToLower();

        switch (inp)
        {
            case "/help":
                inputfield.text = "Commands:\n/help for all commands\n/aipathon to turn on AI pathing\n/aipath to turn off AI pathing";
                break;
            case "/aipathon":
                DebugObject(true, aIs); ResetText();
                break;
            case "/aipathoff":
                DebugObject(false, aIs); ResetText();
                break;
            case "/aiareaon":
                DebugObject(true, astar); ResetText();
                break;
            case "/aiareaoff":
                DebugObject(false, astar); ResetText();
                break;
        }
    }
    public void ResetText()
    {
        inputfield.text = null;
    }

    private void DebugObject(bool todo, GameObject[] objs)
    {
        for (int i = 0; i < objs.Count(); i++)
        {
            objs[i].GetComponent<IDebug>().RunDebug(todo);
        }
    }
}
