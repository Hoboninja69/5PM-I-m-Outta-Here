using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public static Vector2 cursorPosition { get { return Input.mousePosition; } }
    public static Ray cursorRay { get { return Camera.main.ScreenPointToRay (Input.mousePosition); } }

    public event Action<Vector2> OnMouseMoved;
    public event Action OnMouseDownLeft;
    public event Action OnMouseUpLeft;
    public event Action OnMouseDownRight;
    public event Action OnMouseUpRight;

    private void Awake ()
    {
        if (Instance != null)
            Destroy (this);
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        Vector2 mouseInput = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
        if (mouseInput.magnitude > 0f)
            OnMouseMoved?.Invoke (mouseInput); 
        
        if (Input.GetMouseButtonDown (0))
            OnMouseDownLeft?.Invoke ();

        if (Input.GetMouseButtonUp (0))
            OnMouseUpLeft?.Invoke ();
        
        if (Input.GetMouseButtonDown (1))
            OnMouseDownRight?.Invoke ();

        if (Input.GetMouseButtonUp (1))
            OnMouseUpRight?.Invoke ();
    }

    public static bool CastFromCursor (out RaycastHit hitInfo, int layerMask = ~0, float maxDistance = Mathf.Infinity, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Physics.Raycast (cursorRay, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
    }
    
    public static bool CastFromCursor (int layerMask = ~0, float maxDistance = Mathf.Infinity, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Physics.Raycast (cursorRay, maxDistance, layerMask, queryTriggerInteraction);
    }
}
