
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Cache
{
    public class pActionCollection
    {
        public delegate List<pAction> DelegateGetActions();
        public delegate List<pAction> DelegateGetActionsByUser(Int32 UserID, Int32 MaxCacheLength);
        public delegate List<pAction> DelegateGetActionsInModel(Int32 KeyID, Int32 MaxCacheLength);

        public pActionCollection()
        {
            ID = 0;
            _dic = new Dictionary<string, List<pAction>>();
            _dt = new Dictionary<string, DateTime>();
        }
        public pActionCollection(Int32 KeyID)
        {
            ID = KeyID;
            _dic = new Dictionary<string, List<pAction>>();
            _dt = new Dictionary<string, DateTime>();
        }
        Int32 ID { get; set; }
        protected Dictionary<string, List<pAction>> _dic { get; set; }
        private Dictionary<String, DateTime> _dt { get; set; }

        public List<pAction> Get(string Key, DelegateGetActionsByUser F, Int32 MaxCacheLength = 0, Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F(ID, MaxCacheLength);
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            return _dic[Key] ?? new List<pAction>();
        }
        public List<pAction> Get(string Key, DelegateGetActionsInModel F, Int32 MaxCacheLength = 0, Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F(ID, MaxCacheLength);
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            return _dic[Key] ?? new List<pAction>();
        }
        public List<pAction> Get(string Key, DelegateGetActions F, Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F();
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            return _dic[Key] ?? new List<pAction>();
        }

        public void Insert(String Key, pAction Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pAction>();
                Value.Insert(0, Model);
                _dic[Key] = Value;
            }
        }
        public void Insert(String Key, IEnumerable<pAction> Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pAction>();
                Value.InsertRange(0, Model);
                _dic[Key] = Value;
            }
        }

        public void Remove(string Key)
        {
            if (_dic.ContainsKey(Key))
                _dic.Remove(Key);
        }
        public void Remove(String Key, pAction Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pAction>();
                Value.Remove(Model);
                _dic[Key] = Value;
            }
        }
        public void Remove(String Key, Func<pAction, Boolean> RangeFilter)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pAction>();
                var l = Value.Where(p => RangeFilter(p)).ToList();
                foreach (var m in l)
                    Value.Remove(m);
                _dic[Key] = Value;
            }
        }

    }
}