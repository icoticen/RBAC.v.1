using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Static
{
    public class TaskPipe : IDisposable
    {
        private string _key { get; set; }
        private Int32 _expriationseconds { get; set; }
        public TaskPipe(String Key, Int32 ExpriationSeconds = 3)
        {
            _key = $"STATIC/TASKPIPE/{Key}";
            _expriationseconds = ExpriationSeconds > 0 ? ExpriationSeconds : 63072000;
            IsIn = CheckTask();
            SetTask();
        }
        public Boolean IsIn { get; private set; }

        static private Object Default = new object();
        private void SetTask()
        {
            Cache.Cache.SetCache(_key, Default, DateTime.Now.AddSeconds(_expriationseconds));
        }
        private Boolean CheckTask()
        {
            var R = Cache.Cache.GetCache(_key);
            return R != null;
        }
        private void RemoveTask()
        {
            Cache.Cache.RemoveAllCache(_key);
        }
        public void Dispose()
        {
            RemoveTask();
        }
    }

}