using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model {
   [Serializable]
   public class link {
      public link() {
      }

      // Properties
      public DateTime add_time { get; set; }
      public string email { get; set; }
      public int id { get; set; }
      public string img_url { get; set; }
      public int is_image { get; set; }
      public int is_lock { get; set; }
      public int is_red { get; set; }
      public string site_path { get; set; }
      public string site_url { get; set; }
      public int sort_id { get; set; }
      public string title { get; set; }
      public string user_name { get; set; }
      public string user_tel { get; set; }
   }
}
