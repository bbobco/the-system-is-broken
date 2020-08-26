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
            spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
            cameraTransform = _cameraTransform;

            _rigidbody = _prefab.GetComponent<Rigidbody>();
        }

        public void instantiate()
        {
            //Vector3 offset = customOffset * Vector3.Cross(spawnTransform.forward.normalized, spawnTransform.up.normalized);
            Rigidbody projectile = Instantiate(_rigidbody, spawnTransform.position /*+ offset*/, spawnTransform.rotation);
            projectile.transform.localScale = spawnScale;
            projectile.useGravity = false;

            // set dissolve uniform
            float t = Time.time;
            projectile.gameObject.GetComponent<Renderer>().material.SetFloat("_TimeOffset", t);

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

    private bool chargeNoise = false;
    private bool chargeFractal = false;
    private bool chargeNumber = false;
    private bool chargePlatonic = false;
    public int currentNumber;
    private int chargeTime;


    public Transform cameraTransform;

    private Projectile noiseProjectile;
    private Projectile fractalProjectile;
    private Projectile numberBullet;
    private Projectile timaeusProjectile;

    public Rigidbody chargingProjectile;

    public Mesh[] numberBulletMeshes;

    private GameObject player;
    private AudioSource[] playerSounds;
    private AudioSource noiseProjectileSound;
    private AudioSource noiseProjectileChargeSound;
    private AudioSource[] robotNumberSounds;
    private AudioSource fractalProjectileSound;
    private Material[] gunMaterials;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        numberBulletMeshes = new Mesh[10];

        noiseProjectile = new Projectile("Projectiles/ToonProjectile", 0.025f, cameraTransform);
        fractalProjectile = new Projectile("Projectiles/FractalProjectile", 0.025f, cameraTransform);
        numberBullet = new Projectile("Projectiles/NumberBullet", 0.025f, cameraTransform);
        timaeusProjectile = new Projectile("Projectiles/PlatonicSolidSpreadShot", 0.025f, cameraTransform);

        player = GameObject.Find("player");
        playerSounds = player.GetComponents<AudioSource>();
        noiseProjectileChargeSound = playerSounds[1];
        noiseProjectileSound = playerSounds[2];
        robotNumberSounds = new AudioSource[10];
        System.Array.Copy(playerSounds, 3, robotNumberSounds, 0, 9);
        robotNumberSounds[9] = playerSounds[13];
        fractalProjectileSound = playerSounds[12];
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

        gunMaterials = GameObject.Find("ChargeGunModel").GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        int currentWeapon = Camera.main.GetComponent<GunSwap>().currentIndex;
        noiseProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        fractalProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        numberBullet.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        timaeusProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        // Arm Cannon
        if (currentWeapon == 0 && Input.GetMouseButtonDown(0)) // Charge Noise Bullet
        {
            chargeNoise = true;
            noiseProjectile.reset();
            noiseProjectile.instantiate();
        }
        else if (currentWeapon == 0 && Input.GetMouseButtonUp(0)) // Release Noise Bullet
        {
            noiseProjectileChargeSound.Stop();
            noiseProjectileSound.Play();
            chargeNoise = false;
            chargeTime = 0;
            noiseProjectile.shoot();
        }
        if (currentWeapon == 0 && Input.GetMouseButtonDown(1)) // Charge Fractal Bullet
        {
            chargeFractal = true;
            fractalProjectile.reset();
            fractalProjectile.instantiate();
        }
        else if (currentWeapon == 0 && Input.GetMouseButtonUp(1)) // Release Fractal Bullet
        {
            noiseProjectileChargeSound.Stop();
            fractalProjectileSound.Play();
            chargeFractal = false;
            chargeTime = 0;
            fractalProjectile.shoot();
        }
        // Pie Chart Melee Weapon
        if (currentWeapon == 1 && Input.GetMouseButtonDown(1)) // Charge Pie Chart Section
        {

        }
        else if (currentWeapon == 1 && Input.GetMouseButtonUp(1)) // Release Pie Chart Section
        {

        }
        // Randomizer Revolver
        if (currentWeapon == 2 && Input.GetMouseButtonDown(0)) // Charge Number Bullet
        {

            chargeNumber = true;
            numberBullet.reset();
            numberBullet.instantiate();
        }
        else if (currentWeapon == 2 && Input.GetMouseButtonUp(0)) // Release Number Bullet
        {
            if (currentNumber == 0)
            {
                robotNumberSounds[9].Play();
            }
            else
            {
                robotNumberSounds[9 - currentNumber].Play(); //Backwards for some reason

            }
            chargeNumber = false;
            numberBullet.shoot();
        }
        // Timaeus (shotgun which shoots 3 platonic solids)
        if (currentWeapon == 3 && Input.GetMouseButtonDown(0)) // Charge Platonic Solid Spreadshot
        {
            chargePlatonic = true;
            timaeusProjectile.reset();
            timaeusProjectile.instantiate();
        }
        else if (currentWeapon == 3 && Input.GetMouseButtonUp(0)) // Release Platonic Solid Spreadshot
        {
            noiseProjectileChargeSound.Stop();
            noiseProjectileSound.Play();
            chargePlatonic = false;
            chargeTime = 0;
            timaeusProjectile.shoot();
        }


        if (chargeNoise)
            noiseProjectile.updateCharge();
        if (chargeFractal)
            fractalProjectile.updateCharge();
        if (chargeNumber)
            numberBullet.updateCharge();

    }

    void FixedUpdate()
    {
        if (chargeNoise)
        {
            chargeTime++;
            if (chargeTime == 5)
            {
                noiseProjectileChargeSound.Play();
            }
            if (chargeTime >= 5)
            { 
                noiseProjectile.scale();
            }
        }
        if (chargeFractal)
        {
            chargeTime++;
            if (chargeTime == 5)
            {
                noiseProjectileChargeSound.Play();
            }
            if (chargeTime >= 5)
            {
                fractalProjectile.scale();
            }
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
                currentNumber = Random.Range(0, 3) + numberChargeBonus;
                numberBullet.chargingBody.gameObject.GetComponent<MeshFilter>().mesh = numberBulletMeshes[currentNumber];
            }
            numberBullet.timeMousePressed += 1;
        }
        if (chargeNoise)
        {
            chargeTime++;
            if (chargeTime == 5)
            {
                noiseProjectileChargeSound.Play();
            }
            if (chargeTime >= 5)
            {
                timaeusProjectile.scale();
            }
        }

        // add noise to gun mesh according to charge time
        for (int i = 0; i < gunMaterials.Length; i++)
        {   
            if (chargeNoise || chargeNumber || chargeFractal || chargePlatonic)
            {
                float maxNoise = 1f;
                float noiseAmount = (float)chargeTime *0.01f;
                noiseAmount = noiseAmount > maxNoise ? maxNoise : noiseAmount;
                gunMaterials[i].SetFloat("_NoiseAmount", noiseAmount);
            }
                
            else
                gunMaterials[i].SetFloat("_NoiseAmount", 0.0f);
        }
        
    }
}
