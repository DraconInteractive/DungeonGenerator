using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellularAutomata
{
    public class Cell : MonoBehaviour
    {
        public static List<Cell> all = new List<Cell>();
        public int ID;
        public Renderer r;
        public Material white, black;

        public Cell up, down, left, right;

        public int[] state
        {
            get
            {
                return new int[4] { up.ID, down.ID, left.ID, right.ID };
            }
        }
        private void Awake()
        {
            all.Add(this);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        bool hasNullValues;
        private void Reset()
        {
            r = GetComponent<Renderer>();
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell c in cells)
            {
                if (c.transform.position == transform.position + Vector3.forward)
                {
                    up = c;
                }
                else if (c.transform.position == transform.position + Vector3.back)
                {
                    down = c;
                }
                else if (c.transform.position == transform.position + Vector3.left)
                {
                    left = c;
                }
                else if (c.transform.position == transform.position + Vector3.right)
                {
                    right = c;
                }
            }

            if (up == null || down == null || left == null || right == null)
            {
                hasNullValues = true;
            }
            else
            {
                hasNullValues = false;
            }

            name = "Cell_" + transform.position.ToString();
        }

        private void OnValidate()
        {
            if (GUI.changed)
            {
                hasNullValues = (up == null || down == null || left == null || right == null);
            }
        }
        private void OnDrawGizmos()
        {
            if (hasNullValues)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, 0.1f);
            }
        }

        public void ChangeStateVisible ()
        {
            if (ID == 0)
            {
                r.material = white;
            } else
            {
                r.material = black;
            }
        }
    }
}

