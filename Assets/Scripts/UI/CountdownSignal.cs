using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSignal : MonoBehaviour
{
    [SerializeField] private Text displayComponent;

    private Dictionary<int, string> mapping = new Dictionary<int, string>() {
        {3, "Ready"},
        {2, "Set"},
        {1, "Go"}
    };

    public void Toggle(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }

    public void UpdateValue(int value)
    {
        if(mapping.ContainsKey(value))
        {
            this.displayComponent.text = mapping[value];
        }
        else
        {
            this.displayComponent.text = "Ready";
        }
    }
}
