using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models.UserModels;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        [ForeignKey("User")]
        public int UserRefId { get; set; }
        public User User { get; set; }
        
    }
}
