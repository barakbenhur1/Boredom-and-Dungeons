# Latest Repository Size Reduction Report

```text
Generated UTC: 2026-06-09T17:54:57.382342+00:00
Mode: measured safe cleanup; no Git/GitHub write
History rewrite: NOT PERFORMED
Lossy asset optimization: NOT PERFORMED
Unity GUID/meta modification for size: NOT PERFORMED
```

## Summary

| Measurement | Before | After | Saved |
|---|---:|---:|---:|
| Whole repository | 1.07 GiB | 126.31 MiB | +971.24 MiB (+88.49%) |
| Working tree | 1016.98 MiB | 45.74 MiB | +971.24 MiB |
| `.git` | 80.57 MiB | 80.57 MiB | -35.00 B |
| Files | 12335 | 931 | +11404 |

Positive values in the final column are savings; negative values are an honest net increase, commonly caused when a clean repository had no generated cache to remove but the required audit report/manifest were added. Any cleanup removals were limited to untracked generated/reproducible local artifacts. Tracked source, assets, documentation, Unity `.meta` files and Git history were preserved.

## Removed generated artifacts

| Path | Size | Reason |
|---|---|---|
| .DS_Store | 8.00 KiB | untracked, generated and reproducible local artifact |
| .idea | 8.68 KiB | untracked, generated and reproducible local artifact |
| Library | 969.42 MiB | untracked, generated and reproducible local artifact |
| Logs | 917.14 KiB | untracked, generated and reproducible local artifact |
| Temp | 1.04 MiB | untracked, generated and reproducible local artifact |
| .package_tools/__pycache__ | 190.82 KiB | untracked, generated and reproducible local artifact |
| Assets/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| ProjectGuide/.DS_Store | 8.00 KiB | untracked, generated and reproducible local artifact |
| UserSettings/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| Assets/_Project/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| ProjectGuide/Features/.DS_Store | 10.00 KiB | untracked, generated and reproducible local artifact |
| ProjectGuide/Production/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| ProjectGuide/Tasks/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| Assets/_Project/Art/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |
| Assets/_Project/Art/Materials/.DS_Store | 8.00 KiB | untracked, generated and reproducible local artifact |
| Assets/_Project/Art/Prefabs/.DS_Store | 8.00 KiB | untracked, generated and reproducible local artifact |
| Assets/_Project/Art/Textures/.DS_Store | 6.00 KiB | untracked, generated and reproducible local artifact |


## Retained cleanup candidates

_None._


## 100 largest working-tree files before cleanup

| Path | Size |
|---|---|
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/relay_win.exe | 96.06 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/relay_linux | 92.45 MiB |
| Library/PackageCache/com.unity.sysroot.linux-x86_64@1998d1c7730e/data~/payload.tar.7z | 77.74 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/gateway/codex-acp-darwin-x64 | 72.30 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/gateway/codex-acp.exe | 71.83 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/gateway/codex-acp-darwin-arm64 | 68.36 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/relay_mac_x64.app/Contents/MacOS/relay_mac_x64 | 68.22 MiB |
| Library/PackageCache/com.unity.toolchain.macos-x86_64-linux-x86_64@0350a43511cb/data~/payload.tar.7z | 52.23 MiB |
| Library/Artifacts/ae/ae543beacee39aee96206cc2a7f11d5a | 33.56 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/relay_mac_x64 | 25.98 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/RelayApp~/relay_mac_arm64 | 23.53 MiB |
| Library/Artifacts/d3/d308fdf8cbc85e5dd3452a9b0377e1ee | 16.02 MiB |
| Library/ArtifactDB | 10.18 MiB |
| Library/Artifacts/8e/8e6a15a5a36362a3a85b3f6a5ea913ee | 10.12 MiB |
| Library/SourceAssetDB | 7.38 MiB |
| Library/Bee/200b0aE.dag | 6.44 MiB |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_FRONT_TEXTURE_STICKER_V1.png | 6.05 MiB |
| Library/Artifacts/c2/c2c3118b296b3787480461aaf65d786e | 6.02 MiB |
| Library/Artifacts/8e/8e284cdb7038c10fbe6e98d4cf5e5452 | 6.02 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/Plugins/CodeAnalysis/Microsoft.CodeAnalysis.CSharp.dll | 5.82 MiB |
| Library/Artifacts/0f/0f4114e6a297d3c878b0a8b911eb6504 | 5.35 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/ThirdParty~/ripgrep/rg_linux | 5.19 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/ThirdParty~/ripgrep/rg_mac_x64 | 4.29 MiB |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_SHELL_GRADIENT_V1.png | 4.27 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/ThirdParty~/ripgrep/rg_win.exe | 4.08 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/ThirdParty~/ripgrep/rg_mac_arm64 | 3.88 MiB |
| Library/Bee/200b0aE.dag.json | 3.87 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_TABLE_DARK_WOOD_SOURCE_V1.png | 3.72 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_ORTHOGRAPHIC_TEXTURE_SHEET_V1.png | 3.25 MiB |
| Library/Bee/200b0aE.dag.payloads | 3.01 MiB |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/Plugins/CodeAnalysis/Microsoft.CodeAnalysis.dll | 2.77 MiB |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_SHARP_V1.png | 2.63 MiB |
| Library/Artifacts/85/85ecd0f4c634b1364f98f91cc2134612 | 2.32 MiB |
| Library/Artifacts/0d/0d7b118b6f4fc90a288832654acd0f19 | 2.32 MiB |
| Library/Artifacts/e0/e0b5d163e234686f58c4a3a37a721f67 | 2.32 MiB |
| Library/Artifacts/8c/8cc1b077aaae095d6f060e61d751cc5c | 2.32 MiB |
| Library/Artifacts/86/8678476b3ca6a092bfb594d45e3fe4c1 | 2.32 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_THREE_QUARTER_GIRL_V1.png | 2.29 MiB |
| Library/Bee/200b0aE-inputdata.json | 2.22 MiB |
| Library/Artifacts/aa/aacb3fa2d53edcecacd162d505aa1bc9 | 2.17 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_MATERIAL_CLOSEUP_GIRL_V1.png | 2.15 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_FRONT_GIRL_V1.png | 2.14 MiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/Newtonsoft.Json.pdb | 2.09 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_MENU_GIRL_V1.png | 2.08 MiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/AOT/Newtonsoft.Json.pdb | 2.07 MiB |
| Library/PackageCache/com.unity.ugui@52420b88ab53/Package Resources/TMP Examples & Extras.unitypackage | 2.02 MiB |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_MENU_BOY_V1.png | 2.02 MiB |
| Library/Search/98957a664bd18c47a3e41b2a0189ef53.33329.b.index | 1.98 MiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/Modules/Unity.AI.Animate/Motion/Runtime/Prefabs/Actors/biped_v1/Biped_Humanoid.fbx | 1.96 MiB |
| Library/Artifacts/eb/eb2ad1f99d3e1e1c26926b2a1f1d49da | 1.35 MiB |
| Library/Artifacts/94/94249dc3a6897e4d1b15c500663e3e24 | 1.35 MiB |
| Library/Artifacts/b4/b49529acbe32aab17644fc3de46102fa | 1.35 MiB |
| Library/Artifacts/0a/0a52096e6e491d221264e7b446782650 | 1.35 MiB |
| Library/Artifacts/24/24a4ad154c34af565d1cd13c0ccfb65a | 1.35 MiB |
| Library/Artifacts/8c/8c0e83f5c138664885f7a7c47d0f5d5b | 1.35 MiB |
| Library/Artifacts/69/69c48f18a41fb9fdb3c13094cb84da25 | 1.35 MiB |
| Library/Artifacts/7d/7dc5da2b50961b1fff65d2bcdd45d0a6 | 1.35 MiB |
| Library/Artifacts/29/2911b9f074a259f2e623fea8f7b0b782 | 1.35 MiB |
| Library/Artifacts/17/176a5d86a927453ef3aca5de63cecb64 | 1.35 MiB |
| Library/Artifacts/81/81216cd8de79f4edb70882b8f21cdfc9 | 1.35 MiB |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_SETTINGS_V1.png | 1.11 MiB |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_CREDITS_V1.png | 1.10 MiB |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_PROGRESSION_V1.png | 1.06 MiB |
| Temp/__Backupscenes/0.backup | 1.03 MiB |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_QUIT_V1.png | 1.02 MiB |
| Library/Artifacts/e8/e8b19cf5ddda3deed31d7ff988a8c5fe | 1.01 MiB |
| Library/Artifacts/26/26ddf53d61846860889301d84c0775a7 | 1.01 MiB |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_RESUME_V1.png | 960.39 KiB |
| Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Editor.dll | 942.50 KiB |
| Library/ScriptAssemblies/Unity.AI.Assistant.Editor.dll | 942.50 KiB |
| Library/PackageCache/com.unity.ugui@52420b88ab53/Package Resources/TMP Essential Resources.unitypackage | 919.65 KiB |
| Library/Artifacts/0c/0c10022c42adb13aca43ce7f6ad30a4b | 854.74 KiB |
| Library/PackageCache/com.unity.sysroot.linux-x86_64@1998d1c7730e/LICENSE.md | 851.95 KiB |
| Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Editor.dll | 828.50 KiB |
| Library/ScriptAssemblies/Unity.AI.MCP.Editor.dll | 828.50 KiB |
| Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.dll | 737.50 KiB |
| Library/ScriptAssemblies/Unity.Mathematics.dll | 737.50 KiB |
| Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp.dll | 717.50 KiB |
| Library/ScriptAssemblies/Assembly-CSharp.dll | 717.50 KiB |
| Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.UI.Editor.dll | 715.50 KiB |
| Library/ScriptAssemblies/Unity.AI.Assistant.UI.Editor.dll | 715.50 KiB |
| Library/Artifacts/99/990484db7bf6bd7e71931b5d325bbba9 | 713.80 KiB |
| Library/PackageCache/com.unity.ugui@52420b88ab53/Documentation~/images/UI_Main.png | 710.77 KiB |
| Library/Artifacts/f2/f2ff21bd62850f16fef3758a487eb755 | 701.45 KiB |
| Library/Artifacts/fe/fece7d1ed84b2cae065826d71ce52cb7 | 701.44 KiB |
| Library/Artifacts/3b/3b01386bc110d006ec17672b2b9c4f90 | 701.43 KiB |
| Library/Artifacts/38/38c9b259680855766fea72c567b1d590 | 701.43 KiB |
| Library/Artifacts/16/16461b2f5a8f89004c31cf0e768534e6 | 701.42 KiB |
| Library/Artifacts/fa/fa376e065c7dc00714be1929958e3453 | 701.42 KiB |
| Library/Artifacts/44/44bc078d8604e747582313646ac707c0 | 701.41 KiB |
| Library/Artifacts/d6/d650ca127d590957fe9561da2044321e | 688.75 KiB |
| Library/Artifacts/4f/4f04f5a8b95876736e24aaff467ae988 | 687.50 KiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/Newtonsoft.Json.xml | 685.95 KiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/AOT/Newtonsoft.Json.xml | 684.70 KiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/Newtonsoft.Json.dll | 680.00 KiB |
| Library/PackageCache/com.unity.nuget.newtonsoft-json@74deb55db2a0/Runtime/AOT/Newtonsoft.Json.dll | 675.50 KiB |
| Library/Bee/fullprofile.json | 642.53 KiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/Documentation~/Images/chat-interface.png | 612.41 KiB |
| Library/PackageCache/com.unity.ai.assistant@e1f49a972172/Plugins/Shared/System.Text.Json.dll | 594.26 KiB |


## Largest top-level directories before cleanup

| Directory | Size |
|---|---|
| Library | 969.42 MiB |
| Assets | 26.16 MiB |
| ProjectGuide | 19.11 MiB |
| Temp | 1.04 MiB |
| Logs | 917.14 KiB |
| UserSettings | 90.94 KiB |
| Assembly-CSharp.csproj | 81.98 KiB |
| Assembly-CSharp-Editor.csproj | 74.89 KiB |
| ProjectSettings | 56.26 KiB |
| .codex | 15.79 KiB |
| tools | 14.71 KiB |
| Packages | 10.77 KiB |
| .idea | 8.68 KiB |
| .DS_Store | 8.00 KiB |
| .gitignore | 5.08 KiB |
| AGENTS.md | 3.77 KiB |
| Boredom-and-Dungeons.sln | 1018.00 B |
| README.md | 558.00 B |


## File-type distribution before cleanup

| Extension | Count | Size |
|---|---|---|
| [no extension] | 1638 | 537.30 MiB |
| .exe | 3 | 171.96 MiB |
| .7z | 2 | 129.97 MiB |
| .png | 677 | 55.48 MiB |
| .dll | 188 | 36.68 MiB |
| .cs | 2886 | 26.29 MiB |
| .pdb | 112 | 12.23 MiB |
| .json | 100 | 7.30 MiB |
| .dag | 1 | 6.44 MiB |
| .payloads | 1 | 3.01 MiB |
| .rsp | 110 | 2.98 MiB |
| .md | 477 | 2.97 MiB |
| .unitypackage | 2 | 2.92 MiB |
| .unity | 1 | 2.79 MiB |
| .wav | 22 | 2.73 MiB |
| .fbx | 3 | 2.46 MiB |
| .index | 2 | 1.98 MiB |
| .xml | 9 | 1.66 MiB |
| .meta | 4775 | 1.53 MiB |
| .psd | 31 | 1.44 MiB |
| .backup | 1 | 1.03 MiB |
| .jpg | 5 | 712.29 KiB |
| .bin | 119 | 511.71 KiB |
| .jsonl | 1 | 505.84 KiB |
| .digestcache | 1 | 479.92 KiB |
| .traceevents | 3 | 435.28 KiB |
| .txt | 84 | 326.58 KiB |
| .uxml | 287 | 310.80 KiB |
| .uss | 216 | 270.99 KiB |
| .log | 9 | 265.97 KiB |
| .dag_derived | 1 | 239.31 KiB |
| .catalog | 1 | 220.15 KiB |
| .gif | 4 | 190.08 KiB |
| .db | 4 | 187.43 KiB |
| .mvfrm | 242 | 185.99 KiB |
| .csproj | 2 | 156.87 KiB |
| .map | 1 | 138.08 KiB |
| .state | 1 | 138.08 KiB |
| .api | 42 | 117.48 KiB |
| .asset | 41 | 98.92 KiB |
| .dwlt | 2 | 81.95 KiB |
| .shader | 20 | 65.70 KiB |
| .svg | 8 | 65.63 KiB |
| .prefab | 5 | 58.58 KiB |
| .icns | 1 | 44.02 KiB |
| .asmdef | 66 | 41.31 KiB |
| .outputdata | 1 | 16.70 KiB |
| .toml | 6 | 15.79 KiB |
| .py | 3 | 14.71 KiB |
| .recommendations | 1 | 13.55 KiB |
| .questionnaire | 1 | 12.70 KiB |
| .graph | 1 | 7.18 KiB |
| .cginc | 2 | 6.99 KiB |
| .mat | 3 | 6.58 KiB |
| .pref | 4 | 5.77 KiB |
| .tss | 2 | 3.72 KiB |
| .rsp2 | 55 | 3.21 KiB |
| .st | 2 | 2.79 KiB |
| .compute | 1 | 2.08 KiB |
| .settings | 1 | 1.32 KiB |
| .overlay | 1 | 1.05 KiB |
| .sln | 1 | 1018.00 B |
| .plist | 1 | 825.00 B |
| .yml | 2 | 749.00 B |
| .info | 1 | 127.00 B |
| .md5 | 1 | 32.00 B |
| .bytes | 1 | 15.00 B |
| .dag_fsmtime | 1 | 8.00 B |
| .pid | 1 | 5.00 B |
| .prefs | 1 | 0.00 B |
| .modulecompilationtrigger | 34 | 0.00 B |


## Byte-identical duplicate groups

No duplicate is removed automatically. Unity paths, GUIDs, dynamic loading, tooling and documentation may depend on separate copies.

| Each size | Theoretical savings | SHA-256 | Paths |
|---|---|---|---|
| 942.50 KiB | 942.50 KiB | e7fce756ed95036fcead039a85ab36093b385347c1ff17f792b18945141ff418 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.Editor.dll |
| 828.50 KiB | 828.50 KiB | 5983b1a5c0842a0a68e382ffabb30f5a44a8dfbf778721d947744e90e1e10c61 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.MCP.Editor.dll |
| 737.50 KiB | 737.50 KiB | 6e507bc37b07d50b4f65ebe117c8d78d25fa5f86442e968ffa7a3eefc6a929c4 | Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.dll<br>Library/ScriptAssemblies/Unity.Mathematics.dll |
| 717.50 KiB | 717.50 KiB | a411f26a3c96af559d93bd33dbfe10c02c23bc2df93c7ce1198bfe491028fda7 | Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp.dll<br>Library/ScriptAssemblies/Assembly-CSharp.dll |
| 715.50 KiB | 715.50 KiB | 4be5939733f51d3f310b3f9739cd34156bc38ed95c5e341f15fa123242a48dba | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.UI.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.UI.Editor.dll |
| 539.50 KiB | 539.50 KiB | 44cf71fe61870b6d9b36b7fab8231694f127f808cf36aa770611172314ca134d | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Image.dll<br>Library/ScriptAssemblies/Unity.AI.Image.dll |
| 460.00 KiB | 460.00 KiB | 906717e297e0c6ed3c7732150d6a866cddf61e0f2f6e53ff057bae5a533c6955 | Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.dll<br>Library/ScriptAssemblies/Unity.TextMeshPro.dll |
| 403.00 KiB | 403.00 KiB | 5b12485b5ccd2675f6375bd69f9b96ba60e29c10749bb1bbbe53f4fae56bfb09 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Runtime.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.Runtime.dll |
| 352.00 KiB | 352.00 KiB | f2782a7cd44d85e812df4225345582b94196ae7b1288c748fa010d2b16d5f283 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Tools.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.Tools.Editor.dll |
| 348.50 KiB | 348.50 KiB | 82fd76854b4922a611ea47ad567d3b49744fcb72c3ab98cdb0a627e28ffa9e0c | Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp-Editor.dll<br>Library/ScriptAssemblies/Assembly-CSharp-Editor.dll |
| 336.50 KiB | 336.50 KiB | 2c184ae6c8d03014e6d7a9adaa57d712538d74a008e588d0449fc0e987c345f1 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Mesh.dll<br>Library/ScriptAssemblies/Unity.AI.Mesh.dll |
| 331.59 KiB | 331.59 KiB | 800f96a5e0c4154fc6d8a8a4e12fea30014a728ac3129862b9a99e42586536be | Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.pdb<br>Library/ScriptAssemblies/Unity.Mathematics.pdb |
| 327.50 KiB | 327.50 KiB | df42ab21733f58729eb37805b55d79eb182097303f5defad2265ff446e21408f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Pbr.dll<br>Library/ScriptAssemblies/Unity.AI.Pbr.dll |
| 310.50 KiB | 310.50 KiB | 40e37bb89309ae237c0f966e894d1546b9325601db083cf59dbf2e3ad0a5dbd4 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Sound.dll<br>Library/ScriptAssemblies/Unity.AI.Sound.dll |
| 297.27 KiB | 297.27 KiB | b308117ed6beb3b6866ea52330b139318c1ccae0f6e6112063e83dfc2e5e4884 | Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp.pdb<br>Library/ScriptAssemblies/Assembly-CSharp.pdb |
| 297.00 KiB | 297.00 KiB | f8fd189e26112ebedc06f1703c57d11816f125562ccc140018ae1bc1d9c0ea63 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Animate.dll<br>Library/ScriptAssemblies/Unity.AI.Animate.dll |
| 285.50 KiB | 285.50 KiB | 813d24e0d996d2ebe8de6344ca8fc45773b3e692ac88919801ae99abd00e6459 | Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.Editor.dll<br>Library/ScriptAssemblies/Unity.TextMeshPro.Editor.dll |
| 276.91 KiB | 276.91 KiB | 0e945aacbd26026a5427bfd969100f3ec96ad38f1dd2839f1a677d590775b13c | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.UI.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.UI.Editor.pdb |
| 272.44 KiB | 272.44 KiB | 8bf3ccb2f3dc2cf8314da2f588ce505e24e001fddc876353093ebb092616bd60 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Editor.pdb |
| 264.50 KiB | 264.50 KiB | 8bece4af46e5410333cd2f9133d824a2e7d53bad2da2c275f9eef83c3498c581 | Library/Bee/artifacts/200b0aE.dag/UnityEngine.UI.dll<br>Library/ScriptAssemblies/UnityEngine.UI.dll |
| 232.50 KiB | 232.50 KiB | 7e3527e73c291349d39b3730b1463a3ed4ec9eb6aaf920bfe36af12191934626 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.UI.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.UI.dll |
| 210.00 KiB | 210.00 KiB | f3d0fdb87bd1d18439b387a148e6708b8987c81f2b0a7b09b731300e45cc29e5 | Library/Bee/artifacts/200b0aE.dag/Unity.2D.Sprite.Editor.dll<br>Library/ScriptAssemblies/Unity.2D.Sprite.Editor.dll |
| 196.40 KiB | 196.40 KiB | 75a9635d6a08d09099749b6cca95988799e7c2d1c04f7ed9e0b496f3add9fada | Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.MCP.Editor.pdb |
| 192.28 KiB | 192.28 KiB | 1c0d6901df01cb603b55654b60320493e0ef1705d6c4366bda6923c469257d9a | Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.pdb<br>Library/ScriptAssemblies/Unity.TextMeshPro.pdb |
| 185.51 KiB | 185.51 KiB | 6fc4853eff89f88ad270a2b81ae34902b72a4e8a1a1756f1d8039015758037a9 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Image.pdb<br>Library/ScriptAssemblies/Unity.AI.Image.pdb |
| 3.32 KiB | 179.35 KiB | 6a6cd81d17d24d02254d9c771d9b5a0ca294147a5ea4ce2eedc71c8760aa6c67 | Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp-Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/HFDownloader.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.2D.Sprite.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Animate.Motion.Runtime.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Animate.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.API.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Agent.Dynamic.Extension.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Annotations.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.AssetGenerators.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Bridge.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Integrations.Profiler.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Runtime.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Tools.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.UI.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Asset.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Contexts.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.Srp.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Redux.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Sdk.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Tools.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.UI.AIDropdownIntegrations.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.UI.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Image.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Runtime.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Mesh.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.ModelSelector.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Pbr.Srp.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Pbr.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Search.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Sound.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Accounts.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Asset.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Async.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Compliance.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.GenerationContextMenu.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.GenerationObjectPicker.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Utility.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Tracing.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Common.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Rider.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Sysroot.Linux_x86_64.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.SysrootPackage.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.Editor.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/Unity.Toolchain.Macos-x86_64-Linux-x86_64.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/UnityEditor.UI.Analytics.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/UnityEditor.UI.dll.mvfrm<br>Library/Bee/artifacts/200b0aE.dag/UnityEngine.UI.dll.mvfrm |
| 170.00 KiB | 170.00 KiB | efe81ec941585293037c9f47102124f2384e86f8c0080e3be4300d58d8d04f0f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Redux.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.Redux.dll |
| 163.50 KiB | 163.50 KiB | 54ec02b82e60db5b7ea3477615b636bba8d01e211b344b6925620454d6aa7313 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Tools.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.Tools.dll |
| 156.00 KiB | 156.00 KiB | cb0db467a50fc388634d9097d9480c28d1c91ae9c30ba79c15352cfad28616f2 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Accounts.dll<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Accounts.dll |
| 146.55 KiB | 146.55 KiB | d82ecabd68fe72a9e0b87a8a1c4e6ef22bf18489d9477ffc81f3489704221b29 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Runtime.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Runtime.pdb |
| 146.50 KiB | 146.50 KiB | 8a5c84317f37d440680253ca7a8d5d2d3a32a996bcf0fe07729590ea982e0d3d | Library/Bee/artifacts/200b0aE.dag/Unity.AI.ModelSelector.dll<br>Library/ScriptAssemblies/Unity.AI.ModelSelector.dll |
| 140.50 KiB | 140.50 KiB | 9d78a5e75ac5b6de62abe3ea84e45994adf4e8fa48088f515ea2676eeaeabe18 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Search.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Search.Editor.dll |
| 139.11 KiB | 139.11 KiB | a09e4392256afd5390dc5cc27d52271ba839ee63cad32913e6e645f86916458d | Library/Bee/artifacts/200b0aE.dag/UnityEngine.UI.pdb<br>Library/ScriptAssemblies/UnityEngine.UI.pdb |
| 11.44 KiB | 125.89 KiB | 5dad02788105abb0f0e9c317df45520484b8ef6559a7974e65507ddafbb1d5cb | Library/Bee/artifacts/200b0aE.dag/HFDownloader.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.2D.Sprite.Editor.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Agent.Dynamic.Extension.Editor.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Annotations.Editor.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Bridge.Editor.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Contexts.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.Srp.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Async.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Compliance.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Utility.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.Rider.Editor.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.SysrootPackage.Editor.dll.mvfrm.rsp |
| 122.50 KiB | 122.50 KiB | fce36c64a7125a4bc8798c736e2aa8e9db282f429f19b532e9505236dab70eb5 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.AssetGenerators.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.AssetGenerators.Editor.dll |
| 119.39 KiB | 119.39 KiB | e32359ae907e02d78632f3c5bce98e368deaa764ef3ff9b450d9924a70fc2efb | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Mesh.pdb<br>Library/ScriptAssemblies/Unity.AI.Mesh.pdb |
| 117.09 KiB | 117.09 KiB | 3b44278ae5ff5f4e2f6bbaeab0733d57e0a55a5d02993029388c2bdb41785d2f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Pbr.pdb<br>Library/ScriptAssemblies/Unity.AI.Pbr.pdb |
| 113.43 KiB | 113.43 KiB | 5c8b0a9e5d80ed258fc6a0c34524a8045695c62471e6db2582456a60da4d58ba | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Sound.pdb<br>Library/ScriptAssemblies/Unity.AI.Sound.pdb |
| 110.43 KiB | 110.43 KiB | 65a093ddee1bdae1d07159e99bb11611c6b1eca9d2920ec759d2120f28a00b98 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Animate.pdb<br>Library/ScriptAssemblies/Unity.AI.Animate.pdb |
| 102.34 KiB | 102.34 KiB | 78387804503413dc844c0f86385c64b3e760f43d06bad9d2a6b7a6aacf8181ac | Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.Editor.pdb<br>Library/ScriptAssemblies/Unity.TextMeshPro.Editor.pdb |
| 99.50 KiB | 99.50 KiB | 8fa20382f40ad5b4347b2a6ee13513e166da922e926d6d462e3bc14f6783a142 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Integrations.Profiler.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.Integrations.Profiler.Editor.dll |
| 96.00 KiB | 96.00 KiB | 15d12657d55b2c629c76e4818577dd712b61d7b0aa3a2b8aa71817292ce26465 | Library/Bee/artifacts/200b0aE.dag/UnityEditor.UI.dll<br>Library/ScriptAssemblies/UnityEditor.UI.dll |
| 95.53 KiB | 95.53 KiB | d692e3c016f401ddd8f357d390426cce9cdeb07abf667c3346d03e10c7ec40c4 | Library/Bee/artifacts/200b0aE.dag/Unity.2D.Sprite.Editor.pdb<br>Library/ScriptAssemblies/Unity.2D.Sprite.Editor.pdb |
| 95.00 KiB | 95.00 KiB | 9da13f582676d2cfe0913fbb791d3e8d5a4c3f9e1100e1f877afd31454ab0141 | Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Editor.dll<br>Library/ScriptAssemblies/Unity.Multiplayer.Center.Editor.dll |
| 93.50 KiB | 93.50 KiB | ad1b821c3d0305e324535ec36eb0a66e4bd80ccb8e7719bdab528aed4231cc39 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.IO.dll |
| 90.69 KiB | 90.69 KiB | 2ddfcd62c82675208529f6380be46f57f9274a6183536fbae7a68762f15dcdbd | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Tools.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Tools.Editor.pdb |
| 90.35 KiB | 90.35 KiB | 829d4402563bebe2e4fdb3c3718174955b6cbdbade06b0afd5741c0740ced8e0 | Library/Bee/artifacts/200b0aE.dag/Assembly-CSharp-Editor.pdb<br>Library/ScriptAssemblies/Assembly-CSharp-Editor.pdb |
| 87.55 KiB | 87.55 KiB | 8b66382500af2ee46761e10b3231f3d1a611f8a9d929c4f8e797825c8253e09f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.UI.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.UI.pdb |
| 75.65 KiB | 75.65 KiB | 5f5ff50904422a8436172296b5887e75fb96f0a1589d47e12853c069523fad2f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Accounts.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Accounts.pdb |
| 68.50 KiB | 68.50 KiB | 5e45593ee5f0c5eb9a3ffac9a4b1b7c6ebe87b4882c2c59ad7e66706ace9c789 | Library/Bee/artifacts/200b0aE.dag/Unity.Rider.Editor.dll<br>Library/ScriptAssemblies/Unity.Rider.Editor.dll |
| 63.50 KiB | 63.50 KiB | 47225642d70376584cf9b36a1c4657805b89c41463d7f259f309474803bc69b5 | Library/Bee/artifacts/200b0aE.dag/HFDownloader.dll<br>Library/ScriptAssemblies/HFDownloader.dll |
| 57.30 KiB | 57.30 KiB | 941235b80a97cf534834d5f0a5e5a952f4f553557cd908571362326cbde72853 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Search.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Search.Editor.pdb |
| 55.54 KiB | 55.54 KiB | cd1ded8831565437cd2385ef9a2371c14b683fb7522b99ce8f17640c2cf3a703 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Redux.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.Redux.pdb |
| 54.16 KiB | 54.16 KiB | 760d6ea4d50fcb04ead17e5135d93e43c10c1e15ea4b50f0ef67c4bfddbca605 | Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Editor.pdb<br>Library/ScriptAssemblies/Unity.Multiplayer.Center.Editor.pdb |
| 53.90 KiB | 53.90 KiB | 0b1553bc2f78ff95b8f571c18135041608a63ad7729faca20a793036415023ee | Library/Bee/artifacts/200b0aE.dag/Unity.AI.ModelSelector.pdb<br>Library/ScriptAssemblies/Unity.AI.ModelSelector.pdb |
| 53.37 KiB | 53.37 KiB | 10fe2f59e814d0c9de2afe59a08548343166115abf4dc14a76d06e0ded732964 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Tools.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.Tools.pdb |
| 46.84 KiB | 46.84 KiB | 5db99af13743a3db47353e09661ee06417c85b5fab143830b0f9652ea53f90d2 | Library/Bee/artifacts/200b0aE.dag/UnityEditor.UI.pdb<br>Library/ScriptAssemblies/UnityEditor.UI.pdb |
| 46.77 KiB | 46.77 KiB | 38b51cc5a20e9990122e6a87b333705c61df5b4fb5db014a86a55d6fe435a312 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.IO.pdb |
| 11.29 KiB | 45.16 KiB | 49ac0a0dafa2c319d2e125243545090984cc6977d9302e688c9b13410f42deb7 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Runtime.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.AI.Tracing.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Common.dll.mvfrm.rsp<br>Library/Bee/artifacts/200b0aE.dag/Unity.TextMeshPro.dll.mvfrm.rsp |
| 41.57 KiB | 41.57 KiB | b1ef40ad13a564a467b455e90ecaba384a8afd744dcbf0a9034867924613db70 | Library/Bee/artifacts/200b0aE.dag/Unity.Rider.Editor.pdb<br>Library/ScriptAssemblies/Unity.Rider.Editor.pdb |
| 6.75 KiB | 40.51 KiB | 2268b54ca274d130ca604924befd2aae0b5e32cd056bfb770047c54bc8642152 | Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_025820.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_085312.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_093516.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_105216.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_114027.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_133859.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_latest.txt |
| 39.00 KiB | 39.00 KiB | 078b6f27581db47e9ca2526680c199ce53141197e8cc42e480999ac0d44f561a | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Asset.dll<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Asset.dll |
| 38.00 KiB | 38.00 KiB | 156431927c9cba3f6b7e78a7f30e520615ebe2c3eb51f59b940381d74f4b80ec | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Tracing.dll<br>Library/ScriptAssemblies/Unity.AI.Tracing.dll |
| 37.60 KiB | 37.60 KiB | 947a16cb65d84d11f1ac38c66ffdeaa8c3138c47d7cd96c2101ffa233b45d517 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Integrations.Profiler.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Integrations.Profiler.Editor.pdb |
| 36.85 KiB | 36.85 KiB | 19f4e3240cc69041f34da743186cddcabec9754068f978280226e309152d1ff2 | Library/Bee/artifacts/200b0aE.dag/HFDownloader.pdb<br>Library/ScriptAssemblies/HFDownloader.pdb |
| 36.23 KiB | 36.23 KiB | 620df7e5311440824345bb362178b0fe011562d7e3baf23b165a1ce7898ffb87 | Library/PackageCache/com.unity.ugui@52420b88ab53/Documentation~/images/UI_ImageInspector184.png<br>Library/PackageCache/com.unity.ugui@52420b88ab53/Documentation~/images/UI_RawImageInspector184.png |
| 35.64 KiB | 35.64 KiB | 10cb9aa294450a50325ce3c032d5940f45a8f684cbf54e0d859d267c4b76beab | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.AssetGenerators.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.AssetGenerators.Editor.pdb |
| 34.50 KiB | 34.50 KiB | 4dd090208342592880278887bca0a92d48f08fb873c5f30427199848ca80aee3 | Library/Bee/artifacts/200b0aE.dag/Unity.SysrootPackage.Editor.dll<br>Library/ScriptAssemblies/Unity.SysrootPackage.Editor.dll |
| 34.50 KiB | 34.50 KiB | 0898d21b36991aee7cf85f9cf576af9d597fd6d5ac9b503836432873861569cf | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Bridge.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.Bridge.Editor.dll |
| 29.39 KiB | 29.39 KiB | e07c07fbb45ac900083dd966d9de2d6471c92c7836bedb3aade35f76d9d52e79 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Asset.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Asset.pdb |
| 29.20 KiB | 29.20 KiB | 035a405faf334df50360eddcae320544a91f81ee40fe48053964f77a97c7b82b | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Tracing.pdb<br>Library/ScriptAssemblies/Unity.AI.Tracing.pdb |
| 28.83 KiB | 28.83 KiB | 33fa642b0928f181c8b5ff32235b025d3ed3389e4e015fd67544b6e0cf987c30 | Library/Bee/artifacts/200b0aE.dag/Unity.SysrootPackage.Editor.pdb<br>Library/ScriptAssemblies/Unity.SysrootPackage.Editor.pdb |
| 28.83 KiB | 28.83 KiB | dff30048dbc3167eb0127f1fe2f51837ad9e047e61fc202a1da503aace11f3c8 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Bridge.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Bridge.Editor.pdb |
| 6.56 KiB | 26.23 KiB | 0668be7b1c44d341f8f9e6f5689b243819fe7e895ac5f4c584ad943c46f4b134 | Library/BoredomAndDungeons/StabilityReports/source_scan_20260608_235603.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_000533.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_000534.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_001332.txt<br>Library/BoredomAndDungeons/StabilityReports/source_scan_20260609_001334.txt |
| 26.18 KiB | 26.18 KiB | aff7457924412b459e778de28a725551acb27fae1faf0e1134ca46c15b9b20a6 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Asset.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.Asset.pdb |
| 26.00 KiB | 26.00 KiB | 894fc84836055a5257fca194c0ca5adf0043d96f0a1acc0b2efba1fb27488a0b | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Asset.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.Asset.dll |
| 25.50 KiB | 25.50 KiB | a7d69f567eaa75c2779f7c3f655c7bf61822d682d30c29d099f398d1fa200136 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.API.Editor.dll<br>Library/ScriptAssemblies/Unity.AI.Assistant.API.Editor.dll |
| 24.67 KiB | 24.67 KiB | 00a9d7abb44b6bae95efe145800c1a4419c381f0c1330792b9c6bd3ac2624207 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.API.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.API.Editor.pdb |
| 23.86 KiB | 23.86 KiB | 3067edd69db1ff9d01c481fe9f0af66df94edb35d2e0905e3e94f0b1d0d0ea6d | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Sdk.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.Sdk.pdb |
| 23.84 KiB | 23.84 KiB | 9b404f767b82474699d13c7e4562d44005341aee81a830c8c59b44c957a34ff2 | Library/PackageCache/com.unity.ugui@52420b88ab53/Documentation~/images/ImageCtrlExample.png<br>Library/PackageCache/com.unity.ugui@52420b88ab53/Documentation~/images/RawImageCtrlExample.png |
| 23.80 KiB | 23.80 KiB | c9771372c86025d2194dca7e9f7c9470e4942be7f91221460703355ff54d2faa | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Annotations.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Annotations.Editor.pdb |
| 23.37 KiB | 23.37 KiB | d30859b85692542d47c7bb1bfd4ec8ab6246260f851e8fa5d3988a375f57e2ea | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Async.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Async.pdb |
| 23.05 KiB | 23.05 KiB | 0121048d374450f3dcd161b41c8f1aaac63e9f8309154017a47ae00b6ab7f753 | Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.Editor.pdb<br>Library/ScriptAssemblies/Unity.Mathematics.Editor.pdb |
| 23.00 KiB | 23.00 KiB | 852379db787ec07d26fa637143f1695c2b99d3cd6105b68cdd061e4152f72837 | Library/Bee/artifacts/200b0aE.dag/Unity.Mathematics.Editor.dll<br>Library/ScriptAssemblies/Unity.Mathematics.Editor.dll |
| 22.50 KiB | 22.50 KiB | c9d1348b2cc24b81cdbb3882c6677a5a2a69ef994f2fd951174ef0cdc7567c84 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Assistant.Agent.Dynamic.Extension.Editor.pdb<br>Library/ScriptAssemblies/Unity.AI.Assistant.Agent.Dynamic.Extension.Editor.pdb |
| 22.16 KiB | 22.16 KiB | 977707fc444c6d1192ea7276505105c3ff9e6a383679a1cf6dcee372b536bd11 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Contexts.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.Contexts.pdb |
| 21.96 KiB | 21.96 KiB | adb7b462a63cb6f05b97efa099dd294397912d15a96920a69428f810c77a57dc | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Utility.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Utility.pdb |
| 21.50 KiB | 21.50 KiB | 61e252ddad7a5522d1e122b832f0112319889d412b2bfaf5e59734a7c75f978f | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.Sdk.dll<br>Library/ScriptAssemblies/Unity.AI.Generators.Sdk.dll |
| 21.45 KiB | 21.45 KiB | eeea1ff12c17fd2d90cc06494796252955faa053435275a612d6c13829211b48 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.UI.AIDropdownIntegrations.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.UI.AIDropdownIntegrations.pdb |
| 20.91 KiB | 20.91 KiB | b2b09bc80ea5bc83abe8979ab6dc4140c55192a385c38e26d6793bc2d8da0844 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.MCP.Runtime.pdb<br>Library/ScriptAssemblies/Unity.AI.MCP.Runtime.pdb |
| 20.88 KiB | 20.88 KiB | 56a9f99fb5d8593b19adff27981ff72d55a99505fc110e34f6809b9832bf3549 | Library/Bee/artifacts/200b0aE.dag/Unity.Multiplayer.Center.Common.pdb<br>Library/ScriptAssemblies/Unity.Multiplayer.Center.Common.pdb |
| 20.80 KiB | 20.80 KiB | 3abf41501c5a988d1fd8385bc798b6e377a640ef9344ecaf5c280d26c0950a17 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.GenerationObjectPicker.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.GenerationObjectPicker.pdb |
| 20.63 KiB | 20.63 KiB | d6069c83a35586cc676031eeb32fb7c2c0a04dbfcac5fe848e74d636e2f1fd6e | Library/Bee/artifacts/200b0aE.dag/UnityEditor.UI.Analytics.pdb<br>Library/ScriptAssemblies/UnityEditor.UI.Analytics.pdb |
| 20.61 KiB | 20.61 KiB | a063576514ef2818c369841f8210ba020e713980030c7d3994d5d02761c05545 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.GenerationContextMenu.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.GenerationContextMenu.pdb |
| 20.53 KiB | 20.53 KiB | e6349a670bc0fa2398238a8ee5eadd452d4d0e646a5be1d81fd324e08feaa49b | Library/Bee/artifacts/200b0aE.dag/Unity.Toolchain.Macos-x86_64-Linux-x86_64.pdb<br>Library/ScriptAssemblies/Unity.Toolchain.Macos-x86_64-Linux-x86_64.pdb |
| 20.21 KiB | 20.21 KiB | 459886002709513a2a4dcbfdd25d9854cc070c23cd62ee9c32ef153d9be0b524 | Library/Bee/artifacts/200b0aE.dag/Unity.Sysroot.Linux_x86_64.pdb<br>Library/ScriptAssemblies/Unity.Sysroot.Linux_x86_64.pdb |
| 19.86 KiB | 19.86 KiB | c505bbce0a795628a0583ecc4801059902bd7795ec1fac8536fd08dcf89c182e | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Pbr.Srp.pdb<br>Library/ScriptAssemblies/Unity.AI.Pbr.Srp.pdb |
| 19.69 KiB | 19.69 KiB | 3f51bdf86428f4d019855087ada377ecf885528aff87ef729f082c63467c38f6 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Generators.IO.Srp.pdb<br>Library/ScriptAssemblies/Unity.AI.Generators.IO.Srp.pdb |
| 19.61 KiB | 19.61 KiB | 5d369333aae965f1d67c54de49b9b4f697110e6a73293e3f4337ddac7546ff53 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Toolkit.Compliance.pdb<br>Library/ScriptAssemblies/Unity.AI.Toolkit.Compliance.pdb |
| 19.00 KiB | 19.00 KiB | 28403012bab5417c0ed977c4e087820ab5010ddffae571677aadb899bb576ff8 | Library/Bee/artifacts/200b0aE.dag/Unity.AI.Animate.Motion.Runtime.pdb<br>Library/ScriptAssemblies/Unity.AI.Animate.Motion.Runtime.pdb |


## Large objects in Git history

History is analysis-only. Any future rewrite would change commit hashes, require an explicit backup/coordination plan and likely require collaborators to clone again.

| Historical path | Size | Object |
|---|---|---|
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_FRONT_TEXTURE_STICKER_V1.png | 6.05 MiB | 3d375b2ee3cb299ecbde9ed9b2d4645c34434026 |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_SHELL_GRADIENT_V1.png | 4.27 MiB | 2e59d3d50226d92ba4d2939f587309bc8362bad8 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_TABLE_DARK_WOOD_SOURCE_V1.png | 3.72 MiB | f9a1ad52cccd27eb526c78d1aa0c7f644dd5585d |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_ORTHOGRAPHIC_TEXTURE_SHEET_V1.png | 3.25 MiB | e95a4c85474b9693e083776fdb76da07b645cd03 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 3.09 MiB | 760df594dae75d37093c3e0375827b29c9ffa10a |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.80 MiB | 3ff87c4bb12e3108567095e2847dd7a268ac8a76 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.80 MiB | e879a20b79fe22beaf5b24895eeb90d9b5f21b09 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | a1ae1d8ed7bcd7d4c23568da2e8167b1cb0260c2 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | 09b3cd85f96ee822572b56ecfc4186226611617e |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | 6b355df18c8e39fb85af4be17e534354e4b7fd66 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | 1461ebc49585ec9af30f8f1ca214d94c5b91b6d1 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | 7c420fa4bfd1271a341c15bac2aa13413e7eae1f |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.79 MiB | b67a0639c875ddac04ef113a3d3084e4cfa14b16 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.78 MiB | 2f45143c3ee9278560a5b33e97983b234458007d |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.77 MiB | 4d4a9ff999a3feffef3dda0a2411a7d3d030e7d9 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.76 MiB | 06305cbf5828ac9174e1bacf56704e87fb88188d |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.76 MiB | 9336a62fa161177dfa5a5ef8a0b3732ed65ef166 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.75 MiB | f8ee327fea72ad6a7f4c9c2d4e351e7f188694e4 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.75 MiB | 66fde61a24202cb45df2cb3542f7ce60bafb6232 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.74 MiB | 381b3725cefdb6f3f8420bb959ad17d1ee074a18 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.74 MiB | 69b5664947d4e98fe0c7395c1cf6d8efbedfffd0 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.74 MiB | f46870f3988dd96286a5e9626cc5e837066755f4 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.74 MiB | 78a791b7fbdfd31835533aed366aaba02a4760ae |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.74 MiB | 8a6180cfaad9db7d8e9d0b9f0a29909cfa19e81f |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.71 MiB | a245ed994735c68e711e7bef5934fbdeef0d1d29 |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_SHARP_V1.png | 2.63 MiB | 05641cdc9054b636ae9649be2b51e49135d344ff |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.60 MiB | 5a606962686df29399efdfd65326d07b91c280e9 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.59 MiB | edde5ed64e339e0a6979e68bcd2debb6f6d04632 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.58 MiB | ec362b58eaade9d992bc8bc509bdc22ebf985ab6 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.53 MiB | e95a4ad2dfe0e6246295e6d19b92aa6aa1c4b7da |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.46 MiB | be3e4c29d5f5e71420aa077f947e03019294a615 |
| Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity | 2.45 MiB | 665bdd717da5ecad3091ebe5181fe411e8428998 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_THREE_QUARTER_GIRL_V1.png | 2.29 MiB | 7840068b177225ed2b4a0870c0aa150651fa8750 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_MATERIAL_CLOSEUP_GIRL_V1.png | 2.15 MiB | 991d610eb4240805aeb52d058b28d0c71d268870 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_3D_FRONT_GIRL_V1.png | 2.14 MiB | b3571a232a98e5a592f20690c4eea5fc0e7cfe91 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_MENU_GIRL_V1.png | 2.08 MiB | 1e29ee5113cedf9fe1f6de06dcccbde9cbb65db4 |
| ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_MENU_BOY_V1.png | 2.02 MiB | dd4094cc2d31be087d9a73514e195d608531f82d |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_SETTINGS_V1.png | 1.11 MiB | 42dbf7706b970911e3545654a1109cd6899a6927 |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_CREDITS_V1.png | 1.10 MiB | b2051fdec48e3581c15364ac7dcd30433bee799b |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_PROGRESSION_V1.png | 1.06 MiB | 4c6b14860435b7f811a7804a43c656f924908092 |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_QUIT_V1.png | 1.02 MiB | 725ca3e693373f84f1c0018a749b76fac35df28e |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_RESUME_V1.png | 960.39 KiB | 7fe09aabe41dfbc15b95419b28a962d520120e81 |
| ProjectGuide/References/Visual/BOREDOM_AND_DUNGEONS_ART_DIRECTION_REFERENCE_BOARD_V1.jpg | 497.10 KiB | 9b6c4d2ef927a93dc3c1dad11eac99cf6b71f4b7 |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_HERO_GIRL_V1.png | 387.33 KiB | feb23340df4405d9ff09388a1abe7b38fe85565f |
| Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_HERO_BOY_V1.png | 345.91 KiB | 57809aa54968e898bfe3767abaf69e550a464399 |
| Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_BLUR_V1.png | 287.11 KiB | fa9cc2302a7478d6ff5f209d97cec2ea268ff5ba |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_X_V1.png | 189.74 KiB | 46c3f6a57efe513ba9ded9ed98c1c87cd07d5dd5 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_Y_V1.png | 184.67 KiB | 0bcf1b3e285360f6c1a0f2a5b1fc764e5df6680b |
| PROJECT_STATUS.md | 177.90 KiB | 5564a0d26152c2afe55889c8e32d3f79e0c960e2 |
| PROJECT_STATUS.md | 176.51 KiB | 3337a8c373a7d2f4890773fcadfc632f26b123b5 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_A_V1.png | 161.09 KiB | b29ae80bf1291b82971fa0459ba5fca49dae8f47 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_B_V1.png | 160.81 KiB | e27f46208abdd4150acf957a0365bc927ebb04ae |
| Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs | 157.64 KiB | c87edd241f554217a957694b8b9a1bd00276d18e |
| Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs | 143.74 KiB | 5b5914bfe02e2043497a453a6598a26662374094 |
| PROJECT_STATUS.md | 142.75 KiB | 83eec05a6b56e66d5580b8d07a582b125ebb7345 |
| PROJECT_STATUS.md | 140.27 KiB | 1ad5410131d82a7d8f9ac4f01d6f5fb2b99793b0 |
| ProjectGuide/Status/CURRENT.md | 134.62 KiB | 754d0c0ef79b7408d8747c09557985c663705a0a |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 134.14 KiB | 96e2de758020e1d2a5bb39ab76878a108fc589de |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 133.51 KiB | 14092fdcdf6d72c2b6bbb8d1592d169100713a49 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 133.51 KiB | 6f27c67adb580f00a3a9d48a3d86c0480bb865ee |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 133.09 KiB | 6c3f3f0bf82f382c3f69ef71a945d1ea41b3f668 |
| ProjectGuide/Status/CURRENT.md | 132.92 KiB | f9a1ad558ee131952abcf172483edd56262d9add |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 131.12 KiB | 5292f28c98db565e99cb689727df80530ea2da70 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_UP_V1.png | 130.47 KiB | fcf471a562978fe202e912e4dc034c9e02c0c674 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 129.99 KiB | 653fb3c5f252e6f1933704b7ff6acfcf6c0cb7bb |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 128.51 KiB | c9f0ef96526c8beb17c450cc53c10becf0d3c932 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 128.47 KiB | e3e4f41e0b33227fc662fb7fcd587edd2309f4f4 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 128.47 KiB | d62b95b1c845061a74cd90944b80d2f66004bf16 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 128.14 KiB | e9b150797cbdfef1a6d01f2e88dcbd30f7a134a9 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 126.99 KiB | e5838ae5a789112e8b16c3c845fee4e584c6ff7b |
| PROJECT_STATUS.md | 123.10 KiB | c9bb79502007f1b040aaca6aae00f90de26fb5c4 |
| PROJECT_STATUS.md | 122.38 KiB | e450715fd9d67163b51fe35e17b71e3852bd1824 |
| ProjectGuide/Status/CURRENT.md | 122.09 KiB | 3e9082139733ec33d481933a2271061ceba69abd |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_CENTER_V1.png | 121.01 KiB | 7f3652f9604fdea234f04d7d537fe2a4fe3d6b67 |
| PROJECT_STATUS.md | 118.83 KiB | 07a8f52265b0aa160cccdf4358c8aebfad4d2af4 |
| PROJECT_STATUS.md | 116.81 KiB | 8a8d76477d78d0d662dfded91df219dc8fddac83 |
| PROJECT_STATUS.md | 115.94 KiB | dc93a094c5a29a94bc418509e74f38e5408ef622 |
| PROJECT_STATUS.md | 115.90 KiB | 3db35d18dde54c66050de69e24ff70b8898b7113 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_LEFT_V1.png | 112.11 KiB | 501e1a01185e7e789509419e7224b426a1cdfefa |
| PROJECT_STATUS.md | 110.08 KiB | 24584fe3a22c96eb202f9ba475e737ea74ccee31 |
| PROJECT_STATUS.md | 108.90 KiB | d7e083c7cbdf2ea94148ff632f7f15448d9945c3 |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_RIGHT_V1.png | 107.15 KiB | c2329fd331bf96358758e5c9f99e1b444eb3a632 |
| PROJECT_STATUS.md | 98.98 KiB | fb0c42956371d081580bd5d9c8c8fd8d78f72a94 |
| PROJECT_STATUS.md | 90.93 KiB | 25b06c6d5f4af75c11ad2b56aea4e8dca1b6f581 |
| PROJECT_STATUS.md | 89.35 KiB | eb076cf417de3c26a4f2d441d8ea061058a1df67 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 87.53 KiB | bc58dc1dce6a2b079bdbd9b4cde26fee7201dbbf |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 86.12 KiB | a9e71429b1a5d96e8e6a81c73d76a9cc0b46ecdd |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 86.01 KiB | d5a8b9289e9badd748036d7089b8a0d5f5c7b57b |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 84.61 KiB | 7602f79e8bc1b5f405a37eb43b89a2d2c3074da1 |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 81.83 KiB | 5ad0672c40070dde4d2c8a0d470c903bf8e9ce73 |
| PROJECT_STATUS.md | 80.42 KiB | 0e965db0a1b17df4980b524ff25e75158eae39ca |
| Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_DOWN_V1.png | 79.66 KiB | 8ff9e1088021d48b9eaabc35e23faa5aa28f8d60 |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 78.78 KiB | 6034800c2ed1c2c5afd613abc048b8c3d22dc519 |
| PROJECT_STATUS.md | 78.26 KiB | 5c00ab471b548b695d92c0e112eb09c9ea93f4a8 |
| PROJECT_STATUS.md | 77.13 KiB | 1fb6ada6ca53ccdfba0341e0664a2f2e7dfddc12 |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 75.31 KiB | 68156346d8d15225328680b112f19c1579b863bc |
| Assets/_Project/Scripts/Runtime/BDHorseController.cs | 72.04 KiB | b2f2fed154659901bb9453d0d8daecd8408f3d61 |
| Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs | 71.00 KiB | 24d2c299618009f617882f20a832cd221ba06d74 |
| Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs | 69.80 KiB | 4070e53bb1ffe5848eba570c7542cda99a635999 |
| Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs | 66.61 KiB | 3db8a2cb526b5ece06dd6ff374b8f056c416ce89 |


- History was analyzed only. No rewrite, migration, pruning or commit-hash change was performed.

## Git LFS

### Currently reported by Git LFS

_None reported._

### Future candidates (report only)

_None._

- Candidates are report-only. Existing files and history were not migrated to LFS.

## Report-only local directories

The following names are intentionally not removed automatically because they may contain official release artifacts: `Build`, `Builds`, `releases`.

## Verification and remaining limits

- Working-tree measurement, duplicate hashing, generated-artifact cleanup, Git-history object inventory and LFS inventory were executed locally by the package tool.
- `git diff --check`, repository hygiene and source stability are run by the outer package installer.
- Unity compilation, reimport, Editor tests, PlayMode tests, supported builds, screenshot comparison, clean clone and clean-clone build still require the local Unity environment and must be recorded separately.
- No PNG was recompressed because pixel-perfect decoder comparison and Unity import verification were not available in this maintenance pass.
- No uncertain duplicate or tracked build/release file was removed.
