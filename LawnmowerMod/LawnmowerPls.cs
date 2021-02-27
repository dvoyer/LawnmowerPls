using System;
using System.Text.RegularExpressions;
using System.Linq;
using GlobalEnums;
using Modding;
using UnityEngine;

namespace LawnmowerPls
{
    public class SaveModSettings : ModSettings
    {
        public bool validRun;
        public bool[] wasGrassCut;
    }

    public class LawnmowerPls : Mod
    {
        public int totalGrassCut = 0;
        static public int totalGrass = 3259;
        static public int maxGrassInRoom = 500; // there's SO much grass in greenpath
        static public int maxColliders = 15;
        static public string pattern = "[gG]rass";
        public Regex rg = new Regex(pattern);
        public Collider2D[,] GrassList = new Collider2D[maxGrassInRoom, maxColliders];
        public string[] grassRoomList = new string[maxGrassInRoom];
        public int grassCount;
        public string lastGrassRoom;
        Collider2D[] results = new Collider2D[maxGrassInRoom];

        public override string GetVersion() => "1.5";

        bool validateRun()
        {
            totalGrassCut = Settings.wasGrassCut.Count(c => c);
            Settings.validRun = (totalGrassCut == totalGrass);
            return Settings.validRun;
        }

        public override void Initialize()
        {
            Log("LawnmowerPls v." + GetVersion());
            lookupTables.generateHashtable();
            lookupTables.generateDictionary();
            Tracker.loadResources();
            Tracker.makeCanvas();
            Tracker.makeMenu();
            Tracker.showCanvas(false);
            On.GrassCut.ShouldCut += OnShouldCut;
            ModHooks.Instance.AfterAttackHook += UpdateCut;
            ModHooks.Instance.BeforeSceneLoadHook += SceneLoaded;
            ModHooks.Instance.NewGameHook += NewGameStarted;
        }
        
        private void updateGrass()
        {
            if (grassCount != 0)
            {
                for (int i = 0; i < grassCount; i++)
                {
                    for (int j = 0; j < maxColliders; j++)
                    {
                        try
                        {
                            Collider2D c = GrassList[i, j];
                            if (rg.IsMatch(c.name))
                            {
                                string hstring = checkInTable(c, grassRoomList[i]);
                                if (lookupTables.grassIndices.ContainsKey(hstring))
                                {
                                    if (!Settings.wasGrassCut[(int)lookupTables.grassIndices[hstring]])
                                    {
                                        //Log(hstring + " (" + (int)grassIndices[hstring] + ") cut");
                                        Settings.wasGrassCut[(int)lookupTables.grassIndices[hstring]] = true;
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                    }
                }
                checkRoom(lastGrassRoom);
                grassCount = 0;
                validateRun();
                updateText(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }

        private void NewGameStarted()
        {
            Tracker.showCanvas(true);
            lastGrassRoom = "Tutorial_01";
            Settings.validRun = false;
            Settings.wasGrassCut = new bool[totalGrass];
        }

        private string checkInTable(Collider2D c, string lastGrassRoom)
        {
            float rawX = c.transform.GetPositionX();
            float rawY = c.transform.GetPositionY();
            float rawZ = c.transform.GetPositionZ();
            string roundX = Math.Round(rawX, 1).ToString("N1");
            string roundY = Math.Round(rawY, 1).ToString("N1");
            string roundZ = Math.Round(rawZ, 1).ToString("N1");
            string hstring = lastGrassRoom + " (" + roundX + ", " + roundY + ", " + roundZ + ")";
            if (!lookupTables.grassIndices.ContainsKey(hstring))
            {
                string trunX = (Math.Truncate(rawX * 10) / 10).ToString("N1");
                hstring = lastGrassRoom + " (" + trunX + ", " + roundY + ", " + roundZ + ")";
                if (!lookupTables.grassIndices.ContainsKey(hstring))
                {
                    string trunY = (Math.Truncate(rawY * 10) / 10).ToString("N1");
                    hstring = lastGrassRoom + " (" + roundX + ", " + trunY + ", " + roundZ + ")";
                    if (!lookupTables.grassIndices.ContainsKey(hstring))
                    {
                        string trunZ = (Math.Truncate(rawZ * 10) / 10).ToString("N1");
                        hstring = lastGrassRoom + " (" + roundX + ", " + roundY + ", " + trunZ + ")";
                        if (!lookupTables.grassIndices.ContainsKey(hstring))
                        {
                            hstring = lastGrassRoom + " (" + roundX + ", " + trunY + ", " + trunZ + ")";
                            if (!lookupTables.grassIndices.ContainsKey(hstring))
                            {
                                hstring = lastGrassRoom + " (" + trunX + ", " + roundY + ", " + trunZ + ")";
                                if (!lookupTables.grassIndices.ContainsKey(hstring))
                                {
                                    hstring = lastGrassRoom + " (" + trunX + ", " + trunY + ", " + roundZ + ")";
                                    if (!lookupTables.grassIndices.ContainsKey(hstring))
                                    {
                                        hstring = lastGrassRoom + " (" + trunX + ", " + trunY + ", " + trunZ + ")";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return hstring;
        }

        private void UpdateCut(AttackDirection dir)
        {
            updateGrass();
        }

        private string SceneLoaded(string targetScene)
        {
            if(lastGrassRoom != null)
            {
                updateGrass();
                checkRoom(lastGrassRoom, true);
            }
            Tracker.showCanvas(true);
            validateRun();
            updateText(targetScene);
            lastGrassRoom = targetScene;
            return targetScene;
        }

        public int checkRoom(string roomID, bool logging = false)
        {
            if (lookupTables.roomLookup.ContainsKey(roomID))
            {
                int[] dat = lookupTables.roomLookup[roomID];
                int numGrass = dat[1];
                int baseGrassIndex = dat[0];
                int toCut = 0;
                if(logging)
                {
                    Log("total to cut: " + numGrass + " in " + roomID + " starting: " + baseGrassIndex);
                    Log("----------------------------");
                    Log(lastGrassRoom);
                    Log("total to cut: " + dat[1] + " starting at " + dat[0]);
                }

                for (int i = 0; i < numGrass; i++)
                {
                    int currIndex = baseGrassIndex + i;
                    if (!Settings.wasGrassCut[baseGrassIndex + i])
                    {
                        toCut++;
                        if(logging)
                        {
                            Log("not cut ID " + currIndex);
                        }
                    }
                    else
                    {
                        if(logging)
                        {
                            Log("cut ID " + currIndex);
                        }
                    }
                }
                if(logging)
                {
                    Log(toCut + " grass remaining to cut in " + roomID);
                    Log("----------------------------\n");
                }
                return toCut;
            }
            else
            {
                if(logging)
                {
                    Log(lastGrassRoom + " had no grass.");
                }
                return -1;
            }
        }

        public SaveModSettings Settings = new SaveModSettings();

        public override ModSettings SaveSettings
        {
            get => Settings;
            set => Settings = (SaveModSettings)value;
        }

        public bool OnShouldCut(On.GrassCut.orig_ShouldCut orig, Collider2D collision)
        {
            bool shouldCut = orig(collision);

            if (!shouldCut)
            {
                return false;
            }
            //Log("snip snip snip " + grassCount);
            int v = collision.OverlapCollider(new ContactFilter2D().NoFilter(), results);
            for (int i = 0; i < maxColliders; i++)
            {
                GrassList[grassCount, i] = results[i];
                grassRoomList[i] = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            }
            grassCount++;
            return true;
        }

        public bool[] getRoomData(string roomID)
        {
            if (!lookupTables.roomLookup.ContainsKey(roomID))
            {
                return new bool[] { };
            }
            else
            {
                bool[] roomData;
                int[] dat = lookupTables.roomLookup[roomID];
                int numGrass = dat[1];
                int baseGrassIndex = dat[0];
                roomData = Settings.wasGrassCut.Skip(baseGrassIndex).Take(numGrass).ToArray();
                return roomData;
            }
        }

        public void updateText(string roomID)
        {
            string newInfo = "";
            if (lookupTables.roomLookup.ContainsKey(roomID))
            {
                int numGrass = lookupTables.roomLookup[roomID][1];
                int cutGrass = getRoomData(roomID).Count(c => c);
                newInfo += cutGrass + "/" + numGrass + " — ";
            }
            else
            {
                newInfo += "";
            }
            newInfo += totalGrassCut + "/" + totalGrass;
            Tracker.updateText(newInfo);
        }

    }
}
