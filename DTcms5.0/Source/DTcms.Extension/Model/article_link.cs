using System;
namespace DTcms.Model {
   /// <summary>
   /// 内容关联
   /// </summary>
   [Serializable]
   public partial class article_link {
      public article_link() { }
      #region Model
      private int _id = 0;
      private int _site_id = 0;
      private int _channel_id = 0;
      private int _article_id = 0;
      private int _link_site_id = 0;
      private int _link_channel_id = 0;
      private int _link_article_id = 0;
      private int _link_category_id = 0;
      private DateTime _add_time;

      /// <summary>
      /// 自增字段
      /// </summary>
      public int id {
         set { _id = value; }
         get { return _id; }
      }

      /// <summary>
      /// 站点Id
      /// </summary>
      public int site_id {
         set { _site_id = value; }
         get { return _site_id; }
      }

      /// <summary>
      /// 通道Id
      /// </summary>
      public int channel_id {
         set { _channel_id = value; }
         get { return _channel_id; }
      }

      /// <summary>
      /// 文章ID
      /// </summary>
      public int article_id {
         set { _article_id = value; }
         get { return _article_id; }
      }

      /// <summary>
      /// 关联文章所属站点ID
      /// </summary>
      public int link_site_id {
         set { _link_site_id = value; }
         get { return _link_site_id; }
      }

      /// <summary>
      /// 关联文章所属通道Id
      /// </summary>
      public int link_channel_id {
         set { _link_channel_id = value; }
         get { return _link_channel_id; }
      }

      /// <summary>
      /// 关联文章Id
      /// </summary>
      public int link_article_id {
         set { _link_article_id = value; }
         get { return _link_article_id; }
      }

      /// <summary>
      /// 关联文章所属类别Id
      /// </summary>
      public int link_category_id {
         set { _link_category_id = value; }
         get { return _link_category_id; }
      }

      /// <summary>
      /// 添加时间
      /// </summary>
      public DateTime add_time {
         set { _add_time = value; }
         get { return _add_time; }
      }
      #endregion Model

   }
}