# Request Routing

Use this only when the owner is not obvious from `AGENTS.md`.

| Request | Primary owner | Typical support |
|---|---|---|
| Vision, tone, identity | `creative_director` | game_director, art_director, narrative_director |
| Cross-system game experience | `game_director` | creative_director, lead_game_designer, technical_director |
| Roadmap, scope, task status | `producer` | game_director, technical_director, qa_lead |
| Gameplay rule or new mechanic | `lead_game_designer` | relevant designer, technical_director |
| Movement, attack, game feel | `combat_gamefeel_designer` | gameplay_combat_programmer, animation_rigging_lead |
| Enemy or boss | `enemy_boss_designer` | ai_procedural_programmer, animation_rigging_lead, qa_lead |
| Run structure or progression | `roguelite_systems_designer` | economy_balance_designer, ai_procedural_programmer |
| Level, room, encounter | `level_encounter_designer` | enemy_boss_designer, environment_art_lead |
| Economy or numerical balance | `economy_balance_designer` | data_playtest_lead, relevant programmer |
| HUD, menu, onboarding, accessibility | `ux_ui_accessibility_designer` | ui_tools_save_programmer, art_director |
| Story, dialogue, lore | `narrative_director` | cinematic_director, audio_director |
| Cinematic scene or shot plan | `cinematic_director` | camera_cinematics_programmer, animation_rigging_lead |
| Visual style or approval | `art_director` | relevant art lead |
| Environment, prop, texture, material | `environment_art_lead` | technical_art_vfx_lighting_lead |
| Character or creature model | `character_creature_art_lead` | animation_rigging_lead |
| VFX, shader, lighting | `technical_art_vfx_lighting_lead` | rendering_performance_engineer |
| Rig or animation | `animation_rigging_lead` | relevant designer and runtime programmer |
| Music, SFX, voice | `audio_director` | relevant runtime programmer |
| Architecture or refactor | `technical_director` | relevant programmer, qa_lead |
| Player/combat implementation | `gameplay_combat_programmer` | combat_gamefeel_designer |
| Enemy AI or procedural code | `ai_procedural_programmer` | enemy_boss_designer or roguelite_systems_designer |
| UI/save/tools/data code | `ui_tools_save_programmer` | ux_ui_accessibility_designer, technical_director |
| Camera/Timeline code | `camera_cinematics_programmer` | cinematic_director |
| FPS, memory, loading, rendering | `rendering_performance_engineer` | technical_art_vfx_lighting_lead |
| Build, CI/CD, platform | `build_release_platform_engineer` | technical_director, qa_lead |
| Bug verification or release gate | `qa_lead` | domain owner, relevant programmer |
| Telemetry or playtest | `data_playtest_lead` | relevant designer |
| Localization, RTL, certification, legal | `localization_certification_lead` | ux_ui_accessibility_designer, qa_lead |
| Store, trailer, community | `marketing_store_community_lead` | creative_director, game_director |
| Post-launch event/support | `liveops_support_lead` | producer, qa_lead, build_release_platform_engineer |

## Mixed request rule

Choose one primary owner for the user-visible outcome. Assign supporting agents only a concrete artifact or decision. The root manager integrates the result.

## Misroute rule

A specialist does not solve adjacent domains by guessing. It returns `ROUTE`, `REASON`, and a compact `PACKET` to the root manager.
