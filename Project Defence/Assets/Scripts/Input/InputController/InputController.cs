using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewInputSystem;

public class InputController : MonoBehaviour
{
    private static InputController instance;
    public static InputController Instance => instance;
    private Inputs inputs;
    public Inputs Inputs => inputs;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        inputs=new Inputs();
        inputs.Gameplay.Enable();
    }

}
