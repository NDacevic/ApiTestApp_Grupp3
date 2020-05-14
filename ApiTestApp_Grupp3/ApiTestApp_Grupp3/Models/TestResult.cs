using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class TestResult
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public int TotalPoints { get; set; }
    }
}
