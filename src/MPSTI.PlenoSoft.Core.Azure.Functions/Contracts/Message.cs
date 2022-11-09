using System.Collections.Generic;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Contracts
{
    public class Message
    {
        [JsonProperty("messages")]
        public List<string> Messages { get; }

        public Message(string message) => Messages = new List<string> { message };
    }
}