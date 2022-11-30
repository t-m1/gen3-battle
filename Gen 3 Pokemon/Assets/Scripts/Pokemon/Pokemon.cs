using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pokemon
{
    public PokemonBase Base {get; set;} 
    public int Level {get; set;}

    public int HP {get; set;}

    public List<Move> Moves {get; set;}

    public Pokemon(PokemonBase pBase, int pLevel){
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves){
            if (move.Level <= Level){
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 4) break;
            
        }
    }
    
    public int MaxHp {
        get { return Mathf.FloorToInt( (Base.MaxHp * Level) / 100f) + 10; }
    }
    
    public int Att {
        get { return Mathf.FloorToInt(  (Base.Att * Level) / 100f) + 5; }
    }

    public int Def {
        get { return Mathf.FloorToInt(  (Base.Def * Level) / 100f) + 5; }
    }

    public int SpAtt {
        get { return Mathf.FloorToInt(  (Base.SpAtt * Level) / 100f) + 5; }
    }

    public int SpDef {
        get { return Mathf.FloorToInt(  (Base.SpDef * Level) / 100f) + 5; }
    }

    public int Speed {
        get { return Mathf.FloorToInt(  (Base.Speed * Level) / 100f) + 5; }
    }
    // pokemon takes damage, checks for if pokemon fainted
    public DamageDetails TakeDamage(Move move, Pokemon attacker){
        float critical = 1f;
        if (Random.value *  100f <= 6.25f){
            critical = 2f;
        }
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2); // takes both types into account
        var damageDetails = new DamageDetails(){
            Type = type,
            Critical = critical,
            Fainted = false
        };
        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2* attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Att /Def) + 2;
        int damage = Mathf.FloorToInt(d*modifiers);

        HP -= damage;
        if (HP <= 0){
            HP = 0;
           damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove(){
        int r = Random.Range(0, Moves.Count); // Chooses a move for the enemy pokemon
        return Moves[r];
    }

    
}

public class DamageDetails
{
    public bool Fainted { get; set;}
    public float Critical { get; set;}
    public float Type { get; set;}
}
