using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayer;

    public Pokemon Pokemon { get; set;} // A property to store the created pokemon

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake(){
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(){
        
        Pokemon = new Pokemon(_base, level);

        if (isPlayer){
            image.sprite = Pokemon.Base.BackSprite;
        }
        else {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        image.color = originalColor; // Sets the pokemon to the original color, due to it being transparent
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation(){
        if (isPlayer){
            image.transform.localPosition = new Vector3(originalPos.x -720f, originalPos.y);
        }
        else{
            image.transform.localPosition = new Vector3(originalPos.x + 720f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1f); // smoothly enters a battle
    }

    public void PlayAttackAnimation(){
        var sequence = DOTween.Sequence();
        if (isPlayer){
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayFaintAnimation(){
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y -150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f)); // Makes fainted pokemon invisible
    }
}
