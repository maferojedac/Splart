using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Sprite list")]
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private DialogueBox _dialogueBox;
    [SerializeField] private Transform _handPointer;

    [SerializeField] private Spawner _spawner;

    [SerializeField] private string _spawnable; // GARBAGE!!

    [SerializeField] private LevelData _levelData;

    [Header("Sound effects")]
    [SerializeField] private AudioClip _successJingle;
    [SerializeField] private AudioClip _dialogueContinue;
    [SerializeField] private AudioClip _dialogueSplartin;

    private Vector3 pwh_from;
    private Vector3 pwh_to;
    private bool pwh_active;

    void Start()
    {
        _handPointer.gameObject.SetActive(false);
        _spawner.SetComplexity(1);
        StartCoroutine(TutorialSequence());
    }

    void Update()
    {
        
    }

    IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(2.5f);
        _dialogueBox.MakeDialogue("¡Bienvenido a tu primer dia de trabajo!", _sprites[4], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Tu objetivo de ahora en adelante será pintar el mundo", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Será todo un desafío, pero no te preocupes, soy Eli y estoy aquí para ayudarte", _sprites[3], _dialogueContinue);
        yield return new WaitForSeconds(1f);

        // Spawn blue droplet
        _spawner.ForceColor(new ArrayColor(GameColor.Blue));
        // _spawner.AddToQueue(new SpawnableObject(0f, _spawnable.Spawnables[0])); // FIX FIX 

        yield return new WaitForSeconds(1f);
        _dialogueBox.MakeDialogue("Te mostraré como lidiar con ellos", _sprites[0], _dialogueContinue);
        _levelData.SetGlobalSpeedMultiplier(0f);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Es bueno siempre cargar con mis Splartines", _sprites[4], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);

        // Ask player to kill blue droplet
        _dialogueBox.BackgroundDialogue("¡Arrastra hacia arriba con tu dedo el color correcto hacia el enemigo!", _sprites[1], _dialogueSplartin);
        Vector3 palette = GameObject.Find("Color selector").transform.position;
        Vector3 enemy = Entity.GetAll()[0].transform.position;
        enemy = Camera.main.WorldToScreenPoint(enemy);
        StartCoroutine(PointWithHand(palette, enemy));
        yield return new WaitUntil(() => !_spawner.IsStillGenerating());
        _dialogueBox.PlaySound(_successJingle);
        pwh_active = false;

        _dialogueBox.MakeDialogue("¡Eso fue genial!", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Pon atención porque mientras más tiempo sobrevivas, más retos enfrentarás", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("¡Son muy malos!", _sprites[2], _dialogueSplartin);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("¡Pero estaré contigo hasta el final!", _sprites[1], _dialogueSplartin);
        yield return new WaitForSeconds(0.5f);

        // Spawn orange sumo
        GameColor[] colors = { GameColor.Red, GameColor.Yellow};
        _spawner.ForceColor(new ArrayColor(colors));
        _levelData.SetGlobalSpeedMultiplier(1f);
        // _spawner.AddToQueue(new SpawnableObject(0f, _spawnable.Spawnables[1])); // FIX FIX

        _dialogueBox.BackgroundDialogue("Cuando viene un color compuesto, debes descomponerlo para derrotarlo", _sprites[0], _dialogueContinue);

        Vector3 left = GameObject.Find("Red selector").transform.position;
        Vector3 right = GameObject.Find("Black selector").transform.position;
        StartCoroutine(PointWithHand(left, right));

        yield return new WaitForSeconds(1f);
        _levelData.SetGlobalSpeedMultiplier(0f);

        yield return new WaitUntil(() => !_spawner.IsStillGenerating());
        _dialogueBox.PlaySound(_successJingle);
        pwh_active = false;

        _dialogueBox.MakeDialogue("Divide y vencerás", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Debes tener especial cuidado de los magos", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("¡Son escurridizos! ¡Y tiran todo tipo de hechizos!", _sprites[2], _dialogueSplartin);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("¡Pero son demasiado cobardes para atacar!", _sprites[1], _dialogueSplartin);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Son más resistentes que los enemigos comunes, pero impediras que ataquen si atacas primero", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);

        // Generate mage
        _levelData.SetGlobalSpeedMultiplier(1f);
        _dialogueBox.BackgroundDialogue("¡Ahí viene uno!", _sprites[2], _dialogueSplartin);
        //  _spawner.AddToQueue(new SpawnableObject(0f, _spawnable.Spawnables[2])); // FIX FIX 

        StartCoroutine(PointWithHand(left, right));

        yield return new WaitUntil(() => !_spawner.IsStillGenerating());
        _dialogueBox.PlaySound(_successJingle);
        pwh_active = false;

        _dialogueBox.MakeDialogue("Bueno, aún eres un novato y tienes mucho que aprender", _sprites[0], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Pero confio en que podrás convertirte en el mejor artista del mundo", _sprites[4], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("Mientras tanto, yo iré a pintar mi lienzo", _sprites[3], _dialogueContinue);
        yield return new WaitForSeconds(0.5f);
        _dialogueBox.MakeDialogue("¡A la carga!", _sprites[1], _dialogueSplartin);
        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetInt("FirstBoot", 1);
        _levelData.GameOver();
    }

    IEnumerator PointWithHand(Vector3 from, Vector3 to)
    {
        _handPointer.gameObject.SetActive(true);
        pwh_active = true;
        pwh_from = from;
        pwh_to = to;

        while (pwh_active)
        {
            if (Time.time % 2f > 1)
                _handPointer.position = Vector3.Lerp(pwh_from, pwh_to, (Time.time % 2f) - 1);
            else
                _handPointer.position = Vector3.Lerp(pwh_to, pwh_from, Time.time % 2f);
            yield return null;
        }

        _handPointer.gameObject.SetActive(false);
    }

    public void EndTutorial()
    {
        StopAllCoroutines();
        _dialogueBox.CancelDialogue();
    }
}
