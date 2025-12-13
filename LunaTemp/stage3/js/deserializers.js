var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i196 = root || request.c( 'UnityEngine.JointSpring' )
  var i197 = data
  i196.spring = i197[0]
  i196.damper = i197[1]
  i196.targetPosition = i197[2]
  return i196
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i198 = root || request.c( 'UnityEngine.JointMotor' )
  var i199 = data
  i198.m_TargetVelocity = i199[0]
  i198.m_Force = i199[1]
  i198.m_FreeSpin = i199[2]
  return i198
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i200 = root || request.c( 'UnityEngine.JointLimits' )
  var i201 = data
  i200.m_Min = i201[0]
  i200.m_Max = i201[1]
  i200.m_Bounciness = i201[2]
  i200.m_BounceMinVelocity = i201[3]
  i200.m_ContactDistance = i201[4]
  i200.minBounce = i201[5]
  i200.maxBounce = i201[6]
  return i200
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i202 = root || request.c( 'UnityEngine.JointDrive' )
  var i203 = data
  i202.m_PositionSpring = i203[0]
  i202.m_PositionDamper = i203[1]
  i202.m_MaximumForce = i203[2]
  i202.m_UseAcceleration = i203[3]
  return i202
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i204 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i205 = data
  i204.m_Spring = i205[0]
  i204.m_Damper = i205[1]
  return i204
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i206 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i207 = data
  i206.m_Limit = i207[0]
  i206.m_Bounciness = i207[1]
  i206.m_ContactDistance = i207[2]
  return i206
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i208 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i209 = data
  i208.m_ExtremumSlip = i209[0]
  i208.m_ExtremumValue = i209[1]
  i208.m_AsymptoteSlip = i209[2]
  i208.m_AsymptoteValue = i209[3]
  i208.m_Stiffness = i209[4]
  return i208
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i210 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i211 = data
  i210.m_LowerAngle = i211[0]
  i210.m_UpperAngle = i211[1]
  return i210
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i212 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i213 = data
  i212.m_MotorSpeed = i213[0]
  i212.m_MaximumMotorTorque = i213[1]
  return i212
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i214 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i215 = data
  i214.m_DampingRatio = i215[0]
  i214.m_Frequency = i215[1]
  i214.m_Angle = i215[2]
  return i214
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i216 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i217 = data
  i216.m_LowerTranslation = i217[0]
  i216.m_UpperTranslation = i217[1]
  return i216
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i218 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i219 = data
  i218.name = i219[0]
  i218.index = i219[1]
  i218.startup = !!i219[2]
  return i218
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i220 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i221 = data
  i220.position = new pc.Vec3( i221[0], i221[1], i221[2] )
  i220.scale = new pc.Vec3( i221[3], i221[4], i221[5] )
  i220.rotation = new pc.Quat(i221[6], i221[7], i221[8], i221[9])
  return i220
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i222 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i223 = data
  i222.enabled = !!i223[0]
  i222.aspect = i223[1]
  i222.orthographic = !!i223[2]
  i222.orthographicSize = i223[3]
  i222.backgroundColor = new pc.Color(i223[4], i223[5], i223[6], i223[7])
  i222.nearClipPlane = i223[8]
  i222.farClipPlane = i223[9]
  i222.fieldOfView = i223[10]
  i222.depth = i223[11]
  i222.clearFlags = i223[12]
  i222.cullingMask = i223[13]
  i222.rect = i223[14]
  request.r(i223[15], i223[16], 0, i222, 'targetTexture')
  i222.usePhysicalProperties = !!i223[17]
  i222.focalLength = i223[18]
  i222.sensorSize = new pc.Vec2( i223[19], i223[20] )
  i222.lensShift = new pc.Vec2( i223[21], i223[22] )
  i222.gateFit = i223[23]
  i222.commandBufferCount = i223[24]
  i222.cameraType = i223[25]
  return i222
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i224 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i225 = data
  i224.name = i225[0]
  i224.tagId = i225[1]
  i224.enabled = !!i225[2]
  i224.isStatic = !!i225[3]
  i224.layer = i225[4]
  return i224
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i226 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i227 = data
  i226.ambientIntensity = i227[0]
  i226.reflectionIntensity = i227[1]
  i226.ambientMode = i227[2]
  i226.ambientLight = new pc.Color(i227[3], i227[4], i227[5], i227[6])
  i226.ambientSkyColor = new pc.Color(i227[7], i227[8], i227[9], i227[10])
  i226.ambientGroundColor = new pc.Color(i227[11], i227[12], i227[13], i227[14])
  i226.ambientEquatorColor = new pc.Color(i227[15], i227[16], i227[17], i227[18])
  i226.fogColor = new pc.Color(i227[19], i227[20], i227[21], i227[22])
  i226.fogEndDistance = i227[23]
  i226.fogStartDistance = i227[24]
  i226.fogDensity = i227[25]
  i226.fog = !!i227[26]
  request.r(i227[27], i227[28], 0, i226, 'skybox')
  i226.fogMode = i227[29]
  var i229 = i227[30]
  var i228 = []
  for(var i = 0; i < i229.length; i += 1) {
    i228.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i229[i + 0]) );
  }
  i226.lightmaps = i228
  i226.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i227[31], i226.lightProbes)
  i226.lightmapsMode = i227[32]
  i226.mixedBakeMode = i227[33]
  i226.environmentLightingMode = i227[34]
  i226.ambientProbe = new pc.SphericalHarmonicsL2(i227[35])
  i226.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i227[36])
  i226.useReferenceAmbientProbe = !!i227[37]
  request.r(i227[38], i227[39], 0, i226, 'customReflection')
  request.r(i227[40], i227[41], 0, i226, 'defaultReflection')
  i226.defaultReflectionMode = i227[42]
  i226.defaultReflectionResolution = i227[43]
  i226.sunLightObjectId = i227[44]
  i226.pixelLightCount = i227[45]
  i226.defaultReflectionHDR = !!i227[46]
  i226.hasLightDataAsset = !!i227[47]
  i226.hasManualGenerate = !!i227[48]
  return i226
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i232 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i233 = data
  request.r(i233[0], i233[1], 0, i232, 'lightmapColor')
  request.r(i233[2], i233[3], 0, i232, 'lightmapDirection')
  return i232
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i234 = root || new UnityEngine.LightProbes()
  var i235 = data
  return i234
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i242 = root || new pc.UnityMaterial()
  var i243 = data
  i242.name = i243[0]
  request.r(i243[1], i243[2], 0, i242, 'shader')
  i242.renderQueue = i243[3]
  i242.enableInstancing = !!i243[4]
  var i245 = i243[5]
  var i244 = []
  for(var i = 0; i < i245.length; i += 1) {
    i244.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i245[i + 0]) );
  }
  i242.floatParameters = i244
  var i247 = i243[6]
  var i246 = []
  for(var i = 0; i < i247.length; i += 1) {
    i246.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i247[i + 0]) );
  }
  i242.colorParameters = i246
  var i249 = i243[7]
  var i248 = []
  for(var i = 0; i < i249.length; i += 1) {
    i248.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i249[i + 0]) );
  }
  i242.vectorParameters = i248
  var i251 = i243[8]
  var i250 = []
  for(var i = 0; i < i251.length; i += 1) {
    i250.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i251[i + 0]) );
  }
  i242.textureParameters = i250
  var i253 = i243[9]
  var i252 = []
  for(var i = 0; i < i253.length; i += 1) {
    i252.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i253[i + 0]) );
  }
  i242.materialFlags = i252
  return i242
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i256 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i257 = data
  i256.name = i257[0]
  i256.value = i257[1]
  return i256
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i260 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i261 = data
  i260.name = i261[0]
  i260.value = new pc.Color(i261[1], i261[2], i261[3], i261[4])
  return i260
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i264 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i265 = data
  i264.name = i265[0]
  i264.value = new pc.Vec4( i265[1], i265[2], i265[3], i265[4] )
  return i264
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i268 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i269 = data
  i268.name = i269[0]
  request.r(i269[1], i269[2], 0, i268, 'value')
  return i268
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i272 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i273 = data
  i272.name = i273[0]
  i272.enabled = !!i273[1]
  return i272
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i274 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i275 = data
  var i277 = i275[0]
  var i276 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i277.length; i += 1) {
    i276.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i277[i + 0]));
  }
  i274.ShaderCompilationErrors = i276
  i274.name = i275[1]
  i274.guid = i275[2]
  var i279 = i275[3]
  var i278 = []
  for(var i = 0; i < i279.length; i += 1) {
    i278.push( i279[i + 0] );
  }
  i274.shaderDefinedKeywords = i278
  var i281 = i275[4]
  var i280 = []
  for(var i = 0; i < i281.length; i += 1) {
    i280.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i281[i + 0]) );
  }
  i274.passes = i280
  var i283 = i275[5]
  var i282 = []
  for(var i = 0; i < i283.length; i += 1) {
    i282.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i283[i + 0]) );
  }
  i274.usePasses = i282
  var i285 = i275[6]
  var i284 = []
  for(var i = 0; i < i285.length; i += 1) {
    i284.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i285[i + 0]) );
  }
  i274.defaultParameterValues = i284
  request.r(i275[7], i275[8], 0, i274, 'unityFallbackShader')
  i274.readDepth = !!i275[9]
  i274.isCreatedByShaderGraph = !!i275[10]
  i274.compiled = !!i275[11]
  return i274
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i288 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i289 = data
  i288.shaderName = i289[0]
  i288.errorMessage = i289[1]
  return i288
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i294 = root || new pc.UnityShaderPass()
  var i295 = data
  i294.id = i295[0]
  i294.subShaderIndex = i295[1]
  i294.name = i295[2]
  i294.passType = i295[3]
  i294.grabPassTextureName = i295[4]
  i294.usePass = !!i295[5]
  i294.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[6], i294.zTest)
  i294.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[7], i294.zWrite)
  i294.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[8], i294.culling)
  i294.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i295[9], i294.blending)
  i294.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i295[10], i294.alphaBlending)
  i294.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[11], i294.colorWriteMask)
  i294.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[12], i294.offsetUnits)
  i294.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[13], i294.offsetFactor)
  i294.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[14], i294.stencilRef)
  i294.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[15], i294.stencilReadMask)
  i294.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i295[16], i294.stencilWriteMask)
  i294.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i295[17], i294.stencilOp)
  i294.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i295[18], i294.stencilOpFront)
  i294.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i295[19], i294.stencilOpBack)
  var i297 = i295[20]
  var i296 = []
  for(var i = 0; i < i297.length; i += 1) {
    i296.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i297[i + 0]) );
  }
  i294.tags = i296
  var i299 = i295[21]
  var i298 = []
  for(var i = 0; i < i299.length; i += 1) {
    i298.push( i299[i + 0] );
  }
  i294.passDefinedKeywords = i298
  var i301 = i295[22]
  var i300 = []
  for(var i = 0; i < i301.length; i += 1) {
    i300.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i301[i + 0]) );
  }
  i294.passDefinedKeywordGroups = i300
  var i303 = i295[23]
  var i302 = []
  for(var i = 0; i < i303.length; i += 1) {
    i302.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i303[i + 0]) );
  }
  i294.variants = i302
  var i305 = i295[24]
  var i304 = []
  for(var i = 0; i < i305.length; i += 1) {
    i304.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i305[i + 0]) );
  }
  i294.excludedVariants = i304
  i294.hasDepthReader = !!i295[25]
  return i294
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i306 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i307 = data
  i306.val = i307[0]
  i306.name = i307[1]
  return i306
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i308 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i309 = data
  i308.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i309[0], i308.src)
  i308.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i309[1], i308.dst)
  i308.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i309[2], i308.op)
  return i308
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i310 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i311 = data
  i310.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i311[0], i310.pass)
  i310.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i311[1], i310.fail)
  i310.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i311[2], i310.zFail)
  i310.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i311[3], i310.comp)
  return i310
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i314 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i315 = data
  i314.name = i315[0]
  i314.value = i315[1]
  return i314
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i318 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i319 = data
  var i321 = i319[0]
  var i320 = []
  for(var i = 0; i < i321.length; i += 1) {
    i320.push( i321[i + 0] );
  }
  i318.keywords = i320
  i318.hasDiscard = !!i319[1]
  return i318
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i324 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i325 = data
  i324.passId = i325[0]
  i324.subShaderIndex = i325[1]
  var i327 = i325[2]
  var i326 = []
  for(var i = 0; i < i327.length; i += 1) {
    i326.push( i327[i + 0] );
  }
  i324.keywords = i326
  i324.vertexProgram = i325[3]
  i324.fragmentProgram = i325[4]
  i324.exportedForWebGl2 = !!i325[5]
  i324.readDepth = !!i325[6]
  return i324
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i330 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i331 = data
  request.r(i331[0], i331[1], 0, i330, 'shader')
  i330.pass = i331[2]
  return i330
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i334 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i335 = data
  i334.name = i335[0]
  i334.type = i335[1]
  i334.value = new pc.Vec4( i335[2], i335[3], i335[4], i335[5] )
  i334.textureValue = i335[6]
  i334.shaderPropertyFlag = i335[7]
  return i334
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i336 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i337 = data
  var i339 = i337[0]
  var i338 = []
  for(var i = 0; i < i339.length; i += 1) {
    i338.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i339[i + 0]) );
  }
  i336.files = i338
  i336.componentToPrefabIds = i337[1]
  return i336
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i342 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i343 = data
  i342.path = i343[0]
  request.r(i343[1], i343[2], 0, i342, 'unityObject')
  return i342
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i344 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i345 = data
  var i347 = i345[0]
  var i346 = []
  for(var i = 0; i < i347.length; i += 1) {
    i346.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i347[i + 0]) );
  }
  i344.scriptsExecutionOrder = i346
  var i349 = i345[1]
  var i348 = []
  for(var i = 0; i < i349.length; i += 1) {
    i348.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i349[i + 0]) );
  }
  i344.sortingLayers = i348
  var i351 = i345[2]
  var i350 = []
  for(var i = 0; i < i351.length; i += 1) {
    i350.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i351[i + 0]) );
  }
  i344.cullingLayers = i350
  i344.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i345[3], i344.timeSettings)
  i344.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i345[4], i344.physicsSettings)
  i344.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i345[5], i344.physics2DSettings)
  i344.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i345[6], i344.qualitySettings)
  i344.enableRealtimeShadows = !!i345[7]
  i344.enableAutoInstancing = !!i345[8]
  i344.enableDynamicBatching = !!i345[9]
  i344.lightmapEncodingQuality = i345[10]
  i344.desiredColorSpace = i345[11]
  var i353 = i345[12]
  var i352 = []
  for(var i = 0; i < i353.length; i += 1) {
    i352.push( i353[i + 0] );
  }
  i344.allTags = i352
  return i344
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i356 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i357 = data
  i356.name = i357[0]
  i356.value = i357[1]
  return i356
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i360 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i361 = data
  i360.id = i361[0]
  i360.name = i361[1]
  i360.value = i361[2]
  return i360
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i364 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i365 = data
  i364.id = i365[0]
  i364.name = i365[1]
  return i364
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i366 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i367 = data
  i366.fixedDeltaTime = i367[0]
  i366.maximumDeltaTime = i367[1]
  i366.timeScale = i367[2]
  i366.maximumParticleTimestep = i367[3]
  return i366
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i368 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i369 = data
  i368.gravity = new pc.Vec3( i369[0], i369[1], i369[2] )
  i368.defaultSolverIterations = i369[3]
  i368.bounceThreshold = i369[4]
  i368.autoSyncTransforms = !!i369[5]
  i368.autoSimulation = !!i369[6]
  var i371 = i369[7]
  var i370 = []
  for(var i = 0; i < i371.length; i += 1) {
    i370.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i371[i + 0]) );
  }
  i368.collisionMatrix = i370
  return i368
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i374 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i375 = data
  i374.enabled = !!i375[0]
  i374.layerId = i375[1]
  i374.otherLayerId = i375[2]
  return i374
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i376 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i377 = data
  request.r(i377[0], i377[1], 0, i376, 'material')
  i376.gravity = new pc.Vec2( i377[2], i377[3] )
  i376.positionIterations = i377[4]
  i376.velocityIterations = i377[5]
  i376.velocityThreshold = i377[6]
  i376.maxLinearCorrection = i377[7]
  i376.maxAngularCorrection = i377[8]
  i376.maxTranslationSpeed = i377[9]
  i376.maxRotationSpeed = i377[10]
  i376.baumgarteScale = i377[11]
  i376.baumgarteTOIScale = i377[12]
  i376.timeToSleep = i377[13]
  i376.linearSleepTolerance = i377[14]
  i376.angularSleepTolerance = i377[15]
  i376.defaultContactOffset = i377[16]
  i376.autoSimulation = !!i377[17]
  i376.queriesHitTriggers = !!i377[18]
  i376.queriesStartInColliders = !!i377[19]
  i376.callbacksOnDisable = !!i377[20]
  i376.reuseCollisionCallbacks = !!i377[21]
  i376.autoSyncTransforms = !!i377[22]
  var i379 = i377[23]
  var i378 = []
  for(var i = 0; i < i379.length; i += 1) {
    i378.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i379[i + 0]) );
  }
  i376.collisionMatrix = i378
  return i376
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i382 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i383 = data
  i382.enabled = !!i383[0]
  i382.layerId = i383[1]
  i382.otherLayerId = i383[2]
  return i382
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i384 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i385 = data
  var i387 = i385[0]
  var i386 = []
  for(var i = 0; i < i387.length; i += 1) {
    i386.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i387[i + 0]) );
  }
  i384.qualityLevels = i386
  var i389 = i385[1]
  var i388 = []
  for(var i = 0; i < i389.length; i += 1) {
    i388.push( i389[i + 0] );
  }
  i384.names = i388
  i384.shadows = i385[2]
  i384.anisotropicFiltering = i385[3]
  i384.antiAliasing = i385[4]
  i384.lodBias = i385[5]
  i384.shadowCascades = i385[6]
  i384.shadowDistance = i385[7]
  i384.shadowmaskMode = i385[8]
  i384.shadowProjection = i385[9]
  i384.shadowResolution = i385[10]
  i384.softParticles = !!i385[11]
  i384.softVegetation = !!i385[12]
  i384.activeColorSpace = i385[13]
  i384.desiredColorSpace = i385[14]
  i384.masterTextureLimit = i385[15]
  i384.maxQueuedFrames = i385[16]
  i384.particleRaycastBudget = i385[17]
  i384.pixelLightCount = i385[18]
  i384.realtimeReflectionProbes = !!i385[19]
  i384.shadowCascade2Split = i385[20]
  i384.shadowCascade4Split = new pc.Vec3( i385[21], i385[22], i385[23] )
  i384.streamingMipmapsActive = !!i385[24]
  i384.vSyncCount = i385[25]
  i384.asyncUploadBufferSize = i385[26]
  i384.asyncUploadTimeSlice = i385[27]
  i384.billboardsFaceCameraPosition = !!i385[28]
  i384.shadowNearPlaneOffset = i385[29]
  i384.streamingMipmapsMemoryBudget = i385[30]
  i384.maximumLODLevel = i385[31]
  i384.streamingMipmapsAddAllCameras = !!i385[32]
  i384.streamingMipmapsMaxLevelReduction = i385[33]
  i384.streamingMipmapsRenderersPerFrame = i385[34]
  i384.resolutionScalingFixedDPIFactor = i385[35]
  i384.streamingMipmapsMaxFileIORequests = i385[36]
  i384.currentQualityLevel = i385[37]
  return i384
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"enabled":0,"aspect":1,"orthographic":2,"orthographicSize":3,"backgroundColor":4,"nearClipPlane":8,"farClipPlane":9,"fieldOfView":10,"depth":11,"clearFlags":12,"cullingMask":13,"rect":14,"targetTexture":15,"usePhysicalProperties":17,"focalLength":18,"sensorSize":19,"lensShift":21,"gateFit":23,"commandBufferCount":24,"cameraType":25},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"referenceAmbientProbe":36,"useReferenceAmbientProbe":37,"customReflection":38,"defaultReflection":40,"defaultReflectionMode":42,"defaultReflectionResolution":43,"sunLightObjectId":44,"pixelLightCount":45,"defaultReflectionHDR":46,"hasLightDataAsset":47,"hasManualGenerate":48},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"isCreatedByShaderGraph":10,"compiled":11},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableDynamicBatching":9,"lightmapEncodingQuality":10,"desiredColorSpace":11,"allTags":12},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37}}

Deserializers.requiredComponents = {"4":[5],"6":[5],"7":[5],"8":[5],"9":[5],"10":[5],"11":[12],"13":[1],"14":[15],"16":[15],"17":[15],"18":[15],"19":[15],"20":[15],"21":[15],"22":[23],"24":[23],"25":[23],"26":[23],"27":[23],"28":[23],"29":[23],"30":[23],"31":[23],"32":[23],"33":[23],"34":[23],"35":[23],"36":[1],"37":[38],"39":[40],"41":[40],"42":[43],"44":[45],"46":[47],"48":[45],"49":[43],"50":[43],"51":[42],"52":[53,43],"54":[43],"55":[42],"56":[43],"57":[43],"58":[43],"59":[43],"60":[43],"61":[43],"62":[43],"63":[43],"64":[43],"65":[53,43],"66":[43],"67":[43],"68":[43],"69":[43],"70":[53,43],"71":[43],"72":[73],"74":[73],"75":[73],"76":[73],"77":[1],"78":[1],"79":[80],"81":[1],"82":[43],"83":[38,43],"84":[43,53],"85":[43],"86":[53,43],"87":[38],"88":[53,43],"89":[43],"90":[45]}

Deserializers.types = ["UnityEngine.Transform","UnityEngine.Camera","UnityEngine.AudioListener","UnityEngine.Shader","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.SkinnedMeshRenderer","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Rigidbody","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.MeshRenderer","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","UnityEngine.Canvas","UnityEngine.RectTransform","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","UnityEngine.U2D.Animation.SpriteSkin","UnityEngine.SpriteRenderer","Unity.VisualScripting.ScriptMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.GraphicRaycaster","UnityEngine.UI.Image","UnityEngine.CanvasRenderer","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.CanvasScaler","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Text","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","UnityEngine.U2D.SpriteShapeController","UnityEngine.U2D.SpriteShapeRenderer","UnityEngine.U2D.PixelPerfectCamera","TMPro.TextContainer","TMPro.TextMeshPro","TMPro.TextMeshProUGUI","TMPro.TMP_Dropdown","TMPro.TMP_SelectionCaret","TMPro.TMP_SubMesh","TMPro.TMP_SubMeshUI","TMPro.TMP_Text","Unity.VisualScripting.StateMachine"]

Deserializers.unityVersion = "2022.3.51f1";

Deserializers.productName = "Fruit-Hook";

Deserializers.lunaInitializationTime = "12/13/2025 11:26:57";

Deserializers.lunaDaysRunning = "0.0";

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

Deserializers.isRuntimeAnalysisEnabledForCode = "False";

Deserializers.runtimeAnalysisExcludedClassesCount = "0";

Deserializers.runtimeAnalysisExcludedMethodsCount = "0";

Deserializers.runtimeAnalysisExcludedModules = "";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isReferenceAmbientProbeBaked = "False";

Deserializers.isLunaCompilerV2Used = "False";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "StandaloneOSX";

Deserializers.applicationIdentifier = "com.DefaultCompany.2DProject";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 28;

Deserializers.linearColorSpace = true;

Deserializers.buildID = "ebe73867-0b5b-4f6d-a3c4-1d064afc577a";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"],["$BurstDirectCallInitializer","Initialize"]],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

