﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;

namespace ModManager.ModList
{
    [Serializable]
    public class ModInfo: ListElement
    {
        //We can have multiple versions installed either a local and a steam
        //Or multiple local
        public List<ModVersionInfo> version = new List<ModVersionInfo>();
        public string author { get; private set; }
        public string id { get; private set; }
        
        

        public ModInfo(string name, string author, int order): base(order)
        {
            this._name = name;
            this.author = author;
        }

        public ModInfo(List<ModMetaData> data)
        {
            if (data.NullOrEmpty())
                return;

            ModMetaData modMeta = data.Find(m => m.Active);
            if (modMeta == null)
            {
                modMeta = data[0];
            }

            _name = modMeta.Name;
            author = modMeta.Author;
            id = modMeta.PackageIdNonUnique;

            LoadOrder = modMeta.LoadOrder();

            
            foreach (var ver in data)
            {
                version.Add(new ModVersionInfo(ver));
            }
            //version[0].active = true;
        }
    }

    public class ModVersionInfo
    {
        public ModMetaData ModMeta;

        public string version;
        public string path;
        public string targetGameVersion;

        public bool active
        {
            set
            {
                ModMeta.Active = value;
            }
            get
            {
                return ModMeta.Active;
            }
        }

        public ModVersionInfo(ModMetaData data)
        {
            ModMeta = data;
            version = "Read from manifest I guess";
            path = data.RootDir.FullName;
            targetGameVersion = data.TargetVersion;
        }
    }
}
