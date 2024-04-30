using System.Collections.Generic;
using UnityEngine;

namespace SStar
{
    public class EditorMasterSyncConfig : ScriptableObject
    {
        public string sheetId;
        public string outputDir;
        public List<string> sheetNameList = new();
    }
}
