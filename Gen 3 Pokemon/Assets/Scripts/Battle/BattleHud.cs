using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;
    Pokemon _pokemon;
    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp); //Normalize the current HP of the pokemon; also float is needed for scaling
    }

    public IEnumerator UpdateHP(){
         yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp); // adds yield return in front because its a coroutine
    }
}
