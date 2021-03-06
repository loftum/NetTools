﻿using System.Net;

namespace Pat.Models
{
    public interface IGlobalSettings
    {
        bool Expect100Continue { get; set; }
        bool CheckCertificateRevocationList { get; set; }
        SecurityProtocolType TLSVersion { get; set; }
    }

    public class GlobalSettings : IGlobalSettings
    {
        public bool Expect100Continue { get; set; }
        public bool CheckCertificateRevocationList { get; set; }
        public SecurityProtocolType TLSVersion { get; set; }
    }
}