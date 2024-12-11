using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using UnityEngine;
using TMPro;
using System.Collections;

public class TextWriter : MonoBehaviour
{
    [SerializeField] private GameObject parentObject; //store the canvas
    public float letterDelay = 0.05f; //delay between writing each letter
    private TMP_Text textMeshPro; //the text that we are editing

    private void Awake(){
        textMeshPro = GetComponent<TMP_Text>(); // grab our text box
    }

    public void WriteText(){
        //check for any missing components before we show text
        if (parentObject != null){
            TextBoxController textBoxController = parentObject.GetComponent<TextBoxController>();
            if (textBoxController != null){
                StartCoroutine(TypeText(textBoxController.textContent));
            }
        }
        else{
            Debug.LogWarning("missing parent object on textWriter");
        }
    }

    //loops through our string adding one letter at a time to give a typed effect
    private IEnumerator TypeText(string text){
        textMeshPro.text = "";
        foreach (char c in text){
            textMeshPro.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}

