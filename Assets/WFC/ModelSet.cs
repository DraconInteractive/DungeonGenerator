using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(fileName = "Set", menuName = "Model Set", order = 0)]
    public class ModelSet : ScriptableObject
    {
        public List<TileModel> models;
    }
}

