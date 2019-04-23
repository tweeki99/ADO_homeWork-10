using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppEF
{
    public class Song: Entity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public Guid MusicalGroupId { get; set; }
        public virtual MusicalGroup MusicalGroup { get; set; }
    }
}
