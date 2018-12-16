﻿using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Linq;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public class PropertyInfo : InterfaceInfoPart
    {
        public bool CanGet { get; set; }
        public bool CanSet { get; set; }
        public string Type { get; set; }

        public ArgumentCollection Indizes { get; set; } = new ArgumentCollection();

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Name + " ");

            if(Indizes.Any())
            {
                sb.Append("[ ");

                sb.Append(Indizes.ToString());

                sb.Append(" ]");
            }

            sb.Append("{ ");

            if (CanGet) sb.Append("get; ");
            if (CanSet) sb.Append("set; ");

            sb.Append("}");
            sb.Append(" -> " + Type);

            return sb.ToString();
        }
    }
}