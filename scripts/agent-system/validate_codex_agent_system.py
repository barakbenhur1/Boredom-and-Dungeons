#!/usr/bin/env python3
from pathlib import Path
import json
import re
import sys

ROOT = Path(__file__).resolve().parents[2]
AGENTS_DIR = ROOT / ".codex" / "agents"
CONFIG = ROOT / ".codex" / "config.toml"
REGISTRY = ROOT / "docs" / "agent-system" / "ROLE_REGISTRY.json"
REPOSITORY_RULES = ROOT / "docs" / "agent-system" / "REPOSITORY_RULES.md"
SKILLS_DIR = ROOT / ".agents" / "skills"
errors = []

def read_text(path):
    try:
        return path.read_text(encoding="utf-8")
    except Exception as exc:
        errors.append("Cannot read {}: {}".format(path, exc))
        return ""

def scalar(text, key):
    pattern = r'(?m)^\s*{}\s*=\s*(?:"([^"]*)"|([^\s#]+))\s*(?:#.*)?$'.format(re.escape(key))
    match = re.search(pattern, text)
    if not match:
        return None
    return match.group(1) if match.group(1) is not None else match.group(2)

for path in (ROOT / "AGENTS.md", CONFIG, REGISTRY, REPOSITORY_RULES, AGENTS_DIR, SKILLS_DIR):
    if not path.exists():
        errors.append("Missing required path: {}".format(path))

agents_contract = read_text(ROOT / "AGENTS.md")
if "docs/agent-system/REPOSITORY_RULES.md" not in agents_contract:
    errors.append("AGENTS.md must reference docs/agent-system/REPOSITORY_RULES.md")

config = read_text(CONFIG)
if scalar(config, "max_depth") != "1":
    errors.append("agents.max_depth must be 1")
try:
    if int(scalar(config, "max_threads")) > 3:
        errors.append("agents.max_threads exceeds 3")
except (TypeError, ValueError):
    errors.append("agents.max_threads is missing or invalid")

agent_files = sorted(AGENTS_DIR.glob("*.toml"))
seen = set()
for path in agent_files:
    text = read_text(path)
    name = scalar(text, "name")
    if not name:
        errors.append("{}: missing name".format(path.name))
    elif name in seen:
        errors.append("Duplicate agent name: {}".format(name))
    else:
        seen.add(name)
    for key in ("description", "model", "sandbox_mode"):
        if not scalar(text, key):
            errors.append("{}: missing {}".format(path.name, key))
    if not re.search(r'(?s)(?m)^\s*developer_instructions\s*=\s*""".*?"""\s*$', text):
        errors.append("{}: invalid developer_instructions".format(path.name))

try:
    registry = json.loads(read_text(REGISTRY))
    expected = {r["name"] for r in registry.get("roles", []) if r.get("name")}
    if expected - seen:
        errors.append("Missing agents: {}".format(sorted(expected - seen)))
    if seen - expected:
        errors.append("Unregistered agents: {}".format(sorted(seen - expected)))
except Exception as exc:
    errors.append("Invalid registry JSON: {}".format(exc))

skill_files = sorted(SKILLS_DIR.glob("*/SKILL.md"))
for path in skill_files:
    text = read_text(path)
    if not text.startswith("---\n") or "\n---\n" not in text[4:]:
        errors.append("{}: invalid front matter".format(path))
    elif not re.search(r"(?m)^name:\s*\S+", text) or not re.search(r"(?m)^description:\s*\S+", text):
        errors.append("{}: missing name or description".format(path))

if len(agent_files) != 30:
    errors.append("Expected 30 agents; found {}".format(len(agent_files)))
if len(skill_files) != 5:
    errors.append("Expected 5 skills; found {}".format(len(skill_files)))

if errors:
    print("FAIL")
    for error in errors:
        print("- {}".format(error))
    sys.exit(1)

print("PASS: 30 custom agents, 5 skills, config and registry valid")
