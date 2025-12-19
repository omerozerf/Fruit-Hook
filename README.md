# Fruit Hook

A short-loop, performance-focused action prototype developed specifically for the playable ads format. The project was designed around WebGL / Luna constraints, prioritizing stability and fast iteration.

---

## ▶️ Playable Demo
🔗 **Play:** [https://omerozerf.itch.io/fruit-hook](https://omerozerf.itch.io/fruit-hook)

<p align="center">
  <img src="fruit-hook.gif" alt="Fruit Hook Gameplay" width="360"/>
</p>

---

## 🎯 Project Goals

| Goal | Description |
|------|------------|
| Playable compatibility | Architecture suitable for ad formats |
| Performance | Low CPU / GPU usage |
| Fast iteration | ScriptableObject-based tuning |
| Stability | Compliance with Luna constraints |

---

## 🌿 Branch Structure

| Branch | Description |
|------|-------------|
| `main` | Base gameplay version without ScratchCard integration |
| `scratch-card-entegration` | Version with ScratchCard mechanics integrated |

---

## ⚙️ Architecture & Configuration
- Almost all gameplay and balance parameters are managed via **ScriptableObject**.
- Damage, speed, spawn rates, area sizes, and similar values can be:
  - Adjusted without code changes
  - Controlled from a single source
- This enables fast tuning and easy playable variant creation.

---

## 🚀 Performance & Optimization

> **Note:**  
> To improve map performance, **Tilemap** and **Mesh Renderer–based** solutions were initially targeted.  
> However, these systems could not be integrated stably with **Luna** and caused performance degradation.

Because of this, the approach was changed:

| Topic | Applied Approach |
|------|------------------|
| Map system | Manual tile generation |
| Tilemap / MeshRenderer | Not used due to Luna integration issues |
| Performance issue | Excessive render and update cost |
| Solution | **Chunk-based culling** |
| Off-camera areas | Render + update disabled |

This approach resulted in **more predictable and stable performance** in Luna + WebGL environments.

---

## 🧠 Codebase – Key Technical Decisions

### Update Load Management
- Constantly running `Update()` calls were minimized.
- Most systems operate:
  - State-driven
  - Manually triggered
  - Activated only when necessary
- This significantly reduces CPU load in WebGL builds.

---

### Modular System Separation

| System | Responsibility |
|------|----------------|
| Gameplay | Sword / hook / combat |
| Map | Area and chunk management |
| Spawn | Enemy placement |
| Rendering | Visibility and culling |
| Scratch | Erase logic |

Systems are isolated, loosely coupled, and can be disabled independently.

---

### Map & Area Management
- Tiles are generated at runtime.
- Instead of Tilemap, manual generation + chunk culling is used.
- Chunks are enabled/disabled based on camera visibility.
- This acts as a deliberate workaround for Luna Tilemap limitations.

---

### Scratch / Erase Logic (ScratchCard Version)

| Feature | Description |
|------|-------------|
| Physics usage | Minimal |
| Erase area | Vertical, sword-shaped |
| Reference | Sword transform |
| Goal | Stable and performant erase behavior |

---

### Sword Orbit System
- Swords rotate around a central orbit.
- Orbit radius and internal sword parameters are decoupled.
- Visual changes do not affect damage or erase areas.

---

### Enemy Spawn Logic

| Criterion | Implementation |
|---------|----------------|
| Distance from player | Maximized |
| Play area bounds | Enforced |
| Enemy-to-enemy spacing | Overlap prevention |

---

### Luna Compatibility Measures
- Unsupported Unity packages were avoided.
- No Timeline or Async GPU Readback usage.
- Heavy LINQ and unnecessary allocations were minimized.

---

## 🎨 Content Production

| Content | Tool |
|-------|------|
| Audio editing | Audacity |
| Sprite color editing | Picsart |

---

## 🛠️ Technologies
- Unity
- C#
- ScriptableObject
- Chunk-based culling
- Luna / WebGL-compatible architecture

---

## 📌 Final Notes
This project was shaped around **playable ad realities** rather than traditional Unity game development practices.  
The primary goal was not visual complexity, but a **stable, fast, and controllable playable experience**.

As a result, several Unity conveniences were deliberately avoided.
