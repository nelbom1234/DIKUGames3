using System.Collections.Generic;
using System.IO;
using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Breakout.Utilities;

namespace Breakout.LevelCreation {
    public class LevelLoader {
        
        public string levelName{get; private set;}
        public int levelTime{get; private set;}
        public EntityContainer<Block> blocks;
        public List<string> level{get; private set;}
        public List<string> metadata{get; private set;}
        public List<string> legend{get; private set;}
        private float XDivision;
        private float YDivision;
        private char Unbreakable;
        private char Hardened;
        private char PowerUp;
        public LevelLoader() {
            blocks = new EntityContainer<Block>();
            level = new List<string>();
            metadata = new List<string>();
            legend = new List<string>();
        }

        public void AddBlock(char identifier, Vec2F Position) {
            for (int i = 0; i < legend.Count; i++) {
                if (legend[i][0] == identifier) {
                    var imageName = legend[i].Substring(3, legend[i].Length-3);
                    var extension = imageName.IndexOf(".");
                    var damagedImage = imageName.Substring(0, extension) + "-damaged.png";
                    if (identifier == Hardened) {
                        blocks.AddEntity(new HardenedBlock(
                            new StationaryShape(Position, new Vec2F(XDivision, YDivisionXDivision, YDivision)),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets","Images",imageName))),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets", "Images", damagedImage))),
                            identifier));
                    }
                    else if (identifier == Unbreakable) {
                        blocks.AddEntity(new UnbreakableBlock(
                            new StationaryShape(Position, new Vec2F(XDivision, YDivision)),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets","Images",imageName))),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets", "Images", damagedImage))),
                            identifier));
                    }
                    else if (identifier == PowerUp) {
                        blocks.AddEntity(new PowerUpBlock(
                            new StationaryShape(Position, new Vec2F(XDivision, YDivision)),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets","Images",imageName))),
                            new Image(FileCollector.FilePath(
                                Path.Combine("..","Breakout","Assets", "Images", damagedImage))),
                            identifier));
                    }
                    else
                    blocks.AddEntity(new Block(
                        new StationaryShape(Position, new Vec2F(XDivision, YDivision)),
                        new Image(FileCollector.FilePath(
                            Path.Combine("..","Breakout","Assets","Images",imageName))),
                        new Image(FileCollector.FilePath(
                            Path.Combine("..","Breakout","Assets", "Images", damagedImage))),
                        identifier));
                }
            }
        }

        public void addMetaData() {
            for (int i = 0; i < metadata.Count; i++) {
                string line = metadata[i];
                string[] splitLine = line.Split(' ');
                switch(splitLine[0]) {
                    case "Name:":
                        //For loop for names that are more than 1 word
                        for (int j = 1; j < splitLine.Length; j++) {
                            levelName += splitLine[j] + " ";
                        }
                        break;
                    case "Time:":
                        levelTime = int.Parse(splitLine[1]);
                        break;
                    case "Hardened:":
                        Hardened = splitLine[1][0];

                        break;
                    case "PowerUp:":
                        PowerUp = splitLine[1][0];
                        break;
                    case "Unbreakable:":
                        Unbreakable = splitLine[1][0];
                        break;
                    
                }
            }
        }

        public void LoadLevel(string fileName) {
            //set to 0 so it can will show as 0 if there is no time metadata
            levelTime = 0;

            string[] fileString = File.ReadAllLines(fileName);
            
            level = new List<string>();

            metadata = new List<string>();

            legend = new List<string>();

            var count = 0;
            while (count < fileString.Length) {
                switch(fileString[count]) {
                    case "Map:":
                        count++;
                        while (fileString[count] != "Map/") {
                            level.Add(fileString[count]);
                            count++;
                        }
                        count++;
                        break;
                    case "Meta:": 
                        count++;
                        while(fileString[count] != "Meta/") {
                            metadata.Add((fileString[count]));
                            count++;
                        }
                        count++;
                        break;
                    case "Legend:":
                        count++;
                        while(fileString[count] != "Legend/") {
                            legend.Add(fileString[count]);
                            count++;
                        }
                        count++;
                        break;
                    default:
                        count++;
                        break;
                }
            }
            if (level.Count != 0) {
                addMetaData();
                YDivision = 1.0f/(float)level.Count;
                XDivision = 1.0f/(float)level[0].Length;
                for (int i = 0; i < level.Count; i++) {
                    for (int j = 0; j < level[i].Length; j++) {
                        if (level[i][j] != '-') {
                            AddBlock(level[i][j],new Vec2F(j*XDivision,1.0f-(i+1)*YDivision));
                        }
                    }
                }
            }
        }
    }
}