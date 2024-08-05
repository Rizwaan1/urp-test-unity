using KINEMATION.FPSAnimationFramework.Runtime.Camera;
using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.FPSAnimationFramework.Runtime.Playables;
using KINEMATION.FPSAnimationFramework.Runtime.Recoil;
using KINEMATION.KAnimationCore.Runtime.Input;
using MoreMountains.Feedbacks;

using Demo.Scripts.Runtime.AttachmentSystem;

using System.Collections.Generic;
using Demo.Scripts.Runtime.Character;
using UnityEngine;

namespace Demo.Scripts.Runtime.Item
{
    public enum OverlayType
    {
        Default,
        Pistol,
        Rifle
    }

    public class Weapon : FPSItem
    {
        [Header("General")]
        [SerializeField][Range(0f, 120f)] private float fieldOfView = 90f;

        [Header("Weapon Stats")]
        [SerializeField] private float weight = 3f; // Add this line for weapon weight
        [SerializeField] private float weaponWalkSpeed = 4f; // Movement speed with this weapon
        [SerializeField] private float weaponRunSpeed = 8f;  // Sprint speed with this weapon

        [Header("Animations")]
        [SerializeField] private FPSAnimationAsset reloadClip;
        [SerializeField] private FPSCameraAnimation cameraReloadAnimation;

        [SerializeField] private FPSAnimationAsset grenadeClip;
        [SerializeField] private FPSCameraAnimation cameraGrenadeAnimation;
        [SerializeField] private OverlayType overlayType;

        [Header("Recoil")]
        [SerializeField] private RecoilAnimData recoilData;
        [SerializeField] private RecoilPatternSettings recoilPatternSettings;
        [SerializeField] private FPSCameraShake cameraShake;
        [Min(0f)][SerializeField] private float fireRate;

        [SerializeField] private bool supportsAuto;
        [SerializeField] private bool supportsBurst;
        [SerializeField] private int burstLength;

        [Header("Attachments")]
        [SerializeField] private AttachmentGroup<BaseAttachment> barrelAttachments = new AttachmentGroup<BaseAttachment>();
        [SerializeField] private AttachmentGroup<BaseAttachment> gripAttachments = new AttachmentGroup<BaseAttachment>();
        [SerializeField] private List<AttachmentGroup<ScopeAttachment>> scopeGroups = new List<AttachmentGroup<ScopeAttachment>>();

        [Header("Ammo")]
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private int currentAmmo;
        [SerializeField] private float reloadTime = 2f;
        [SerializeField] public MMFeedbacks gunShotFeedBack, reloadMagFeedback;
        [SerializeField] private float maxAmmoBelt = 90f; // Maximum ammo belt capacity
        [SerializeField] private float currentAmmoBelt; // Current ammo belt

        [Header("Shooting")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        public bool Noise;

        [Header("Movement Speed During Reload")]
        [SerializeField] private float reducedWalkSpeed = 1.5f;
        [SerializeField] private float reducedRunSpeed = 3.0f;

        //~ Controller references
        private FPSController _fpsController;
        private FPSMovement _fpsMovement;  // Add this reference
        private Animator _controllerAnimator;
        private UserInputController _userInputController;
        private IPlayablesController _playablesController;
        private FPSCameraController _fpsCameraController;
        private FPSAnimator _fpsAnimator;
        private FPSAnimatorEntity _fpsAnimatorEntity;
        private RecoilAnimation _recoilAnimation;
        private RecoilPattern _recoilPattern;
        private Animator _weaponAnimator;
        private int _scopeIndex;
        private float _lastRecoilTime;
        private int _bursts;
        private FireMode _fireMode = FireMode.Semi;
        private bool _isReloading;

        private static readonly int OverlayType = Animator.StringToHash("OverlayType");
        private static readonly int CurveEquip = Animator.StringToHash("CurveEquip");
        private static readonly int CurveUnequip = Animator.StringToHash("CurveUnequip");

        private AmmoUIManager _ammoUIManager;

        private void Awake()
        {
            _ammoUIManager = FindObjectOfType<AmmoUIManager>();
        }

        private void Start()
        {
            currentAmmoBelt = maxAmmoBelt; // Initialize current ammo belt to max ammo belt at start
            UpdateAmmoUI();
        }

        private void UpdateAmmoUI()
        {
            if (_ammoUIManager != null)
            {
                _ammoUIManager.UpdateAmmoUI(currentAmmo, currentAmmoBelt);
            }
        }

        private void OnActionEnded()
        {
            if (_fpsController == null) return;
            _fpsController.ResetActionState();
        }

        protected void UpdateTargetFOV(bool isAiming)
        {
            float fov = fieldOfView;
            float sensitivityMultiplier = 1f;

            if (isAiming && scopeGroups.Count != 0)
            {
                var scope = scopeGroups[_scopeIndex].GetActiveAttachment();
                fov *= scope.aimFovZoom;
                sensitivityMultiplier = scopeGroups[_scopeIndex].GetActiveAttachment().sensitivityMultiplier;
            }

            _userInputController.SetValue("SensitivityMultiplier", sensitivityMultiplier);
            _fpsCameraController.UpdateTargetFOV(fov);
        }

        protected void UpdateAimPoint()
        {
            if (scopeGroups.Count == 0) return;
            var scope = scopeGroups[_scopeIndex].GetActiveAttachment().aimPoint;
            _fpsAnimatorEntity.defaultAimPoint = scope;
        }

        protected void InitializeAttachments()
        {
            foreach (var attachmentGroup in scopeGroups)
            {
                attachmentGroup.Initialize(_fpsAnimator);
            }

            _scopeIndex = 0;
            if (scopeGroups.Count == 0) return;
            UpdateAimPoint();
            UpdateTargetFOV(false);
        }

        public override void OnEquip(GameObject parent)
        {
            if (parent == null) return;

            _fpsAnimator = parent.GetComponent<FPSAnimator>();
            _fpsAnimatorEntity = GetComponent<FPSAnimatorEntity>();

            _fpsController = parent.GetComponent<FPSController>();
            _fpsMovement = parent.GetComponent<FPSMovement>();  // Get the FPSMovement component
            _weaponAnimator = GetComponentInChildren<Animator>();

            _controllerAnimator = parent.GetComponent<Animator>();
            _userInputController = parent.GetComponent<UserInputController>();
            _playablesController = parent.GetComponent<IPlayablesController>();
            _fpsCameraController = parent.GetComponentInChildren<FPSCameraController>();

            InitializeAttachments();

            _recoilAnimation = parent.GetComponent<RecoilAnimation>();
            _recoilPattern = parent.GetComponent<RecoilPattern>();

            _controllerAnimator.SetFloat(OverlayType, (float)overlayType);
            _fpsAnimator.LinkAnimatorProfile(gameObject);

            barrelAttachments.Initialize(_fpsAnimator);
            gripAttachments.Initialize(_fpsAnimator);

            _recoilAnimation.Init(recoilData, fireRate, _fireMode);
            if (_recoilPattern != null)
            {
                _recoilPattern.Init(recoilPatternSettings);
            }

            _controllerAnimator.CrossFade(CurveEquip, 0.15f);

            currentAmmo = maxAmmo;
            UpdateAmmoUI();

            // Set the current weapon in FPSMovement to adjust the speed
            _fpsMovement.SetCurrentWeapon(this);
        }

        public override void OnUnEquip()
        {
            _controllerAnimator.CrossFade(CurveUnequip, 0.15f);
            _fpsMovement.ResetMovementSpeed();  // Reset movement speed when weapon is unequipped
        }

        public override void OnUnarmedEnabled()
        {
            _controllerAnimator.SetFloat(OverlayType, 0);
            _userInputController.SetValue(FPSANames.PlayablesWeight, 0f);
            _userInputController.SetValue(FPSANames.StabilizationWeight, 0f);
        }

        public override void OnUnarmedDisabled()
        {
            _controllerAnimator.SetFloat(OverlayType, (int)overlayType);
            _userInputController.SetValue(FPSANames.PlayablesWeight, 1f);
            _userInputController.SetValue(FPSANames.StabilizationWeight, 1f);
            _fpsAnimator.LinkAnimatorProfile(gameObject);
        }

        public override bool OnAimPressed()
        {
            _userInputController.SetValue(FPSANames.IsAiming, true);
            UpdateTargetFOV(true);
            _recoilAnimation.isAiming = true;
            return true;
        }

        public override bool OnAimReleased()
        {
            _userInputController.SetValue(FPSANames.IsAiming, false);
            UpdateTargetFOV(false);
            _recoilAnimation.isAiming = false;
            return true;
        }

        public override bool OnFirePressed()
        {
            if (_isReloading || currentAmmo <= 0 || Time.unscaledTime - _lastRecoilTime < 60f / fireRate)
            {
                return false;
            }

            _lastRecoilTime = Time.unscaledTime;
            _bursts = burstLength;

            OnFire();

            return true;
        }

        public override bool OnFireReleased()
        {
            if (_recoilAnimation != null)
            {
                _recoilAnimation.Stop();
            }

            if (_recoilPattern != null)
            {
                _recoilPattern.OnFireEnd();
            }

            CancelInvoke(nameof(OnFire));
            return true;
        }

        public override bool OnReload()
        {
            if (_isReloading || currentAmmo == maxAmmo || currentAmmoBelt <= 0)
            {
                return false;
            }

            _isReloading = true;
            _fpsMovement.AdjustMovementSpeed(reducedWalkSpeed, reducedRunSpeed);  // Reduce movement speed during reload
            _playablesController.PlayAnimation(reloadClip, 0f);

            if (_weaponAnimator != null)
            {
                _weaponAnimator.Rebind();
                _weaponAnimator.Play("Reload", 0);
                reloadMagFeedback?.PlayFeedbacks();
            }

            if (_fpsCameraController != null)
            {
                _fpsCameraController.PlayCameraAnimation(cameraReloadAnimation);
            }

            OnFireReleased();
            Invoke(nameof(FinishReload), reloadTime);
            return true;
        }

        private void FinishReload()
        {
            int ammoNeeded = maxAmmo - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, (int)currentAmmoBelt);

            currentAmmo += ammoToReload;
            currentAmmoBelt -= ammoToReload;

            _isReloading = false;
            _fpsMovement.SetCurrentWeapon(this);  // Reapply movement speed after reload
            OnActionEnded();
            UpdateAmmoUI();
        }

        public void RefillAmmoBelt(float amount)
        {
            currentAmmoBelt = Mathf.Clamp(currentAmmoBelt + amount, 0, maxAmmoBelt); // Ensure it does not exceed maxAmmoBelt
            UpdateAmmoUI();
        }

        public override bool OnGrenadeThrow()
        {
            if (!FPSAnimationAsset.IsValid(grenadeClip))
            {
                return false;
            }

            _playablesController.PlayAnimation(grenadeClip, 0f);

            if (_fpsCameraController != null)
            {
                _fpsCameraController.PlayCameraAnimation(cameraGrenadeAnimation);
            }

            Invoke(nameof(OnActionEnded), grenadeClip.clip.length * 0.8f);
            return true;
        }

        private void OnFire()
        {
            if (currentAmmo <= 0)
            {
                OnFireReleased();
                return;
            }

            if (_weaponAnimator != null)
            {
                _weaponAnimator.Play("Fire", 0, 0f);
            }

            _fpsCameraController.PlayCameraShake(cameraShake);
            gunShotFeedBack?.PlayFeedbacks();

            currentAmmo--;
            UpdateAmmoUI();

            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }

            if (_recoilAnimation != null && recoilData != null)
            {
                _recoilAnimation.Play();
            }

            if (_recoilPattern != null)
            {
                _recoilPattern.OnFireStart();
            }

            if (_recoilAnimation.fireMode == FireMode.Semi)
            {
                Invoke(nameof(OnFireReleased), 60f / fireRate);
                return;
            }

            if (_recoilAnimation.fireMode == FireMode.Burst)
            {
                _bursts--;
                if (_bursts == 0)
                {
                    OnFireReleased();
                    return;
                }
            }

            Invoke(nameof(OnFire), 60f / fireRate);
        }

        public override void OnCycleScope()
        {
            if (scopeGroups.Count == 0) return;

            _scopeIndex++;
            _scopeIndex = _scopeIndex > scopeGroups.Count - 1 ? 0 : _scopeIndex;

            UpdateAimPoint();
            UpdateTargetFOV(true);
        }

        private void CycleFireMode()
        {
            if (_fireMode == FireMode.Semi && supportsBurst)
            {
                _fireMode = FireMode.Burst;
                _bursts = burstLength;
                return;
            }

            if (_fireMode != FireMode.Auto && supportsAuto)
            {
                _fireMode = FireMode.Auto;
                return;
            }

            _fireMode = FireMode.Semi;
        }

        public override void OnChangeFireMode()
        {
            CycleFireMode();
            _recoilAnimation.fireMode = _fireMode;
        }

        public override void OnAttachmentChanged(int attachmentTypeIndex)
        {
            if (attachmentTypeIndex == 1)
            {
                barrelAttachments.CycleAttachments(_fpsAnimator);
                return;
            }

            if (attachmentTypeIndex == 2)
            {
                gripAttachments.CycleAttachments(_fpsAnimator);
                return;
            }

            if (scopeGroups.Count == 0) return;
            scopeGroups[_scopeIndex].CycleAttachments(_fpsAnimator);
            UpdateAimPoint();
        }

        // Method to get the weapon's weight
        public float GetWeight()
        {
            return weight;
        }

        // Method to get the weapon's walk speed
        public float GetWeaponWalkSpeed()
        {
            return weaponWalkSpeed;
        }

        // Method to get the weapon's run speed
        public float GetWeaponRunSpeed()
        {
            return weaponRunSpeed;
        }
    }
}
