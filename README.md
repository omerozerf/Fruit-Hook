# Fruit Hook  
*A performance-driven playable ad prototype*

---

### ▶ Playable Demo  
🔗 **Play now:** [https://omerozerf.itch.io/fruit-hook](https://omerozerf.itch.io/fruit-hook)

<p align="center">
  <img src="fruit-hook.gif" alt="Fruit Hook gameplay" width="360" />
</p>

---

## Overview
**Fruit Hook** is a short-loop action prototype built specifically for **playable ads**.  
The project is shaped around **WebGL and Luna constraints**, with a clear focus on runtime stability, predictable performance, and fast iteration.

---

## Project Goals

| Focus | Description |
|------|------------|
| Playable-first | Designed for ad networks and playable runtimes |
| Performance | Low CPU / GPU footprint |
| Iteration speed | ScriptableObject-driven tuning |
| Stability | Luna-compatible architecture |

---

## Branches

| Branch | Purpose |
|------|--------|
| `main` | Core gameplay, no ScratchCard integration |
| `scratch-card-entegration` | Gameplay with ScratchCard erase mechanics |

---

## Architecture & Configuration
- Nearly all gameplay and balance values are controlled via **ScriptableObjects**.
- Damage, speed, spawn rates, area sizes, and thresholds can be adjusted:
  - Without touching code  
  - From a single, centralized configuration
- This enables rapid tuning and easy creation of playable variants.

---

## Performance Strategy

> **Initial approach:**  
> Tilemap and MeshRenderer-based map systems were planned to improve rendering performance.  
>  
> **Problem:**  
> These systems could not be integrated reliably with **Luna**, leading to unstable behavior and performance drops.

### Final Approach

| Aspect | Solution |
|------|----------|
| Map generation | Manual tile spawning |
| Tilemap / MeshRenderer | Abandoned due to Luna issues |
| Performance bottleneck | Excessive render & update cost |
| Optimization | **Chunk-based culling** |
| Off-screen areas | Rendering & updates disabled |

This resulted in a **more stable and predictable runtime** for Luna + WebGL.

---

## Codebase Highlights

### Update Load Control
- Continuous `Update()` calls are minimized.
- Systems are:
  - State-driven  
  - Manually triggered  
  - Activated only when needed  
- This significantly reduces CPU load in WebGL environments.

---

### Modular System Design

| Module | Responsibility |
|------|---------------|
| Gameplay | Sword / hook / combat logic |
| World | Map & chunk management |
| Spawning | Enemy placement logic |
| Rendering | Visibility & culling |
| Scratch | Erase mechanics |

Each module is isolated, loosely coupled, and can be disabled independently.

---

### World & Map Management
- Tiles are generated at runtime.
- Tilemap is intentionally avoided.
- Chunk culling enables:
  - Visibility-based activation
  - Reduced render and physics overhead
- Acts as a deliberate workaround for Luna limitations.

---

### Scratch / Erase Mechanics (ScratchCard Branch)

| Feature | Implementation |
|------|----------------|
| Physics usage | Minimal |
| Erase shape | Vertical, sword-like |
| Reference | Sword transform |
| Goal | Stable, low-cost erase logic |

---

### Sword Orbit System
- Swords rotate around a shared orbit center.
- Orbit radius and sword parameters are decoupled.
- Visual changes do not affect damage or erase behavior.

---

### Enemy Spawning

| Rule | Behavior |
|----|----------|
| Player distance | Maximized |
| Play area bounds | Enforced |
| Enemy spacing | Overlap prevented |

Ensures clean first frames and avoids collision spikes.

---

## Luna Compatibility
- Unsupported Unity packages are avoided.
- No Timeline or Async GPU Readback usage.
- Heavy LINQ and unnecessary allocations are minimized.

**Goal:** predictable behavior across ad networks.

---

## Content Pipeline

| Asset | Tool |
|-----|------|
| Audio | Audacity |
| Sprite color edits | Picsart |

---

## Tech Stack
- Unity  
- C#  
- ScriptableObject architecture  
- Chunk-based culling  
- Luna / WebGL-friendly design  

---

## Final Notes
This project prioritizes **playable ad realities** over traditional Unity workflows.  
Visual complexity was intentionally sacrificed in favor of:

> **Stability · Performance · Control**

Several Unity conveniences were deliberately avoided to meet these goals.
