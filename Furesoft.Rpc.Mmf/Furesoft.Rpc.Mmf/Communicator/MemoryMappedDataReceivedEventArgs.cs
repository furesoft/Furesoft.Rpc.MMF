﻿/* Copyright 2012 Marco Minerva, marco.minerva@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;

namespace Furesoft.Rpc.Mmf.Communicator
{
    public class MemoryMappedDataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        internal MemoryMappedDataReceivedEventArgs(byte[] data, long length)
        {
            if (data != null)
            {
                Data = new byte[length];
                Array.Copy(data, Data, length);
            }
        }
    }
}
