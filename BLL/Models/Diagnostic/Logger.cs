using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Diagnostic
{
    public class Logger
    {
        public Guid ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int Line { get; set; }
        public string Message { get; set; }
        public string Inner { get; set; }
        public string Json { get; set; }
    }
}
