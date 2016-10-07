using System;
using System.Collections;
using System.Web.SessionState;
using System.Web;

namespace Core.Web
{
    /// <summary>
    /// 类名:SessionAdapter
    /// 功能描述:Session封装	
    /// 作 者: abaal
    /// 日 期: 2008-09-03
    /// </summary>
    public class SessionAdapter : MarshalByRefObject, IDictionary
    {
        private HttpSessionState _session;

        private static SessionAdapter _SessionAdapter;

        public static SessionAdapter Session
        {
            get
            {
                if (_SessionAdapter == null)
                {
                    _SessionAdapter= new SessionAdapter();
                }
                return _SessionAdapter;
            }
        }

      
        private SessionAdapter()
        {
            _session = HttpContext.Current.Session;
        }

      
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

     
        public ICollection Keys
        {
            get { return _session.Keys; }
        }

      
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

    
        public int Count
        {
            get { return _session.Count; }
        }

      
        public object SyncRoot
        {
            get { return _session.SyncRoot; }
        }

     
        public bool IsSynchronized
        {
            get { return _session.IsSynchronized; }
        }

   
        public IEnumerator GetEnumerator()
        {
            return _session.GetEnumerator();
        }

     
        public bool Contains(object key)
        {
            return _session[(String)key] != null;
        }

     
        public void Add(object key, object value)
        {
            _session.Add((String)key, value);
        }

     
        public void Clear()
        {
            _session.Clear();
        }

    
        public void Remove(object key)
        {
            _session.Remove((String)key);
        }

       
        public ICollection Values
        {
            get { throw new NotImplementedException(); }
        }

        
        public bool IsReadOnly
        {
            get { return _session.IsReadOnly; }
        }

      
        public bool IsFixedSize
        {
            get { return false; }
        }

      
        public object this[object key]
        {
            get { return _session[(String)key]; }
            set
            {
                _session[(String)key] = value;
            }
        }
    }
}
