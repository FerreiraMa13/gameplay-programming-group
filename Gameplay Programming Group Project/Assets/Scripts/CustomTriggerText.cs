using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomTriggerText : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;

    [System.NonSerialized] public bool valid = true;

    private void Awake()
    {
        if(texts == null || texts.Count < 1)
        {
            valid = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(valid && other.tag == "Player")
        {
            if (CheckConditions())
            {
                foreach (TextMeshProUGUI text in texts)
                {
                    text.enabled = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (valid && other.tag == "Player")
        {
            if (CheckConditions())
            {
                foreach (TextMeshProUGUI text in texts)
                {
                    text.enabled = false;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !valid)
        {
            DisableAll();
        }
    }
    private bool CheckConditions()
    {
        var conditions = GetComponents<Condition>();
        if (conditions != null)
        {
            foreach (Condition condition in conditions)
            {
                if (!condition.CheckCondition())
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void DisableAll()
    {
        valid = false;
        foreach (TextMeshProUGUI text in texts)
        {
            text.enabled = false;
        }
    }
    private void OnDestroy()
    {
        DisableAll();
    }
    private void OnDisable()
    {
        DisableAll();
    }
}
