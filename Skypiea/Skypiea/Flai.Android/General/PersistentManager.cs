using System.IO;
using System.IO.IsolatedStorage;

namespace Flai.General
{
    public enum LoadOption
    {
        None,
        Automatic,
    }

    public abstract class PersistentManager
    {
        protected readonly string _filePath;
        protected PersistentManager(string filePath)
            : this(filePath, LoadOption.None)
        {
        }

        protected PersistentManager(string filePath, LoadOption loadOption)
        {
            Ensure.IsValidPath(filePath);
            _filePath = filePath;

            if (loadOption == LoadOption.Automatic)
            {
                this.Load();
            }
        }

        public void Save()
        {
            if (!this.NeedsSaving())
            {
                return;
            }

#if WINDOWS_PHONE
            try
            {
                using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = isolatedStorage.OpenFile(_filePath, FileMode.Create, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            this.WriteInner(writer);
                        }
                    }
                }
            }
            catch { }
#else
            throw new NotImplementedException("");
#endif
        }

        protected bool Load()
        {
            bool success = false;
            try
            {
#if WINDOWS_PHONE
                using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // could be not necessary
                    if (isolatedStorage.FileExists(_filePath))
                    {
                        using (IsolatedStorageFileStream stream = isolatedStorage.OpenFile(_filePath, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                this.ReadInner(reader);
                                success = true;
                            }
                        }
                    }
                }
#else
                if (File.Exists(_filePath))
                {
                    using (var stream = File.OpenRead(_filePath))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            this.ReadInner(reader);
                            success = true;
                        }
                    }
                }
#endif
            }
            catch { }

            return success;
        }

        protected abstract void WriteInner(BinaryWriter writer);
        protected abstract void ReadInner(BinaryReader reader);

        protected virtual bool NeedsSaving()
        {
            return true;
        }
    }
}
