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
    public event Action OnMouseStayLeft;
    public event Action OnMouseDownRight;
    public event Action OnMouseUpRight;
    public event Action OnMouseStayRight;
    public event Action<float> OnMouseWheel;

    private bool frozen = false;

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
        
        Cursor.lockState = CursorLockMode.Confined;

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnFreeze += OnFreeze;
            EventManager.Instance.OnUnfreeze += OnUnfreeze;
        }
    }

    private void Update()
    {
        if (frozen) return;

        Vector2 mouseInput = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
        if (mouseInput.magnitude > 0f)
            OnMouseMoved?.Invoke (mouseInput);

        float mouseWheel = Input.GetAxis ("Mouse ScrollWheel");
        if (mouseWheel != 0)
            OnMouseWheel?.Invoke (mouseWheel);

        if (Input.GetMouseButtonDown (0))
            OnMouseDownLeft?.Invoke ();

        if (Input.GetMouseButtonUp (0))
            OnMouseUpLeft?.Invoke ();

        if (Input.GetMouseButton (0))
            OnMouseStayLeft?.Invoke ();
        
        if (Input.GetMouseButtonDown (1))
            OnMouseDownRight?.Invoke ();

        if (Input.GetMouseButtonUp (1))
            OnMouseUpRight?.Invoke ();

        if (Input.GetMouseButton (1))
            OnMouseStayRight?.Invoke ();
    }

    public static bool CastFromCursor (out RaycastHit hitInfo, int layerMask = ~0, float maxDistance = Mathf.Infinity, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Physics.Raycast (cursorRay, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
    }
    
    public static bool CastFromCursor (int layerMask = ~0, float maxDistance = Mathf.Infinity, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Physics.Raycast (cursorRay, maxDistance, layerMask, queryTriggerInteraction);
    }

    private void OnFreeze () => frozen = true;
    //void OnFreeze ()
    //{
    //    frozen = true;
    //    //print ("INPUT FROZEN");
    //}

    private void OnUnfreeze () => frozen = false;
    //void OnUnfreeze ()
    //{
    //    frozen = false;
    //    print ("INPUT UNFROZEN");
    //}
}
