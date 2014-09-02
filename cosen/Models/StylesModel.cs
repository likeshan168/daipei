using System.Collections.Generic;

namespace cosen.Models
{
    public class StylesModel
    {
        /// <summary>
        /// 款式代码
        /// </summary>
        public string StyleCode { get; set; }
        /// <summary>
        /// 颜色代码
        /// </summary>
        public string CmCode { get; set; }
        /// <summary>
        /// 款式名称
        /// </summary>
        //public string StyleName { get; set; }
    }
    public class NamesModel
    {
        public string Name { get; set; }
    }
    public class StylesJsonData
    {
        /// <summary>
        /// 所有的款式
        /// </summary>
        public IEnumerable<StylesModel> Styles { get; set; }
        /// <summary>
        /// 对应的图片名称
        /// </summary>
        public IEnumerable<NamesModel> Names { get; set; }
    }

}