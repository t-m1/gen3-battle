using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;
    public event Action<bool> OnBattleOver;
    BattleState state;
    int currentAction;
    int currentMove;

    public void StartBattle(){
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle(){
        playerUnit.Setup(); // Setup is from Battle Unit
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon); // SetData from Battle Hud
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogueBox.SetMoveNames(playerUnit.Pokemon.Moves);
        yield return dialogueBox.TypeDialogue($"A wild {enemyUnit.Pokemon.Base.Name} appeared!"); // adds the name of the pokemon encountered; inside a coroutine, you can use yield return to execute another coroutine
        //yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction(){
        state = BattleState.PlayerAction; // sets to player action state
        StartCoroutine(dialogueBox.TypeDialogue("Choose an action"));
        dialogueBox.EnableActionSelector(true); // Enables action selector;
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogueBox.EnableActionSelector(false); //removes the action selector 
        dialogueBox.EnableDialogueText(false); // Removes Dialogue text for fight/run
        dialogueBox.EnableMoveSelector(true); // Allows you to select moves
    }
    IEnumerator PerformPlayerMove(){
        state = BattleState.Busy;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        //yield return new WaitForSeconds(1f);
        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon); // If the HP of the enemy pokemon is less than 0
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted){
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} Fainted");
            enemyUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else{
            StartCoroutine(EnemyMove());
        }
    }
    IEnumerator EnemyMove(){ // coroutine
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        //yield return new WaitForSeconds(1f);
        var damageDetails = playerUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon); // If the HP of the enemy pokemon is less than 0
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted){
            yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.Base.Name} Fainted");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else{
            PlayerAction(); // Player can choose again if pokemon not fainted
        }
    }
    // Prints out dialogue if critical hit; super effective; etc
    IEnumerator ShowDamageDetails(DamageDetails damageDetails){
        if (damageDetails.Critical > 1f){
            yield return dialogueBox.TypeDialogue("A critical hit!");
        }

        if (damageDetails.Type > 1f){
            yield return dialogueBox.TypeDialogue("It's super effective!");
        }
        else if (damageDetails.Type < 1f){
            yield return dialogueBox.TypeDialogue("It's not very effective.");
        }
    }
    //Updates where we are in the UI, and what we can select
    public void HandleUpdate(){
        if (state == BattleState.PlayerAction){
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove){
            HandleMoveSelection();
        }
    }

    void HandleActionSelection(){
        if (Input.GetKeyDown(KeyCode.DownArrow)){ // If down arrow key is pressed
            if (currentAction < 1){
                currentAction++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentAction > 0){
                currentAction--;
            }
        }

        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z)){
            if (currentAction == 0){ // Fight is selected
                PlayerMove();
            }
            else if (currentAction == 1){ // Run is selected

            }
        }
    }

    void HandleMoveSelection(){
        if (Input.GetKeyDown(KeyCode.RightArrow)){ // If down arrow key is pressed
            if (currentMove < playerUnit.Pokemon.Moves.Count-1){
                currentMove++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currentMove > 0){
                currentMove--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){ // If down arrow key is pressed
            if (currentMove < playerUnit.Pokemon.Moves.Count-2){ // Increments the current move by two to go down
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){ 
            if (currentMove > 1){                       // Decreases the current move by two to go up
                currentMove -= 2;
            }
        }

        dialogueBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]); // gets a reference of the current move
        if (Input.GetKeyDown(KeyCode.Z)){
            dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableDialogueText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }

    

}
