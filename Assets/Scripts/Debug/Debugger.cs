using System.Linq;
using TMPro;
using UnityEngine;

namespace CostumDebug 
{
    public class Debugger : MonoBehaviour
    {
        // The text box the user writes on
        [SerializeField] private TMP_InputField _inputfield = null;
        // Target canvas
        [SerializeField] private Canvas         _canvas = null;
        // Array of A* algorithms
        private GameObject[] astar;
        // Array of Ghosts
        private GameObject[] aIs;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            aIs = GameObject.FindGameObjectsWithTag("GhostEnemy");
            astar = GameObject.FindGameObjectsWithTag("GhostArea");
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                _canvas.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Checks the user string on the text box
        /// </summary>
        public void CheckString()
        {
            string inp = _inputfield.text.ToLower();

            switch (inp)
            {
                case "/help":
                    _inputfield.text = "Commands:\n/help for all commands\n/aipathon to turn on AI pathing\n/aipath to turn off AI pathing";
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

        /// <summary>
        /// Cleans the text box
        /// </summary>
        public void ResetText()
        {
            _inputfield.text = null;
        }

        /// <summary>
        /// Calls the RunDebug on all objects that have an IDebug
        /// </summary>
        /// <param name="todo"> If it should activate or deactivate </param>
        /// <param name="objs"> The list objects to activate the debug </param>
        private void DebugObject(bool todo, GameObject[] objs)
        {
            for (int i = 0; i < objs.Count(); i++)
            {
                objs[i].GetComponent<IDebug>().RunDebug(todo);
            }
        }
    }
}