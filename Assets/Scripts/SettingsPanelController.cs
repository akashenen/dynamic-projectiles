using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SettingsPanelController : MonoBehaviour {

    public Actor actor;

    public Text openButton;

    public TMP_Dropdown presetsDropdown;
    public TMP_Dropdown styleDropdown;
    public TMP_Dropdown colorDropdown;
    public Slider trailLengthSlider;
    public Slider trailWidthSlider;
    public Slider bulletCountSlider;
    public TMP_InputField distanceXField;
    public TMP_InputField distanceYField;
    public TMP_InputField angleField;
    public Slider burstSpreadSlider;
    public Toggle novaToggle;
    public Toggle randomOrderToggle;
    public Slider speedSlider;
    public Slider durationSlider;
    public Slider fireRateSlider;
    public TMP_Dropdown behaviourDropdown;

    public List<SettingsColorPalette> colorPalettes;
    public List<Bullet> bulletPrefabs;

    private bool opened = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ToggleOpened() {
        opened = !opened;
        animator.SetBool("opened", opened);
        openButton.text = opened? "◄": "►";
    }

    public void ApplySettings() {
        WeaponConfig newConfig = (WeaponConfig) ScriptableObject.CreateInstance("WeaponConfig");
        if (angleField.text != "") {
            newConfig.angle = float.Parse(angleField.text, CultureInfo.InvariantCulture);
        } else {
            newConfig.angle = 0f;
        }
        switch (behaviourDropdown.value) {
            case 0:
                newConfig.behaviour = WeaponConfig.BulletBehaviour.Straight;
                break;
            case 1:
                newConfig.behaviour = WeaponConfig.BulletBehaviour.Orbit;
                break;
            default:
                newConfig.behaviour = WeaponConfig.BulletBehaviour.Straight;
                break;
        }
        newConfig.bulletCount = bulletCountSlider.value;
        newConfig.burstSpread = burstSpreadSlider.value;
        newConfig.colorGradient = colorPalettes[colorDropdown.value].gradient;
        float x = distanceXField.text == "" ? 0f : float.Parse(distanceXField.text, CultureInfo.InvariantCulture);
        float y = distanceYField.text == "" ? 0f : float.Parse(distanceYField.text, CultureInfo.InvariantCulture);
        newConfig.distance = new Vector2(x, y);
        newConfig.duration = durationSlider.value;
        newConfig.mainColor = colorPalettes[colorDropdown.value].mainColor;
        newConfig.nova = novaToggle.isOn;
        newConfig.randomOrder = randomOrderToggle.isOn;
        newConfig.rateMulti = fireRateSlider.value;
        newConfig.speed = speedSlider.value;
        newConfig.trailLength = trailLengthSlider.value;
        newConfig.trailWidth = trailWidthSlider.value;

        actor.weapon = newConfig;

        BulletManager.Instance.SetNewPrefab(bulletPrefabs[styleDropdown.value]);

    }

    [System.Serializable]
    public class SettingsColorPalette {
        public Color mainColor;
        public Gradient gradient;
    }
}