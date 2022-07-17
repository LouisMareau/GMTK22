using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMenu : MonoBehaviour
{
    [SerializeField] public Canvas canvas;

    public MenuOptionButton button1; 
    public MenuOptionButton button2; 
    public MenuOptionButton button3;

    IDictionary<Effects, (string, System.Action<PlayerData>)> effectsMap = new Dictionary<Effects, (string, System.Action<PlayerData>)>();

    Effects[] options1 = new Effects[6] { Effects.AddLife, Effects.AddProjectile, Effects.AddLife, Effects.AddLife, Effects.AddLife, Effects.AddLife };
    Effects[] options2 = new Effects[6] { Effects.AddSpeed, Effects.AddDamage, Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed };
    Effects[] options3 = new Effects[6] { Effects.AddFireRate, Effects.AddJump, Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate };
    // Start is called before the first frame update
    void Start()
    {
        //hide the menu
        canvas.enabled = false;

        //initialize the Effects
        effectsMap.Add(Effects.AddLife, ("+1 Life", add_life));
        effectsMap.Add(Effects.AddSpeed, ("+1 Speed", add_speed));
        effectsMap.Add(Effects.AddFireRate, ("+1 FireRate", add_firerate));

        effectsMap.Add(Effects.AddProjectile, ("+1 Projectile", add_projectile));
        effectsMap.Add(Effects.AddDamage, ("+1 Damage", add_damage));
        effectsMap.Add(Effects.AddJump, ("+1 Jump", add_jump));

        effectsMap.Add(Effects.AddProjectileSpeed, ("+1 Projectile Speed", add_projectile_speed));
        effectsMap.Add(Effects.AddProjectileRadius, ("+1 ProjectileRadius", add_projectile_radius));
        effectsMap.Add(Effects.AddKnockback, ("+1 Knockback", add_knockback));
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
            var effect1 = effectsMap[options1[die_throw_result-1]];
            var effect2 = effectsMap[options2[die_throw_result-1]];
            var effect3 = effectsMap[options3[die_throw_result-1]];
            button1.setup(effect1, die_throw_result);
            button2.setup(effect2, die_throw_result);
            button3.setup(effect3, die_throw_result);
        }
    }

    static void add_life(PlayerData pd) {
        pd.GainLife(1);
    }

    static void add_speed(PlayerData pd) {
        pd.UpdateSpeed(1);
    }

    static void add_firerate(PlayerData pd) {
        pd.UpdateFireRate(1);
    }

    static void add_projectile(PlayerData pd) {
        pd.AddProjectile(1);
    }

    static void add_damage(PlayerData pd) {
        pd.UpdateDamage(1);
    }

    static void add_jump(PlayerData pd) {
        pd.AddJump(1);
    }

    static void add_projectile_speed(PlayerData pd) {
    }

    static void add_projectile_radius(PlayerData pd) {
    }

    static void add_knockback(PlayerData pd) {
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
    AddJump,
    //throw3
    AddKnockback,
    AddProjectileRadius,
    AddProjectileSpeed,
    //throw4
    


}
