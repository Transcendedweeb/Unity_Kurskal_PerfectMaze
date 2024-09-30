using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    [HideInInspector] public int inputValue;

    void Start()
    {
        // the listener starts at editing the input field
        // when it loses focus the ValidateInputTMP function is called
        inputField.onEndEdit.AddListener(ValidateInputTMP);
        inputValue = 30;
    }

    public void ValidateInputTMP(string input)
    {
        if (int.TryParse(input, out int parsedValue)) // attempts to convert the input string to an integer
        {
            if (parsedValue < 10 || parsedValue > 250)  // if its outside the range return the value to default 30
            {
                inputValue = 30;
                inputField.text = "30";
            }
            else inputValue = parsedValue;
        }
    }
}
