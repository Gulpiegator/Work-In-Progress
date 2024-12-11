using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxController : MonoBehaviour{
    public string textContent;

    public void SetText(string text){
        //store our text
        textContent = text;


        //tell our writer to write the text. If we just do the text write on awake the text may not make it before it types it
        TextWriter writer = GetComponentInChildren<TextWriter>();
        if (writer != null){
            writer.WriteText();
        }
    }
}
