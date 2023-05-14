using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UniTool : MonoBehaviour
{
    public enum Mode { SCAN, LASER };
    public Mode mode;
    public UnityEvent OnModeSwitch;
    public UnityEvent OnScan;
    public Discoverable currentDisc;

    [Header("Properties")]
    public float maxDistanceScan = 15;
    public float maxDistanceLaser = 20;
    public float scanningSpeed;
    public float damage = 10;
    public float damageDelay = 0.5f;
    public LayerMask layerMask;

    [Header("Objects")]
    public Material hologramMat;
    public Material collectMat;
    public GameObject discoverIndicator;
    public Camera cam;
    public Transform gunTip;
    public LineRenderer laserRenderer;
    public AudioSource laserAudioSource;

    [Header("UI")]
    public GameObject scanningUI;
    public Image loadingBar;
    public Transform modeSelectorArrow;
    public Transform scanIconUI;
    public Transform laserIconUI;

    bool allowScan;
    float currentDamageTimer;
    [ReadOnly] public bool shootingLaser;
    RaycastHit hitScan;
    RaycastHit hitLaser;

    public static UniTool Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        allowScan = true;
        laserRenderer.useWorldSpace = true;

        currentDamageTimer = damageDelay;
    }

    void Update()
    {
        discoverIndicator.SetActive(currentDisc != null);


        ModeSwitchUpdate();
        ScanningUpdate();
        LaserUpdate();
    }

    void ModeSwitchUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mode = mode == Mode.SCAN ? Mode.LASER : Mode.SCAN;
            OnModeSwitch?.Invoke();
        }

        modeSelectorArrow.position = Vector3.MoveTowards(modeSelectorArrow.position,
        mode == Mode.SCAN ? new Vector3(scanIconUI.position.x, modeSelectorArrow.position.y, modeSelectorArrow.position.z) :
        new Vector3(laserIconUI.position.x, modeSelectorArrow.position.y, modeSelectorArrow.position.z), 1000 * Time.deltaTime);
    }

    void ScanningUpdate()
    {

        if (Input.GetMouseButtonUp(0)) allowScan = true;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitScan, maxDistanceScan, layerMask))
        {
            if (hitScan.transform.GetComponent<Discoverable>() != null)
            {
                currentDisc = hitScan.transform.GetComponent<Discoverable>();

                if (Input.GetMouseButton(0) && allowScan && mode == Mode.SCAN && !UIManager.Instance.hasUIOpen)
                {
                    scanningUI.SetActive(true);
                    loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1, Time.deltaTime * scanningSpeed);

                    if (loadingBar.fillAmount >= 1) ScanComplete(hitScan.transform.GetComponent<DiscoveryHolder>());

                }
                else
                {
                    scanningUI.SetActive(false);
                    loadingBar.fillAmount = 0;
                }
            }
            else
            {
                scanningUI.SetActive(false);
                currentDisc = null;
                loadingBar.fillAmount = 0;
            }
        }
        else
        {
            scanningUI.SetActive(false);
            currentDisc = null;
            loadingBar.fillAmount = 0;
        }
    }

    void LaserUpdate()
    {
        shootingLaser = Input.GetMouseButton(0) && mode == Mode.LASER && !UIManager.Instance.hasUIOpen;
        if (shootingLaser)
        {
            if (!laserAudioSource.isPlaying)
                laserAudioSource.Play();
            laserRenderer.enabled = true;
            // Check if the ray hits something within the maximum distance
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitLaser, maxDistanceScan, layerMask))
            {
                // Set the Line Renderer positions from the gun tip to the hit point
                laserRenderer.SetPosition(0, gunTip.position);
                laserRenderer.SetPosition(1, hitLaser.point);

                if (hitLaser.transform.GetComponent<Discoverable>() != null)
                {
                    currentDisc = hitLaser.transform.GetComponent<Discoverable>();
                    DoDamage(hitLaser.transform.GetComponent<Discoverable>());
                }
            }
            else
            {
                // If the ray doesn't hit anything, set the Line Renderer positions to extend to the maximum distance
                laserRenderer.SetPosition(0, gunTip.position);
                laserRenderer.SetPosition(1, gunTip.position + cam.transform.forward * maxDistanceLaser);
            }
        }
        else
        {
            laserRenderer.enabled = false;
            laserAudioSource.Stop();
        }
    }

    void DoDamage(Discoverable discoverable)
    {
        currentDamageTimer -= Time.deltaTime * damageDelay;
        if(currentDamageTimer <= 0)
        {
            currentDamageTimer = damageDelay;
            discoverable.ApplyDamage(damage);
        }
    }

    void ScanComplete(DiscoveryHolder discoveryHolder)
    {
        allowScan = false;
        OnScan?.Invoke();
        DiscoveryManager.Instance.AddNewDiscovery(discoveryHolder.discovery);
    }
}
