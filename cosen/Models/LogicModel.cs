using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using System.Drawing;


namespace cosen.Models
{
    public class LogicModel
    {
        private DataContextDataContext dataContext = null;
        public LogicModel()
        {
            //this.dataContext = new DataContextDataContext();
        }
        /// <summary>
        /// 获取所有店铺（专用于下拉框的）
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetDianpus()
        {
            //var dps = (from c in dataContext.dianpu
            //           select c).Take(10);
            //return dps.ToList<dianpu>();
            using (dataContext = new DataContextDataContext())
            {
                return dataContext.dianpu.OrderBy(p => p.Use_nm).Select(p => new SelectListItem
                {
                    Text = p.Use_nm,
                    Value = p.Use_id
                }).ToList();
            }

        }
        /// <summary>
        /// 获取店铺列表
        /// </summary>
        /// <returns></returns>
        public IList<dianpu> GetDianpuList()
        {
            IList<dianpu> dps = (List<dianpu>)HttpContext.Current.Cache["dps"];
            if (dps == null)
            {
                using (dataContext = new DataContextDataContext())
                {
                    dps = dataContext.dianpu.OrderBy(p => p.Use_nm).ToList();
                    HttpContext.Current.Cache["dps"] = dps;
                }
            }
            return dps;

        }
        /// <summary>
        /// 获取所有款式信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public StylesJsonData GetStyleJson(HttpContext context)
        {
            dataContext = new DataContextDataContext();
            var styles = from q in dataContext.style
                         select new StylesModel { StyleCode = q.sty_no, CmCode = q.col_no };
            string path = context.Server.MapPath("~/static/images/compose");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();//返回所有的文件
            var names = from q in files
                        select new NamesModel { Name = q.Name.Split('.')[0] };

            return new StylesJsonData { Styles = styles, Names = names };


        }
        /// <summary>
        /// 上传图片（没有改变其大小）
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        //public Task<IList<ImageInfo>> UploadImg(HttpContext context, HttpRequestMessage request)
        //{
        //    IList<ImageInfo> imgList = new List<ImageInfo>();//图片信息列表
        //    if (!request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = context.Server.MapPath("~/static/images");//作为临时存储用的（tmp文件夹）

        //    string tmp = root + "/tmp";
        //    if (!Directory.Exists(tmp))
        //    {
        //        Directory.CreateDirectory(tmp);
        //    }
        //    var provider = new MultipartFormDataStreamProvider(tmp);

        //    var task = request.Content.ReadAsMultipartAsync(provider).
        //        ContinueWith<IList<ImageInfo>>(t =>
        //        {
        //            ImageInfo imginfo = new ImageInfo();
        //            if (t.IsFaulted || t.IsCanceled)
        //            {
        //                imginfo.error = t.Exception.Message;
        //            }
        //            string web_name = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('"');

        //            var file = provider.FileData[0];

        //            //临时存储在服务器上的文件名路径
        //            string server_name = provider.FileData[0].LocalFileName;

        //            //指的的是单款图还是组合图或者是场地图
        //            string imgtype = provider.FormData.Get("imgtype");
        //            //最终的文件位置
        //            string url = string.Format("{0}/{1}/{2}", root, imgtype, web_name);
        //            #region 将临时文件移动到最终的目录下面
        //            FileInfo fi = new FileInfo(server_name);
        //            if (fi.Exists)
        //            {
        //                if (File.Exists(url))//将原来已有的同名图片文件删除掉
        //                {
        //                    File.Delete(url);

        //                }
        //                fi.MoveTo(url);

        //            }
        //            #endregion

        //            #region 返回上传之后的文件信息
        //            imginfo.name = web_name;
        //            imginfo.size = fi.Length / 1024;
        //            string file_url = string.Format("/static/images/{0}/{1}", imgtype, web_name);
        //            imginfo.file_url = file_url;
        //            imginfo.thumbnail_url = file_url;
        //            imginfo.delete_url = string.Format("/api/DpApi/DelImg?imgType={0}&imgName={1}", imgtype, web_name);
        //            imginfo.delete_type = "POST";
        //            imgList.Add(imginfo);
        //            return imgList;
        //            #endregion

        //        });

        //    return task;

        //}

        public bool ThumbnailCallback()
        {
            return false;
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, string mode)
        {
            //System.Drawing.Image originalImage = System.Drawing.Image.FromStream(inputStream);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        public IList<ImageInfo> UploadImg(HttpContext context, HttpRequestMessage request)
        {

            HttpPostedFile file = context.Request.Files.Get("files[]");

            IList<ImageInfo> imgList = new List<ImageInfo>();//图片信息列表

            //指的的是单款图还是组合图或者是场地图
            string imgtype = context.Request.Form["imgtype"];
            string newName = context.Request.Form[file.FileName];//.Split('.')[0];

            string[] names = file.FileName.Split('.');
            if (!string.IsNullOrEmpty(newName))
            {
                newName = newName.Split('.')[0] + "." + names[1];
            }

            string root = context.Server.MapPath("~/static/images");
            string ext = file.FileName.Split('.')[1];//file.FileName.Substring(file.FileName.LastIndexOf("/"));
            //最终的文件位置
            //newName += "." + ext;
            string url = string.Empty;
            string dpid = string.Empty;
            //bit.Save(url);
            Image img = Image.FromStream(file.InputStream);
            if (imgtype == "area")//场地图直接保存（不进行压缩）
            {
                dpid = context.Request.Form["sltdp"];
                using (dataContext = new DataContextDataContext())
                {
                    var entity = dataContext.areapic.FirstOrDefault(p => p.dpid == dpid && p.pic_name == newName);
                    string path = string.Format("{0}/{1}/{2}", root, imgtype, dpid);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (entity == null)
                    {
                        entity = new areapic();
                        entity.pic_name = newName;
                        entity.dpid = dpid;
                        dataContext.areapic.InsertOnSubmit(entity);
                    }
                    url = string.Format("{0}/{1}", path, newName);
                    img.Save(url);
                    dataContext.SubmitChanges();
                    img.Dispose();
                }

            }
            else
            {
                //这里是生成缩略图（压缩之后会看不太清楚）
                //Image ReducedImage;
                //Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                //ReducedImage = img.GetThumbnailImage(260, 260, callb, IntPtr.Zero);
                //ReducedImage.Save(url);

                //这个压缩还能看的清楚
                //Bitmap bit = new Bitmap(img, 260, 260);
                //bit.Save(url);
                url = string.Format("{0}/{1}/{2}", root, imgtype, newName);
                MakeThumbnail(img, url, 260, 260, "Cut");

            }

            ImageInfo imginfo = new ImageInfo();
            imginfo.name = newName;
            imginfo.size = file.ContentLength / 1024;
            string file_url = string.Empty;

            if (imgtype == "area")
            {
                file_url = string.Format("/static/images/{0}/{1}/{2}", imgtype, dpid, newName);
            }
            else
            {
                file_url = string.Format("/static/images/{0}/{1}", imgtype, newName);
            }


            imginfo.file_url = file_url;
            imginfo.thumbnail_url = file_url;
            imginfo.delete_url = string.Format("/api/DpApi/DelImg?imgType={0}&imgName={1}", imgtype, newName);
            imginfo.delete_type = "POST";
            imgList.Add(imginfo);
            return imgList;



        }


        /// <summary>
        /// 删除图片文件
        /// </summary>
        /// <param name="imgType">图片类型（单款图还是组合图还是场地图）</param>
        /// <param name="imgName">图片名称</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public HttpResponseMessage DelImg(string imgType, string imgName, string dpid, HttpRequestMessage request)
        {
            string root = HttpContext.Current.Server.MapPath("~/static/images");
            //string path = string.Format("{0}/{1}/{2}", root, imgType, imgName);
            string path = string.Empty;
            if (imgType == "area")
            {
                path = string.Format("{0}/{1}/{2}/{3}", root, imgType, dpid, imgName);
            }
            else
            {
                path = string.Format("{0}/{1}/{2}", root, imgType, imgName);
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// 查询图片信息
        /// </summary>
        /// <param name="imgName">图片名称（支持模糊查询）</param>
        /// <param name="imgType">图片类型（单款图还是组合图，还是场地图）</param>
        /// <param name="imgPage">页码</param>
        /// <param name="context">请求上下文信息</param>
        /// <returns></returns>
        public IList<ImageInfo> SearchImg(string imgName, string imgType, int imgPage, string dpid, HttpContext context)
        {
            IEnumerable<ImageInfo> names;
            string dir = string.Empty;

            if (imgType == "area")
            {
                dir = string.Format("{0}/{1}/{2}", context.Server.MapPath("~/static/images"), imgType, dpid);
            }
            else
            {
                dir = string.Format("{0}/{1}", context.Server.MapPath("~/static/images"), imgType);
            }
            #region
            if (imgPage == 1)
            {
                context.Cache.Remove(imgType);
            }
            if (context.Cache[imgType] != null)
            {
                names = (IEnumerable<ImageInfo>)context.Cache[imgType];
            }
            else
            {
                names = GetAllImgs(dir, imgType, dpid, imgPage);
                if (names != null)
                    context.Cache[imgType] = names;
            }
            #endregion


            if (!string.IsNullOrEmpty(imgName))//查找制定名称的图片
            {
                return names.Where(p => p.name.StartsWith(imgName)).Skip((imgPage - 1) * 10).Take(10).ToList();
            }
            else//查找所有的图片(注意要分页显示)
            {
                if (names != null)
                    return names.Skip((imgPage - 1) * 10).Take(10).ToList();
                return new List<ImageInfo>();
            }


        }

        /// <summary>
        /// 根据路径获取所有图片的名称信息
        /// </summary>
        /// <param name="path"></param>
        private IEnumerable<ImageInfo> GetAllImgs(string dir, string imgType, string dpid, int imgPage)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (di.Exists)
            {
                string url = string.Empty;
                if (imgType == "area")
                {
                    url = string.Format("/static/images/{0}/{1}", imgType, dpid);
                }
                else
                {
                    url = string.Format("/static/images/{0}", imgType);
                }
                return di.GetFiles().Select((p, index) => new ImageInfo
                {
                    idx = (imgPage - 1) * 10 + index + 1,
                    name = p.Name,
                    size = p.Length / 1024,
                    file_url = string.Format("{0}/{1}", url, p.Name),
                    thumbnail_url = string.Format("{0}/{1}", url, p.Name),
                    delete_url = string.Format("/api/DpApi/DelImg?imgType={0}&imgName={1}&dpid={2}", imgType, p.Name, dpid),
                    delete_type = "POST"

                });
            }
            return null;
        }
        /// <summary>
        /// 修改图片名称
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="imgType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string SaveImgName(string oldName, string newName, string imgType, string dpid, HttpContext context)
        {
            string path = string.Empty;
            if (imgType == "area")
            {
                path = context.Server.MapPath(string.Format("~/static/images/{0}/{1}/", imgType, dpid));
            }
            else
            {
                path = context.Server.MapPath(string.Format("~/static/images/{0}/", imgType));
            }

            //FileInfo fi = new FileInfo(path);

            //数据库中保存的场地图信息也要更新
            newName = newName.Split('.')[0] + "." + oldName.Split('.')[1];
            using (dataContext = new DataContextDataContext())
            {
                var entity = dataContext.areapic.FirstOrDefault(p => p.dpid == dpid && p.pic_name == oldName);
                if (entity != null)
                {
                    entity.pic_name = newName;
                }
                dataContext.SubmitChanges();
            }

            string oldFile = string.Format("{0}/{1}", path, oldName);
            string newFile = string.Format("{0}/{1}", path, newName);
            if (File.Exists(oldFile))
            {
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
                File.Move(oldFile, newFile);
                return "ok";
            }
            return "no";
        }
        /// <summary>
        /// 获取搭配信息
        /// </summary>
        /// <param name="sltDp">推荐店铺id</param>
        /// <param name="pageNum">页码</param>
        /// <param name="tjDate">推荐时间（这个时间主要是用来查询库存用的）</param>
        /// <returns></returns>
        public DaPeiInfo GetDaiPei(string sltDp, int pageNum, string tjDate, string filterType, string filterValue, HttpContext context)
        {
            dataContext = new DataContextDataContext();

            IMultipleResults result = null;
            IEnumerable<getdpResult> allrows = null;
            int offset = (pageNum - 1) * 10;
            if (pageNum == 1)
            {
                if (!string.IsNullOrEmpty(filterValue))
                {
                    allrows = (IEnumerable<getdpResult>)context.Cache["alldps"];
                }
                else
                {
                    short flag = (short)(sltDp.ToUpper().StartsWith("Z") ? 1 : 0);
                    result = dataContext.getdp2(sltDp, DateTime.Now.ToString("yyyy-MM-dd"), tjDate, offset);
                    allrows = result.GetResult<getdpResult>().ToList();
                    context.Cache.Remove("alldps");
                    context.Cache["alldps"] = allrows;
                }


                if (!string.IsNullOrEmpty(filterValue))//如果筛选条件不为空
                {
                    int total = (int)context.Cache["total"];
                    IEnumerable<TjDateInfo> tjrows = (IEnumerable<TjDateInfo>)context.Cache["tjrows"];
                    if (filterType == "style")
                    {

                        return new DaPeiInfo
                       {

                           rows = allrows.Where(p => p.masterstyle.StartsWith(filterValue)).Skip(offset).Take(10),//注意顺序
                           total = total,
                           tjrows = tjrows

                       };
                    }
                    else
                    {
                        return new DaPeiInfo
                        {

                            rows = allrows.Where(p => p.mqu > int.Parse(filterValue)).Skip(offset).Take(10),//注意顺序
                            total = total,
                            tjrows = tjrows

                        };
                    }

                }
                else
                {
                    //下面就是不进行筛选
                    int total = result.GetResult<int>().SingleOrDefault();
                    context.Cache["total"] = total;
                    IEnumerable<TjDateInfo> tjrows = result.GetResult<TjDateInfo>().ToList();
                    context.Cache["tjrows"] = tjrows;
                    return new DaPeiInfo
                    {

                        rows = allrows.Skip(offset).Take(10),//注意顺序
                        total = total,
                        tjrows = tjrows

                    };
                }
            }
            else
            {
                allrows = (IEnumerable<getdpResult>)context.Cache["alldps"];

                if (!string.IsNullOrEmpty(filterValue))//如果筛选条件不为空
                {
                    if (filterType == "style")
                    {
                        return new DaPeiInfo
                        {

                            rows = allrows.Where(p => p.masterstyle.StartsWith(filterValue)).Skip(offset).Take(10)//注意顺序

                        };
                    }
                    else
                    {
                        return new DaPeiInfo
                        {

                            rows = allrows.Where(p => p.mqu > int.Parse(filterValue)).Skip(offset).Take(10)//注意顺序

                        };
                    }

                }
                return new DaPeiInfo
                {

                    rows = allrows.Skip(offset).Take(10)

                };
            }



        }
        /// <summary>
        /// 保存店铺的推荐搭配
        /// </summary>
        /// <param name="dpid">店铺id</param>
        /// <param name="tjdate">推荐时间</param>
        /// <param name="tjdp">推荐的内容（是有字符串拼接起来的）</param>
        /// <returns></returns>
        public string SaveTuiJianDp(string dpid, string tjdate, string tjdp, string remark)
        {
            if (!string.IsNullOrEmpty(dpid) && !string.IsNullOrEmpty(tjdate) && !string.IsNullOrEmpty(tjdp))//参数都不为空
            {
                using (dataContext = new DataContextDataContext())//默认启用事务
                {
                    IEnumerable<tjdp> dels = dataContext.tjdp.Where(p => p.use_id == dpid && p.tjdate == tjdate);
                    dataContext.tjdp.DeleteAllOnSubmit(dels);//先删除（再加入）

                    string[] entities = tjdp.Split('$');
                    IList<tjdp> tjdps = new List<tjdp>();
                    foreach (string entity in entities)
                    {
                        string[] tmps = entity.Split('@');
                        tjdps.Add(new tjdp()
                        {
                            cbpicture = tmps[0],
                            mqu = tmps[1],
                            lqu1 = tmps[2],
                            lqu2 = tmps[3],
                            bqu1 = tmps[4],
                            bqu2 = tmps[5],
                            aqu1 = tmps[6],
                            aqu2 = tmps[7],
                            use_id = dpid,
                            tjdate = tjdate,
                            remark = remark
                        });
                    }

                    dataContext.tjdp.InsertAllOnSubmit(tjdps);
                    dataContext.SubmitChanges();
                    return "保存成功";
                }


            }
            return "参数不完整";
        }

        /// <summary>
        /// 查询某店铺，某天的推荐搭配信息
        /// </summary>
        /// <param name="dpid">店铺id</param>
        /// <param name="tjdate">推荐日期</param>
        /// <returns></returns>
        public IList<DaPei> LookUpTjDp(string dpid, string tjdate)
        {
            using (dataContext = new DataContextDataContext())
            {
                return (from t in dataContext.tjdp
                        join d in dataContext.dapei on t.cbpicture equals d.cbpicture
                        where t.tjdate == tjdate && t.use_id == dpid
                        select new DaPei
                        {
                            cbpicture = t.cbpicture,
                            masterstyle = d.masterstyle,
                            mqu = t.mqu,
                            legging1 = d.legging1,
                            lqu1 = t.lqu1,
                            legging2 = d.legging2,
                            lqu2 = t.lqu2,
                            bottom1 = d.bottom1,
                            bqu1 = t.bqu1,
                            bottom2 = d.bottom2,
                            bqu2 = t.bqu2,
                            accessory1 = d.accessory1,
                            aqu1 = t.aqu1,
                            accessory2 = d.accessory2,
                            aqu2 = t.aqu2,
                            remark = t.remark

                        }).ToList();
            }
        }


        /// <summary>
        /// 查询所有的店铺推荐信息
        /// </summary>
        /// <returns></returns>
        internal IQueryable LookAllTjDp()
        {
            dataContext = new DataContextDataContext();

            return (from t in dataContext.tjdp
                    join d in dataContext.dianpu
                    on t.use_id equals d.Use_id
                    select new
                    {
                        User_Id = t.use_id,
                        User_Nm = d.Use_nm
                    }).Distinct();
        }

        /// <summary>
        /// 查询某店铺的推荐日期
        /// </summary>
        /// <param name="dpId">店铺id</param>
        /// <returns></returns>
        internal IQueryable LookAllTjDp(string dpId)
        {
            dataContext = new DataContextDataContext();
            return dataContext.tjdp.Where(p => p.use_id == dpId).OrderByDescending(p => p.tjdate).Select(p => new { tjdate = p.tjdate }).Distinct().Take(10).OrderByDescending(p => p.tjdate);
        }


        /// <summary>
        /// 根据主打款号，或者组合图获取搭配图片信息
        /// </summary>
        /// <param name="style">主打款号，或者组合图的名称</param>
        /// <param name="dpType">搭配类型</param>
        /// <param name="gtype">标志：single表示根据主打款查询，compose表示根据组合图进行查询</param>
        /// <returns></returns>
        internal IQueryable GetDpByMaster(string style, string dpType, string gtype)
        {
            dataContext = new DataContextDataContext();
            if (gtype == "single")//说明是按主打款进行查询搭配图片信息的
            {
                return dataContext.dapei.Where(p => p.masterstyle == style && p.type == dpType).Select(p => new
                {
                    masterstyle = p.masterstyle,
                    type = p.type,
                    legging1 = p.legging1,
                    legging2 = p.legging2,
                    bottom1 = p.bottom1,
                    bottom2 = p.bottom2,
                    accessory1 = p.accessory1,
                    accessory2 = p.accessory2,
                    cbpicture = p.cbpicture
                });
            }
            else
            {
                return dataContext.dapei.Where(p => p.cbpicture == style).Select(p => new
                {
                    masterstyle = p.masterstyle,
                    type = p.type,
                    legging1 = p.legging1,
                    legging2 = p.legging2,
                    bottom1 = p.bottom1,
                    bottom2 = p.bottom2,
                    accessory1 = p.accessory1,
                    accessory2 = p.accessory2,
                    cbpicture = p.cbpicture
                });
            }
        }

        /// <summary>
        /// 获取报表
        /// </summary>
        /// <param name="dps">店铺ids</param>
        /// <param name="pageNum">页码</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回报表</returns>
        public IList<ReportInfo> GetReportsInfo(string sltType, string dps, int pageNum, string startDate, string endDate, string styleNo, string sort, string sortT)
        {
            if (pageNum == 1 && sort == "default")
            {
                HttpContext.Current.Cache.Remove("reports");
            }
            HttpContext.Current.Cache["dpids"] = dps;
            HttpContext.Current.Cache["startDate"] = startDate;//给下面导出excel表格用的
            HttpContext.Current.Cache["endDate"] = endDate;
            IList<ReportInfo> infos = (List<ReportInfo>)HttpContext.Current.Cache["reports"];

            if (infos == null)
            {
                using (dataContext = new DataContextDataContext())
                {
                    infos = dataContext.report2(sltType, startDate, endDate, dps.Replace("$", ",")).ToList();
                    HttpContext.Current.Cache["reports"] = infos;
                }
            }

            if (sort == "nsty_no")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
            }
            else if (sort == "search_name")
            {
                if (!string.IsNullOrEmpty(styleNo))
                    return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.com_nm.StartsWith(sortT, StringComparison.CurrentCultureIgnoreCase)).OrderBy(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                return infos.Where(p => p.com_nm.StartsWith(sortT, StringComparison.CurrentCultureIgnoreCase)).OrderBy(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
            }
            else if (sort == "search_style")
            {


                return infos.Where(p => p.nsty_no.StartsWith(sortT)).OrderBy(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

            }

            else if (sort == "ssdate")//上市日期不排序，只进行筛选
            {
                if (sortT == "empty")//获取上市日期为空的
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo) && string.IsNullOrEmpty(p.editionhandle)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.Where(p => string.IsNullOrEmpty(p.editionhandle)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "all")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {
                    string month = string.Empty;
                    #region
                    switch (sortT)
                    {
                        case "m_01":
                            month = "01";
                            break;
                        case "m_02":
                            month = "02";
                            break;
                        case "m_03":
                            month = "03";
                            break;
                        case "m_04":
                            month = "04";
                            break;
                        case "m_05":
                            month = "05";
                            break;
                        case "m_06":
                            month = "06";
                            break;
                        case "m_07":
                            month = "07";
                            break;
                        case "m_08":
                            month = "08";
                            break;
                        case "m_09":
                            month = "09";
                            break;
                        case "m_10":
                            month = "10";
                            break;
                        case "m_11":
                            month = "11";
                            break;
                        case "m_12":
                            month = "12";
                            break;

                    }
                    #endregion


                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.editionhandle == month).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.Where(p => p.editionhandle == month).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
            }
            else if (sort == "cxinfo")//促销信息过滤
            {
                if (!string.IsNullOrEmpty(styleNo))
                    return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.p_id == int.Parse(sortT)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                return infos.Where(p => p.p_id == int.Parse(sortT)).OrderByDescending(p => p.nsty_no).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
            }
            else if (sort == "rknum")//入库排序
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.rknum < 5).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum < 5).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.rknum > 5).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum > 5).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.rknum > 10).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum > 10).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.rknum > 15).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum > 15).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.rknum > 20).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum > 20).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.rknum > 20).OrderByDescending(p => p.rknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                    }


                }

            }
            else if (sort == "chnum")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.chnum < 5).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum < 5).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.chnum > 5).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum > 5).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.chnum > 10).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum > 10).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.chnum > 15).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum > 15).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.chnum > 20).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum > 20).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.chnum > 20).OrderByDescending(p => p.chnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    }


                }

            }
            else if (sort == "dbnum")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.dbnum < 5).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum < 5).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.dbnum > 5).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum > 5).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.dbnum > 10).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum > 10).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.dbnum > 15).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum > 15).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.dbnum > 20).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum > 20).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.dbnum > 20).OrderByDescending(p => p.dbnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    }


                }

            }
            else if (sort == "thnum")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.thnum < 5).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum < 5).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.thnum > 5).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum > 5).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.thnum > 10).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum > 10).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.thnum > 15).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum > 15).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.thnum > 20).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum > 20).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.thnum > 20).OrderByDescending(p => p.thnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    }


                }

            }
            else if (sort == "xsnum")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.xsnum < 5).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum < 5).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.xsnum > 5).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum > 5).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.xsnum > 10).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum > 10).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.xsnum > 15).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum > 15).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.xsnum > 20).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum > 20).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.xsnum > 20).OrderByDescending(p => p.xsnum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    }


                }
            }
            else if (sort == "cknum")
            {
                if (sortT == "desc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else if (sortT == "asc")
                {
                    if (!string.IsNullOrEmpty(styleNo))
                        return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderBy(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    return infos.OrderBy(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                }
                else
                {

                    switch (sortT)
                    {

                        case "num1"://<5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.cknum < 5).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum < 5).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num2"://>5

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.cknum > 5).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum > 5).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num3"://>10

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.cknum > 10).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum > 10).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();

                        case "num4"://>15

                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.cknum > 15).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum > 15).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        case "num5"://>20
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo) && p.cknum > 20).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum > 20).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                        default:
                            if (!string.IsNullOrEmpty(styleNo))
                                return infos.Where(p => p.nsty_no.StartsWith(styleNo)).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                            return infos.Where(p => p.cknum > 20).OrderByDescending(p => p.cknum).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                    }


                }

            }
            else//默认不进行排序（但是存储过程是默认按照款式的降序）
            {
                if (!string.IsNullOrEmpty(styleNo))
                    return infos.Where(p => p.nsty_no.StartsWith(styleNo)).Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
                return infos.Skip((pageNum - 1) * 10).Take(10).ToList<ReportInfo>();
            }
        }
        /// <summary>
        /// 修改上市日期
        /// </summary>
        /// <param name="styleNo">款式</param>
        /// <param name="date">上市日期</param>
        /// <returns></returns>
        public string UpdateDate(string styleNo, string date, int? cxinfo)
        {
            if (!string.IsNullOrEmpty(styleNo))
            {
                using (dataContext = new DataContextDataContext())
                {
                    //Style_EditionHandle style = new Style_EditionHandle();

                    Style_EditionHandle style = dataContext.Style_EditionHandle.FirstOrDefault(p => p.StyleCode == styleNo);

                    if (style == null)
                    {
                        style = new Style_EditionHandle();
                        style.StyleCode = styleNo;
                        style.EditionHandle = date;
                        style.p_id = cxinfo;
                        dataContext.Style_EditionHandle.InsertOnSubmit(style);
                    }
                    else
                    {
                        style.EditionHandle = date;
                        style.p_id = cxinfo;
                    }


                    dataContext.SubmitChanges();
                    HttpContext.Current.Cache.Remove("reports");
                    return "success";
                }
            }
            else
            {
                return "failure";
            }
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <returns></returns>
        public string OutExcel()
        {
            #region
            //var sbHtml = new StringBuilder();
            //sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            //sbHtml.Append("<tr>");
            //var lstTitle = new List<string> { "编号", "姓名", "年龄", "创建时间", "图片" };
            //foreach (var item in lstTitle)
            //{
            //    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            //}
            //sbHtml.Append("</tr>");

            //for (int i = 0; i < 10; i++)
            //{
            //    sbHtml.Append("<tr>");
            //    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", i);
            //    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>屌丝{0}号</td>", i);
            //    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", new Random().Next(20, 30) + i);
            //    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", DateTime.Now);
            //    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'><img src='http://www.cosen168.com:83/static/images/single/23067CO1-CF1J2-1.jpg' /></td>");
            //    sbHtml.Append("</tr>");
            //}
            //sbHtml.Append("</table>");
            //return sbHtml.ToString(); 
            #endregion

            string startDate = (string)HttpContext.Current.Cache["startDate"];
            string endDate = (string)HttpContext.Current.Cache["endDate"];

            string dpidStr = (string)HttpContext.Current.Cache["dpids"];
            string[] dpids = dpidStr.Split('$');

            IList<dianpu> dps = (List<dianpu>)HttpContext.Current.Cache["dps"];
            IList<ChuHuoDetails> chdetails = null;
            IList<DiaoBoDetails> dbdetails = null;
            IList<TuiHuoDetails> thdetails = null;
            using (dataContext = new DataContextDataContext())
            {
                IMultipleResults result = dataContext.getchdbdetail2(startDate, endDate, dpidStr.Replace('$', ','));
                chdetails = result.GetResult<ChuHuoDetails>().ToList();
                dbdetails = result.GetResult<DiaoBoDetails>().ToList();
                thdetails = result.GetResult<TuiHuoDetails>().ToList();
            }

            var daili = dps.Where(p => (p.Use_id.StartsWith("D") && p.Use_id != "D006") || p.Use_id == "Z010" || p.Use_id == "Z015").Select(p => p.Use_nm);

            var zhiying = dps.Where(p => (p.Use_id.StartsWith("Z") && p.Use_id != "Z010" && p.Use_id != "Z015") || p.Use_id == "D006" || p.Use_id == "G002" || p.Use_id == "g003").Select(p => p.Use_nm);

            var tuihuo = dps.Select(p => p.Use_nm);

            int dailiCount = daili.Count();//出货列数 29
            int zhiyingCount = zhiying.Count();//调拨列数 12
            int tuihuoCount = tuihuo.Count();//退货列数 45

            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "图片", "款式", "名称", "入库", "上市日期", "出货", "调拨", "退货", "销售", "库存", "单价", "金额" };
            foreach (var item in lstTitle)
            {
                if (item == "出货")
                {
                    sbHtml.AppendFormat("<td colspan='{0}' style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='60'>{1}</td>", dailiCount + 1, item);
                }
                else if (item == "调拨")
                {
                    sbHtml.AppendFormat("<td colspan='{0}' style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='60'>{1}</td>", zhiyingCount + 1, item);
                }
                else if (item == "退货")
                {
                    sbHtml.AppendFormat("<td colspan='{0}' style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='60'>{1}</td>", tuihuoCount + 1, item);
                }
                else
                {
                    sbHtml.AppendFormat("<td rowspan='2' style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='60'>{0}</td>", item);
                }
            }
            sbHtml.Append("</tr>");

            IList<ReportInfo> infos = (List<ReportInfo>)HttpContext.Current.Cache["reports"];
            //string imgUrl=WebConfiguration

            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='80'>{0}</td>", "合计");
            foreach (string item in daili)
            {
                sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='60'>{0}</td>", item);
            }
            sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='80'>{0}</td>", "合计");
            foreach (string item in zhiying)
            {
                sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='80'>{0}</td>", item);
            }
            sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='80'>{0}</td>", "合计");
            foreach (var item in tuihuo)
            {
                sbHtml.AppendFormat("<td  style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='20' width='80'>{0}</td>", item);
            }

            sbHtml.Append("</tr>");
            string imgUrl = ConfigurationManager.AppSettings["imageUrl"].ToString();

            foreach (ReportInfo info in infos)
            {
                sbHtml.Append("<tr>");
                //图片
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'><img src='{0}{1}.jpg' /></td>", imgUrl, info.nsty_no);

                //款式
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.nsty_no);
                //名称
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.com_nm);
                //入库
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.rknum);

                //上市日期
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.editionhandle);

                //出货合计
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.chnum);
                //出货店铺明细
                foreach (var item in daili)
                {
                    var q = chdetails.Where(p => p.ent_nm == item && p.style == info.nsty_no).Select(p => new { p.style, p.com_qu }).Sum(p => p.com_qu);


                    sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", q);
                }

                //调拨合计
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.dbnum);
                //调拨店铺明细
                foreach (var item in zhiying)
                {
                    var q = dbdetails.Where(p => p.ent_nm == item && p.style == info.nsty_no).Select(p => new { p.style, p.com_qu }).Sum(p => p.com_qu);
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", q);
                }

                //退货合计
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.thnum);
                //退货店铺明细
                foreach (var item in tuihuo)
                {
                    var q = thdetails.Where(p => p.out_nm == item && p.style == info.nsty_no).Select(p => new { p.style, p.com_qu }).Sum(p => p.com_qu);
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", q);
                }
                //销售
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.xsnum);
                //库存
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.cknum);
                //单价
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.unitprice);
                //金额
                sbHtml.AppendFormat("<td style='font-size: 12px;height:60px;'>{0}</td>", info.xsmoney);
                sbHtml.Append("</tr>");
            }


            sbHtml.Append("</table>");
            return sbHtml.ToString();

            //return string.Empty;
        }

        /// <summary>
        /// 获取所有的查询到的符合条件的款式
        /// </summary>
        /// <param name="styleNo"></param>
        /// <returns></returns>
        public IList<string> GetUp4Images(string styleNo)
        {
            using (dataContext = new DataContextDataContext())
            {
                IList<Up4Image> imgs = dataContext.Up4Image.Where(p => p.Code.StartsWith(styleNo)).ToList();
                HttpContext.Current.Cache["imgs"] = imgs;
                return imgs.Select(p => p.Code).ToList();
            }
        }
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="styleNo">款式代码</param>
        /// <returns></returns>
        public Binary ShowImage(string styleNo)
        {
            IList<Up4Image> imgs = (List<Up4Image>)HttpContext.Current.Cache["imgs"];
            return imgs.Where(p => p.Code == styleNo).Select(p => p.StyleImage).FirstOrDefault();

        }
        /// <summary>
        /// 保存搭配
        /// </summary>
        /// <param name="httpContext">上下文信息（获取表单信息）</param>
        /// <returns></returns>
        internal string SaveDp(HttpContext httpContext)
        {
            try
            {
                string masterStyle = httpContext.Request.Form["txtMaster"];
                string cbpicture = httpContext.Request.Form["txtCompose"];
                if (string.IsNullOrEmpty(masterStyle))
                {
                    return "主打款不能为空";
                }
                if (string.IsNullOrEmpty(cbpicture))
                {
                    return "组合图不能为空";
                }
                string dptype = httpContext.Request.Form["stdp"];

                using (dataContext = new DataContextDataContext())
                {
                    var entity = dataContext.dapei.FirstOrDefault(p => p.masterstyle == masterStyle && p.type == dptype);
                    if (entity == null)//如果不存在就添加
                    {
                        entity = new dapei();
                        entity.masterstyle = masterStyle;
                        entity.type = dptype;
                        entity.legging1 = httpContext.Request.Form["txtLegging1"];
                        entity.legging2 = httpContext.Request.Form["txtLegging2"];
                        entity.bottom1 = httpContext.Request.Form["txtBottom1"];
                        entity.bottom2 = httpContext.Request.Form["txtBottom2"];
                        entity.accessory1 = httpContext.Request.Form["txtAccessory1"];
                        entity.accessory2 = httpContext.Request.Form["txtAccessory2"];
                        entity.cbpicture = httpContext.Request.Form["txtCompose"];
                        dataContext.dapei.InsertOnSubmit(entity);

                    }
                    else//那就更新
                    {
                        entity.legging1 = httpContext.Request.Form["txtLegging1"];
                        entity.legging2 = httpContext.Request.Form["txtLegging2"];
                        entity.bottom1 = httpContext.Request.Form["txtBottom1"];
                        entity.bottom2 = httpContext.Request.Form["txtBottom2"];
                        entity.accessory1 = httpContext.Request.Form["txtAccessory1"];
                        entity.accessory2 = httpContext.Request.Form["txtAccessory2"];
                        entity.cbpicture = cbpicture;
                    }


                    dataContext.SubmitChanges();
                    return "保存搭配成功！";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }


        /// <summary>
        /// 获取促销信息
        /// </summary>
        /// <returns></returns>
        internal IList<promotions> GetPromotionInfo()
        {
            using (dataContext = new DataContextDataContext())
            {
                return dataContext.promotions.ToList();
            }
        }
        /// <summary>
        /// 添加促销信息
        /// </summary>
        /// <param name="pro"></param>
        internal void AddPromotionInfo(promotions pro)
        {
            using (dataContext = new DataContextDataContext())
            {
                var entity = new promotions();
                entity.p_content = pro.p_content;
                entity.remark = pro.remark;
                dataContext.promotions.InsertOnSubmit(entity);
                dataContext.SubmitChanges();
            }
        }
        /// <summary>
        /// 获取单个促销信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        internal promotions GetPromotionInfoById(int? id)
        {
            using (dataContext = new DataContextDataContext())
            {

                return dataContext.promotions.FirstOrDefault(p => p.p_id == id);

            }
        }
        /// <summary>
        /// 更新促销信息
        /// </summary>
        /// <param name="pro"></param>
        internal void UpdatePromotionInfo(promotions pro)
        {
            using (dataContext = new DataContextDataContext())
            {
                var entity = dataContext.promotions.FirstOrDefault(p => p.p_id == pro.p_id);
                if (entity != null)
                {
                    entity.p_content = pro.p_content;
                    entity.remark = pro.remark;
                }
                dataContext.SubmitChanges();
            }
        }
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="id"></param>
        internal void DelPromotionInfo(int id)
        {
            using (dataContext = new DataContextDataContext())
            {
                var entity = dataContext.promotions.FirstOrDefault(p => p.p_id == id);
                if (entity != null)
                    dataContext.promotions.DeleteOnSubmit(entity);
                dataContext.SubmitChanges();
            }
        }
    }
}