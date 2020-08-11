using System;
using System.ComponentModel;

namespace EasyNow.Dto.Script
{
    public class ScriptInfo
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
