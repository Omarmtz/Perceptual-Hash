// <copyright>
// Copyright (c) 2015, All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>
// <author>Omar Martínez Rosas</author>
// <email>omack47@gmail.com</email>
// <date>3-07-2015</date>

using System;

namespace ImageFilesProcessor
{
    interface IMediaObjectsHasher
    {
        void ScanDatabaseSystem(IProgress<Tuple<int, int>> onProgressChanged);
    }
}
