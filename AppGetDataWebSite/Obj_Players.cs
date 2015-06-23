using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGetDataWebSite
{
    public class Obj_Players
    {
        public Obj_Player Player { get; set; }
        public List<Obj_IndexPosition> LstIndexPosition { get; set; }
        public Obj_PlayerDetail PlayerDetail { get; set; }
        public List<Obj_IndexHidden> LstIndexHidden { get; set; }
    }
}
