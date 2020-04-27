using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class Debugger : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfield = null;
    [SerializeField] private Canvas canvas = null;
    List<GameObject> aIs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        aIs = GameObject.FindGameObjectsWithTag("GhostEnemy").ToList();
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
            case "/help": inputfield.text = "Commands:\n/help for all commands\n/aipathon to turn on AI pathing\n/aipath to turn off AI pathing";
                break;
            case "/aipathon": ShowAIPath(true); inputfield.text = null;
                break;
            case "/aipathoff": ShowAIPath(false); inputfield.text = null;
                break;
        }
    }
    public void ResetText()
    {
        inputfield.text = null;
    }

    private void ShowAIPath(bool todo)
    {
        for (int i = 0; i < aIs.Count; i++)
        {
            aIs[i].GetComponent<AIMovement>().SetupLine(todo);
        }
    }
}
