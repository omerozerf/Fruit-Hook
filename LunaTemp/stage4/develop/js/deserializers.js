var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i206 = root || request.c( 'UnityEngine.JointSpring' )
  var i207 = data
  i206.spring = i207[0]
  i206.damper = i207[1]
  i206.targetPosition = i207[2]
  return i206
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i208 = root || request.c( 'UnityEngine.JointMotor' )
  var i209 = data
  i208.m_TargetVelocity = i209[0]
  i208.m_Force = i209[1]
  i208.m_FreeSpin = i209[2]
  return i208
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i210 = root || request.c( 'UnityEngine.JointLimits' )
  var i211 = data
  i210.m_Min = i211[0]
  i210.m_Max = i211[1]
  i210.m_Bounciness = i211[2]
  i210.m_BounceMinVelocity = i211[3]
  i210.m_ContactDistance = i211[4]
  i210.minBounce = i211[5]
  i210.maxBounce = i211[6]
  return i210
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i212 = root || request.c( 'UnityEngine.JointDrive' )
  var i213 = data
  i212.m_PositionSpring = i213[0]
  i212.m_PositionDamper = i213[1]
  i212.m_MaximumForce = i213[2]
  i212.m_UseAcceleration = i213[3]
  return i212
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i214 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i215 = data
  i214.m_Spring = i215[0]
  i214.m_Damper = i215[1]
  return i214
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i216 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i217 = data
  i216.m_Limit = i217[0]
  i216.m_Bounciness = i217[1]
  i216.m_ContactDistance = i217[2]
  return i216
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i218 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i219 = data
  i218.m_ExtremumSlip = i219[0]
  i218.m_ExtremumValue = i219[1]
  i218.m_AsymptoteSlip = i219[2]
  i218.m_AsymptoteValue = i219[3]
  i218.m_Stiffness = i219[4]
  return i218
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i220 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i221 = data
  i220.m_LowerAngle = i221[0]
  i220.m_UpperAngle = i221[1]
  return i220
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i222 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i223 = data
  i222.m_MotorSpeed = i223[0]
  i222.m_MaximumMotorTorque = i223[1]
  return i222
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i224 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i225 = data
  i224.m_DampingRatio = i225[0]
  i224.m_Frequency = i225[1]
  i224.m_Angle = i225[2]
  return i224
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i226 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i227 = data
  i226.m_LowerTranslation = i227[0]
  i226.m_UpperTranslation = i227[1]
  return i226
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i228 = root || new pc.UnityMaterial()
  var i229 = data
  i228.name = i229[0]
  request.r(i229[1], i229[2], 0, i228, 'shader')
  i228.renderQueue = i229[3]
  i228.enableInstancing = !!i229[4]
  var i231 = i229[5]
  var i230 = []
  for(var i = 0; i < i231.length; i += 1) {
    i230.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i231[i + 0]) );
  }
  i228.floatParameters = i230
  var i233 = i229[6]
  var i232 = []
  for(var i = 0; i < i233.length; i += 1) {
    i232.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i233[i + 0]) );
  }
  i228.colorParameters = i232
  var i235 = i229[7]
  var i234 = []
  for(var i = 0; i < i235.length; i += 1) {
    i234.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i235[i + 0]) );
  }
  i228.vectorParameters = i234
  var i237 = i229[8]
  var i236 = []
  for(var i = 0; i < i237.length; i += 1) {
    i236.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i237[i + 0]) );
  }
  i228.textureParameters = i236
  var i239 = i229[9]
  var i238 = []
  for(var i = 0; i < i239.length; i += 1) {
    i238.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i239[i + 0]) );
  }
  i228.materialFlags = i238
  return i228
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i242 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i243 = data
  i242.name = i243[0]
  i242.value = i243[1]
  return i242
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i246 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i247 = data
  i246.name = i247[0]
  i246.value = new pc.Color(i247[1], i247[2], i247[3], i247[4])
  return i246
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i250 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i251 = data
  i250.name = i251[0]
  i250.value = new pc.Vec4( i251[1], i251[2], i251[3], i251[4] )
  return i250
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i254 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i255 = data
  i254.name = i255[0]
  request.r(i255[1], i255[2], 0, i254, 'value')
  return i254
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i258 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i259 = data
  i258.name = i259[0]
  i258.enabled = !!i259[1]
  return i258
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i260 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i261 = data
  i260.name = i261[0]
  i260.width = i261[1]
  i260.height = i261[2]
  i260.mipmapCount = i261[3]
  i260.anisoLevel = i261[4]
  i260.filterMode = i261[5]
  i260.hdr = !!i261[6]
  i260.format = i261[7]
  i260.wrapMode = i261[8]
  i260.alphaIsTransparency = !!i261[9]
  i260.alphaSource = i261[10]
  i260.graphicsFormat = i261[11]
  i260.sRGBTexture = !!i261[12]
  i260.desiredColorSpace = i261[13]
  i260.wrapU = i261[14]
  i260.wrapV = i261[15]
  return i260
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i262 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i263 = data
  i262.name = i263[0]
  i262.index = i263[1]
  i262.startup = !!i263[2]
  return i262
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i264 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i265 = data
  i264.position = new pc.Vec3( i265[0], i265[1], i265[2] )
  i264.scale = new pc.Vec3( i265[3], i265[4], i265[5] )
  i264.rotation = new pc.Quat(i265[6], i265[7], i265[8], i265[9])
  return i264
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i266 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i267 = data
  i266.enabled = !!i267[0]
  i266.aspect = i267[1]
  i266.orthographic = !!i267[2]
  i266.orthographicSize = i267[3]
  i266.backgroundColor = new pc.Color(i267[4], i267[5], i267[6], i267[7])
  i266.nearClipPlane = i267[8]
  i266.farClipPlane = i267[9]
  i266.fieldOfView = i267[10]
  i266.depth = i267[11]
  i266.clearFlags = i267[12]
  i266.cullingMask = i267[13]
  i266.rect = i267[14]
  request.r(i267[15], i267[16], 0, i266, 'targetTexture')
  i266.usePhysicalProperties = !!i267[17]
  i266.focalLength = i267[18]
  i266.sensorSize = new pc.Vec2( i267[19], i267[20] )
  i266.lensShift = new pc.Vec2( i267[21], i267[22] )
  i266.gateFit = i267[23]
  i266.commandBufferCount = i267[24]
  i266.cameraType = i267[25]
  return i266
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i268 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i269 = data
  i268.name = i269[0]
  i268.tagId = i269[1]
  i268.enabled = !!i269[2]
  i268.isStatic = !!i269[3]
  i268.layer = i269[4]
  return i268
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer"] = function (request, data, root) {
  var i270 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer' )
  var i271 = data
  i270.enabled = !!i271[0]
  request.r(i271[1], i271[2], 0, i270, 'sharedMaterial')
  var i273 = i271[3]
  var i272 = []
  for(var i = 0; i < i273.length; i += 2) {
  request.r(i273[i + 0], i273[i + 1], 2, i272, '')
  }
  i270.sharedMaterials = i272
  i270.receiveShadows = !!i271[4]
  i270.shadowCastingMode = i271[5]
  i270.sortingLayerID = i271[6]
  i270.sortingOrder = i271[7]
  i270.lightmapIndex = i271[8]
  i270.lightmapSceneIndex = i271[9]
  i270.lightmapScaleOffset = new pc.Vec4( i271[10], i271[11], i271[12], i271[13] )
  i270.lightProbeUsage = i271[14]
  i270.reflectionProbeUsage = i271[15]
  i270.color = new pc.Color(i271[16], i271[17], i271[18], i271[19])
  request.r(i271[20], i271[21], 0, i270, 'sprite')
  i270.flipX = !!i271[22]
  i270.flipY = !!i271[23]
  i270.drawMode = i271[24]
  i270.size = new pc.Vec2( i271[25], i271[26] )
  i270.tileMode = i271[27]
  i270.adaptiveModeThreshold = i271[28]
  i270.maskInteraction = i271[29]
  i270.spriteSortPoint = i271[30]
  return i270
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i276 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i277 = data
  i276.ambientIntensity = i277[0]
  i276.reflectionIntensity = i277[1]
  i276.ambientMode = i277[2]
  i276.ambientLight = new pc.Color(i277[3], i277[4], i277[5], i277[6])
  i276.ambientSkyColor = new pc.Color(i277[7], i277[8], i277[9], i277[10])
  i276.ambientGroundColor = new pc.Color(i277[11], i277[12], i277[13], i277[14])
  i276.ambientEquatorColor = new pc.Color(i277[15], i277[16], i277[17], i277[18])
  i276.fogColor = new pc.Color(i277[19], i277[20], i277[21], i277[22])
  i276.fogEndDistance = i277[23]
  i276.fogStartDistance = i277[24]
  i276.fogDensity = i277[25]
  i276.fog = !!i277[26]
  request.r(i277[27], i277[28], 0, i276, 'skybox')
  i276.fogMode = i277[29]
  var i279 = i277[30]
  var i278 = []
  for(var i = 0; i < i279.length; i += 1) {
    i278.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i279[i + 0]) );
  }
  i276.lightmaps = i278
  i276.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i277[31], i276.lightProbes)
  i276.lightmapsMode = i277[32]
  i276.mixedBakeMode = i277[33]
  i276.environmentLightingMode = i277[34]
  i276.ambientProbe = new pc.SphericalHarmonicsL2(i277[35])
  i276.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i277[36])
  i276.useReferenceAmbientProbe = !!i277[37]
  request.r(i277[38], i277[39], 0, i276, 'customReflection')
  request.r(i277[40], i277[41], 0, i276, 'defaultReflection')
  i276.defaultReflectionMode = i277[42]
  i276.defaultReflectionResolution = i277[43]
  i276.sunLightObjectId = i277[44]
  i276.pixelLightCount = i277[45]
  i276.defaultReflectionHDR = !!i277[46]
  i276.hasLightDataAsset = !!i277[47]
  i276.hasManualGenerate = !!i277[48]
  return i276
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i282 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i283 = data
  request.r(i283[0], i283[1], 0, i282, 'lightmapColor')
  request.r(i283[2], i283[3], 0, i282, 'lightmapDirection')
  return i282
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i284 = root || new UnityEngine.LightProbes()
  var i285 = data
  return i284
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i292 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i293 = data
  var i295 = i293[0]
  var i294 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i295.length; i += 1) {
    i294.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i295[i + 0]));
  }
  i292.ShaderCompilationErrors = i294
  i292.name = i293[1]
  i292.guid = i293[2]
  var i297 = i293[3]
  var i296 = []
  for(var i = 0; i < i297.length; i += 1) {
    i296.push( i297[i + 0] );
  }
  i292.shaderDefinedKeywords = i296
  var i299 = i293[4]
  var i298 = []
  for(var i = 0; i < i299.length; i += 1) {
    i298.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i299[i + 0]) );
  }
  i292.passes = i298
  var i301 = i293[5]
  var i300 = []
  for(var i = 0; i < i301.length; i += 1) {
    i300.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i301[i + 0]) );
  }
  i292.usePasses = i300
  var i303 = i293[6]
  var i302 = []
  for(var i = 0; i < i303.length; i += 1) {
    i302.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i303[i + 0]) );
  }
  i292.defaultParameterValues = i302
  request.r(i293[7], i293[8], 0, i292, 'unityFallbackShader')
  i292.readDepth = !!i293[9]
  i292.isCreatedByShaderGraph = !!i293[10]
  i292.compiled = !!i293[11]
  return i292
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i306 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i307 = data
  i306.shaderName = i307[0]
  i306.errorMessage = i307[1]
  return i306
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i312 = root || new pc.UnityShaderPass()
  var i313 = data
  i312.id = i313[0]
  i312.subShaderIndex = i313[1]
  i312.name = i313[2]
  i312.passType = i313[3]
  i312.grabPassTextureName = i313[4]
  i312.usePass = !!i313[5]
  i312.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[6], i312.zTest)
  i312.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[7], i312.zWrite)
  i312.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[8], i312.culling)
  i312.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i313[9], i312.blending)
  i312.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i313[10], i312.alphaBlending)
  i312.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[11], i312.colorWriteMask)
  i312.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[12], i312.offsetUnits)
  i312.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[13], i312.offsetFactor)
  i312.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[14], i312.stencilRef)
  i312.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[15], i312.stencilReadMask)
  i312.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i313[16], i312.stencilWriteMask)
  i312.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i313[17], i312.stencilOp)
  i312.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i313[18], i312.stencilOpFront)
  i312.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i313[19], i312.stencilOpBack)
  var i315 = i313[20]
  var i314 = []
  for(var i = 0; i < i315.length; i += 1) {
    i314.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i315[i + 0]) );
  }
  i312.tags = i314
  var i317 = i313[21]
  var i316 = []
  for(var i = 0; i < i317.length; i += 1) {
    i316.push( i317[i + 0] );
  }
  i312.passDefinedKeywords = i316
  var i319 = i313[22]
  var i318 = []
  for(var i = 0; i < i319.length; i += 1) {
    i318.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i319[i + 0]) );
  }
  i312.passDefinedKeywordGroups = i318
  var i321 = i313[23]
  var i320 = []
  for(var i = 0; i < i321.length; i += 1) {
    i320.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i321[i + 0]) );
  }
  i312.variants = i320
  var i323 = i313[24]
  var i322 = []
  for(var i = 0; i < i323.length; i += 1) {
    i322.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i323[i + 0]) );
  }
  i312.excludedVariants = i322
  i312.hasDepthReader = !!i313[25]
  return i312
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i324 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i325 = data
  i324.val = i325[0]
  i324.name = i325[1]
  return i324
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i326 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i327 = data
  i326.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i327[0], i326.src)
  i326.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i327[1], i326.dst)
  i326.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i327[2], i326.op)
  return i326
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i328 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i329 = data
  i328.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i329[0], i328.pass)
  i328.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i329[1], i328.fail)
  i328.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i329[2], i328.zFail)
  i328.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i329[3], i328.comp)
  return i328
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i332 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i333 = data
  i332.name = i333[0]
  i332.value = i333[1]
  return i332
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i336 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i337 = data
  var i339 = i337[0]
  var i338 = []
  for(var i = 0; i < i339.length; i += 1) {
    i338.push( i339[i + 0] );
  }
  i336.keywords = i338
  i336.hasDiscard = !!i337[1]
  return i336
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i342 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i343 = data
  i342.passId = i343[0]
  i342.subShaderIndex = i343[1]
  var i345 = i343[2]
  var i344 = []
  for(var i = 0; i < i345.length; i += 1) {
    i344.push( i345[i + 0] );
  }
  i342.keywords = i344
  i342.vertexProgram = i343[3]
  i342.fragmentProgram = i343[4]
  i342.exportedForWebGl2 = !!i343[5]
  i342.readDepth = !!i343[6]
  return i342
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i348 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i349 = data
  request.r(i349[0], i349[1], 0, i348, 'shader')
  i348.pass = i349[2]
  return i348
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i352 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i353 = data
  i352.name = i353[0]
  i352.type = i353[1]
  i352.value = new pc.Vec4( i353[2], i353[3], i353[4], i353[5] )
  i352.textureValue = i353[6]
  i352.shaderPropertyFlag = i353[7]
  return i352
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i354 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i355 = data
  i354.name = i355[0]
  request.r(i355[1], i355[2], 0, i354, 'texture')
  i354.aabb = i355[3]
  i354.vertices = i355[4]
  i354.triangles = i355[5]
  i354.textureRect = UnityEngine.Rect.MinMaxRect(i355[6], i355[7], i355[8], i355[9])
  i354.packedRect = UnityEngine.Rect.MinMaxRect(i355[10], i355[11], i355[12], i355[13])
  i354.border = new pc.Vec4( i355[14], i355[15], i355[16], i355[17] )
  i354.transparency = i355[18]
  i354.bounds = i355[19]
  i354.pixelsPerUnit = i355[20]
  i354.textureWidth = i355[21]
  i354.textureHeight = i355[22]
  i354.nativeSize = new pc.Vec2( i355[23], i355[24] )
  i354.pivot = new pc.Vec2( i355[25], i355[26] )
  i354.textureRectOffset = new pc.Vec2( i355[27], i355[28] )
  return i354
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i356 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i357 = data
  var i359 = i357[0]
  var i358 = []
  for(var i = 0; i < i359.length; i += 1) {
    i358.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i359[i + 0]) );
  }
  i356.files = i358
  i356.componentToPrefabIds = i357[1]
  return i356
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i362 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i363 = data
  i362.path = i363[0]
  request.r(i363[1], i363[2], 0, i362, 'unityObject')
  return i362
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i364 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i365 = data
  var i367 = i365[0]
  var i366 = []
  for(var i = 0; i < i367.length; i += 1) {
    i366.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i367[i + 0]) );
  }
  i364.scriptsExecutionOrder = i366
  var i369 = i365[1]
  var i368 = []
  for(var i = 0; i < i369.length; i += 1) {
    i368.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i369[i + 0]) );
  }
  i364.sortingLayers = i368
  var i371 = i365[2]
  var i370 = []
  for(var i = 0; i < i371.length; i += 1) {
    i370.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i371[i + 0]) );
  }
  i364.cullingLayers = i370
  i364.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i365[3], i364.timeSettings)
  i364.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i365[4], i364.physicsSettings)
  i364.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i365[5], i364.physics2DSettings)
  i364.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i365[6], i364.qualitySettings)
  i364.enableRealtimeShadows = !!i365[7]
  i364.enableAutoInstancing = !!i365[8]
  i364.enableDynamicBatching = !!i365[9]
  i364.lightmapEncodingQuality = i365[10]
  i364.desiredColorSpace = i365[11]
  var i373 = i365[12]
  var i372 = []
  for(var i = 0; i < i373.length; i += 1) {
    i372.push( i373[i + 0] );
  }
  i364.allTags = i372
  return i364
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i376 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i377 = data
  i376.name = i377[0]
  i376.value = i377[1]
  return i376
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i380 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i381 = data
  i380.id = i381[0]
  i380.name = i381[1]
  i380.value = i381[2]
  return i380
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i384 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i385 = data
  i384.id = i385[0]
  i384.name = i385[1]
  return i384
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i386 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i387 = data
  i386.fixedDeltaTime = i387[0]
  i386.maximumDeltaTime = i387[1]
  i386.timeScale = i387[2]
  i386.maximumParticleTimestep = i387[3]
  return i386
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i388 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i389 = data
  i388.gravity = new pc.Vec3( i389[0], i389[1], i389[2] )
  i388.defaultSolverIterations = i389[3]
  i388.bounceThreshold = i389[4]
  i388.autoSyncTransforms = !!i389[5]
  i388.autoSimulation = !!i389[6]
  var i391 = i389[7]
  var i390 = []
  for(var i = 0; i < i391.length; i += 1) {
    i390.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i391[i + 0]) );
  }
  i388.collisionMatrix = i390
  return i388
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i394 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i395 = data
  i394.enabled = !!i395[0]
  i394.layerId = i395[1]
  i394.otherLayerId = i395[2]
  return i394
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i396 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i397 = data
  request.r(i397[0], i397[1], 0, i396, 'material')
  i396.gravity = new pc.Vec2( i397[2], i397[3] )
  i396.positionIterations = i397[4]
  i396.velocityIterations = i397[5]
  i396.velocityThreshold = i397[6]
  i396.maxLinearCorrection = i397[7]
  i396.maxAngularCorrection = i397[8]
  i396.maxTranslationSpeed = i397[9]
  i396.maxRotationSpeed = i397[10]
  i396.baumgarteScale = i397[11]
  i396.baumgarteTOIScale = i397[12]
  i396.timeToSleep = i397[13]
  i396.linearSleepTolerance = i397[14]
  i396.angularSleepTolerance = i397[15]
  i396.defaultContactOffset = i397[16]
  i396.autoSimulation = !!i397[17]
  i396.queriesHitTriggers = !!i397[18]
  i396.queriesStartInColliders = !!i397[19]
  i396.callbacksOnDisable = !!i397[20]
  i396.reuseCollisionCallbacks = !!i397[21]
  i396.autoSyncTransforms = !!i397[22]
  var i399 = i397[23]
  var i398 = []
  for(var i = 0; i < i399.length; i += 1) {
    i398.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i399[i + 0]) );
  }
  i396.collisionMatrix = i398
  return i396
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i402 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i403 = data
  i402.enabled = !!i403[0]
  i402.layerId = i403[1]
  i402.otherLayerId = i403[2]
  return i402
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i404 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i405 = data
  var i407 = i405[0]
  var i406 = []
  for(var i = 0; i < i407.length; i += 1) {
    i406.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i407[i + 0]) );
  }
  i404.qualityLevels = i406
  var i409 = i405[1]
  var i408 = []
  for(var i = 0; i < i409.length; i += 1) {
    i408.push( i409[i + 0] );
  }
  i404.names = i408
  i404.shadows = i405[2]
  i404.anisotropicFiltering = i405[3]
  i404.antiAliasing = i405[4]
  i404.lodBias = i405[5]
  i404.shadowCascades = i405[6]
  i404.shadowDistance = i405[7]
  i404.shadowmaskMode = i405[8]
  i404.shadowProjection = i405[9]
  i404.shadowResolution = i405[10]
  i404.softParticles = !!i405[11]
  i404.softVegetation = !!i405[12]
  i404.activeColorSpace = i405[13]
  i404.desiredColorSpace = i405[14]
  i404.masterTextureLimit = i405[15]
  i404.maxQueuedFrames = i405[16]
  i404.particleRaycastBudget = i405[17]
  i404.pixelLightCount = i405[18]
  i404.realtimeReflectionProbes = !!i405[19]
  i404.shadowCascade2Split = i405[20]
  i404.shadowCascade4Split = new pc.Vec3( i405[21], i405[22], i405[23] )
  i404.streamingMipmapsActive = !!i405[24]
  i404.vSyncCount = i405[25]
  i404.asyncUploadBufferSize = i405[26]
  i404.asyncUploadTimeSlice = i405[27]
  i404.billboardsFaceCameraPosition = !!i405[28]
  i404.shadowNearPlaneOffset = i405[29]
  i404.streamingMipmapsMemoryBudget = i405[30]
  i404.maximumLODLevel = i405[31]
  i404.streamingMipmapsAddAllCameras = !!i405[32]
  i404.streamingMipmapsMaxLevelReduction = i405[33]
  i404.streamingMipmapsRenderersPerFrame = i405[34]
  i404.resolutionScalingFixedDPIFactor = i405[35]
  i404.streamingMipmapsMaxFileIORequests = i405[36]
  i404.currentQualityLevel = i405[37]
  return i404
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"enabled":0,"aspect":1,"orthographic":2,"orthographicSize":3,"backgroundColor":4,"nearClipPlane":8,"farClipPlane":9,"fieldOfView":10,"depth":11,"clearFlags":12,"cullingMask":13,"rect":14,"targetTexture":15,"usePhysicalProperties":17,"focalLength":18,"sensorSize":19,"lensShift":21,"gateFit":23,"commandBufferCount":24,"cameraType":25},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"color":16,"sprite":20,"flipX":22,"flipY":23,"drawMode":24,"size":25,"tileMode":27,"adaptiveModeThreshold":28,"maskInteraction":29,"spriteSortPoint":30},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"referenceAmbientProbe":36,"useReferenceAmbientProbe":37,"customReflection":38,"defaultReflection":40,"defaultReflectionMode":42,"defaultReflectionResolution":43,"sunLightObjectId":44,"pixelLightCount":45,"defaultReflectionHDR":46,"hasLightDataAsset":47,"hasManualGenerate":48},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"isCreatedByShaderGraph":10,"compiled":11},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Textures.Sprite":{"name":0,"texture":1,"aabb":3,"vertices":4,"triangles":5,"textureRect":6,"packedRect":10,"border":14,"transparency":18,"bounds":19,"pixelsPerUnit":20,"textureWidth":21,"textureHeight":22,"nativeSize":23,"pivot":25,"textureRectOffset":27},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableDynamicBatching":9,"lightmapEncodingQuality":10,"desiredColorSpace":11,"allTags":12},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37}}

Deserializers.requiredComponents = {"8":[9],"10":[9],"11":[9],"12":[9],"13":[9],"14":[9],"15":[16],"17":[2],"18":[19],"20":[19],"21":[19],"22":[19],"23":[19],"24":[19],"25":[19],"26":[27],"28":[27],"29":[27],"30":[27],"31":[27],"32":[27],"33":[27],"34":[27],"35":[27],"36":[27],"37":[27],"38":[27],"39":[27],"40":[2],"41":[42],"43":[44],"45":[44],"46":[47],"48":[49],"50":[4],"51":[49],"52":[47],"53":[47],"54":[46],"55":[56,47],"57":[47],"58":[46],"59":[47],"60":[47],"61":[47],"62":[47],"63":[47],"64":[47],"65":[47],"66":[47],"67":[47],"68":[56,47],"69":[47],"70":[47],"71":[47],"72":[47],"73":[56,47],"74":[47],"75":[76],"77":[76],"78":[76],"79":[76],"80":[2],"81":[2],"82":[83],"84":[2],"85":[47],"86":[42,47],"87":[47,56],"88":[47],"89":[56,47],"90":[42],"91":[56,47],"92":[47],"93":[49]}

Deserializers.types = ["UnityEngine.Shader","UnityEngine.Transform","UnityEngine.Camera","UnityEngine.AudioListener","UnityEngine.SpriteRenderer","UnityEngine.Material","UnityEngine.Sprite","UnityEngine.Texture2D","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.SkinnedMeshRenderer","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Rigidbody","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.MeshRenderer","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","UnityEngine.Canvas","UnityEngine.RectTransform","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","UnityEngine.U2D.Animation.SpriteSkin","Unity.VisualScripting.ScriptMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.GraphicRaycaster","UnityEngine.UI.Image","UnityEngine.CanvasRenderer","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.CanvasScaler","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Text","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","UnityEngine.U2D.SpriteShapeController","UnityEngine.U2D.SpriteShapeRenderer","UnityEngine.U2D.PixelPerfectCamera","TMPro.TextContainer","TMPro.TextMeshPro","TMPro.TextMeshProUGUI","TMPro.TMP_Dropdown","TMPro.TMP_SelectionCaret","TMPro.TMP_SubMesh","TMPro.TMP_SubMeshUI","TMPro.TMP_Text","Unity.VisualScripting.StateMachine"]

Deserializers.unityVersion = "2022.3.51f1";

Deserializers.productName = "Fruit-Hook";

Deserializers.lunaInitializationTime = "12/13/2025 11:26:57";

Deserializers.lunaDaysRunning = "0.1";

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

Deserializers.runtimeAnalysisExcludedClassesCount = "1866";

Deserializers.runtimeAnalysisExcludedMethodsCount = "2567";

Deserializers.runtimeAnalysisExcludedModules = "physics3d, physics2d, particle-system, reflection, prefabs, mecanim-wasm";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isReferenceAmbientProbeBaked = "False";

Deserializers.isLunaCompilerV2Used = "False";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "WebGL";

Deserializers.applicationIdentifier = "com.DefaultCompany.Fruit-Hook";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 28;

Deserializers.linearColorSpace = false;

Deserializers.buildID = "89b87493-43b1-4577-b204-0c180a47f411";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"]],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

