#!/usr/bin/env python3
from __future__ import annotations

import shutil
import sys
import tempfile
from datetime import datetime
from pathlib import Path


def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        if (candidate / "Assets/_Project/Scripts/Runtime").is_dir():
            return candidate

    fail("Run this script from the Boredom-and-Dungeons project root.")


def find_method_span(text: str, signature: str) -> tuple[int, int]:
    start = text.find(signature)
    if start < 0:
        fail(f"Method not found: {signature}")

    open_brace = text.find("{", start)
    if open_brace < 0:
        fail(f"Opening brace not found: {signature}")

    depth = 0
    for index in range(open_brace, len(text)):
        if text[index] == "{":
            depth += 1
        elif text[index] == "}":
            depth -= 1
            if depth == 0:
                return start, index + 1

    fail(f"Closing brace not found: {signature}")


def patch_square_jumper_runtime(path: Path) -> None:
    text = path.read_text(encoding="utf-8")
    marker = "// BD SQUARE JUMPER EDIT-MODE SAFETY FIX"

    if marker in text:
        print("SKIP: BDSquareJumperMiniBoss.cs already fixed.")
        return

    text = text.replace(
        "Material material = renderer.material;",
        "Material material = ResolveWritableMaterial(renderer);",
    )

    text = text.replace(
        "Material material =\n                    bladeRenderer.material;",
        "Material material =\n                    ResolveWritableMaterial(bladeRenderer);",
    )

    text = text.replace(
        "Destroy(bladeCollider);",
        "DestroyObjectSafely(bladeCollider);",
    )

    insert_marker = "        private void OnDrawGizmosSelected()"
    insert_at = text.find(insert_marker)

    if insert_at < 0:
        fail("OnDrawGizmosSelected was not found in Square Jumper.")

    helper = r'''        // BD SQUARE JUMPER EDIT-MODE SAFETY FIX
        private static Material ResolveWritableMaterial(
            Renderer renderer)
        {
            if (renderer == null)
                return null;

            if (Application.isPlaying)
                return renderer.material;

            Material source = renderer.sharedMaterial;
            Shader shader =
                source != null
                    ? source.shader
                    : Shader.Find(
                        "Universal Render Pipeline/Lit"
                    );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            Material editable =
                source != null
                    ? new Material(source)
                    : new Material(shader);

            editable.name =
                $"{renderer.gameObject.name}_EditModeMaterial";

            renderer.sharedMaterial = editable;
            return editable;
        }

        private static void DestroyObjectSafely(
            UnityEngine.Object target)
        {
            if (target == null)
                return;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(target);
            else
                UnityEngine.Object.DestroyImmediate(target);
        }

'''

    text = text[:insert_at] + helper + text[insert_at:]

    if "bladeRenderer.material" in text:
        fail("bladeRenderer.material remained after patch.")

    if "Destroy(bladeCollider)" in text:
        fail("Destroy(bladeCollider) remained after patch.")

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDSquareJumperMiniBoss.cs")


def patch_square_jumper_editor(path: Path) -> None:
    if not path.is_file():
        print("SKIP: BDSquareJumperSetupTools.cs not installed.")
        return

    text = path.read_text(encoding="utf-8")
    marker = "// BD SQUARE JUMPER MATERIAL-ASSET FIX"

    if marker in text:
        print("SKIP: BDSquareJumperSetupTools.cs already fixed.")
        return

    start, end = find_method_span(
        text,
        "        private static void ApplyMaterial("
    )

    replacement = r'''        // BD SQUARE JUMPER MATERIAL-ASSET FIX
        private const string GeneratedMaterialRoot =
            "Assets/_Project/Materials/Generated/SquareJumper";

        private static void ApplyMaterial(
            GameObject visual,
            Color color)
        {
            Renderer renderer =
                visual.GetComponent<Renderer>();

            if (renderer == null)
                return;

            renderer.sharedMaterial =
                GetOrCreateGeneratedMaterial(color);
        }

        private static Material GetOrCreateGeneratedMaterial(
            Color color)
        {
            EnsureGeneratedMaterialFolder();

            string colorKey =
                ColorUtility.ToHtmlStringRGBA(color);

            string path =
                $"{GeneratedMaterialRoot}/" +
                $"SquareJumper_{colorKey}.mat";

            Material material =
                AssetDatabase.LoadAssetAtPath<Material>(
                    path
                );

            if (material != null)
                return material;

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            material = new Material(shader)
            {
                name = $"SquareJumper_{colorKey}",
                color = color
            };

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(
                    "_EmissionColor",
                    color * 0.65f
                );
            }

            AssetDatabase.CreateAsset(material, path);
            AssetDatabase.SaveAssets();

            return material;
        }

        private static void EnsureGeneratedMaterialFolder()
        {
            EnsureFolder(
                "Assets/_Project/Materials"
            );

            EnsureFolder(
                "Assets/_Project/Materials/Generated"
            );

            EnsureFolder(GeneratedMaterialRoot);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            int slash = path.LastIndexOf('/');

            if (slash <= 0)
                return;

            string parent = path.Substring(0, slash);
            string name = path.Substring(slash + 1);

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }'''

    text = text[:start] + replacement + text[end:]

    if "renderer.material" in text:
        fail("renderer.material remained in the editor setup tool.")

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDSquareJumperSetupTools.cs")


def move_old_qa_menus(path: Path) -> None:
    if not path.is_file():
        print("SKIP: old stability gate is not installed.")
        return

    text = path.read_text(encoding="utf-8")
    old = '''        private const string MenuRoot =
            "Boredom And Dungeons/Validation/";
'''
    new = '''        private const string MenuRoot =
            "Boredom And Dungeons/Advanced QA/";
'''

    if old in text:
        text = text.replace(old, new, 1)
        path.write_text(text, encoding="utf-8")
        print("MOVED: old QA menus to Advanced QA.")
    else:
        print("SKIP: old QA menus already moved or changed.")


def main() -> None:
    root = find_project_root(Path.cwd())

    runtime_boss = (
        root /
        "Assets/_Project/Scripts/Runtime/Bosses/MiniBosses/"
        "SquareJumper/BDSquareJumperMiniBoss.cs"
    )

    editor_setup = (
        root /
        "Assets/_Project/Scripts/Editor/BossesMiniBosses/"
        "BDSquareJumperSetupTools.cs"
    )

    old_gate = (
        root /
        "Assets/_Project/Scripts/Editor/Validation/"
        "BDProjectStabilityGate.cs"
    )

    new_qa = (
        root /
        "Assets/_Project/Scripts/Editor/Validation/"
        "BDOneClickQAWindow.cs"
    )

    if not runtime_boss.is_file():
        fail(
            "BDSquareJumperMiniBoss.cs was not found. "
            "Install the Square Jumper package first."
        )

    if not new_qa.is_file():
        fail(
            "BDOneClickQAWindow.cs is missing. "
            "Extract the complete ZIP first."
        )

    backup = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_one_click_qa_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )

    backup.mkdir(parents=True, exist_ok=True)

    for path in [runtime_boss, editor_setup, old_gate]:
        if path.is_file():
            shutil.copy2(path, backup / path.name)

    patch_square_jumper_runtime(runtime_boss)
    patch_square_jumper_editor(editor_setup)
    move_old_qa_menus(old_gate)

    print()
    print("One-click QA and Square Jumper Edit Mode fixes applied.")
    print(f"Backup: {backup}")
    print("Next: return to Unity and wait for compilation.")
    print("Run only: Boredom And Dungeons -> TEST EVERYTHING")


if __name__ == "__main__":
    main()
