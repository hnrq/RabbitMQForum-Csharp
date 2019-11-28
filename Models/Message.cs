using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System;

namespace MessagingApi.Models
{
    [Serializable]
    public class Message
    {
        public string Content { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public override string ToString() {
            return  "Content: "+ Content + '\n' +
                    "Type: "+ Type + '\n' +
                    "Id: "+ Id + '\n' +
                    "CreatedOn: "+ CreatedOn + '\n';
        }
    }

}