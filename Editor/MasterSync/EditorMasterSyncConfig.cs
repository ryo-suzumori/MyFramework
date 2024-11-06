using System.Collections.Generic;
using UnityEngine;

namespace MyFw
{
    public class EditorMasterSyncConfig : ScriptableObject
    {
        public string nameSpace;
        public string sheetId;
        public string outputDir;
        public string scriptDir;
        public List<string> sheetNameList = new();
    }
}
