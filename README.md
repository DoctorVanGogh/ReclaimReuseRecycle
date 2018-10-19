# Reclaim, Reuse, Recycle

[![RimWorld 1.0](https://img.shields.io/badge/RimWorld-1.0-green.svg?style=popout-square)](http://rimworldgame.com/) 
[![License: CC BY-NC-SA 4.0](https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg?style=popout-square)](https://creativecommons.org/licenses/by-nc-sa/4.0/) ![Github Total Downloads](https://img.shields.io/github/downloads-pre/doctorvangogh/ReclaimReuseRecycle/total.svg?style=popout-square)  [![GitHub Latest Release Version](https://img.shields.io/github/release/doctorvangogh/ReclaimReuseRecycle.svg?style=popout-square)](../../releases/latest) [![GitHub Latest Pre-Release Version](https://img.shields.io/github/release/doctorvangogh/ReclaimReuseRecycle/all.svg?style=popout-square)](../../releases)

![Logo](https://raw.githubusercontent.com/DoctorVanGogh/ReclaimReuseRecycle/master/Textures/UI/Recycle.png)

Rimworld mod that allows extracting added parts & implants from corpses.

This mods basically adds two types of jobs/recipes:

- A set of "Harvest corpse" jobs at the Butcher & Machining Tables which will try to extract all recoverable artificial body parts.
- A set of "refurbish" type jobs on the Drug Lab which will transform extracted parts into fully reusable body parts again.

Also included are research projects (un)locking the harvesting & refurbishment type jobs.

## Basic concepts

### Extracted part status

Reclaimed parts (which can be any added part or implant that _could_ be removed from a corresponding live pawn) are either extracted in a _non-sterile_ or _mangled variant_. Undamaged (or slightly damaged) parts are extracted as _non-sterile_, while moderately damaged parts get extracted as _mangled_ versions. The thresholds for non-sterile/mangled/no extraction can be changed in the mod options. (Defaults are 100%-85%: non-sterile, 85%-50%: mangled, otherwise: non reclaimable).
Non-sterile parts only need the part and medicine to be usable again, while mangled parts need additional material(s) for restoration.

### Extracted part 'Complexity'

Parts are categorized by Complexity (a visible stat on the reclaimed part). Available levels are

- Primitive
- Advanced
- Glittertech

This complexity is derived from the extracted part by internal tags, techlevel or ultimately price and should account for all mod-added parts.
Each complexity level corresponds to the medicine (and possibly other materials) needed to make the part usable again. _Primitive_ needs Herbal Medicine (plus wood/cloth), _Advanced_ uses regular Medicine (plus steel/components) and _Glittertech_ requires Glitterworld Medicine (plus components & plasteel).

### Harvesting Jobs

Extraction is a composite job that will always try to remove _all_ possible parts in a corpse.
The extraction _can fail_ and that failure is calculated _per part_. Failures work similar to surgery and are in fact calculated from the _surgery/mechanoid extraction success chance_ of the executing pawn. Success chance is multiplied by 1.5 compared to regular surgeries to accomodate for the composite nature of the harvesting job as well as the lack of 'supporting' medicine (Room stats still apply for biological harvests).
A minor failure will typically still allow extraction of a (mostly) undamaged part, while catastrophic failure will almost certainly destroy the part.

## Changes to vanilla behavior

This mods adds two special ThingFilters to the animal, humanlike & mechanoid corpse categories: _Harvested_ & _Unharvested_ corpses.
_Harvested_ corpses contain no reclaimable parts, while _Unharvested_ corpses ultimately could have (some) parts removed.
The vanilla _Butcher corpse_ job get's changed to only use _Harvested_ corpses by default. If you want to make it use any corpse, make sure to activate
_Unharvested_ corpses too (but why would you?).

## Compatibility

Mod load order should not matter for this mod. It also should support _any and all_ additional parts from 3rd party mods.
While the complexity might not be 100% as expected for 'weird' extra parts it will always guess _something_ and thus allow reclamation & reuse of the part(s).

## Bugs & issues

Please report any errors or issues on the [github issue tracker](https://github.com/DoctorVanGogh/ReclaimReuseRecycle/issues).

---

Polished textures by *Syrchalis*

## Powered by ![Harmony](https://github.com/pardeike/Harmony)

<p align="center">
<img alt="Powered by Harmony" src="https://camo.githubusercontent.com/074bf079275fa90809f51b74e9dd0deccc70328f/68747470733a2f2f7332342e706f7374696d672e6f72672f3538626c31727a33392f6c6f676f2e706e67" />
</p>

Harmony is lisenced under a [MIT license](https://raw.githubusercontent.com/pardeike/Harmony/master/LICENSE).
