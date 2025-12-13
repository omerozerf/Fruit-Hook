var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i738 = root || request.c( 'UnityEngine.JointSpring' )
  var i739 = data
  i738.spring = i739[0]
  i738.damper = i739[1]
  i738.targetPosition = i739[2]
  return i738
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i740 = root || request.c( 'UnityEngine.JointMotor' )
  var i741 = data
  i740.m_TargetVelocity = i741[0]
  i740.m_Force = i741[1]
  i740.m_FreeSpin = i741[2]
  return i740
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i742 = root || request.c( 'UnityEngine.JointLimits' )
  var i743 = data
  i742.m_Min = i743[0]
  i742.m_Max = i743[1]
  i742.m_Bounciness = i743[2]
  i742.m_BounceMinVelocity = i743[3]
  i742.m_ContactDistance = i743[4]
  i742.minBounce = i743[5]
  i742.maxBounce = i743[6]
  return i742
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i744 = root || request.c( 'UnityEngine.JointDrive' )
  var i745 = data
  i744.m_PositionSpring = i745[0]
  i744.m_PositionDamper = i745[1]
  i744.m_MaximumForce = i745[2]
  i744.m_UseAcceleration = i745[3]
  return i744
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i746 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i747 = data
  i746.m_Spring = i747[0]
  i746.m_Damper = i747[1]
  return i746
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i748 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i749 = data
  i748.m_Limit = i749[0]
  i748.m_Bounciness = i749[1]
  i748.m_ContactDistance = i749[2]
  return i748
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i750 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i751 = data
  i750.m_ExtremumSlip = i751[0]
  i750.m_ExtremumValue = i751[1]
  i750.m_AsymptoteSlip = i751[2]
  i750.m_AsymptoteValue = i751[3]
  i750.m_Stiffness = i751[4]
  return i750
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i752 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i753 = data
  i752.m_LowerAngle = i753[0]
  i752.m_UpperAngle = i753[1]
  return i752
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i754 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i755 = data
  i754.m_MotorSpeed = i755[0]
  i754.m_MaximumMotorTorque = i755[1]
  return i754
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i756 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i757 = data
  i756.m_DampingRatio = i757[0]
  i756.m_Frequency = i757[1]
  i756.m_Angle = i757[2]
  return i756
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i758 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i759 = data
  i758.m_LowerTranslation = i759[0]
  i758.m_UpperTranslation = i759[1]
  return i758
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i760 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i761 = data
  i760.position = new pc.Vec3( i761[0], i761[1], i761[2] )
  i760.scale = new pc.Vec3( i761[3], i761[4], i761[5] )
  i760.rotation = new pc.Quat(i761[6], i761[7], i761[8], i761[9])
  return i760
}

Deserializers["Tile"] = function (request, data, root) {
  var i762 = root || request.c( 'Tile' )
  var i763 = data
  request.r(i763[0], i763[1], 0, i762, '_spriteRenderer')
  return i762
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer"] = function (request, data, root) {
  var i764 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer' )
  var i765 = data
  i764.enabled = !!i765[0]
  request.r(i765[1], i765[2], 0, i764, 'sharedMaterial')
  var i767 = i765[3]
  var i766 = []
  for(var i = 0; i < i767.length; i += 2) {
  request.r(i767[i + 0], i767[i + 1], 2, i766, '')
  }
  i764.sharedMaterials = i766
  i764.receiveShadows = !!i765[4]
  i764.shadowCastingMode = i765[5]
  i764.sortingLayerID = i765[6]
  i764.sortingOrder = i765[7]
  i764.lightmapIndex = i765[8]
  i764.lightmapSceneIndex = i765[9]
  i764.lightmapScaleOffset = new pc.Vec4( i765[10], i765[11], i765[12], i765[13] )
  i764.lightProbeUsage = i765[14]
  i764.reflectionProbeUsage = i765[15]
  i764.color = new pc.Color(i765[16], i765[17], i765[18], i765[19])
  request.r(i765[20], i765[21], 0, i764, 'sprite')
  i764.flipX = !!i765[22]
  i764.flipY = !!i765[23]
  i764.drawMode = i765[24]
  i764.size = new pc.Vec2( i765[25], i765[26] )
  i764.tileMode = i765[27]
  i764.adaptiveModeThreshold = i765[28]
  i764.maskInteraction = i765[29]
  i764.spriteSortPoint = i765[30]
  return i764
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i770 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i771 = data
  i770.name = i771[0]
  i770.tagId = i771[1]
  i770.enabled = !!i771[2]
  i770.isStatic = !!i771[3]
  i770.layer = i771[4]
  return i770
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i772 = root || new pc.UnityMaterial()
  var i773 = data
  i772.name = i773[0]
  request.r(i773[1], i773[2], 0, i772, 'shader')
  i772.renderQueue = i773[3]
  i772.enableInstancing = !!i773[4]
  var i775 = i773[5]
  var i774 = []
  for(var i = 0; i < i775.length; i += 1) {
    i774.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i775[i + 0]) );
  }
  i772.floatParameters = i774
  var i777 = i773[6]
  var i776 = []
  for(var i = 0; i < i777.length; i += 1) {
    i776.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i777[i + 0]) );
  }
  i772.colorParameters = i776
  var i779 = i773[7]
  var i778 = []
  for(var i = 0; i < i779.length; i += 1) {
    i778.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i779[i + 0]) );
  }
  i772.vectorParameters = i778
  var i781 = i773[8]
  var i780 = []
  for(var i = 0; i < i781.length; i += 1) {
    i780.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i781[i + 0]) );
  }
  i772.textureParameters = i780
  var i783 = i773[9]
  var i782 = []
  for(var i = 0; i < i783.length; i += 1) {
    i782.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i783[i + 0]) );
  }
  i772.materialFlags = i782
  return i772
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i786 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i787 = data
  i786.name = i787[0]
  i786.value = i787[1]
  return i786
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i790 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i791 = data
  i790.name = i791[0]
  i790.value = new pc.Color(i791[1], i791[2], i791[3], i791[4])
  return i790
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i794 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i795 = data
  i794.name = i795[0]
  i794.value = new pc.Vec4( i795[1], i795[2], i795[3], i795[4] )
  return i794
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i798 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i799 = data
  i798.name = i799[0]
  request.r(i799[1], i799[2], 0, i798, 'value')
  return i798
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i802 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i803 = data
  i802.name = i803[0]
  i802.enabled = !!i803[1]
  return i802
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i804 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i805 = data
  i804.name = i805[0]
  i804.width = i805[1]
  i804.height = i805[2]
  i804.mipmapCount = i805[3]
  i804.anisoLevel = i805[4]
  i804.filterMode = i805[5]
  i804.hdr = !!i805[6]
  i804.format = i805[7]
  i804.wrapMode = i805[8]
  i804.alphaIsTransparency = !!i805[9]
  i804.alphaSource = i805[10]
  i804.graphicsFormat = i805[11]
  i804.sRGBTexture = !!i805[12]
  i804.desiredColorSpace = i805[13]
  i804.wrapU = i805[14]
  i804.wrapV = i805[15]
  return i804
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i806 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i807 = data
  i806.name = i807[0]
  i806.index = i807[1]
  i806.startup = !!i807[2]
  return i806
}

Deserializers["LoopGames.Playable.GridMapBuilder"] = function (request, data, root) {
  var i808 = root || request.c( 'LoopGames.Playable.GridMapBuilder' )
  var i809 = data
  i808.width = i809[0]
  i808.height = i809[1]
  i808.cellSize = i809[2]
  request.r(i809[3], i809[4], 0, i808, 'groundPrefab')
  var i811 = i809[5]
  var i810 = []
  for(var i = 0; i < i811.length; i += 2) {
  request.r(i811[i + 0], i811[i + 1], 2, i810, '')
  }
  i808.groundVariants = i810
  request.r(i809[6], i809[7], 0, i808, 'fenceHorizontal')
  request.r(i809[8], i809[9], 0, i808, 'fenceVertical')
  request.r(i809[10], i809[11], 0, i808, 'fenceCorner')
  return i808
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.RectTransform"] = function (request, data, root) {
  var i814 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.RectTransform' )
  var i815 = data
  i814.pivot = new pc.Vec2( i815[0], i815[1] )
  i814.anchorMin = new pc.Vec2( i815[2], i815[3] )
  i814.anchorMax = new pc.Vec2( i815[4], i815[5] )
  i814.sizeDelta = new pc.Vec2( i815[6], i815[7] )
  i814.anchoredPosition3D = new pc.Vec3( i815[8], i815[9], i815[10] )
  i814.rotation = new pc.Quat(i815[11], i815[12], i815[13], i815[14])
  i814.scale = new pc.Vec3( i815[15], i815[16], i815[17] )
  return i814
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Canvas"] = function (request, data, root) {
  var i816 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Canvas' )
  var i817 = data
  i816.enabled = !!i817[0]
  i816.planeDistance = i817[1]
  i816.referencePixelsPerUnit = i817[2]
  i816.isFallbackOverlay = !!i817[3]
  i816.renderMode = i817[4]
  i816.renderOrder = i817[5]
  i816.sortingLayerName = i817[6]
  i816.sortingOrder = i817[7]
  i816.scaleFactor = i817[8]
  request.r(i817[9], i817[10], 0, i816, 'worldCamera')
  i816.overrideSorting = !!i817[11]
  i816.pixelPerfect = !!i817[12]
  i816.targetDisplay = i817[13]
  i816.overridePixelPerfect = !!i817[14]
  return i816
}

Deserializers["UnityEngine.UI.CanvasScaler"] = function (request, data, root) {
  var i818 = root || request.c( 'UnityEngine.UI.CanvasScaler' )
  var i819 = data
  i818.m_UiScaleMode = i819[0]
  i818.m_ReferencePixelsPerUnit = i819[1]
  i818.m_ScaleFactor = i819[2]
  i818.m_ReferenceResolution = new pc.Vec2( i819[3], i819[4] )
  i818.m_ScreenMatchMode = i819[5]
  i818.m_MatchWidthOrHeight = i819[6]
  i818.m_PhysicalUnit = i819[7]
  i818.m_FallbackScreenDPI = i819[8]
  i818.m_DefaultSpriteDPI = i819[9]
  i818.m_DynamicPixelsPerUnit = i819[10]
  i818.m_PresetInfoIsWorld = !!i819[11]
  return i818
}

Deserializers["UnityEngine.UI.GraphicRaycaster"] = function (request, data, root) {
  var i820 = root || request.c( 'UnityEngine.UI.GraphicRaycaster' )
  var i821 = data
  i820.m_IgnoreReversedGraphics = !!i821[0]
  i820.m_BlockingObjects = i821[1]
  i820.m_BlockingMask = UnityEngine.LayerMask.FromIntegerValue( i821[2] )
  return i820
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer"] = function (request, data, root) {
  var i822 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer' )
  var i823 = data
  i822.cullTransparentMesh = !!i823[0]
  return i822
}

Deserializers["UnityEngine.UI.Image"] = function (request, data, root) {
  var i824 = root || request.c( 'UnityEngine.UI.Image' )
  var i825 = data
  request.r(i825[0], i825[1], 0, i824, 'm_Sprite')
  i824.m_Type = i825[2]
  i824.m_PreserveAspect = !!i825[3]
  i824.m_FillCenter = !!i825[4]
  i824.m_FillMethod = i825[5]
  i824.m_FillAmount = i825[6]
  i824.m_FillClockwise = !!i825[7]
  i824.m_FillOrigin = i825[8]
  i824.m_UseSpriteMesh = !!i825[9]
  i824.m_PixelsPerUnitMultiplier = i825[10]
  request.r(i825[11], i825[12], 0, i824, 'm_Material')
  i824.m_Maskable = !!i825[13]
  i824.m_Color = new pc.Color(i825[14], i825[15], i825[16], i825[17])
  i824.m_RaycastTarget = !!i825[18]
  i824.m_RaycastPadding = new pc.Vec4( i825[19], i825[20], i825[21], i825[22] )
  return i824
}

Deserializers["FloatingJoystick"] = function (request, data, root) {
  var i826 = root || request.c( 'FloatingJoystick' )
  var i827 = data
  request.r(i827[0], i827[1], 0, i826, 'background')
  i826.handleRange = i827[2]
  i826.deadZone = i827[3]
  i826.axisOptions = i827[4]
  i826.snapX = !!i827[5]
  i826.snapY = !!i827[6]
  request.r(i827[7], i827[8], 0, i826, 'handle')
  return i826
}

Deserializers["UnityEngine.EventSystems.EventSystem"] = function (request, data, root) {
  var i828 = root || request.c( 'UnityEngine.EventSystems.EventSystem' )
  var i829 = data
  request.r(i829[0], i829[1], 0, i828, 'm_FirstSelected')
  i828.m_sendNavigationEvents = !!i829[2]
  i828.m_DragThreshold = i829[3]
  return i828
}

Deserializers["UnityEngine.EventSystems.StandaloneInputModule"] = function (request, data, root) {
  var i830 = root || request.c( 'UnityEngine.EventSystems.StandaloneInputModule' )
  var i831 = data
  i830.m_HorizontalAxis = i831[0]
  i830.m_VerticalAxis = i831[1]
  i830.m_SubmitButton = i831[2]
  i830.m_CancelButton = i831[3]
  i830.m_InputActionsPerSecond = i831[4]
  i830.m_RepeatDelay = i831[5]
  i830.m_ForceModuleActive = !!i831[6]
  i830.m_SendPointerHoverToParent = !!i831[7]
  return i830
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Rigidbody2D"] = function (request, data, root) {
  var i832 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Rigidbody2D' )
  var i833 = data
  i832.bodyType = i833[0]
  request.r(i833[1], i833[2], 0, i832, 'material')
  i832.simulated = !!i833[3]
  i832.useAutoMass = !!i833[4]
  i832.mass = i833[5]
  i832.drag = i833[6]
  i832.angularDrag = i833[7]
  i832.gravityScale = i833[8]
  i832.collisionDetectionMode = i833[9]
  i832.sleepMode = i833[10]
  i832.constraints = i833[11]
  return i832
}

Deserializers["_Game._Scripts.PlayerMovement"] = function (request, data, root) {
  var i834 = root || request.c( '_Game._Scripts.PlayerMovement' )
  var i835 = data
  request.r(i835[0], i835[1], 0, i834, '_floatingJoystick')
  i834._moveSpeed = i835[2]
  request.r(i835[3], i835[4], 0, i834, '_rigidbody2D')
  request.r(i835[5], i835[6], 0, i834, '_visualsTransform')
  return i834
}

Deserializers["_Game._Scripts.PlayerAnimator"] = function (request, data, root) {
  var i836 = root || request.c( '_Game._Scripts.PlayerAnimator' )
  var i837 = data
  request.r(i837[0], i837[1], 0, i836, '_bodyTransform')
  i836._idleMoveY = i837[2]
  i836._idleMoveX = i837[3]
  i836._idleDuration = i837[4]
  i836._movingIdleSpeedMultiplier = i837[5]
  request.r(i837[6], i837[7], 0, i836, '_rigidbody2D')
  request.r(i837[8], i837[9], 0, i836, '_leftFootTransform')
  request.r(i837[10], i837[11], 0, i836, '_rightFootTransform')
  i836._footStepAngle = i837[12]
  i836._footStepDuration = i837[13]
  i836._footStepMoveX = i837[14]
  return i836
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i838 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i839 = data
  i838.enabled = !!i839[0]
  i838.aspect = i839[1]
  i838.orthographic = !!i839[2]
  i838.orthographicSize = i839[3]
  i838.backgroundColor = new pc.Color(i839[4], i839[5], i839[6], i839[7])
  i838.nearClipPlane = i839[8]
  i838.farClipPlane = i839[9]
  i838.fieldOfView = i839[10]
  i838.depth = i839[11]
  i838.clearFlags = i839[12]
  i838.cullingMask = i839[13]
  i838.rect = i839[14]
  request.r(i839[15], i839[16], 0, i838, 'targetTexture')
  i838.usePhysicalProperties = !!i839[17]
  i838.focalLength = i839[18]
  i838.sensorSize = new pc.Vec2( i839[19], i839[20] )
  i838.lensShift = new pc.Vec2( i839[21], i839[22] )
  i838.gateFit = i839[23]
  i838.commandBufferCount = i839[24]
  i838.cameraType = i839[25]
  return i838
}

Deserializers["LoopGames.Combat.SwordOrbitController"] = function (request, data, root) {
  var i840 = root || request.c( 'LoopGames.Combat.SwordOrbitController' )
  var i841 = data
  i840._radius = i841[0]
  i840._rotationSpeed = i841[1]
  i840._smoothTime = i841[2]
  i840._spawnGrowDuration = i841[3]
  request.r(i841[4], i841[5], 0, i840, '_center')
  request.r(i841[6], i841[7], 0, i840, '_swordPrefab')
  i840._spawnInterval = i841[8]
  return i840
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i842 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i843 = data
  i842.ambientIntensity = i843[0]
  i842.reflectionIntensity = i843[1]
  i842.ambientMode = i843[2]
  i842.ambientLight = new pc.Color(i843[3], i843[4], i843[5], i843[6])
  i842.ambientSkyColor = new pc.Color(i843[7], i843[8], i843[9], i843[10])
  i842.ambientGroundColor = new pc.Color(i843[11], i843[12], i843[13], i843[14])
  i842.ambientEquatorColor = new pc.Color(i843[15], i843[16], i843[17], i843[18])
  i842.fogColor = new pc.Color(i843[19], i843[20], i843[21], i843[22])
  i842.fogEndDistance = i843[23]
  i842.fogStartDistance = i843[24]
  i842.fogDensity = i843[25]
  i842.fog = !!i843[26]
  request.r(i843[27], i843[28], 0, i842, 'skybox')
  i842.fogMode = i843[29]
  var i845 = i843[30]
  var i844 = []
  for(var i = 0; i < i845.length; i += 1) {
    i844.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i845[i + 0]) );
  }
  i842.lightmaps = i844
  i842.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i843[31], i842.lightProbes)
  i842.lightmapsMode = i843[32]
  i842.mixedBakeMode = i843[33]
  i842.environmentLightingMode = i843[34]
  i842.ambientProbe = new pc.SphericalHarmonicsL2(i843[35])
  i842.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i843[36])
  i842.useReferenceAmbientProbe = !!i843[37]
  request.r(i843[38], i843[39], 0, i842, 'customReflection')
  request.r(i843[40], i843[41], 0, i842, 'defaultReflection')
  i842.defaultReflectionMode = i843[42]
  i842.defaultReflectionResolution = i843[43]
  i842.sunLightObjectId = i843[44]
  i842.pixelLightCount = i843[45]
  i842.defaultReflectionHDR = !!i843[46]
  i842.hasLightDataAsset = !!i843[47]
  i842.hasManualGenerate = !!i843[48]
  return i842
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i848 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i849 = data
  request.r(i849[0], i849[1], 0, i848, 'lightmapColor')
  request.r(i849[2], i849[3], 0, i848, 'lightmapDirection')
  return i848
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i850 = root || new UnityEngine.LightProbes()
  var i851 = data
  return i850
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i858 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i859 = data
  var i861 = i859[0]
  var i860 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i861.length; i += 1) {
    i860.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i861[i + 0]));
  }
  i858.ShaderCompilationErrors = i860
  i858.name = i859[1]
  i858.guid = i859[2]
  var i863 = i859[3]
  var i862 = []
  for(var i = 0; i < i863.length; i += 1) {
    i862.push( i863[i + 0] );
  }
  i858.shaderDefinedKeywords = i862
  var i865 = i859[4]
  var i864 = []
  for(var i = 0; i < i865.length; i += 1) {
    i864.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i865[i + 0]) );
  }
  i858.passes = i864
  var i867 = i859[5]
  var i866 = []
  for(var i = 0; i < i867.length; i += 1) {
    i866.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i867[i + 0]) );
  }
  i858.usePasses = i866
  var i869 = i859[6]
  var i868 = []
  for(var i = 0; i < i869.length; i += 1) {
    i868.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i869[i + 0]) );
  }
  i858.defaultParameterValues = i868
  request.r(i859[7], i859[8], 0, i858, 'unityFallbackShader')
  i858.readDepth = !!i859[9]
  i858.isCreatedByShaderGraph = !!i859[10]
  i858.compiled = !!i859[11]
  return i858
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i872 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i873 = data
  i872.shaderName = i873[0]
  i872.errorMessage = i873[1]
  return i872
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i878 = root || new pc.UnityShaderPass()
  var i879 = data
  i878.id = i879[0]
  i878.subShaderIndex = i879[1]
  i878.name = i879[2]
  i878.passType = i879[3]
  i878.grabPassTextureName = i879[4]
  i878.usePass = !!i879[5]
  i878.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[6], i878.zTest)
  i878.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[7], i878.zWrite)
  i878.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[8], i878.culling)
  i878.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i879[9], i878.blending)
  i878.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i879[10], i878.alphaBlending)
  i878.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[11], i878.colorWriteMask)
  i878.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[12], i878.offsetUnits)
  i878.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[13], i878.offsetFactor)
  i878.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[14], i878.stencilRef)
  i878.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[15], i878.stencilReadMask)
  i878.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i879[16], i878.stencilWriteMask)
  i878.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i879[17], i878.stencilOp)
  i878.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i879[18], i878.stencilOpFront)
  i878.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i879[19], i878.stencilOpBack)
  var i881 = i879[20]
  var i880 = []
  for(var i = 0; i < i881.length; i += 1) {
    i880.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i881[i + 0]) );
  }
  i878.tags = i880
  var i883 = i879[21]
  var i882 = []
  for(var i = 0; i < i883.length; i += 1) {
    i882.push( i883[i + 0] );
  }
  i878.passDefinedKeywords = i882
  var i885 = i879[22]
  var i884 = []
  for(var i = 0; i < i885.length; i += 1) {
    i884.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i885[i + 0]) );
  }
  i878.passDefinedKeywordGroups = i884
  var i887 = i879[23]
  var i886 = []
  for(var i = 0; i < i887.length; i += 1) {
    i886.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i887[i + 0]) );
  }
  i878.variants = i886
  var i889 = i879[24]
  var i888 = []
  for(var i = 0; i < i889.length; i += 1) {
    i888.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i889[i + 0]) );
  }
  i878.excludedVariants = i888
  i878.hasDepthReader = !!i879[25]
  return i878
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i890 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i891 = data
  i890.val = i891[0]
  i890.name = i891[1]
  return i890
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i892 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i893 = data
  i892.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i893[0], i892.src)
  i892.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i893[1], i892.dst)
  i892.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i893[2], i892.op)
  return i892
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i894 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i895 = data
  i894.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i895[0], i894.pass)
  i894.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i895[1], i894.fail)
  i894.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i895[2], i894.zFail)
  i894.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i895[3], i894.comp)
  return i894
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i898 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i899 = data
  i898.name = i899[0]
  i898.value = i899[1]
  return i898
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i902 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i903 = data
  var i905 = i903[0]
  var i904 = []
  for(var i = 0; i < i905.length; i += 1) {
    i904.push( i905[i + 0] );
  }
  i902.keywords = i904
  i902.hasDiscard = !!i903[1]
  return i902
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i908 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i909 = data
  i908.passId = i909[0]
  i908.subShaderIndex = i909[1]
  var i911 = i909[2]
  var i910 = []
  for(var i = 0; i < i911.length; i += 1) {
    i910.push( i911[i + 0] );
  }
  i908.keywords = i910
  i908.vertexProgram = i909[3]
  i908.fragmentProgram = i909[4]
  i908.exportedForWebGl2 = !!i909[5]
  i908.readDepth = !!i909[6]
  return i908
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i914 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i915 = data
  request.r(i915[0], i915[1], 0, i914, 'shader')
  i914.pass = i915[2]
  return i914
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i918 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i919 = data
  i918.name = i919[0]
  i918.type = i919[1]
  i918.value = new pc.Vec4( i919[2], i919[3], i919[4], i919[5] )
  i918.textureValue = i919[6]
  i918.shaderPropertyFlag = i919[7]
  return i918
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i920 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i921 = data
  i920.name = i921[0]
  request.r(i921[1], i921[2], 0, i920, 'texture')
  i920.aabb = i921[3]
  i920.vertices = i921[4]
  i920.triangles = i921[5]
  i920.textureRect = UnityEngine.Rect.MinMaxRect(i921[6], i921[7], i921[8], i921[9])
  i920.packedRect = UnityEngine.Rect.MinMaxRect(i921[10], i921[11], i921[12], i921[13])
  i920.border = new pc.Vec4( i921[14], i921[15], i921[16], i921[17] )
  i920.transparency = i921[18]
  i920.bounds = i921[19]
  i920.pixelsPerUnit = i921[20]
  i920.textureWidth = i921[21]
  i920.textureHeight = i921[22]
  i920.nativeSize = new pc.Vec2( i921[23], i921[24] )
  i920.pivot = new pc.Vec2( i921[25], i921[26] )
  i920.textureRectOffset = new pc.Vec2( i921[27], i921[28] )
  return i920
}

Deserializers["DG.Tweening.Core.DOTweenSettings"] = function (request, data, root) {
  var i922 = root || request.c( 'DG.Tweening.Core.DOTweenSettings' )
  var i923 = data
  i922.useSafeMode = !!i923[0]
  i922.safeModeOptions = request.d('DG.Tweening.Core.DOTweenSettings+SafeModeOptions', i923[1], i922.safeModeOptions)
  i922.timeScale = i923[2]
  i922.unscaledTimeScale = i923[3]
  i922.useSmoothDeltaTime = !!i923[4]
  i922.maxSmoothUnscaledTime = i923[5]
  i922.rewindCallbackMode = i923[6]
  i922.showUnityEditorReport = !!i923[7]
  i922.logBehaviour = i923[8]
  i922.drawGizmos = !!i923[9]
  i922.defaultRecyclable = !!i923[10]
  i922.defaultAutoPlay = i923[11]
  i922.defaultUpdateType = i923[12]
  i922.defaultTimeScaleIndependent = !!i923[13]
  i922.defaultEaseType = i923[14]
  i922.defaultEaseOvershootOrAmplitude = i923[15]
  i922.defaultEasePeriod = i923[16]
  i922.defaultAutoKill = !!i923[17]
  i922.defaultLoopType = i923[18]
  i922.debugMode = !!i923[19]
  i922.debugStoreTargetId = !!i923[20]
  i922.showPreviewPanel = !!i923[21]
  i922.storeSettingsLocation = i923[22]
  i922.modules = request.d('DG.Tweening.Core.DOTweenSettings+ModulesSetup', i923[23], i922.modules)
  i922.createASMDEF = !!i923[24]
  i922.showPlayingTweens = !!i923[25]
  i922.showPausedTweens = !!i923[26]
  return i922
}

Deserializers["DG.Tweening.Core.DOTweenSettings+SafeModeOptions"] = function (request, data, root) {
  var i924 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+SafeModeOptions' )
  var i925 = data
  i924.logBehaviour = i925[0]
  i924.nestedTweenFailureBehaviour = i925[1]
  return i924
}

Deserializers["DG.Tweening.Core.DOTweenSettings+ModulesSetup"] = function (request, data, root) {
  var i926 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+ModulesSetup' )
  var i927 = data
  i926.showPanel = !!i927[0]
  i926.audioEnabled = !!i927[1]
  i926.physicsEnabled = !!i927[2]
  i926.physics2DEnabled = !!i927[3]
  i926.spriteEnabled = !!i927[4]
  i926.uiEnabled = !!i927[5]
  i926.textMeshProEnabled = !!i927[6]
  i926.tk2DEnabled = !!i927[7]
  i926.deAudioEnabled = !!i927[8]
  i926.deUnityExtendedEnabled = !!i927[9]
  i926.epoOutlineEnabled = !!i927[10]
  return i926
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i928 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i929 = data
  var i931 = i929[0]
  var i930 = []
  for(var i = 0; i < i931.length; i += 1) {
    i930.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i931[i + 0]) );
  }
  i928.files = i930
  i928.componentToPrefabIds = i929[1]
  return i928
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i934 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i935 = data
  i934.path = i935[0]
  request.r(i935[1], i935[2], 0, i934, 'unityObject')
  return i934
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i936 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i937 = data
  var i939 = i937[0]
  var i938 = []
  for(var i = 0; i < i939.length; i += 1) {
    i938.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i939[i + 0]) );
  }
  i936.scriptsExecutionOrder = i938
  var i941 = i937[1]
  var i940 = []
  for(var i = 0; i < i941.length; i += 1) {
    i940.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i941[i + 0]) );
  }
  i936.sortingLayers = i940
  var i943 = i937[2]
  var i942 = []
  for(var i = 0; i < i943.length; i += 1) {
    i942.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i943[i + 0]) );
  }
  i936.cullingLayers = i942
  i936.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i937[3], i936.timeSettings)
  i936.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i937[4], i936.physicsSettings)
  i936.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i937[5], i936.physics2DSettings)
  i936.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i937[6], i936.qualitySettings)
  i936.enableRealtimeShadows = !!i937[7]
  i936.enableAutoInstancing = !!i937[8]
  i936.enableDynamicBatching = !!i937[9]
  i936.lightmapEncodingQuality = i937[10]
  i936.desiredColorSpace = i937[11]
  var i945 = i937[12]
  var i944 = []
  for(var i = 0; i < i945.length; i += 1) {
    i944.push( i945[i + 0] );
  }
  i936.allTags = i944
  return i936
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i948 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i949 = data
  i948.name = i949[0]
  i948.value = i949[1]
  return i948
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i952 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i953 = data
  i952.id = i953[0]
  i952.name = i953[1]
  i952.value = i953[2]
  return i952
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i956 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i957 = data
  i956.id = i957[0]
  i956.name = i957[1]
  return i956
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i958 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i959 = data
  i958.fixedDeltaTime = i959[0]
  i958.maximumDeltaTime = i959[1]
  i958.timeScale = i959[2]
  i958.maximumParticleTimestep = i959[3]
  return i958
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i960 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i961 = data
  i960.gravity = new pc.Vec3( i961[0], i961[1], i961[2] )
  i960.defaultSolverIterations = i961[3]
  i960.bounceThreshold = i961[4]
  i960.autoSyncTransforms = !!i961[5]
  i960.autoSimulation = !!i961[6]
  var i963 = i961[7]
  var i962 = []
  for(var i = 0; i < i963.length; i += 1) {
    i962.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i963[i + 0]) );
  }
  i960.collisionMatrix = i962
  return i960
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i966 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i967 = data
  i966.enabled = !!i967[0]
  i966.layerId = i967[1]
  i966.otherLayerId = i967[2]
  return i966
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i968 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i969 = data
  request.r(i969[0], i969[1], 0, i968, 'material')
  i968.gravity = new pc.Vec2( i969[2], i969[3] )
  i968.positionIterations = i969[4]
  i968.velocityIterations = i969[5]
  i968.velocityThreshold = i969[6]
  i968.maxLinearCorrection = i969[7]
  i968.maxAngularCorrection = i969[8]
  i968.maxTranslationSpeed = i969[9]
  i968.maxRotationSpeed = i969[10]
  i968.baumgarteScale = i969[11]
  i968.baumgarteTOIScale = i969[12]
  i968.timeToSleep = i969[13]
  i968.linearSleepTolerance = i969[14]
  i968.angularSleepTolerance = i969[15]
  i968.defaultContactOffset = i969[16]
  i968.autoSimulation = !!i969[17]
  i968.queriesHitTriggers = !!i969[18]
  i968.queriesStartInColliders = !!i969[19]
  i968.callbacksOnDisable = !!i969[20]
  i968.reuseCollisionCallbacks = !!i969[21]
  i968.autoSyncTransforms = !!i969[22]
  var i971 = i969[23]
  var i970 = []
  for(var i = 0; i < i971.length; i += 1) {
    i970.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i971[i + 0]) );
  }
  i968.collisionMatrix = i970
  return i968
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i974 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i975 = data
  i974.enabled = !!i975[0]
  i974.layerId = i975[1]
  i974.otherLayerId = i975[2]
  return i974
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i976 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i977 = data
  var i979 = i977[0]
  var i978 = []
  for(var i = 0; i < i979.length; i += 1) {
    i978.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i979[i + 0]) );
  }
  i976.qualityLevels = i978
  var i981 = i977[1]
  var i980 = []
  for(var i = 0; i < i981.length; i += 1) {
    i980.push( i981[i + 0] );
  }
  i976.names = i980
  i976.shadows = i977[2]
  i976.anisotropicFiltering = i977[3]
  i976.antiAliasing = i977[4]
  i976.lodBias = i977[5]
  i976.shadowCascades = i977[6]
  i976.shadowDistance = i977[7]
  i976.shadowmaskMode = i977[8]
  i976.shadowProjection = i977[9]
  i976.shadowResolution = i977[10]
  i976.softParticles = !!i977[11]
  i976.softVegetation = !!i977[12]
  i976.activeColorSpace = i977[13]
  i976.desiredColorSpace = i977[14]
  i976.masterTextureLimit = i977[15]
  i976.maxQueuedFrames = i977[16]
  i976.particleRaycastBudget = i977[17]
  i976.pixelLightCount = i977[18]
  i976.realtimeReflectionProbes = !!i977[19]
  i976.shadowCascade2Split = i977[20]
  i976.shadowCascade4Split = new pc.Vec3( i977[21], i977[22], i977[23] )
  i976.streamingMipmapsActive = !!i977[24]
  i976.vSyncCount = i977[25]
  i976.asyncUploadBufferSize = i977[26]
  i976.asyncUploadTimeSlice = i977[27]
  i976.billboardsFaceCameraPosition = !!i977[28]
  i976.shadowNearPlaneOffset = i977[29]
  i976.streamingMipmapsMemoryBudget = i977[30]
  i976.maximumLODLevel = i977[31]
  i976.streamingMipmapsAddAllCameras = !!i977[32]
  i976.streamingMipmapsMaxLevelReduction = i977[33]
  i976.streamingMipmapsRenderersPerFrame = i977[34]
  i976.resolutionScalingFixedDPIFactor = i977[35]
  i976.streamingMipmapsMaxFileIORequests = i977[36]
  i976.currentQualityLevel = i977[37]
  return i976
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"color":16,"sprite":20,"flipX":22,"flipY":23,"drawMode":24,"size":25,"tileMode":27,"adaptiveModeThreshold":28,"maskInteraction":29,"spriteSortPoint":30},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.RectTransform":{"pivot":0,"anchorMin":2,"anchorMax":4,"sizeDelta":6,"anchoredPosition3D":8,"rotation":11,"scale":15},"Luna.Unity.DTO.UnityEngine.Components.Canvas":{"enabled":0,"planeDistance":1,"referencePixelsPerUnit":2,"isFallbackOverlay":3,"renderMode":4,"renderOrder":5,"sortingLayerName":6,"sortingOrder":7,"scaleFactor":8,"worldCamera":9,"overrideSorting":11,"pixelPerfect":12,"targetDisplay":13,"overridePixelPerfect":14},"Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer":{"cullTransparentMesh":0},"Luna.Unity.DTO.UnityEngine.Components.Rigidbody2D":{"bodyType":0,"material":1,"simulated":3,"useAutoMass":4,"mass":5,"drag":6,"angularDrag":7,"gravityScale":8,"collisionDetectionMode":9,"sleepMode":10,"constraints":11},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"enabled":0,"aspect":1,"orthographic":2,"orthographicSize":3,"backgroundColor":4,"nearClipPlane":8,"farClipPlane":9,"fieldOfView":10,"depth":11,"clearFlags":12,"cullingMask":13,"rect":14,"targetTexture":15,"usePhysicalProperties":17,"focalLength":18,"sensorSize":19,"lensShift":21,"gateFit":23,"commandBufferCount":24,"cameraType":25},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"referenceAmbientProbe":36,"useReferenceAmbientProbe":37,"customReflection":38,"defaultReflection":40,"defaultReflectionMode":42,"defaultReflectionResolution":43,"sunLightObjectId":44,"pixelLightCount":45,"defaultReflectionHDR":46,"hasLightDataAsset":47,"hasManualGenerate":48},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"isCreatedByShaderGraph":10,"compiled":11},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Textures.Sprite":{"name":0,"texture":1,"aabb":3,"vertices":4,"triangles":5,"textureRect":6,"packedRect":10,"border":14,"transparency":18,"bounds":19,"pixelsPerUnit":20,"textureWidth":21,"textureHeight":22,"nativeSize":23,"pivot":25,"textureRectOffset":27},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableDynamicBatching":9,"lightmapEncodingQuality":10,"desiredColorSpace":11,"allTags":12},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37}}

Deserializers.requiredComponents = {"27":[28],"29":[28],"30":[28],"31":[28],"32":[28],"33":[28],"34":[35],"36":[22],"37":[38],"39":[38],"40":[38],"41":[38],"42":[38],"43":[38],"44":[38],"45":[19],"46":[19],"47":[19],"48":[19],"49":[19],"50":[19],"51":[19],"52":[19],"53":[19],"54":[19],"55":[19],"56":[19],"57":[19],"58":[22],"59":[60],"61":[62],"63":[62],"10":[9],"64":[65],"66":[3],"67":[65],"68":[9],"69":[9],"13":[10],"15":[14,9],"70":[9],"12":[10],"71":[9],"72":[9],"73":[9],"74":[9],"75":[9],"76":[9],"77":[9],"78":[9],"79":[9],"80":[14,9],"81":[9],"82":[9],"83":[9],"84":[9],"85":[14,9],"86":[9],"87":[17],"88":[17],"18":[17],"89":[17],"90":[22],"91":[22],"92":[93],"94":[22],"95":[9],"96":[60,9],"97":[9,14],"98":[9],"99":[14,9],"100":[60],"101":[14,9],"102":[9],"103":[65]}

Deserializers.types = ["UnityEngine.Transform","UnityEngine.MonoBehaviour","Tile","UnityEngine.SpriteRenderer","UnityEngine.Material","UnityEngine.Sprite","UnityEngine.Shader","LoopGames.Playable.GridMapBuilder","UnityEngine.GameObject","UnityEngine.RectTransform","UnityEngine.Canvas","UnityEngine.EventSystems.UIBehaviour","UnityEngine.UI.CanvasScaler","UnityEngine.UI.GraphicRaycaster","UnityEngine.CanvasRenderer","UnityEngine.UI.Image","FloatingJoystick","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.Rigidbody2D","_Game._Scripts.PlayerMovement","_Game._Scripts.PlayerAnimator","UnityEngine.Camera","UnityEngine.AudioListener","LoopGames.Combat.SwordOrbitController","UnityEngine.Texture2D","DG.Tweening.Core.DOTweenSettings","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.SkinnedMeshRenderer","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Rigidbody","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.MeshRenderer","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","UnityEngine.U2D.Animation.SpriteSkin","Unity.VisualScripting.ScriptMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Text","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","UnityEngine.U2D.SpriteShapeController","UnityEngine.U2D.SpriteShapeRenderer","UnityEngine.U2D.PixelPerfectCamera","TMPro.TextContainer","TMPro.TextMeshPro","TMPro.TextMeshProUGUI","TMPro.TMP_Dropdown","TMPro.TMP_SelectionCaret","TMPro.TMP_SubMesh","TMPro.TMP_SubMeshUI","TMPro.TMP_Text","Unity.VisualScripting.StateMachine"]

Deserializers.unityVersion = "2022.3.51f1";

Deserializers.productName = "Fruit-Hook";

Deserializers.lunaInitializationTime = "12/13/2025 11:26:57";

Deserializers.lunaDaysRunning = "0.2";

Deserializers.lunaVersion = "6.2.1";

Deserializers.lunaSHA = "28f227c1b455c28500de29df936f0d1376ee9c43";

Deserializers.creativeName = "";

Deserializers.lunaAppID = "23025";

Deserializers.projectId = "14a77b16bfe8a47b8adf999ff416b621";

Deserializers.packagesInfo = "com.unity.textmeshpro: 3.0.7\ncom.unity.timeline: 1.7.6\ncom.unity.ugui: 1.0.0";

Deserializers.externalJsLibraries = "";

Deserializers.androidLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.androidLink?window.$environment.packageConfig.androidLink:'Empty';

Deserializers.iosLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.iosLink?window.$environment.packageConfig.iosLink:'Empty';

Deserializers.base64Enabled = "False";

Deserializers.minifyEnabled = "True";

Deserializers.isForceUncompressed = "False";

Deserializers.isAntiAliasingEnabled = "False";

Deserializers.isRuntimeAnalysisEnabledForCode = "True";

Deserializers.runtimeAnalysisExcludedClassesCount = "1856";

Deserializers.runtimeAnalysisExcludedMethodsCount = "3726";

Deserializers.runtimeAnalysisExcludedModules = "physics3d, particle-system, mecanim-wasm";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isReferenceAmbientProbeBaked = "False";

Deserializers.isLunaCompilerV2Used = "True";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "WebGL";

Deserializers.applicationIdentifier = "com.DefaultCompany.Fruit-Hook";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 28;

Deserializers.linearColorSpace = false;

Deserializers.buildID = "c59e8a59-6ac8-4d5c-bc5a-d9d2426fd8ae";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["DG","Tweening","DOTween","RuntimeOnLoad"],["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"]],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

