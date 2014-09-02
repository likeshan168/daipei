using System.Collections.Generic;

namespace cosen.Models
{
    /// <summary>
    /// 图片信息表
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int idx { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图片大小（k）
        /// </summary>
        public double size { get; set; }
        /// <summary>
        /// 图片链接地址（相对）
        /// </summary>
        public string file_url { get; set; }
        /// <summary>
        /// 图片缩略图地址（好像没有怎么用，暂且不过，放在这里就行）
        /// </summary>
        public string thumbnail_url { get; set; }
        /// <summary>
        /// 删除链接
        /// </summary>
        public string delete_url { get; set; }
        /// <summary>
        /// 删除的动作
        /// </summary>
        public string delete_type { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string error { get; set; }
    }

    //public class ImageUpload
    //{
    //    public HttpPostedFileBase Image { get; set; }
    //    public string ImageType { get; set; }
    //}
    /// <summary>
    /// 为了查询图片方便而设计的类
    /// </summary>
    public class ImageViewInfo
    {
        public IList<ImageInfo> files { get; set; }
    }
}