using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] Color highlightedColor;
    [SerializeField] Text dialogueText; //We use SerializeField so we can see the variable in Inspector and easily change it
    [SerializeField] int lettersPerSecond;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;


    public void SetDialogue(string dialogue){
        dialogueText.text = dialogue;
    }

    //Prints out the text slowly by animating each letter one by one
    public IEnumerator TypeDialogue(string dialogue){
        dialogueText.text = "";
        foreach(var letter in dialogue.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/30);
        }
        yield return new WaitForSeconds(1f);
    }
    //enables/disables dialgoue texts + selectors
    public void EnableDialogueText(bool enabled){
        dialogueText.enabled = enabled; //has property called enabled; assigns boolean to it
    }

    public void EnableActionSelector(bool enabled){
        actionSelector.SetActive(enabled); // has SetActive function because its a game object
    }

    public void EnableMoveSelector(bool enabled){
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction){
        for (int i = 0; i < actionTexts.Count; i++){
            if (i == selectedAction){
                actionTexts[i].color = highlightedColor; // Highlights which action option you are on
            }
            else{
                actionTexts[i].color = Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move){
        for (int i = 0; i < moveTexts.Count; i++){
            if (i == selectedMove){
                moveTexts[i].color = highlightedColor; // Highlights which move option you are on
            }
            else{
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP {move.PP}/{move.Base.PP}"; // shows both PP of the move and how much has been used
        typeText.text = move.Base.Type.ToString(); // is an enum so convert to string
    }
    //Sets the name of the moves
    public void SetMoveNames(List<Move> moves){
        for (int i = 0; i < moveTexts.Count; i++){
            if (i < moves.Count){
                moveTexts[i].text = moves[i].Base.Name;
            }
            else{
                moveTexts[i].text = "-";
            }
        }
    }

}
