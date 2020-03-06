using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Retrieved from https://github.com/tutsplus/3d-dungeons-with-procedural-recipes
//Modified by Peter Carey, 14/02/2020
public class ModularWorldGenerator : MonoBehaviour
{
	public Module[] Modules;
	public Module StartModule;

	public int Iterations = 5;


	void Start()
	{
        StartCoroutine(Generate());
    }

    IEnumerator Generate ()
    {
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<ModuleConnector>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(Modules, newTag);
                var newModule = (Module)Instantiate(newModulePrefab);
                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);
                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                yield return null;
            }

            pendingExits = newExits;
            yield return null;
        }
        yield break;
    }

	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}
