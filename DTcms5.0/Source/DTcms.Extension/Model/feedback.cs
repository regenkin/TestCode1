using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model {
   [Serializable]
   public class feedback {
      // Methods
      public feedback() {
      }

      // Properties
      public DateTime add_time { get; set; }
      public string content { get; set; }
      public int id { get; set; }
      public int is_lock { get; set; }
      public string reply_content { get; set; }
      public DateTime? reply_time { get; set; }
      public string site_path { get; set; }
      public string title { get; set; }
      public string user_email { get; set; }
      public string user_name { get; set; }
      public string user_qq { get; set; }
      public string user_tel { get; set; }
   }
}
