using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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

            _prefab.gameObject.tag = "playerAttack";

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

        public void instantiate(float[] offset = null)
        {
            /*
            Vector3 spawnPosition;
            if (offset != null)
            {
                Vector3 relativePosition = new Vector3(offset[0], offset[1], offset[2]);
                spawnPosition = spawnTransform.position + relativePosition;
            }
            else
            {
                spawnPosition = spawnTransform.position;
            }
            */
            // UnityEngine.Debug.Log("player gun boner end  "+GameObject.Find("PlayerGunBoneEnd").transform.position);


            UnityEngine.Debug.Log(GameObject.Find("ChargeGunModel").transform.position);
            Vector3 gunEnd = GameObject.Find("ChargeGunModel").transform.position;
       //     UnityEngine.Debug.Log("gunmodel end " + gunEnd);
            GameObject player = GameObject.Find("player");
            Rigidbody projectile = Instantiate(_rigidbody, gunEnd, player.transform.rotation);//spawnTransform.rotation);
            projectile.transform.localScale = spawnScale;
            projectile.useGravity = false;
            Destroy(projectile, 2);


            // set dissolve uniform
            float t = Time.time;
            projectile.gameObject.GetComponent<Renderer>().material.SetFloat("_TimeOffset", t);
            chargingBody = projectile;
        }
        
        public void shoot()
        {
            chargingBody.useGravity = false;
        //    UnityEngine.Debug.Log("SDGKHSDGKJDS HGSDJKHSDG JSDGSJKKJ"+GameObject.Find("player").GetComponent<FirstPersonController>().currentVelocity);
            chargingBody.velocity = GameObject.Find("player").GetComponent<FirstPersonController>().currentVelocity + cameraTransform.forward * speed*5.2f;
            chargingBody.transform.localScale = new Vector3(spawnScale.x, spawnScale.y, spawnScale.z)*9.5f;

           // UnityEngine.Debug.Log("difference "+(GameObject.Find("PlayerGunBoneEnd").transform.position- GameObject.Find("player").transform.position));
            chargingBody.transform.position = GameObject.Find("PlayerGunBoneEnd").transform.position;

            ninja.DodgeStuff();

            Destroy(chargingBody, 2);
            // chargingBody.gameObject.AddComponent<GravityBody>();
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
    private Projectile diagonalCubeProjectile;
    private Projectile tetrahedronHourglassProjectile;
    private Projectile octahedronProjectile;

    public Rigidbody chargingProjectile;

    public Mesh[] numberBulletMeshes;

    private GameObject player;
    private AudioClip noiseProjectileSound;
    private AudioSource noiseProjectileChargeSound;
    private AudioSource heard_u_guys_like_AudioSources;
    private AudioClip[] robotNumberSoundsDURR;
    private AudioClip fractalProjectileSound;
    private Material[] gunMaterials;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        numberBulletMeshes = new Mesh[10];

        noiseProjectile = new Projectile("Projectiles/ToonProjectile", 0.025f, cameraTransform);
        fractalProjectile = new Projectile("Projectiles/FractalProjectile", 0.025f, cameraTransform);
        numberBullet = new Projectile("Projectiles/NumberBullet", 0.025f, cameraTransform);
        diagonalCubeProjectile = new Projectile("Projectiles/DiagonalCubeProjectile", 0.025f, cameraTransform);
        tetrahedronHourglassProjectile = new Projectile("Projectiles/TetrahedronHourglassProjectile", 0.025f, cameraTransform);
        octahedronProjectile = new Projectile("Projectiles/OctahedronProjectile", 0.025f, cameraTransform);

        player = GameObject.Find("player");

        // this is sum bullshit right here
        AudioSource[] playerSounds;
        playerSounds = player.GetComponents<AudioSource>();
        //noiseProjectileChargeSound = playerSounds[1];
        noiseProjectileChargeSound = gameObject.AddComponent<AudioSource>();
        noiseProjectileChargeSound.clip = Resources.Load("Audio/NoiseProjectileCharge") as AudioClip;
        noiseProjectileSound = Resources.Load("Audio/NoiseProjectileSound") as AudioClip;

        // get robot audio clips
        noiseProjectileSound = Resources.Load("Audio/NoiseProjectileSound") as AudioClip;  
        fractalProjectileSound = Resources.Load("Audio/SphereProjectileSound") as AudioClip;
        robotNumberSoundsDURR = new AudioClip[10]; 
        for ( int i = 0; i < 10; i++) {
            robotNumberSoundsDURR[i] = Resources.Load("Audio/RobotVoiceNumber" + i.ToString()) as AudioClip;
        }
        // for ( ;; )
        heard_u_guys_like_AudioSources = gameObject.AddComponent<AudioSource>();
       
        fractalProjectile.speed = 11.4f;

        // fix flipped numbers
        numberBullet.startingScale.x *= 8;
        numberBullet.startingScale.y *= 3;

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
    int framecounter = 0;
    // Update is called once per frame
    float timeSinceLastShot = 0;
    void Update()
    {

        int currentWeapon = Camera.main.GetComponent<GunSwap>().currentIndex;

        noiseProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        
        fractalProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        
        numberBullet.spawnTransform = (GameObject.Find("PlayerGunBoneEnd").transform);
        Quaternion q = new Quaternion();// Quaternion.Euler(0, 180, 0);
       // numberBullet.spawnTransform.rotation = q;

        //rotate bullets correctly

        //  q.SetFromToRotation(Vector3.forward, Vector3.right);
        //  numberBullet.spawnTransform.rotation *= q;


        diagonalCubeProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        tetrahedronHourglassProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        octahedronProjectile.spawnTransform = GameObject.Find("PlayerGunBoneEnd").transform;
        
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
            heard_u_guys_like_AudioSources.PlayOneShot(noiseProjectileSound);
            chargeNoise = false;
            chargeTime = 0;
            //    noiseProjectile.transform.position = GameObject.Find("PlayerGunBoneEnd").transform.position;
            if (Time.time - timeSinceLastShot > .13f)
            {
                noiseProjectile.shoot();
                timeSinceLastShot = Time.time;
            }
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
            //fractalProjectileSound.Play();
            heard_u_guys_like_AudioSources.PlayOneShot(fractalProjectileSound);
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
            // if (currentNumber == 0)
            // {
            //     // robotNumberSounds[9].Play();
            // }
            // else
            // {
            //     robotNumberSounds[9 - currentNumber].Play(); //Backwards for some reason
            // }
            heard_u_guys_like_AudioSources.PlayOneShot(robotNumberSoundsDURR[currentNumber]);
            chargeNumber = false;
            numberBullet.shoot();
        }
        // Timaeus (shotgun which shoots 3 platonic solids)
        if (currentWeapon == 3 && Input.GetMouseButtonDown(0)) // Charge Platonic Solid Spreadshot
        {
            chargePlatonic = true;
            float[] offset = new float[3] {.25f, .06f, .9f};
            diagonalCubeProjectile.reset();
            diagonalCubeProjectile.instantiate(offset);
            tetrahedronHourglassProjectile.reset();
            offset = new float[3] { 0f, 0f, .9f };
            tetrahedronHourglassProjectile.instantiate(offset);
            octahedronProjectile.reset();
            offset = new float[3] { .55f, .06f, .9f };
            octahedronProjectile.instantiate(offset);
        }
        else if (currentWeapon == 3 && Input.GetMouseButtonUp(0)) // Release Platonic Solid Spreadshot
        {
            noiseProjectileChargeSound.Stop();
            heard_u_guys_like_AudioSources.PlayOneShot(noiseProjectileSound);
            chargePlatonic = false;
            chargeTime = 0;
            diagonalCubeProjectile.shoot();
            tetrahedronHourglassProjectile.shoot();
            octahedronProjectile.shoot();
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
                diagonalCubeProjectile.scale();
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
