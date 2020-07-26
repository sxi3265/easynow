using System.Collections.Generic;
using System.Text;

namespace EasyNow.Dal.Entities
{
    public class User: BaseEntity
    {
        public string Account { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserScript> UserScripts { get; set; }
        public virtual ICollection<UserDevice> UserDevices { get; set; }
    }
}
