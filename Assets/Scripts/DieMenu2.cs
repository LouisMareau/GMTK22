using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieMenu2 : MonoBehaviour
{
    [SerializeField] public Canvas canvas;

    public TextMeshProUGUI diceResultID;
    public MenuOptionButton button1; 
    public MenuOptionButton button2; 
    public MenuOptionButton button3;

    IDictionary<Effects, (string, System.Action<PlayerData>)> effectsMap = new Dictionary<Effects, (string, System.Action<PlayerData>)>();

    Effects[] options1 = new Effects[6] { Effects.AddLife, Effects.AddProjectile, Effects.AddProjectileSpeed, Effects.AddLife, Effects.AddProjectile, Effects.AddProjectileSpeed };
    Effects[] options2 = new Effects[6] { Effects.AddSpeed, Effects.AddDamage, Effects.AddProjectileRadius, Effects.AddSpeed, Effects.AddDamage, Effects.AddProjectileRadius };
    Effects[] options3 = new Effects[6] { Effects.AddFireRate, Effects.AddFireRate, Effects.AddKnockback, Effects.AddFireRate, Effects.AddFireRate, Effects.AddKnockback };
    // Start is called before the first frame update
    void Start()
    {
        //hide the menu
        canvas.enabled = false;

        //initialize the Effects
        effectsMap.Add(Effects.AddLife, ("+1 Life", add_life));
        effectsMap.Add(Effects.AddSpeed, ("+2 Speed", add_speed));
        effectsMap.Add(Effects.AddFireRate, ("+1 FireRate", add_firerate));

        effectsMap.Add(Effects.AddProjectile, ("+1 Projectile", add_projectile));
        effectsMap.Add(Effects.AddDamage, ("+0.5 Damage", add_damage));

        effectsMap.Add(Effects.AddProjectileSpeed, ("Projectiles are faster", add_projectile_speed));
        effectsMap.Add(Effects.AddProjectileRadius, ("Projectiles are bigger", add_projectile_radius));
        effectsMap.Add(Effects.AddKnockback, ("Adds knockback to projectiles", add_knockback));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(int die_throw_result) {
        //throwing the dice !
        //TODO get the throw result
        if (die_throw_result != -1) {
            //pause the game
            Time.timeScale = 0.0f;

            //display
            //Renderer test = GetComponent<Renderer>();
            //test.enabled = true;
            canvas.enabled = true;

            //TODO
            //update buttons
            diceResultID.text = $"Result { die_throw_result }";
            var effect1 = effectsMap[options1[die_throw_result-1]];
            var effect2 = effectsMap[options2[die_throw_result-1]];
            var effect3 = effectsMap[options3[die_throw_result-1]];
            button1.setup(effect1);
            button2.setup(effect2);
            button3.setup(effect3);
        }
    }

    static void add_life(PlayerData pd) {
        pd.GainLife(1);
    }

    static void add_speed(PlayerData pd) {
        pd.UpdateSpeed(2);
    }

    static void add_firerate(PlayerData pd) {
        pd.UpdateFireRate(1);
    }

    static void add_projectile(PlayerData pd) {
        pd.AddProjectile(1);
    }

    static void add_damage(PlayerData pd) {
        pd.UpdateDamage(0.5f);
    }

    static void add_projectile_speed(PlayerData pd) {
        pd.AddProjectileSpeed(1);
    }

    static void add_projectile_radius(PlayerData pd) {
        pd.AddProjectileRadius(1);
    }

    static void add_knockback(PlayerData pd) {
        pd.AddKnockback(1);
    }
}

public enum Effects {
    //throw 1
    AddLife,
    AddSpeed,
    AddFireRate,
    //throw 2
    AddProjectile,
    AddDamage,
    //throw3
    AddKnockback,
    AddProjectileRadius,
    AddProjectileSpeed,
    //throw4
    


}
