using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatrolPoints : MonoBehaviour
{
    [Range (0f, 2f)]
    [SerializeField] private float waypointSize = 1f;

    [Header("Patrol Settings")]
    public bool isLooping = false;
    public bool isReverse = false;
    private bool isMovingForward = true;

    private void OnDrawGizmos()
    {
        if (transform.childCount != 0)
        {
            foreach (Transform t in transform)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(t.position, waypointSize); // Use t.position directly
            }

            Gizmos.color = Color.red;

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position); // Use t.position directly
            }

            // Connect the last waypoint back to the first if isLooping is true
            if (isLooping && transform.childCount > 1)
            {
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position); // Use t.position directly
            }
        }
    }

    public Transform GetNextWaypoint(Transform waypoint)
    { 
        if (waypoint == null)
        {
            return transform.GetChild(0);
        }

        int nextIndex;

        if (isReverse)
        {
            if (isMovingForward)
            {
                nextIndex = waypoint.GetSiblingIndex() + 1;
                if (nextIndex >= transform.childCount)
                {
                    isMovingForward = false;
                    nextIndex -= 2; // Go back to the previous waypoint
                }
            }
            else
            {
                nextIndex = waypoint.GetSiblingIndex() - 1;
                if (nextIndex < 0)
                {
                    isMovingForward = true;
                    nextIndex = 1; // Move forward again from the second waypoint
                }
            }
        }
        else if (isLooping)
        {
            nextIndex = waypoint.GetSiblingIndex() + 1;
            if (nextIndex >= transform.childCount)
            {
                nextIndex = 0; // Loop back to the start
            }
        }
        else
        {
            nextIndex = waypoint.GetSiblingIndex() + 1;
            if (nextIndex >= transform.childCount)
            {
                return null; // No next waypoint if not looping or reversing
            }
        }

        return transform.GetChild(nextIndex);
    }

    public void RemoveLastWaypoint()
    {
        if(transform.childCount > 1)
        {
            DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
        }
        else Debug.Log("Can't destroy starting position");
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PatrolPoints))]
public class WaypointsListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var thisTarget = (PatrolPoints)target;

        if (thisTarget == null) return;

        DrawDefaultInspector();

        // Ensure only one of the booleans can be checked at a time
        if (thisTarget.isLooping && thisTarget.isReverse)
        {
            EditorGUILayout.HelpBox("Only one of 'isLooping' or 'isReverse' can be checked at a time.", MessageType.Error);
            thisTarget.isReverse = false;  // Turn off isReverse if both are checked
        }

        // Button for creating a new waypoint
        if (GUILayout.Button("Create New Waypoint"))
        {
            GameObject waypoint = new("Waypoint " + (thisTarget.transform.childCount + 1));
            waypoint.transform.parent = thisTarget.transform;
            waypoint.transform.position = Vector3.zero; // Or provide some other default position
        }

        // Button for removing the last waypoint
        if (GUILayout.Button("Remove Last Waypoint"))
        {
            thisTarget.RemoveLastWaypoint();
        }
    }
}
#endif
