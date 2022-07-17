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
        //pause the game
        Time.timeScale = 0f;

        //display
        //Renderer test = GetComponent<Renderer>();
        //test.enabled = true;
        canvas.enabled = true;

        //set up button options

        //throwing the dice !
        int die_throw_result = Random.Range(0, 5);


        //TODO
        //update buttons
        var effect = effectsMap[options1[die_throw_result]];
        button1.setText(effect.Item1);
        button1.setAction(effect.Item2);

        
    }

    static void add_life(PlayerData pd) {
        pd.GainLife(1);
    }

    static void add_speed(PlayerData pd) {
        
    }

    static void add_firerate(PlayerData pd) {
        
    }
}

public enum Effects {
    AddLife,
    AddSpeed,
    AddFireRate,
}
