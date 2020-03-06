using System.Linq;
using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

    public ModuleConnector GetClosestExit (Vector3 pos)
    {
        return GetExits().OrderByDescending(element => Vector3.Distance(pos, element.transform.position)).FirstOrDefault();
    }
}
