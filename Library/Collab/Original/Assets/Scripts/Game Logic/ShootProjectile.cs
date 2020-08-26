using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    // class for all projectile types
    public class Projectile
    {
        public GameObject _prefab;
        public Rigidbody _rigidbody;
        public float speed = 50F;
        public Vector3 chargeIncrement;
        public Rigidbody chargingBody;

        public Vector3 startingScale;
        public Vector3 spawnScale;
        public float customOffset;
        public float growthSpeed;
        public int timeMousePressed;
        public Transform spawnTransform;
        public Transform cameraTransform;

        public Projectile(string prefabPath, float _growthSpeed, Transform _cameraTransform)
        {
            customOffset = 0;

            _prefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;

            growthSpeed = _growthSpeed;
            timeMousePressed = 0;
            startingScale = _prefab.transform.localScale * 0.2f;
            spawnScale = startingScale;
            chargeIncrement = new Vector3(growthSpeed, growthSpeed, growthSpeed);
            // TODO: Normalize charge speeds
            chargeIncrement.x *= startingScale.x;
            chargeIncrement.y *= startingScale.y;
            chargeIncrement.z *= startingScale.z;
            spawnTransform = GameObject.Find("gUN_end").transform;
            cameraTransform = _cameraTransform;

            _rigidbody = _prefab.GetComponent<Rigidbody>();
        }

        public void instantiate()
        {
            //Vector3 offset = customOffset * Vector3.Cross(spawnTransform.forward.normalized, spawnTransform.up.normalized);

            Rigidbody projectile = Instantiate(_rigidbody, spawnTransform.position /*+ offset*/, spawnTransform.rotation);
            projectile.transform.localScale = spawnScale;
            projectile.useGravity = false;
            
            //projectile.transform.position += spawnTransform.forward * 2;

            chargingBody = projectile;
        }

        public void shoot()
        {
            chargingBody.velocity = cameraTransform.forward * speed;
            chargingBody.transform.localScale = new Vector3(spawnScale.x, spawnScale.y, spawnScale.z);
            chargingBody.gameObject.AddComponent<GravityBody>();
        }

        public void updateCharge()
        {
            //scale();
            GameObject temp = new GameObject();
            temp.transform.position = spawnTransform.position;
            temp.transform.rotation = spawnTransform.rotation;
            temp.transform.localScale = spawnTransform.localScale;

            //Vector3 offset = customOffset * Vector3.Cross(spawnTransform.forward.normalized, spawnTransform.up.normalized);
           // temp.transform.position += offset + spawnTransform.forward.normalized*0.5f; 

            chargingBody.transform.position = temp.transform.position;
            chargingBody.transform.rotation = temp.transform.rotation;
            chargingBody.transform.localScale = spawnScale;
        }

        public void scale()
        {
            if (spawnScale.x / startingScale.x <=2)
            {
                spawnScale += chargeIncrement;
            }
        }

        public void reset()
        {
            spawnScale = startingScale;
            timeMousePressed = 0;
        }

        public void setStartScale(Vector3 newScale)
        {
            startingScale = newScale;
            chargeIncrement.x = growthSpeed * startingScale.x;
            chargeIncrement.y = growthSpeed * startingScale.y;
            chargeIncrement.z = growthSpeed * startingScale.z;
        }
    }

    public bool chargeNoise = false;
    public bool chargeSphere = false;
    public bool chargeNumber = false;


    public Transform cameraTransform;

    public Projectile noiseProjectile;
    public Projectile sphereProjectile;
    public Projectile numberBullet;

    public Rigidbody chargingProjectile;

    public Mesh[] numberBulletMeshes;

    public Transform gunEndTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        numberBulletMeshes = new Mesh[10];

        noiseProjectile = new Projectile("Projectiles/ToonProjectile", 0.1f, cameraTransform);
        sphereProjectile = new Projectile("Projectiles/SphereProjectile", 0.1f, cameraTransform);
        numberBullet = new Projectile("Projectiles/NumberBullet", 0.1f, cameraTransform);

        // fix flipped numbers
        numberBullet.startingScale.x *= 1;
        numberBullet.setStartScale(numberBullet.startingScale);
        numberBullet.customOffset =0f; // center the number 

        for (int i = 0; i < numberBulletMeshes.Length; i++)
        {
            string filepath = "StringCharacters/Impact_" + i.ToString();
            GameObject import = Resources.Load(filepath, typeof(GameObject)) as GameObject;
            numberBulletMeshes[i] = import.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) // Charge Noise Bullet
        {
            chargeNoise = true;
            noiseProjectile.reset();
            noiseProjectile.instantiate();
        }
        else if (Input.GetMouseButtonUp(0)) // Release Noise Bullet
        {
            chargeNoise = false;
            noiseProjectile.shoot();
        }
        if (Input.GetMouseButtonDown(1)) // Charge Sphere Bullet
        {
            chargeSphere = true;
            sphereProjectile.reset();
            sphereProjectile.instantiate();
        }
        else if (Input.GetMouseButtonUp(1)) // Release Sphere Bullet
        {
            chargeSphere = false;
            sphereProjectile.shoot();
        }
        if (Input.GetMouseButtonDown(2)) // Charge Number Bullet
        {

            chargeNumber = true;
            numberBullet.reset();
            numberBullet.instantiate();
        }
        else if (Input.GetMouseButtonUp(2)) // Release Number Bullet
        {
            chargeNumber = false;
            numberBullet.shoot();
        }

        if (chargeNoise)
            noiseProjectile.updateCharge();
        if (chargeSphere)
            sphereProjectile.updateCharge();
        if (chargeNumber)
            numberBullet.updateCharge();

    }

    void FixedUpdate()
    {
        if (chargeNoise)
        {
            noiseProjectile.scale();
        }
        if (chargeSphere)
        {
            sphereProjectile.scale();
        }
        if (chargeNumber)
        {
            numberBullet.scale();
            if ( numberBullet.timeMousePressed % 10 == 0)
            {
                int numberChargeBonus = Mathf.RoundToInt(numberBullet.timeMousePressed / 8);
                if (numberChargeBonus > 7)
                {
                    numberChargeBonus = 7;
                }
                numberBullet.chargingBody.gameObject.GetComponent<MeshFilter>().mesh = numberBulletMeshes[Random.Range(0, 3) + numberChargeBonus];
            }
            numberBullet.timeMousePressed += 1;
        }
    }
}
