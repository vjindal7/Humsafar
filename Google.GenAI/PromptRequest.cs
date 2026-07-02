using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleGenAI
{
    public class PromptRequest
    {
        public string SystemMessage { get; set; } = string.Empty;
        public string UserInput { get; set; } = string.Empty;
        public float Temperature { get; set; } = 0.2f;
    }
}
