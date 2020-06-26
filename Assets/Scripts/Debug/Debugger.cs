using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CostumDebug
{
    public class Debugger : MonoBehaviour
    {
        // The text box the user writes on
        [SerializeField] private TMP_InputField _inputfield = null;
        // Target canvas
        [SerializeField] private Canvas _canvas = null;
        // Array of A* algorithms
        private GameObject[] _astar;
        // Array of Ghosts
        private GameObject[] _aIs;
        // Array of Doors
        private LiftDoor[] _doors;
        // The player on the game
        private GameObject _player;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _aIs = GameObject.FindGameObjectsWithTag("GhostEnemy");
            _astar = GameObject.FindGameObjectsWithTag("GhostArea");
            _player = GameObject.FindGameObjectWithTag("Player");
            _doors = FindObjectsOfType<LiftDoor>();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backslash)
                && !_canvas.gameObject.activeSelf)
            {
                _canvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetKeyDown(KeyCode.Backslash)
                && _canvas.gameObject.activeSelf)
            {
                _canvas.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
                    _inputfield.text = "Commands:\n" +
                        "/help for all commands\n" +
                        "/aipathon to turn on AI pathing\n" +
                        "/aipath to turn off AI pathing\n" +
                        "/reloadlvl to start again";
                    break;

                case "/aipathon":
                    DebugObject(true, _aIs); ResetText();
                    break;

                case "/aipathoff":
                    DebugObject(false, _aIs); ResetText();
                    break;

                case "/aiareaon":
                    DebugObject(true, _astar); ResetText();
                    break;

                case "/aiareaoff":
                    DebugObject(false, _astar); ResetText();
                    break;

                case "/openalldoors":
                    OpenAllDoors(); ResetText();
                    break;

                case "/reloadlvl":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    ResetText();
                    break;
            }

            if (inp.Contains("/addhp") && Input.GetKey(KeyCode.Return))
            {
                string[] a = inp.Split(' ');

                int.TryParse(a[1], out int amount);

                _player.GetComponent<IEntity>().Heal(amount);

                ResetText();
            }
        }

        /// <summary>
        /// Cleans the text box
        /// </summary>
        public void ResetText()
        {
            _inputfield.text = null;
        }

        private void OpenAllDoors()
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                if (_doors[i].gameObject.layer != LayerMask.NameToLayer("Debug"))
                {
                    _doors[i].ActivatePuzzlePiece(true, 1);
                }
            }
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
                if (objs[i] == null) continue;

                objs[i].GetComponent<IDebug>().RunDebug(todo);
            }
        }
    }
}