using System.Collections;
using UnityEngine;

public class ExampleHelicopter : MonoBehaviour
{
    [SerializeField] Transform modelTransform;
    [SerializeField] Vector3 startPos;
    [SerializeField] float startDistanceHeight = 0.5f;
    [SerializeField] float maxHeightOffGround = 1f;
    [SerializeField] float timeToMove = 2f;
    [SerializeField] KeyCode raise;
    [SerializeField] KeyCode lower;

    private Vector3 targetPosition;
    private Vector3 targetDirection;

    private void Awake()
    {
        GetDefaultPos();
        SetInitialPos();
    }

    private void Start() 
    {
        StartCoroutine(StartAnimation());
    }

    private void GetDefaultPos()
    {
        targetPosition = modelTransform.localPosition;
    }

    private void SetInitialPos()
    {
        modelTransform.localPosition = startPos;
    }

    private IEnumerator StartAnimation()
    {
        var currentPos = modelTransform.localPosition;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            modelTransform.localPosition = Vector3.Lerp(currentPos, targetPosition, t);
            yield return null;
        }
    }

    private void Update()
    {
        targetDirection = Vector3.zero;
        if (Input.GetKey(raise) == true && modelTransform.localPosition.y < maxHeightOffGround) { ChangeHelicopterHeight(1); }
        if (Input.GetKey(lower) == true && modelTransform.localPosition.y > startPos.y) { ChangeHelicopterHeight(-1); }
    }

    private void ChangeHelicopterHeight(int direction)
    {
        targetDirection.y = direction;
        modelTransform.Translate(targetDirection * timeToMove * Time.deltaTime);
    }
}