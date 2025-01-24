using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IntValue")]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] int initialValue;
    [SerializeField] public int value;
    public void OnAfterDeserialize()
    {
        value = initialValue;
        Debug.Log(value);

    }

    public void OnBeforeSerialize()
    {

    }
}
