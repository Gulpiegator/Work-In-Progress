using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTriggerController : MonoBehaviour{
    public string displayText; //text we want to write out
    public GameObject textPrefab; //the UI canvas
    public bool isTriggered = false; //so we dont get multiple triggers
    public float triggerDelay = 0.5f; //so we can control when it appears
    public float textDuration = 5f; //control how long it appears
    public int stageSet = -1; //set player stage to this
    private GameObject triggeringPlayer;

    private void OnTriggerEnter(Collider other){
        //only triggered by player
        if (isTriggered || other.CompareTag("Player") == false) return;

        isTriggered = true;
        triggeringPlayer = other.gameObject;
        StartCoroutine(TriggerTextDisplay());
    }

    private IEnumerator TriggerTextDisplay(){
        //wait for the delay
        yield return new WaitForSeconds(triggerDelay);
        //create the UI
        GameObject textInstance = Instantiate(textPrefab, this.transform);
        TextBoxController textBoxController = textInstance.GetComponent<TextBoxController>();
        if (textBoxController != null){
            //send the UI our text
            textBoxController.SetText(displayText);
        }
        //wait to destroy
        yield return new WaitForSeconds(textDuration);

        // Set the player's stage if stageSet is not -1
        if (stageSet != -1){
            PlayerMovement playerController = triggeringPlayer.GetComponent<PlayerMovement>();
            playerController.stage = stageSet;
        }

        Destroy(textInstance);
    }
}
