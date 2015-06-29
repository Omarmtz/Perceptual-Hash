using System;

namespace ImageFilesProcessor
{
    interface IMediaObjectsHasher
    {
        void ScanDatabaseSystem(IProgress<Tuple<int, int>> onProgressChanged);
    }
}
