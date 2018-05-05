using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Static
{
    public class zTree
    {
        public class Data
        {
            public LayUI.Button onDoubleClick { get; set; }
            public List<zTree.ITreeNodeBase> TreeNodes { get; set; }
        }
        public class TreeNodeBase : ITreeNodeBase
        {
            public TreeNodeBase()
            {
                this.children = new List<TreeNodeBase>().Select(p => (ITreeNodeBase)p).ToList();
            }

            public int id { get; set; }
            public int pId { get; set; }
            public string name { get; set; }

            public string color { get; set; }

            public bool open { get; set; } = true;
            public bool @checked { get; set; } = false;
            public bool chkDisabled { get; set; } = false;

            public string icon { get; set; }
            public string iconOpen { get; set; }
            public string iconClose { get; set; }

            public Dictionary<string, string> data { get; set; }
            public List<ITreeNodeBase> children { get; set; }
        }
        public interface ITreeNodeBase
        {
            int id { get; set; }
            bool open { get; set; }
            string name { get; set; }
            int pId { get; set; }
            bool @checked { get; set; }
            bool chkDisabled { get; set; }

            Dictionary<string, string> data { get; set; }

            List<ITreeNodeBase> children { get; set; }
        }

        public enum E_Node_Color
        {
            DISABLE = 0x4c4f5645,

        }
    }
}