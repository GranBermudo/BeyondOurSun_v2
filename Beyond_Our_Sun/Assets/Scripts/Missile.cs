using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;

    public float speed;
    public float turnRate;
    private Quaternion GuideRotation;

    public bool tracking;

    public ParticleSystem explosion;

    [SerializeField]
    private float trackingDelay;

    public float degats;
    public int layerNumber;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GuideMIssile(); //appeler une fonction qui calcule un vecteur pour faire tourner le missile vers la cible choisie dans la liste des cibles du WeaponSystem

        rb.velocity = transform.forward * speed;    //donner une vitesse au missile (a laquelle est rajout�e celle du vaisseau depuis un autre script)
        
        if(target != null && tracking == true)      //si le missile � une cible et que le timer de guidage est fini
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, GuideRotation, turnRate * Time.deltaTime);    //alors il se tourne vers la cible a une certaine vitesse donn�e par turnRate
        }

        StartCoroutine(AutoDestruction());  //d�marrer le timer d'autodestruction du missile
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerNumber)    //si le GameObject avec lequel le missile entre en collision est sur le layer 9
        {
            other.gameObject.GetComponent<Health>().TakeDammage(degats); //infliger des d�gats par la suite
            Debug.Log(other.gameObject.name + "hit by missile");
            Instantiate(explosion, transform.position, Quaternion.identity);    //faire apparaitre un fx d'explosion
            Destroy(this.gameObject);   //et d�truire le missile
        }
    }

    void GuideMIssile()
    {
        if (target == null) return;
        else
        {
            Vector3 relativePosition = target.position - transform.position;
            GuideRotation = Quaternion.LookRotation(relativePosition, transform.up);
        }
    }

    public void initiateTrackingDelay() //c'est la fonction qu'on appelle depuis le WeaponSystem
    {
        StartCoroutine(TargetTrackingDelay());  //elle demarre un countdown pour le guidage vers la cible
    }

    IEnumerator TargetTrackingDelay() //C'est une coroutine pas une simple fonction regarde l'API si besoin
    {
        yield return new WaitForSeconds(trackingDelay);     //on d�marre le timer trackingDelay
        tracking = true;                                    //quand il arrive a z�ro le missile peut suivre la cible
        speed = speed * 3;                                  //et sa vitesse augmente parce que c'est styl�

    }

    IEnumerator AutoDestruction()   //pour ne pas encombrer la scene d'objets inutiles si le missile n'as pas touch� sa cible au bout de 10 secondes il s'autod�truit
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
