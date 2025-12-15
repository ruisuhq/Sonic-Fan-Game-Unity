//Para mover plataformas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_movimiento : MonoBehaviour {

public Transform target;
public float speed;

private Vector3 start, end;

// Use this for initialization
void Start () {
if(target != null) {
target.parent = null;
start = transform.position;
end = target.position;
}
}

// Update is called once per frame
void Update () {

}

void FixedUpdate(){
if (target != null) {
float fixedSpeed = speed * Time.deltaTime;
transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);
}

if (transform.position == target.position){
target.position = (target.position == start) ? end : start;
}
}

private void OnCollisionEnter2D(Collision2D other){
    if (other.gameObject.CompareTag("Sonic")){
        other.transform.SetParent(this.transform);
    }
}
private void OnCollisionExit2D(Collision2D other){
    if (other.gameObject.CompareTag("Sonic")){
        other.transform.SetParent(null);
    }
}
}