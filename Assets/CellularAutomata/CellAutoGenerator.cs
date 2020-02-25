using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CellularAutomata
{
    public class CellAutoGenerator : MonoBehaviour
    {
        public float generations;
        public RuleSetHolder rule;
        public bool useSpaceWait;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Generate());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator Generate ()
        {
            foreach (Cell c in Cell.all)
            {
                c.ID = 0;
                float level = Random.value;
                if (level > 0.5f)
                {
                    c.ID = 1;
                }
                c.ChangeStateVisible();
            }
            while (!Input.GetKeyDown(KeyCode.Space) && useSpaceWait)
            {
                yield return null;
            }
            if (generations == 0)
            {
                while (true)
                {
                    foreach (Cell c in Cell.all)
                    {
                        int[] state = c.state;
                        foreach (RuleSet set in rule.rulesets)
                        {
                            if (set.Compare(state))
                            {
                                c.ID = set.result;
                            }
                        }

                        c.ChangeStateVisible();
                    }
                    print("Done");
                    while (!Input.GetKeyDown(KeyCode.Space) && useSpaceWait)
                    {
                        yield return null;
                    }
                    yield return null;
                }
                print("Impossible Done");
            } else
            {
                for (int gen = 0; gen < generations; gen++)
                {
                    foreach (Cell c in Cell.all)
                    {
                        int[] state = c.state;
                        foreach (RuleSet set in rule.rulesets)
                        {
                            if (set.Compare(state))
                            {
                                c.ID = set.result;
                            }
                        }

                        c.ChangeStateVisible();
                    }

                    yield return null;
                }
            }
            
            yield break;
        }
    }
}

