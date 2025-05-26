using UnityEditor;
using UnityEngine;
using System;
using Eflatun.SceneReference;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    public SceneReference To;
    [Range(-1,1)] public int ExitDirection;
    private LevelManager levelManager;
    public bool isActive = true;

    private void Awake() {
        StartCoroutine(DisableForSeconds(.5f));
    }

    private IEnumerator DisableForSeconds(float time) {
        isActive = false;
        yield return new WaitForSeconds(time);
        isActive = true;
    }

    private void Start() {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isActive) return;
        levelManager.TransitionToLevel(gameObject.scene.name, To.Name, ExitDirection);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isActive = true;
    }
}
