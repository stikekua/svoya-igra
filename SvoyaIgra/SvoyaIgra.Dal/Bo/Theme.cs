﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.Dal.Bo
{
    public class Theme
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ThemeDifficulty Difficulty { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}
