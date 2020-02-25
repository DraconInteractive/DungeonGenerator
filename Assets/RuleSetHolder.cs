using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellularAutomata
{
    [CreateAssetMenu(fileName = "Ruleset", menuName = "Rule Set", order = 0)]
    public class RuleSetHolder : ScriptableObject
    {
        public RuleSet[] rulesets;

        
    }

    [System.Serializable]
    public class RuleSet
    {
        public string name;
        public int[] _IDs = new int[4];
        public int result;

        public bool Compare (int[] values)
        {
            if (values.Length != 4)
            {
                Debug.Log("Invalid entry, " + values.Length + " values detected");
                return false;
            }

            bool pass = true;
            
            for (int i = 0; i < 4; i++)
            {
                if (values[i] != _IDs[i])
                {
                    pass = false;
                    break;
                }
            }

            return pass;
        }
    }
}

