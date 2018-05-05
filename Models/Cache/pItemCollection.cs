using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Cache
{
    public class pItemCollection
    {
        public delegate List<pItem> DelegateGetItems();
        public delegate List<pItem> DelegateGetItemsByUser(Int32 UserID,Int32 MaxCacheLength=0);
        public delegate List<pItem> DelegateGetItemsInModel(Int32 KeyID, Int32 MaxCacheLength=0);

        public pItemCollection()
        {
            ID = 0;
            _dic = new Dictionary<string, List<pItem>>();
            _dt = new Dictionary<string, DateTime>();
        }
        public pItemCollection(Int32 KeyID)
        {
            ID = KeyID;
            _dic = new Dictionary<string, List<pItem>>();
            _dt = new Dictionary<string, DateTime>();
        }
        Int32 ID { get; set; }
        protected Dictionary<string, List<pItem>> _dic { get; set; }
        private Dictionary<String, DateTime> _dt { get; set; }


        public List<pItem> Get(string Key, DelegateGetItemsByUser F, Int32 MaxCacheLength = 0, Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F(ID, MaxCacheLength);
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            return _dic[Key] ?? new List<pItem>();
        }
        public List<pItem> Get(string Key, DelegateGetItemsInModel F, Int32 MaxCacheLength = 0, Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F(ID, MaxCacheLength);
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            return _dic[Key] ?? new List<pItem>();
        }
        public List<pItem> Get(string Key, DelegateGetItems F,  Int32 Seconds = 600)
        {
            if (!_dic.Keys.Contains(Key) || !_dt.Keys.Contains(Key) || _dt[Key].AddSeconds(Seconds) < DateTime.Now)
            {
                var Value = F();
                _dic[Key] = Value;
                _dt[Key] = DateTime.Now;
            }
            var dt = _dt[Key];
            return _dic[Key] ?? new List<pItem>();
        }

        public void Insert(String Key, pItem Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pItem>();
                Value.Insert(0, Model);
                _dic[Key] = Value;
            }
        }
        public void Insert(String Key, IEnumerable<pItem> Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pItem>();
                Value.InsertRange(0, Model);
                _dic[Key] = Value;
            }
        }

        public void Remove(string Key)
        {
            if (_dic.ContainsKey(Key))
                _dic.Remove(Key);
        }
        public void Remove(String Key, pItem Model)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pItem>();
                Value.Remove(Model);
                _dic[Key] = Value;
            }
        }
        public void Remove(String Key, Func<pItem, Boolean> RangeFilter)
        {
            if (_dic.Keys.Contains(Key))
            {
                var Value = _dic[Key] ?? new List<pItem>();
                var l = Value.Where(p => RangeFilter(p)).ToList();
                foreach (var m in l)
                    Value.Remove(m);
                _dic[Key] = Value;
            }
        }
    }
}