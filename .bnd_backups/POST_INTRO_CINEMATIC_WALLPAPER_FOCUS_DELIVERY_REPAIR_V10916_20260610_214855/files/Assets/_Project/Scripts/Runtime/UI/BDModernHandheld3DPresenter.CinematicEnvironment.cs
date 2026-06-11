using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float CinematicTableWidth = 24f;
        private const float CinematicTableDepth = 12.4f;
        private const float CinematicTableTopThickness = 0.90f;
        private const float CinematicTableTopSurfaceY = -7.24f;
        private const float CinematicTableCenterZ = -1f;
        private const float CinematicFloorSurfaceY = -14.84f;

        private void BuildCinematicProductEnvironment()
        {
            tableRoot = new GameObject(
                "Modern Handheld Full 3D Table Environment"
            ).transform;
            tableRoot.SetParent(presentationRoot.transform, false);
            tableRoot.localPosition = TableRestPosition;
            tableRoot.localRotation = Quaternion.identity;
            tableRoot.localScale = Vector3.one;
            SetLayerRecursively(tableRoot.gameObject, DeviceLayer);

            Material woodTopMaterial = CreateCinematicWoodMaterial(
                "BD Premium Dark Wood Top",
                new Color(0.70f, 0.57f, 0.47f, 1f),
                0.31f
            );
            Material woodFrameMaterial = CreateCinematicWoodMaterial(
                "BD Premium Dark Wood Frame",
                new Color(0.48f, 0.35f, 0.27f, 1f),
                0.22f
            );
            Material floorMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Charcoal Floor",
                new Color(0.055f, 0.060f, 0.068f, 1f),
                0.08f
            );
            Material backdropMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Charcoal Cyclorama",
                new Color(0.025f, 0.029f, 0.038f, 1f),
                0.04f
            );
            Material roomWoodMaterial = CreateCinematicWoodMaterial(
                "BD Cinematic Walnut Room Wood",
                new Color(0.34f, 0.22f, 0.15f, 1f),
                0.18f
            );
            Material rugMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Woven Rug",
                new Color(0.045f, 0.055f, 0.070f, 1f),
                0.03f
            );
            Material brassMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Aged Brass",
                new Color(0.34f, 0.24f, 0.11f, 1f),
                0.30f
            );
            Material fabricMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Charcoal Fabric",
                new Color(0.075f, 0.085f, 0.105f, 1f),
                0.02f
            );
            Material leafMaterial = CreateCinematicEnvironmentMaterial(
                "BD Cinematic Plant Green",
                new Color(0.055f, 0.17f, 0.105f, 1f),
                0.08f
            );
            Material practicalGlowMaterial = CreateCinematicEmissiveMaterial(
                "BD Cinematic Warm Practical Glow",
                new Color(1f, 0.48f, 0.16f, 1f),
                1.65f
            );

            BuildFullThreeDimensionalTable(
                woodTopMaterial,
                woodFrameMaterial
            );
            BuildCinematicFloorAndCyclorama(
                floorMaterial,
                backdropMaterial
            );
            BuildCinematicRoomDressing(
                roomWoodMaterial,
                rugMaterial,
                brassMaterial,
                fabricMaterial,
                leafMaterial,
                practicalGlowMaterial
            );
            BuildHorizontalDeviceAndFurnitureShadows();
            BuildCinematicEnvironmentLights();
        }

        private void BuildFullThreeDimensionalTable(
            Material topMaterial,
            Material frameMaterial)
        {
            float topCenterY =
                CinematicTableTopSurfaceY -
                CinematicTableTopThickness * 0.5f;

            CreateCinematicCube(
                "Full 3D Tabletop",
                tableRoot,
                new Vector3(0f, topCenterY, CinematicTableCenterZ),
                new Vector3(
                    CinematicTableWidth,
                    CinematicTableTopThickness,
                    CinematicTableDepth
                ),
                topMaterial,
                castShadows: true,
                receiveShadows: true
            );

            // A restrained solid-wood lip gives the final frame two readable
            // planes: the top surface and the front thickness below it.
            CreateCinematicCube(
                "Table Front Edge Lip",
                tableRoot,
                new Vector3(
                    0f,
                    CinematicTableTopSurfaceY - 0.52f,
                    CinematicTableCenterZ -
                    CinematicTableDepth * 0.5f + 0.18f
                ),
                new Vector3(
                    CinematicTableWidth + 0.28f,
                    0.72f,
                    0.38f
                ),
                topMaterial,
                castShadows: true,
                receiveShadows: true
            );

            float apronY = CinematicTableTopSurfaceY - 1.28f;
            float apronHeight = 1.45f;
            float frontZ =
                CinematicTableCenterZ -
                CinematicTableDepth * 0.5f + 0.58f;
            float backZ =
                CinematicTableCenterZ +
                CinematicTableDepth * 0.5f - 0.58f;

            CreateCinematicCube(
                "Table Front Apron",
                tableRoot,
                new Vector3(0f, apronY, frontZ),
                new Vector3(
                    CinematicTableWidth - 1.55f,
                    apronHeight,
                    0.72f
                ),
                frameMaterial,
                castShadows: true,
                receiveShadows: true
            );
            CreateCinematicCube(
                "Table Back Apron",
                tableRoot,
                new Vector3(0f, apronY, backZ),
                new Vector3(
                    CinematicTableWidth - 1.55f,
                    apronHeight,
                    0.72f
                ),
                frameMaterial,
                castShadows: true,
                receiveShadows: true
            );

            float sideX = CinematicTableWidth * 0.5f - 0.58f;
            CreateCinematicCube(
                "Table Left Apron",
                tableRoot,
                new Vector3(-sideX, apronY, CinematicTableCenterZ),
                new Vector3(
                    0.72f,
                    apronHeight,
                    CinematicTableDepth - 1.55f
                ),
                frameMaterial,
                castShadows: true,
                receiveShadows: true
            );
            CreateCinematicCube(
                "Table Right Apron",
                tableRoot,
                new Vector3(sideX, apronY, CinematicTableCenterZ),
                new Vector3(
                    0.72f,
                    apronHeight,
                    CinematicTableDepth - 1.55f
                ),
                frameMaterial,
                castShadows: true,
                receiveShadows: true
            );

            float legTop = apronY - apronHeight * 0.5f + 0.10f;
            float legBottom = CinematicFloorSurfaceY + 0.18f;
            float legHeight = legTop - legBottom;
            float legCenterY = (legTop + legBottom) * 0.5f;
            float legX = CinematicTableWidth * 0.5f - 1.32f;
            float frontLegZ =
                CinematicTableCenterZ -
                CinematicTableDepth * 0.5f + 1.18f;
            float backLegZ =
                CinematicTableCenterZ +
                CinematicTableDepth * 0.5f - 1.18f;

            CreateCinematicTableLeg(
                "Table Front Left Leg",
                new Vector3(-legX, legCenterY, frontLegZ),
                legHeight,
                frameMaterial
            );
            CreateCinematicTableLeg(
                "Table Front Right Leg",
                new Vector3(legX, legCenterY, frontLegZ),
                legHeight,
                frameMaterial
            );
            CreateCinematicTableLeg(
                "Table Back Left Leg",
                new Vector3(-legX, legCenterY, backLegZ),
                legHeight,
                frameMaterial
            );
            CreateCinematicTableLeg(
                "Table Back Right Leg",
                new Vector3(legX, legCenterY, backLegZ),
                legHeight,
                frameMaterial
            );
        }

        private void CreateCinematicTableLeg(
            string objectName,
            Vector3 localPosition,
            float height,
            Material material)
        {
            CreateCinematicCube(
                objectName,
                tableRoot,
                localPosition,
                new Vector3(1.08f, height, 1.08f),
                material,
                castShadows: true,
                receiveShadows: true
            );

            CreateCinematicCube(
                objectName + " Foot",
                tableRoot,
                new Vector3(
                    localPosition.x,
                    CinematicFloorSurfaceY + 0.20f,
                    localPosition.z
                ),
                new Vector3(1.22f, 0.40f, 1.22f),
                material,
                castShadows: true,
                receiveShadows: true
            );
        }

        private void BuildCinematicFloorAndCyclorama(
            Material floorMaterial,
            Material backdropMaterial)
        {
            float floorWidth = Mathf.Max(
                80f,
                TableEnvironmentWidth * 1.8f
            );
            float floorDepth = Mathf.Max(
                72f,
                TableEnvironmentHeight * 2.35f
            );

            CreateCinematicCube(
                "Cinematic Floor",
                tableRoot,
                new Vector3(
                    0f,
                    CinematicFloorSurfaceY - 0.25f,
                    -19.5f
                ),
                new Vector3(floorWidth, 0.50f, floorDepth),
                floorMaterial,
                castShadows: false,
                receiveShadows: true
            );

            Mesh cycloramaMesh = CreateCycloramaMesh(
                floorWidth,
                16f,
                24f,
                CinematicFloorSurfaceY,
                18f,
                14
            );
            GameObject cyclorama = CreateCinematicMeshObject(
                "Cinematic Cyclorama",
                tableRoot,
                cycloramaMesh,
                backdropMaterial
            );
            MeshRenderer renderer =
                cyclorama.GetComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }


        private void BuildCinematicRoomDressing(
            Material roomWoodMaterial,
            Material rugMaterial,
            Material brassMaterial,
            Material fabricMaterial,
            Material leafMaterial,
            Material practicalGlowMaterial)
        {
            CreateCinematicCube(
                "Cinematic Woven Rug",
                tableRoot,
                new Vector3(
                    0f,
                    CinematicFloorSurfaceY + 0.035f,
                    -1.2f
                ),
                new Vector3(30f, 0.07f, 18f),
                rugMaterial,
                castShadows: false,
                receiveShadows: true
            );

            BuildCinematicWallSlats(roomWoodMaterial);
            BuildCinematicCredenza(
                roomWoodMaterial,
                brassMaterial,
                fabricMaterial
            );
            BuildCinematicPracticalLamp(
                brassMaterial,
                fabricMaterial,
                practicalGlowMaterial
            );
            BuildCinematicPlant(
                roomWoodMaterial,
                leafMaterial
            );
            BuildCinematicWallAccents(
                roomWoodMaterial,
                brassMaterial,
                fabricMaterial
            );
        }

        private void BuildCinematicWallSlats(Material material)
        {
            for (int index = -8; index <= 8; index++)
            {
                CreateCinematicCube(
                    "Cinematic Wall Slat " + (index + 9),
                    tableRoot,
                    new Vector3(
                        index * 1.55f,
                        -2.6f,
                        22.55f
                    ),
                    new Vector3(0.24f, 19.5f, 0.34f),
                    material,
                    castShadows: false,
                    receiveShadows: true
                );
            }
        }

        private void BuildCinematicCredenza(
            Material woodMaterial,
            Material accentMaterial,
            Material fabricMaterial)
        {
            const float centerX = 4.8f;
            const float centerZ = 11.5f;
            float bodyY = CinematicFloorSurfaceY + 1.68f;

            CreateCinematicCube(
                "Cinematic Walnut Credenza",
                tableRoot,
                new Vector3(centerX, bodyY, centerZ),
                new Vector3(12.0f, 2.80f, 2.0f),
                woodMaterial,
                castShadows: true,
                receiveShadows: true
            );

            for (int index = -1; index <= 1; index++)
            {
                CreateCinematicCube(
                    "Cinematic Credenza Door " + (index + 2),
                    tableRoot,
                    new Vector3(
                        centerX + index * 3.75f,
                        bodyY,
                        centerZ - 1.03f
                    ),
                    new Vector3(3.45f, 2.35f, 0.10f),
                    fabricMaterial,
                    castShadows: false,
                    receiveShadows: true
                );

                CreateCinematicPrimitive(
                    PrimitiveType.Sphere,
                    "Cinematic Credenza Knob " + (index + 2),
                    tableRoot,
                    new Vector3(
                        centerX + index * 3.75f + 1.15f,
                        bodyY,
                        centerZ - 1.10f
                    ),
                    Quaternion.identity,
                    new Vector3(0.13f, 0.13f, 0.13f),
                    accentMaterial,
                    castShadows: false,
                    receiveShadows: true
                );
            }

            float legY = CinematicFloorSurfaceY + 0.38f;
            float[] legOffsets = {-5.1f, 5.1f};
            for (int index = 0; index < legOffsets.Length; index++)
            {
                CreateCinematicCube(
                    "Cinematic Credenza Leg " + (index + 1),
                    tableRoot,
                    new Vector3(
                        centerX + legOffsets[index],
                        legY,
                        centerZ
                    ),
                    new Vector3(0.32f, 0.76f, 1.30f),
                    accentMaterial,
                    castShadows: true,
                    receiveShadows: true
                );
            }

            float topY = CinematicFloorSurfaceY + 3.18f;
            for (int index = 0; index < 5; index++)
            {
                CreateCinematicCube(
                    "Cinematic Book " + (index + 1),
                    tableRoot,
                    new Vector3(
                        centerX + 0.55f + index * 0.34f,
                        topY + 0.10f + index * 0.035f,
                        centerZ - 0.12f
                    ),
                    new Vector3(0.27f, 0.20f, 1.08f),
                    index % 2 == 0
                        ? fabricMaterial
                        : accentMaterial,
                    castShadows: true,
                    receiveShadows: true
                );
            }
        }

        private void BuildCinematicPracticalLamp(
            Material metalMaterial,
            Material shadeMaterial,
            Material glowMaterial)
        {
            Vector3 basePosition = new Vector3(
                -2.5f,
                CinematicFloorSurfaceY + 3.15f,
                11.2f
            );

            CreateCinematicPrimitive(
                PrimitiveType.Cylinder,
                "Cinematic Practical Lamp Base",
                tableRoot,
                basePosition,
                Quaternion.identity,
                new Vector3(0.62f, 0.10f, 0.62f),
                metalMaterial,
                castShadows: true,
                receiveShadows: true
            );
            CreateCinematicPrimitive(
                PrimitiveType.Cylinder,
                "Cinematic Practical Lamp Stem",
                tableRoot,
                basePosition + new Vector3(0f, 1.12f, 0f),
                Quaternion.identity,
                new Vector3(0.10f, 1.12f, 0.10f),
                metalMaterial,
                castShadows: true,
                receiveShadows: true
            );
            CreateCinematicPrimitive(
                PrimitiveType.Sphere,
                "Cinematic Practical Lamp Shade",
                tableRoot,
                basePosition + new Vector3(0f, 2.35f, 0f),
                Quaternion.identity,
                new Vector3(1.05f, 0.68f, 1.05f),
                shadeMaterial,
                castShadows: true,
                receiveShadows: true
            );
            CreateCinematicPrimitive(
                PrimitiveType.Sphere,
                "Cinematic Practical Lamp Glow",
                tableRoot,
                basePosition + new Vector3(0f, 2.18f, -0.12f),
                Quaternion.identity,
                new Vector3(0.48f, 0.42f, 0.48f),
                glowMaterial,
                castShadows: false,
                receiveShadows: false
            );
            CreateCinematicPointLight(
                "Cinematic Warm Practical",
                basePosition + new Vector3(0f, 2.12f, -0.35f),
                new Color(1f, 0.62f, 0.32f, 1f),
                1.55f,
                14f
            );
        }

        private void BuildCinematicPlant(
            Material potMaterial,
            Material leafMaterial)
        {
            Vector3 potPosition = new Vector3(
                13.4f,
                CinematicFloorSurfaceY + 0.85f,
                9.4f
            );

            CreateCinematicPrimitive(
                PrimitiveType.Cylinder,
                "Cinematic Plant Pot",
                tableRoot,
                potPosition,
                Quaternion.identity,
                new Vector3(1.15f, 0.85f, 1.15f),
                potMaterial,
                castShadows: true,
                receiveShadows: true
            );

            Vector3[] stemOffsets =
            {
                new Vector3(-0.25f, 2.10f, 0.08f),
                new Vector3(0.22f, 2.55f, -0.12f),
                new Vector3(0.02f, 3.05f, 0.18f)
            };

            for (int index = 0; index < stemOffsets.Length; index++)
            {
                Vector3 stemCenter = potPosition + stemOffsets[index];
                CreateCinematicPrimitive(
                    PrimitiveType.Cylinder,
                    "Cinematic Plant Stem " + (index + 1),
                    tableRoot,
                    stemCenter,
                    Quaternion.Euler(
                        index == 0 ? -8f : 8f,
                        0f,
                        index == 2 ? -5f : 5f
                    ),
                    new Vector3(0.08f, 1.55f, 0.08f),
                    leafMaterial,
                    castShadows: true,
                    receiveShadows: true
                );

                CreateCinematicPrimitive(
                    PrimitiveType.Sphere,
                    "Cinematic Plant Leaf " + (index * 2 + 1),
                    tableRoot,
                    stemCenter + new Vector3(-0.48f, 1.05f, 0f),
                    Quaternion.Euler(0f, 0f, 24f),
                    new Vector3(0.78f, 0.24f, 0.44f),
                    leafMaterial,
                    castShadows: true,
                    receiveShadows: true
                );
                CreateCinematicPrimitive(
                    PrimitiveType.Sphere,
                    "Cinematic Plant Leaf " + (index * 2 + 2),
                    tableRoot,
                    stemCenter + new Vector3(0.48f, 0.72f, 0.10f),
                    Quaternion.Euler(0f, 0f, -24f),
                    new Vector3(0.78f, 0.24f, 0.44f),
                    leafMaterial,
                    castShadows: true,
                    receiveShadows: true
                );
            }
        }

        private void BuildCinematicWallAccents(
            Material frameMaterial,
            Material accentMaterial,
            Material panelMaterial)
        {
            Vector3[] centers =
            {
                new Vector3(-8.2f, -2.2f, 22.18f),
                new Vector3(6.4f, -1.4f, 22.18f)
            };
            Vector2[] sizes =
            {
                new Vector2(5.2f, 7.0f),
                new Vector2(4.2f, 5.6f)
            };

            for (int index = 0; index < centers.Length; index++)
            {
                CreateCinematicCube(
                    "Cinematic Framed Wall Accent " + (index + 1),
                    tableRoot,
                    centers[index],
                    new Vector3(
                        sizes[index].x,
                        sizes[index].y,
                        0.24f
                    ),
                    frameMaterial,
                    castShadows: false,
                    receiveShadows: true
                );
                CreateCinematicCube(
                    "Cinematic Wall Accent Panel " + (index + 1),
                    tableRoot,
                    centers[index] + new Vector3(0f, 0f, -0.16f),
                    new Vector3(
                        sizes[index].x - 0.42f,
                        sizes[index].y - 0.42f,
                        0.08f
                    ),
                    index == 0 ? panelMaterial : accentMaterial,
                    castShadows: false,
                    receiveShadows: true
                );
            }
        }

        private GameObject CreateCinematicPrimitive(
            PrimitiveType primitiveType,
            string objectName,
            Transform parent,
            Vector3 localPosition,
            Quaternion localRotation,
            Vector3 localScale,
            Material material,
            bool castShadows,
            bool receiveShadows)
        {
            GameObject primitive = GameObject.CreatePrimitive(primitiveType);
            primitive.name = objectName;

            Collider collider = primitive.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            primitive.transform.SetParent(parent, false);
            primitive.transform.localPosition = localPosition;
            primitive.transform.localRotation = localRotation;
            primitive.transform.localScale = localScale;

            Renderer renderer = primitive.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = castShadows
                ? ShadowCastingMode.On
                : ShadowCastingMode.Off;
            renderer.receiveShadows = receiveShadows;
            SetLayerRecursively(primitive, DeviceLayer);
            return primitive;
        }

        private void CreateCinematicPointLight(
            string objectName,
            Vector3 localPosition,
            Color color,
            float intensity,
            float range)
        {
            GameObject lightObject = new GameObject(objectName);
            lightObject.transform.SetParent(presentationRoot.transform, false);
            lightObject.transform.localPosition = localPosition;

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
            light.cullingMask = 1 << DeviceLayer;
            SetLayerRecursively(lightObject, DeviceLayer);
        }

        private void BuildHorizontalDeviceAndFurnitureShadows()
        {
            shadowRoot = new GameObject(
                "Modern Handheld Grounded Shadows"
            ).transform;
            shadowRoot.SetParent(presentationRoot.transform, false);
            shadowRoot.localPosition = Vector3.zero;
            shadowRoot.localRotation = Quaternion.identity;
            shadowRoot.localScale = Vector3.one;
            SetLayerRecursively(shadowRoot.gameObject, DeviceLayer);

            float tableSurfaceWorldY =
                TableRestPosition.y + CinematicTableTopSurfaceY;
            // The flat handheld has a stable 1.568 × 2.464 unit footprint.
            // Keep the masks close to that footprint instead of the superseded
            // full-size upright projection.
            CreateHorizontalShadowQuad(
                "Device Soft Contact Penumbra",
                new Vector3(-0.12f, tableSurfaceWorldY + 0.020f, DeviceRestPosition.z + 0.10f),
                new Vector2(2.35f, 3.45f),
                -3f,
                softShadowMaterial
            );
            CreateHorizontalShadowQuad(
                "Device Core Contact Shadow",
                new Vector3(-0.06f, tableSurfaceWorldY + 0.026f, DeviceRestPosition.z + 0.04f),
                new Vector2(1.95f, 2.95f),
                -1.5f,
                coreShadowMaterial
            );
            CreateHorizontalShadowQuad(
                "Device Base Contact Shadow",
                new Vector3(0f, tableSurfaceWorldY + 0.032f, DeviceRestPosition.z),
                new Vector2(1.62f, 2.55f),
                0f,
                contactShadowMaterial
            );

            float floorWorldY =
                TableRestPosition.y + CinematicFloorSurfaceY + 0.015f;
            float legX = CinematicTableWidth * 0.5f - 1.32f;
            float frontLegZ =
                CinematicTableCenterZ -
                CinematicTableDepth * 0.5f + 1.18f;
            float backLegZ =
                CinematicTableCenterZ +
                CinematicTableDepth * 0.5f - 1.18f;

            CreateLegContactShadow(-legX, frontLegZ, floorWorldY);
            CreateLegContactShadow(legX, frontLegZ, floorWorldY);
            CreateLegContactShadow(-legX, backLegZ, floorWorldY);
            CreateLegContactShadow(legX, backLegZ, floorWorldY);
        }

        private void CreateLegContactShadow(
            float x,
            float z,
            float y)
        {
            CreateHorizontalShadowQuad(
                "Table Leg Contact Shadow",
                new Vector3(x - 0.12f, y, z + 0.08f),
                new Vector2(1.85f, 1.55f),
                0f,
                contactShadowMaterial
            );
        }

        private void CreateHorizontalShadowQuad(
            string objectName,
            Vector3 position,
            Vector2 size,
            float yaw,
            Material material)
        {
            GameObject shadow = GameObject.CreatePrimitive(
                PrimitiveType.Quad
            );
            shadow.name = objectName;
            Destroy(shadow.GetComponent<Collider>());
            shadow.transform.SetParent(shadowRoot, false);
            shadow.transform.localPosition = position;
            shadow.transform.localRotation =
                Quaternion.Euler(90f, yaw, 0f);
            shadow.transform.localScale =
                new Vector3(size.x, size.y, 1f);
            shadow.GetComponent<Renderer>().sharedMaterial = material;
            SetLayerRecursively(shadow, DeviceLayer);
        }

        private void BuildCinematicEnvironmentLights()
        {
            CreateCinematicSpotLight(
                "Cinematic Key Light",
                new Vector3(-8f, 10f, -10f),
                new Vector3(0f, -7.15f, DeviceRestPosition.z + 0.40f),
                new Color(1f, 0.87f, 0.72f, 1f),
                4.2f,
                65f,
                58f,
                LightShadows.Soft
            );
            CreateCinematicSpotLight(
                "Cinematic Camera Fill",
                new Vector3(7f, 1f, -14f),
                new Vector3(0f, -7.12f, DeviceRestPosition.z + 0.45f),
                new Color(0.46f, 0.58f, 0.78f, 1f),
                0.95f,
                60f,
                66f,
                LightShadows.None
            );
            CreateCinematicSpotLight(
                "Cinematic Separation Light",
                new Vector3(3f, 8f, 12f),
                new Vector3(0f, -7.05f, DeviceRestPosition.z + 0.60f),
                new Color(0.38f, 0.50f, 0.72f, 1f),
                0.72f,
                55f,
                52f,
                LightShadows.None
            );
        }

        private void CreateCinematicSpotLight(
            string objectName,
            Vector3 localPosition,
            Vector3 localTarget,
            Color color,
            float intensity,
            float range,
            float spotAngle,
            LightShadows shadows)
        {
            GameObject lightObject = new GameObject(objectName);
            lightObject.transform.SetParent(presentationRoot.transform, false);
            lightObject.transform.localPosition = localPosition;
            lightObject.transform.localRotation = Quaternion.LookRotation(
                localTarget - localPosition,
                Vector3.up
            );

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Spot;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.spotAngle = spotAngle;
            light.shadows = shadows;
            light.shadowStrength = 0.62f;
            light.shadowBias = 0.035f;
            light.shadowNormalBias = 0.30f;
            light.cullingMask = 1 << DeviceLayer;
            SetLayerRecursively(lightObject, DeviceLayer);
        }

        private Material CreateCinematicWoodMaterial(
            string materialName,
            Color tint,
            float smoothness)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find(
                    "BoredomAndDungeons/ModernHandheldTable"
                );

            Material material = CreateMaterial(shader);
            material.name = materialName;
            material.mainTexture =
                tableTexture != null
                    ? tableTexture
                    : Texture2D.grayTexture;
            material.mainTextureScale = new Vector2(1.0f, 1.0f);
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", tint);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", smoothness);
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0f);
            if (material.HasProperty("_SmoothnessTextureChannel"))
                material.SetFloat("_SmoothnessTextureChannel", 0f);
            return material;
        }

        private Material CreateCinematicEnvironmentMaterial(
            string materialName,
            Color color,
            float smoothness)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find(
                    "BoredomAndDungeons/ModernHandheldSurface"
                );
            }

            Material material = CreateMaterial(shader);
            material.name = materialName;
            material.mainTexture = Texture2D.whiteTexture;
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", smoothness);
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0f);
            if (material.HasProperty("_Roughness"))
                material.SetFloat("_Roughness", 1f - smoothness);
            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor(
                    "_EmissionColor",
                    new Color(
                        color.r * 0.025f,
                        color.g * 0.025f,
                        color.b * 0.025f,
                        1f
                    )
                );
            }
            return material;
        }


        private Material CreateCinematicEmissiveMaterial(
            string materialName,
            Color color,
            float intensity)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find(
                    "BoredomAndDungeons/ModernHandheldSurface"
                );
            }

            Material material = CreateMaterial(shader);
            material.name = materialName;
            material.mainTexture = Texture2D.whiteTexture;

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0f);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", 0.18f);
            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(
                    "_EmissionColor",
                    new Color(
                        color.r * intensity,
                        color.g * intensity,
                        color.b * intensity,
                        1f
                    )
                );
            }

            return material;
        }

        private GameObject CreateCinematicCube(
            string objectName,
            Transform parent,
            Vector3 localPosition,
            Vector3 localScale,
            Material material,
            bool castShadows,
            bool receiveShadows)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objectName;
            Destroy(cube.GetComponent<Collider>());
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localRotation = Quaternion.identity;
            cube.transform.localScale = localScale;

            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = castShadows
                ? ShadowCastingMode.On
                : ShadowCastingMode.Off;
            renderer.receiveShadows = receiveShadows;
            SetLayerRecursively(cube, DeviceLayer);
            return cube;
        }

        private GameObject CreateCinematicMeshObject(
            string objectName,
            Transform parent,
            Mesh mesh,
            Material material)
        {
            GameObject meshObject = new GameObject(
                objectName,
                typeof(MeshFilter),
                typeof(MeshRenderer)
            );
            meshObject.transform.SetParent(parent, false);
            meshObject.transform.localPosition = Vector3.zero;
            meshObject.transform.localRotation = Quaternion.identity;
            meshObject.transform.localScale = Vector3.one;

            meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            meshObject.GetComponent<MeshRenderer>().sharedMaterial = material;
            SetLayerRecursively(meshObject, DeviceLayer);
            return meshObject;
        }

        private Mesh CreateCycloramaMesh(
            float width,
            float curveStartZ,
            float wallZ,
            float floorY,
            float wallTopY,
            int curveSegments)
        {
            int profileCount = curveSegments + 2;
            Vector3[] vertices = new Vector3[profileCount * 2];
            Vector2[] uv = new Vector2[profileCount * 2];
            int[] triangles = new int[(profileCount - 1) * 6];
            float radius = wallZ - curveStartZ;
            float halfWidth = width * 0.5f;

            for (int index = 0; index <= curveSegments; index++)
            {
                float progress = index / (float)curveSegments;
                float angle = Mathf.Lerp(-90f, 0f, progress) *
                              Mathf.Deg2Rad;
                float z = curveStartZ + Mathf.Cos(angle) * radius;
                float y = floorY + radius + Mathf.Sin(angle) * radius;
                int vertex = index * 2;
                vertices[vertex] = new Vector3(-halfWidth, y, z);
                vertices[vertex + 1] = new Vector3(halfWidth, y, z);
                uv[vertex] = new Vector2(0f, progress * 0.45f);
                uv[vertex + 1] = new Vector2(1f, progress * 0.45f);
            }

            int topVertex = (profileCount - 1) * 2;
            vertices[topVertex] =
                new Vector3(-halfWidth, wallTopY, wallZ);
            vertices[topVertex + 1] =
                new Vector3(halfWidth, wallTopY, wallZ);
            uv[topVertex] = new Vector2(0f, 1f);
            uv[topVertex + 1] = new Vector2(1f, 1f);

            for (int index = 0; index < profileCount - 1; index++)
            {
                int vertex = index * 2;
                int triangle = index * 6;
                triangles[triangle] = vertex;
                triangles[triangle + 1] = vertex + 2;
                triangles[triangle + 2] = vertex + 1;
                triangles[triangle + 3] = vertex + 1;
                triangles[triangle + 4] = vertex + 2;
                triangles[triangle + 5] = vertex + 3;
            }

            Mesh mesh = new Mesh
            {
                name = "BD Cinematic Cyclorama Mesh",
                vertices = vertices,
                uv = uv,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            generatedMeshes.Add(mesh);
            return mesh;
        }
    }
}
