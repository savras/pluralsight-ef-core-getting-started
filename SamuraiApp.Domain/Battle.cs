﻿using System;
using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Battle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDAte { get; set; }
        public DateTime EndDate { get; set; }
        public List<Samurai> Samurais { get; set; }
    }
}
