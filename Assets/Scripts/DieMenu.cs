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

    Effects[] options1 = new Effects[6] { Effects.AddLife, Effects.AddLife, Effects.AddLife, Effects.AddLife, Effects.AddLife, Effects.AddLife };
    Effects[] options2 = new Effects[6] { Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed, Effects.AddSpeed };
    Effects[] options3 = new Effects[6] { Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate, Effects.AddFireRate };
    // Start is called before the first frame update
    void Start()
    {
        //hide the menu
        //Renderer test = GetComponent<Renderer>();
        //test.enabled = false;
        canvas.enabled = false;

        //initialize the Effects
        effectsMap.Add(Effects.AddLife, ("+1 Life", add_life));
        effectsMap.Add(Effects.AddSpeed, ("+1 Speed", add_speed));
        effectsMap.Add(Effects.AddFireRate, ("+1 FireRate", add_firerate));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch() {
        //throwing the dice !
        //TODO get the throw result
        int die_throw_result = Random.Range(0, 5);
        if (die_throw_result != null) {
            //pause the game
            Time.timeScale = 0.0f;

            //display
            //Renderer test = GetComponent<Renderer>();
            //test.enabled = true;
            canvas.enabled = true;

            //TODO
            //update buttons
            var effect1 = effectsMap[options1[die_throw_result]];
            var effect2 = effectsMap[options2[die_throw_result]];
            var effect3 = effectsMap[options3[die_throw_result]];
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
}

public enum Effects {
    AddLife,
    AddSpeed,
    AddFireRate,
}
