using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEvents : MonoBehaviour
{
    public static InputEvents instance;
    [SerializeField] private InputActionAsset input;

    public Action triggerAction;
    public Action<Vector2> walkAction;
    public Action skillCheckAction;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }


    public void Trigger(InputAction.CallbackContext callback)
    {
        triggerAction?.Invoke();
    }

    public void Walk(InputAction.CallbackContext callback)
    {
        walkAction?.Invoke(callback.ReadValue<Vector2>());
        Debug.Log(callback.ReadValue<Vector2>());
    }

    public void Skillcheck(InputAction.CallbackContext callback)
    {
        skillCheckAction.Invoke();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
