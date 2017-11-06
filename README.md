# MSC-RecorderFlute
Recorder Flute mod for My Summer Car

This mod adds recorder flute -instrument to My Summer Car as playable instrument.

To compile this project you need to add following .dll -references which can be found from mysummercar_Data/Managed -folder:
- Assembly-CSharp.dll
- PlayMaker.dll
- MSCLoader.dll (comes with MSC Mod Loader)
- cInput.dll

To load this mod in-game, you need MSC Mod Loader (made by piotrulos), which can be found here: https://github.com/piotrulos/MSCModLoader/. Put compiled .dll file and 'Assets' -folder to your MSC Mod Loader Mods -folder. 

This mod uses GameHook.cs and SaveUtil.cs made by zamp (released under MIT-license) to implement saving.
Link to zamp's github: https://github.com/zamp/MSC-Mods

The "g5.wav" sample is made by MTG and released under Attribution 3.0 Unported (CC BY 3.0) -license. Link to original unmodified sample: https://freesound.org/people/MTG/sounds/361080/
