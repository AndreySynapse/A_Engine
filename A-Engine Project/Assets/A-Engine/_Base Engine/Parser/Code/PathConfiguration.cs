// A-Engine, Code version: 1

using UnityEngine;
using System.IO;

namespace AEngine.Parser
{
	public class PathConfiguration : MonoBehaviour
	{
        [SerializeField] private LocationKinds _location;
        [SerializeField] private OperationKinds _operation;
        [SerializeField] private string _directory;
        [SerializeField] private string _shortFileName;
        [SerializeField] private string _extension;

        #region Interface
        public string GetPath()
        {
            return FilePath.GetPath(_location, _operation, _directory, _shortFileName, _extension);
        }

        public bool Exists()
        {
            return File.Exists(FilePath.GetPath(_location, OperationKinds.Write, _directory, _shortFileName, _extension));
        }
        #endregion
	}
}