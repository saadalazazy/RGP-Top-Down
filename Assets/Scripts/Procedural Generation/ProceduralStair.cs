using UnityEngine;

[ExecuteAlways] // Makes it run in Edit mode
public class ProceduralStair : MonoBehaviour
{
    [Header("Stair Settings")]
    [Tooltip("Width of the central stair piece.")]
    [SerializeField] private float stairWidth = 1f;

    [Tooltip("Horizontal offset applied to left and right stairs.")]
    [SerializeField] private float offset = -1.2f;

    [Header("Side Stairs Toggle")]
    [Tooltip("Enable the left stair.")]
    [SerializeField] private bool left = true;

    [Tooltip("Enable the right stair.")]
    [SerializeField] private bool right = true;

    [Header("Stair References")]
    [SerializeField] private GameObject stair;
    [SerializeField] private GameObject leftStairs;
    [SerializeField] private GameObject rightStairs;

    private void OnEnable()
    {
        ApplySettings();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplySettings();
    }
#endif

    private void ApplySettings()
    {
        if (stair != null)
            stair.transform.localScale = new Vector3(stairWidth, 1, 1);

        if (leftStairs != null)
        {
            leftStairs.SetActive(left);
            if (left)
                leftStairs.transform.localPosition = new Vector3(stairWidth + offset, 0, 0);
        }

        if (rightStairs != null)
        {
            rightStairs.SetActive(right);
            if (right)
                rightStairs.transform.localPosition = new Vector3(-(stairWidth + offset), 0, 0);
        }
    }
}
