using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public interface ISingleton
    {
        void SelfDeleteInstance();
    }

    public class Singleton<T> : ISingleton where T : class, new()
    {
#nullable disable
        protected static T instance_;
#nullable restore

        public static T Instance
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static bool IsValid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static void CreateInstance()
        {
            throw new NotImplementedException();
        }

        public static void DeleteInstance()
        {
            throw new NotImplementedException();
        }

        public void SelfDeleteInstance()
        {
            throw new NotImplementedException();
        }

        protected virtual void Startup()
        {
            throw new NotImplementedException();
        }

        protected virtual void Shutdown()
        {
            throw new NotImplementedException();
        }

        public Singleton()
        {
            throw new NotImplementedException();
        }
    }

}
