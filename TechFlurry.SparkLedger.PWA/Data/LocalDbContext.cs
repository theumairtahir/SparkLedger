using Blazor.IndexedDB.Framework;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechFlurry.SparkLedger.PWA.Data
{
    public class LocalDbContext : IndexedDb
    {
        public LocalDbContext(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version)
        {
        }
        public IndexedSet<Token> TableTokens { get; set; }
    }
    public class Token
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
