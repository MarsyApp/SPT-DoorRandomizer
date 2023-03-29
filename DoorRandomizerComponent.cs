﻿using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using System;
using UnityEngine;

namespace DrakiaXYZ_DoorRandomizer
{
    internal class DoorRandomizerComponent : MonoBehaviour
    {
        protected static ManualLogSource Logger { get; private set; }

        private DoorRandomizerComponent()
        {
            if (Logger == null)
            {
                Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(DoorRandomizerComponent));
            }
        }

        public void Awake()
        {
            int doorCount = 0;
            int changedCount = 0;
            int incompatibleDoors = 0;
            FindObjectsOfType<Door>().ExecuteForEach(door =>
            {
                doorCount++;

                if (door.DoorState == EDoorState.Open || door.DoorState == EDoorState.Shut)
                {
                    // Have a 50% chance to change the initial state of the door
                    if (UnityEngine.Random.Range(0, 100) < 50)
                    {
                        changedCount++;
                        door.DoorState = (door.InitialDoorState == EDoorState.Open ? EDoorState.Shut : EDoorState.Open);

                        // Trigger "OnEnable" to make sure the properties are set correctly for interaction
                        door.OnEnable();
                    }
                }
                else
                {
                    incompatibleDoors++;
                }
            });

            Logger.LogDebug($"Total Doors: {doorCount}");
            Logger.LogDebug($"Changed Doors: {changedCount}");
            Logger.LogDebug($"Incompatible Doors: {incompatibleDoors}");
        }

        public static void Enable()
        {
            if (Singleton<IBotGame>.Instantiated)
            {
                var gameWorld = Singleton<GameWorld>.Instance;
                gameWorld.GetOrAddComponent<DoorRandomizerComponent>();
            }
        }
    }
}
