using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoomGen
{
    public class PresetEditorComponent : MonoBehaviour
    {

        public RoomPreset preset;

        [HideInInspector]
        public List<Floor> floors = new List<Floor>();
        [HideInInspector]
        public List<Wall> walls = new List<Wall>();
        [HideInInspector]
        public List<Door> doors = new List<Door>();
        [HideInInspector]
        public List<Window> windows = new List<Window>();

        [HideInInspector]
        public List<Decoration> characters = new List<Decoration>();
        [HideInInspector]
        public List<Decoration> wallDecorations = new List<Decoration>();
        [HideInInspector]
        public List<Decoration> floorDecorations = new List<Decoration>(); 


        #region inspectorValidations;


        public void UpdateStoredValues()
        {
            if (preset == null)
                return;

            walls = preset.wallTiles;
            doors = preset.doorTiles;
            floors = preset.floorTiles;
            windows = preset.windowTiles;
            characters = preset.characters;
            wallDecorations = preset.wallDecorations;
            floorDecorations = preset.floorDecorations;

            // save them to the preset.
            UpdatePreset();
        }

        public void UpdatePreset()
        {
            preset.wallTiles = walls;
            preset.doorTiles = doors;
            preset.floorTiles = floors;
            preset.windowTiles = windows;
            preset.characters = characters;
            preset.wallDecorations = wallDecorations;
            preset.floorDecorations = floorDecorations;
        }

        #endregion


    }
}
