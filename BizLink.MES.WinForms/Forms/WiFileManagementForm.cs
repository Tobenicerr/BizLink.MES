using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using ClosedXML.Graphics;
using DiffMatchPatch;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PdfiumViewer;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Tesseract;
using UglyToad.PdfPig.Content;

namespace BizLink.MES.WinForms.Forms
{
    public partial class WiFileManagementForm : MesBaseForm
    {
        private PdfViewerControl viewerLeft;
        private PdfViewerControl viewerRight;
        private List<DocVersion> versionList = new List<DocVersion>();
        private readonly IMaterialViewService _materialViewService;
        private readonly IFactoryService _factoryService;
        private readonly IWiDocumentService _wiDocumentService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ServiceEndpointSettings _apiSettings; // 假设您有配置类


        private string _currentLocalFilePath = string.Empty;
        private const string PARAM_OCRDICT = "OcrCorrectionDict";
        private const string PARAMITEM_OCRDICTJSON = "OcrDictJson";



        public WiFileManagementForm(IMaterialViewService materialViewService, IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IFactoryService factoryService, IWiDocumentService wiDocumentService, IParameterGroupService parameterGroupService)
        {
            InitializeComponent();
            InitializeCustomComponents();
            _materialViewService = materialViewService;
            _factoryService = factoryService;
            _wiDocumentService = wiDocumentService;
            _parameterGroupService = parameterGroupService;
            _apiSettings = apiSettings.Value["MesApi"];


        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConstructionNoSelect.PlaceholderText = "请选择图纸号...";
            DocumentNoSelect.PlaceholderText = "请选择文件编号...";
            await LoadOcrDictionaryFromDatabaseAsync();
            await LoadConstructionNoAsync();
        }

        private void InitializeCustomComponents()
        {
            // 直接传入 splitter1.Panel1 和 Panel2 作为父容器


            viewerLeft = CreateViewer(splitter1.Panel1, "Reference / Base");
            viewerRight = CreateViewer(splitter1.Panel2, "Target / Diff Highlight", isDiffView: true);
        }


        /// <summary>
        /// 从数据库加载 OCR 纠错字典并赋给提取器
        /// </summary>
        private async Task LoadOcrDictionaryFromDatabaseAsync()
        {
            try
            {
                var dictParam = await _parameterGroupService.GetGroupWithItemsAsync(PARAM_OCRDICT);


                if (dictParam != null && dictParam.Items.Count() > 0)
                {
                    // 将 JSON 字符串反序列化为 Dictionary
                    var corrections = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictParam.Items.Where(x => x.Key == PARAMITEM_OCRDICTJSON).First().Value);

                    // 将字典传给提取器
                    TesseractExtractor.SetCorrections(corrections);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("加载 OCR 纠错字典失败：" + ex.Message);
            }
        }

        private async Task LoadConstructionNoAsync()
        {
            ConstructionNoSelect.Items.Clear();
            var dtos = await _wiDocumentService.GetAllAsync();
            ConstructionNoSelect.Items.AddRange(dtos.Select(x => x.ConstructionNo).Distinct().Select(x => new MenuItem()
            {
                Name = x,
                Text = x
            }).ToArray());
        }

        private PdfViewerControl CreateViewer(Control parentContainer, string labelText, bool isDiffView = false)
        {
            // 1. 创建包裹用的 Panel
            var wrapperPanel = new AntdUI.Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4), // 内边距
                BackColor = Color.WhiteSmoke
            };
            // 2. 创建标题 Label
            var label = new AntdUI.Label
            {
                Text = labelText,
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
                ForeColor = isDiffView ? Color.Red : Color.FromArgb(100, 100, 100)
            };
            // 3. 创建 Viewer
            var viewer = new PdfViewerControl
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.AppWorkspace,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 4. 将控件加入 Wrapper Panel
            wrapperPanel.Controls.Add(label);
            wrapperPanel.Controls.Add(viewer);

            // 【关键布局修正】
            // WinForms 的 Dock 布局是从 Z-Order 底层开始计算的。
            // 我们需要 Label 先占据顶部，所以 Label 必须沉底 (SendToBack)。
            // Viewer 占据剩余空间，所以 Viewer 必须浮顶 (BringToFront)。
            label.SendToBack();
            viewer.BringToFront();

            // 5. 【核心修复】将 Wrapper Panel 添加到真正的父容器（如 SplitterPanel）中
            // 之前的问题是 wrapperPanel 没被加到界面上
            parentContainer.Controls.Add(wrapperPanel);

            return viewer;
        }

        private async void CompareButton_Click(object sender, EventArgs e)
        {

            await RunAsync(CompareButton, async () =>
            {
                // 1. 直接从左右两个 Viewer 控件中获取当前已经加载好的页面数据
                var leftPages = viewerLeft.GetPagesData();
                var rightPages = viewerRight.GetPagesData();

                // 防呆校验：确保两侧都有数据
                if (leftPages == null || leftPages.Count == 0 || rightPages == null || rightPages.Count == 0)
                {
                    throw new Exception("请先在左右两侧加载需要比对的文件（可点击列表或上传）");
                }

                // 清理上一轮比对遗留的旧截图资源，防止内存泄漏
                foreach (var page in rightPages)
                {
                    if (page.Regions != null)
                    {
                        foreach (var r in page.Regions)
                        {
                            r.OldImageSnippet?.Dispose();
                            r.NewImageSnippet?.Dispose();
                        }
                    }
                }

                try
                {
                    int maxPages = Math.Max(leftPages.Count, rightPages.Count);

                    if (maxPages > 50)
                        throw new Exception("文档页数过多 (>50页)，建议拆分后比对");

                    int totalDiffCount = 0;
                    var newLeftPages = new List<PageDiffData>();
                    var newRightPages = new List<PageDiffData>();

                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

                    // 2. 循环处理每一页 (直接复用内存中的 Bitmap 图片，快如闪电)
                    Parallel.For(0, maxPages, parallelOptions, i =>
                    {
                        Bitmap originalImgA = i < leftPages.Count ? leftPages[i].Image : null;
                        Bitmap originalImgB = i < rightPages.Count ? rightPages[i].Image : null;

                        List<DiffRegion> regions = new List<DiffRegion>();

                        if (originalImgA != null && originalImgB != null)
                        {
                            // 【非常关键】：因为原图 (originalImgA/B) 当前正被 UI 控件显示着。
                            // 在后台多线程进行 LockBits 像素扫描时，如果 UI 线程同时触发了重绘，
                            // 就会发生资源抢占，导致 "GDI+ 一般性错误" 程序崩溃。
                            // 解决办法：在比对前，快速克隆一份只用于后台计算的安全副本。
                            using (Bitmap cloneA = new Bitmap(originalImgA))
                            using (Bitmap cloneB = new Bitmap(originalImgB))
                            {
                                regions = PdfDiffEngine.DetectAndOptimize(cloneA, cloneB);
                            }
                        }

                        lock (newLeftPages)
                        {
                            // 3. 组装新数据，重点是 Image 属性【必须继续指向】原来的 originalImgA/B
                            // 【核心修复】：增加 null 判断，只有真实存在的页面才添加，避免两侧页数不一致时出现 100x100 的空白占位块
                            if (originalImgA != null)
                            {
                                newLeftPages.Add(new PageDiffData { Image = originalImgA, Regions = null, PageIndex = i });
                            }

                            if (originalImgB != null)
                            {
                                newRightPages.Add(new PageDiffData { Image = originalImgB, Regions = regions, PageIndex = i });
                            }

                            Interlocked.Add(ref totalDiffCount, regions.Count);
                        }
                    });

                    // 重新按页码排序
                    newLeftPages = newLeftPages.OrderBy(p => p.PageIndex).ToList();
                    newRightPages = newRightPages.OrderBy(p => p.PageIndex).ToList();

                    this.Invoke(new Action(() =>
                    {
                        if (this.IsDisposed || !this.IsHandleCreated) return;

                        // 将携带了高亮框的新数据重新绑定给控件
                        viewerLeft.SetPages(newLeftPages);
                        viewerRight.SetPages(newRightPages);

                        string msg = $"内存极速比对完成 (共 {maxPages} 页)：发现 {totalDiffCount} 处视觉差异";
                        AntdUI.Message.success(this, msg);
                    }));
                }
                catch (Exception ex)
                {
                    throw new Exception("比对出错：" + ex.Message);
                }
            });
            

        }

        private Bitmap RenderPdfPage(string filePath, int pageIndex, float dpi)
        {
            try
            {
                using (var doc = PdfDocument.Load(filePath))
                {
                    if (pageIndex >= doc.PageCount)
                        return null;
                    var pageSize = doc.PageSizes[pageIndex];
                    int width = (int)(pageSize.Width / 72.0 * dpi);
                    int height = (int)(pageSize.Height / 72.0 * dpi);
                    return (Bitmap)doc.Render(pageIndex, width, height, (int)dpi, (int)dpi, true);
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// 【新封装】单独加载 PDF 并显示在指定的 Viewer 上，不进行比对
        /// </summary>
        /// <param name="filePath">PDF文件路径</param>
        /// <param name="targetViewer">要显示的目标控件 (viewerLeft 或 viewerRight)</param>
        public void DisplayPdfOnViewer(string filePath, PdfViewerControl targetViewer)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                AntdUI.Message.warn(this, "文件路径无效");
                return;
            }

            // 1. 设置文件名 (用于悬浮窗标题显示)
            //targetViewer.FileName = Path.GetFileName(filePath);

            // 2. 清理旧资源，防止内存泄漏
            targetViewer.ClearResources();

            // 3. 开启后台线程处理渲染，避免卡顿 UI
            Task.Run(() =>
            {
                try
                {
                    float dpi = 150f; // 渲染清晰度
                    var pagesData = new List<PageDiffData>();

                    // 使用 using 块一次性打开文档，性能比循环中反复 Load 高得多
                    using (var doc = PdfDocument.Load(filePath))
                    {
                        //int pageCount = GetPageCount(filePath);
                        int pageCount = doc.PageCount;
                        // 限制页数，防止过大文件撑爆内存
                        if (pageCount > 100)
                            throw new Exception("文档过大 (>100页)，请拆分后上传");

                        for (int i = 0; i < pageCount; i++)
                        {
                            // 渲染当前页
                            var img = RenderPageFromDoc(doc, i, dpi);

                            if (img != null)
                            {
                                pagesData.Add(new PageDiffData
                                {
                                    PageIndex = i,
                                    Image = img,
                                    Regions = null // 单独显示时没有差异区域
                                });
                            }
                        }
                    }

                    // 4. 回到主线程更新 UI
                    this.Invoke(new Action(() =>
                    {
                        if (this.IsDisposed || !this.IsHandleCreated) return; // 增加此行防御
                        targetViewer.SetPages(pagesData);
                        // AntdUI.Message.success(this, $"已加载: {Path.GetFileName(filePath)}");
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        AntdUI.Message.error(this, "PDF 加载失败: " + ex.Message);
                    }));
                }
            });
        }

        /// <summary>
        /// 【辅助方法】从已打开的 PdfDocument 对象中渲染指定页
        /// (比原来的 RenderPdfPage 更高效，因为不需要重复 IO)
        /// </summary>
        private Bitmap RenderPageFromDoc(PdfDocument doc, int pageIndex, float dpi)
        {
            try
            {
                var pageSize = doc.PageSizes[pageIndex];
                int width = (int)(pageSize.Width / 72.0 * dpi);
                int height = (int)(pageSize.Height / 72.0 * dpi);

                return (Bitmap)doc.Render(pageIndex, width, height, (int)dpi, (int)dpi, true);
            }
            catch
            {
                return null;
            }
        }

        private async void UploadButton_Click(object sender, EventArgs e)
        {

            await RunAsync(UploadButton, async () =>
            {
                using (var ofd = new OpenFileDialog { Filter = "PDF Files|*.pdf" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        viewerRight.SetPages(null);

                        //var verTag = $"v1.{versionList.Count}";
                        var newVer = new DocVersion
                        {
                            //VersionTag = verTag,
                            FileName = Path.GetFileName(ofd.FileName),
                            FilePath = ofd.FileName,
                            RawTime = DateTime.Now,
                            UploadTime = DateTime.Now.ToString("MM-dd HH:mm")
                        };
                        DisplayPdfOnViewer(newVer.FilePath, viewerRight);
                        DocNameInput.Text = newVer.FileName;
                        _currentLocalFilePath = newVer.FilePath;
                        Bitmap firstPageImage = RenderPdfPage(newVer.FilePath, 0, 300f); // 150f或300f 渲染清晰度

                        if (firstPageImage != null)
                        {
                            // 调用 Tesseract 提取器
                            var extractedData = TesseractExtractor.ExtractFromImage(firstPageImage);

                            // UI 赋值 (注意如果是在 Task 里，需要 Invoke 回主线程)
                            this.Invoke(new Action(() =>
                            {
                                if (!string.IsNullOrEmpty(extractedData.DocumentNo))
                                {
                                    // 3. UI 赋值
                                    DocumentNoInput.Text = extractedData.DocumentNo;
                                    ConstructionNoInput.Text = extractedData.ConstructionNo;
                                    ProcessDescInput.Text = extractedData.ProcessName;
                                    AntdUI.Message.success(this, $"提取文件编号: {extractedData.DocumentNo}");
                                }
                            }));
                        }
                        AntdUI.Message.success(this, $"上传成功：{newVer.FileName}");
                    }
                }
            });

        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {
                // 1. 从界面的 TextBox 等控件获取用户确认后的数据
                // (假设界面控件命名如下，您可以根据实际情况替换)
                string documentNo = DocumentNoInput.Text.Trim();
                string constructionNo = ConstructionNoInput.Text.Trim();
                string processName = ProcessDescInput.Text.Trim();
                string remark = RemarkInput.Text.Trim();

                string localFilePath = _currentLocalFilePath;

                // 2. 基础防呆校验
                if (string.IsNullOrEmpty(localFilePath) || !File.Exists(localFilePath))
                    throw new Exception("请先选择并上传有效的 PDF 文件！");
                if (string.IsNullOrEmpty(documentNo) || string.IsNullOrEmpty(constructionNo))
                    throw new Exception("文件编号和图纸号不能为空！");
                var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);

                var doc = await _wiDocumentService.GetByDocumentNoAsync(documentNo);
                if (doc != null)
                    throw new Exception($"当前文件编号{documentNo}已存在，请勿重复上传！");


                // 构建之前生成的数据库实体对象
                var newDoc = new WiDocumentCreateDto
                {
                    FactoryId = factory.Id,
                    FactoryCode = factory.FactoryCode,
                    DocumentNo = documentNo,
                    ConstructionNo = constructionNo,
                    ProcessName = processName,
                    DocVersion = documentNo.LastIndexOf('-') != -1 ? documentNo[(documentNo.LastIndexOf('-') + 1)..] : string.Empty,
                    DocumentName = Path.GetFileName(localFilePath), // 原始文件名
                    Status = "Pending",                             // 初始状态为待审核
                    IsActive = false,                               // 待审核状态下不作为有效版本
                    Remark = remark,
                    CreatedBy = AppSession.CurrentUser.EmployeeId
                };

                await _wiDocumentService.UploadAndSaveAsync(localFilePath, newDoc);

                await LoadConstructionNoAsync();


            }, confirmMsg: "即将更新最新WI文件，是否继续？", successMsg: "保存成功！");
        }

        private async void ConstructionNoSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {

            if (e.Value == null)
                return;
            await RunAsync(async () =>
            {
                DocumentNoSelect.Items.Clear();
                var documentNos = await _wiDocumentService.GetListByConstructionAsync(((MenuItem)e.Value).Name);
                if (documentNos.Any())
                {
                    DocumentNoSelect.Items.AddRange(documentNos.Select(x => x.DocumentNo).Distinct().Select(x => new MenuItem()
                    {
                        Name = x,
                        Text = x
                    }).ToArray());
                }
            });
        }

        public async Task LoadPdfFromServerToMemoryAsync(string serverRelativePath, PdfViewerControl targetViewer)
        {
            try
            {
                string apiBaseUrl = _apiSettings.BaseUrl;
                string fileUrl = apiBaseUrl + serverRelativePath.TrimStart('/');

                using (HttpClient client = new HttpClient())
                {
                    // 1. 直接将服务器文件下载为字节数组（都在内存中发生）
                    byte[] fileBytes = await client.GetByteArrayAsync(fileUrl);

                    // 2. 将字节数组转换为内存流
                    // 注意：不要在这里加 using 释放 MemoryStream，因为 PdfiumViewer 渲染期间需要一直持有这个流
                    MemoryStream pdfStream = new MemoryStream(fileBytes);

                    // 3. 调用新的从 Stream 渲染的方法
                    DisplayPdfFromStream(pdfStream, targetViewer);
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, "从服务器加载文件时出错: " + ex.Message);
            }
        }

        public void DisplayPdfFromStream(Stream pdfStream, PdfViewerControl targetViewer)
        {
            if (pdfStream == null || pdfStream.Length == 0)
            {
                AntdUI.Message.error(this, "PDF 文件流无效");
                return;
            }

            targetViewer.ClearResources();

            Task.Run(() =>
            {
                try
                {
                    float dpi = 150f;
                    var pagesData = new List<PageDiffData>();

                    // 【核心区别】：从内存流中加载 PDF
                    using (var doc = PdfDocument.Load(pdfStream))
                    {
                        int pageCount = doc.PageCount;
                        if (pageCount > 100)
                            throw new Exception("文档过大 (>100页)，请拆分");

                        for (int i = 0; i < pageCount; i++)
                        {
                            var img = RenderPageFromDoc(doc, i, dpi);
                            if (img != null)
                            {
                                pagesData.Add(new PageDiffData
                                {
                                    PageIndex = i,
                                    Image = img,
                                    Regions = null
                                });
                            }
                        }
                    }

                    // PDF 渲染完毕，可以安全地释放内存流了
                    pdfStream.Dispose();

                    this.Invoke(new Action(() =>
                    {
                        targetViewer.SetPages(pagesData);
                    }));
                }
                catch (Exception ex)
                {
                    pdfStream.Dispose(); // 发生异常也要记得释放流
                    this.Invoke(new Action(() =>
                    {
                        AntdUI.Message.error(this, "PDF 渲染失败: " + ex.Message);
                    }));
                }
            });
        }

        private async void DocumentNoSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (e.Value == null)
                return;

            await RunAsync(async () => 
            {
                if (string.IsNullOrWhiteSpace(((MenuItem)e.Value).Name))
                    throw new Exception("请重新选择文件编号！");
                var doc = await _wiDocumentService.GetByDocumentNoAsync(((MenuItem)e.Value).Name);
                if (doc != null && !string.IsNullOrEmpty(doc.DocumentPath))
                {
                    var pathObj = Newtonsoft.Json.Linq.JObject.Parse(doc.DocumentPath);

                    // 2. 获取值并明确转为标准的 string 字符串 (兼容 filePath 和 FilePath 大小写)
                    string serverPath = pathObj["filePath"]?.ToString() ?? pathObj["FilePath"]?.ToString();
                    await LoadPdfFromServerToMemoryAsync(serverPath, viewerLeft);
                }
                else
                    throw new Exception($"未获取到文件编号{((MenuItem)e.Value).Name}的文件，请重新上传！");
            });
        }
    }

    // ==========================================
    // 数据类：单页数据 (移除文字高亮)
    // ==========================================
    public class PageDiffData
    {
        public int PageIndex
        {
            get; set;
        }
        public Bitmap Image
        {
            get; set;
        }
        public List<DiffRegion> Regions
        {
            get; set;
        }
    }

    // ==========================================
    // 多页 Viewer：支持垂直连续滚动
    // ==========================================
    public class PdfViewerControl : System.Windows.Forms.Panel
    {
        private FlowLayoutPanel _flowPanel;
        private List<PageDiffData> _pagesData = new List<PageDiffData>();
        private float _zoom = 1.0f;

        // 记录悬浮窗需要的上下文
        private DiffRegion _hoveredRegion;
        private PagePictureBox _hoveredPictureBox;

        private DiffPopup _popupWindow;

        public float Zoom => _zoom;

        public PdfViewerControl()
        {
            this.AutoScroll = true;
            this.DoubleBuffered = true;
            this.BackColor = SystemColors.AppWorkspace;

            _flowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = SystemColors.AppWorkspace,
                Padding = new Padding(10),
                Margin = new Padding(0)
            };

            this.Controls.Add(_flowPanel);

            // 【新增】初始化悬浮窗
            _popupWindow = new DiffPopup();

            // 鼠标点击或进入时获取焦点，确保滚轮可用
            this.MouseEnter += (s, e) => this.Focus();
            this.Click += (s, e) => this.Focus();
            _flowPanel.Click += (s, e) => this.Focus();
            _flowPanel.MouseEnter += (s, e) => this.Focus();
        }

        // 新增：资源清理方法 (防止内存泄漏)
        // 销毁时清理悬浮窗

        /// <summary>
        /// 获取当前控件中已加载的所有页面数据
        /// </summary>
        public List<PageDiffData> GetPagesData()
        {
            return _pagesData;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _popupWindow?.Dispose();
                ClearResources(); // 必须加上这行，清理内部存储的所有 Bitmap 和 ImageSnippet
            }
            base.Dispose(disposing);
        }

        public void ClearResources()
        {
            _flowPanel.Controls.Clear();
            if (_pagesData != null)
            {
                foreach (var page in _pagesData)
                {
                    page.Image?.Dispose();
                    if (page.Regions != null)
                    {
                        foreach (var r in page.Regions)
                        {
                            r.OldImageSnippet?.Dispose();
                            r.NewImageSnippet?.Dispose();
                        }
                    }
                }
                _pagesData.Clear();
            }
        }

        public void SetPages(List<PageDiffData> pages)
        {
            _flowPanel.Controls.Clear();
            _pagesData = pages;
            _zoom = 1.0f;

            if (pages == null || pages.Count == 0)
            {
                _flowPanel.ResumeLayout();
                return;
            }

            // 初始适应宽度
            if (pages[0].Image != null)
            {
                // 减去滚动条的大致宽度 (25) 以防出现横向滚动条
                float fitZoom = (float)(this.ClientSize.Width - 30) / pages[0].Image.Width;
                _zoom = fitZoom > 1.0f ? 1.0f : fitZoom;
            }

            // 动态创建每一页的 PictureBox
            foreach (var page in pages)
            {
                var pb = new PagePictureBox(page);
                pb.Margin = new Padding(0, 0, 0, 10);

                // 绑定事件
                pb.MouseMove += (s, e) => OnPageMouseMove(pb, e);
                // 关键：让子控件的滚轮事件冒泡给父容器
                pb.MouseWheel += (s, e) => this.OnMouseWheel(e);

                UpdatePageSize(pb);
                _flowPanel.Controls.Add(pb);
            }
            _flowPanel.ResumeLayout();
        }

        private void UpdatePageSize(PagePictureBox pb)
        {
            if (pb.PageData.Image == null)
            {
                pb.Size = new Size(100, 100);
                return;
            }
            int w = (int)(pb.PageData.Image.Width * _zoom);
            int h = (int)(pb.PageData.Image.Height * _zoom);
            pb.Size = new Size(w, h);
            pb.Zoom = _zoom; // 传递缩放给控件内部
            pb.Invalidate();
        }

        public void ScrollToRatio(float ratio)
        {
            // 简单的同步滚动实现
            int val = (int)(this.VerticalScroll.Maximum * ratio);
            // 范围检查
            val = Math.Max(this.VerticalScroll.Minimum, Math.Min(val, this.VerticalScroll.Maximum));
            this.VerticalScroll.Value = val;
        }

        public float GetScrollRatio()
        {
            if (this.VerticalScroll.Maximum == 0)
                return 0;
            return (float)this.VerticalScroll.Value / this.VerticalScroll.Maximum;
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                // ==========================
                // 【关键修复 3】定点缩放逻辑
                // ==========================
                if (_pagesData.Count == 0)
                    return;

                float oldZoom = _zoom;

                // 1. 计算缩放比例
                if (e.Delta > 0)
                    _zoom *= 1.2f;
                else
                    _zoom /= 1.2f;

                // 限制缩放范围
                _zoom = Math.Max(0.1f, Math.Min(_zoom, 5.0f));

                if (Math.Abs(oldZoom - _zoom) < 0.001f)
                    return;

                // 2. 记录当前鼠标相对于内容的相对位置 (0.0 - 1.0)
                // 获取当前视口左上角在整个内容中的偏移量
                Point scrollPos = this.AutoScrollPosition;
                // AutoScrollPosition 的值是负的，代表内容偏移，我们取绝对值
                float absoluteX = -scrollPos.X + e.X;
                float absoluteY = -scrollPos.Y + e.Y;

                float ratioX = absoluteX / (_flowPanel.Width);
                float ratioY = absoluteY / (_flowPanel.Height);

                // 3. 调整尺寸
                _flowPanel.SuspendLayout();
                foreach (Control c in _flowPanel.Controls)
                {
                    if (c is PagePictureBox pb)
                        UpdatePageSize(pb);
                }
                _flowPanel.ResumeLayout(true);

                // 4. 恢复滚动位置 (保持鼠标指向的点不变)
                // 新的内容尺寸
                float newContentW = _flowPanel.Width;
                float newContentH = _flowPanel.Height;

                int newScrollX = (int)(newContentW * ratioX - e.X);
                int newScrollY = (int)(newContentH * ratioY - e.Y);

                // 设置 AutoScrollPosition 需要正值
                this.AutoScrollPosition = new Point(Math.Max(0, newScrollX), Math.Max(0, newScrollY));

                ((HandledMouseEventArgs)e).Handled = true;
            }
            else
            {
                // 普通滚动：由 AutoScroll 属性自动处理
                base.OnMouseWheel(e);
            }
        }

        private void OnPageMouseMove(PagePictureBox pb, System.Windows.Forms.MouseEventArgs e)
        {
            var region = pb.FindRegionAt(e.Location);

            // ==========================
            // 【关键修复】使用独立窗口显示悬浮层
            // ==========================
            if (region != null)
            {
                // 将鼠标坐标转换为屏幕坐标
                Point screenPos = pb.PointToScreen(e.Location);
                _popupWindow.ShowDiff(region, pb.PageData.PageIndex, screenPos);
            }
            else
            {
                _popupWindow.Hide();
            }

            // 处理红框高亮
            if (region != _hoveredRegion || pb != _hoveredPictureBox)
            {
                if (_hoveredPictureBox != null && !_hoveredPictureBox.IsDisposed)
                    _hoveredPictureBox.SetHoverRegion(null);

                _hoveredRegion = region;
                _hoveredPictureBox = pb;

                if (_hoveredPictureBox != null)
                    _hoveredPictureBox.SetHoverRegion(_hoveredRegion);
            }
        }
    }
    // ==========================================
    // 【新增】独立的悬浮窗 Form
    // 能够显示在屏幕最顶层，不会被父控件裁剪
    // ==========================================
    public class DiffPopup : Form
    {
        private DiffRegion _region;
        private int _pageIndex;

        public DiffPopup()
        {
            this.FormBorderStyle = FormBorderStyle.None; // 无边框
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(1, 1);
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.TopMost = true; // 确保在最上层
        }

        // 防止窗口抢夺焦点
        protected override bool ShowWithoutActivation => true;

        public void ShowDiff(DiffRegion region, int pageIndex, Point mouseScreenPos)
        {
            if (region == null || region.NewImageSnippet == null)
                return;

            _region = region;
            _pageIndex = pageIndex;

            // 计算尺寸
            int maxW = 300;
            float scale = Math.Min(1.0f, (float)maxW / region.NewImageSnippet.Width);
            int displayW = (int)(region.NewImageSnippet.Width * scale);
            int displayH = (int)(region.NewImageSnippet.Height * scale);

            int totalW = displayW * 2 + 20;
            int totalH = displayH + 30;
            this.Size = new Size(totalW, totalH);

            // 计算位置 (鼠标右下方)
            int x = mouseScreenPos.X + 20;
            int y = mouseScreenPos.Y + 20;

            // 边界检测：防止超出屏幕右侧或下侧
            Screen screen = Screen.FromPoint(mouseScreenPos);
            if (x + totalW > screen.WorkingArea.Right)
                x = screen.WorkingArea.Right - totalW; // 直接贴住右边缘
            if (y + totalH > screen.WorkingArea.Bottom)
                y = screen.WorkingArea.Bottom - totalH; // 直接贴住下边缘

            this.Location = new Point(x, y);

            if (!this.Visible)
                this.Show();
            this.Invalidate(); // 触发重绘
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_region == null || _region.NewImageSnippet == null)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 绘制边框
            g.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);

            // 标题
            g.DrawString($"页 {_pageIndex + 1} 变更前&变更后", new Font("Microsoft YaHei UI", 9), Brushes.Black, 5, 5);

            // 图像尺寸
            int displayW = (this.Width - 20) / 2;
            int displayH = this.Height - 30;

            // 绘制旧图和新图
            g.DrawImage(_region.OldImageSnippet, 5, 25, displayW, displayH);
            g.DrawImage(_region.NewImageSnippet, 5 + displayW + 10, 25, displayW, displayH);

            // 绘制红框 (在新图上)
            float sX = (float)displayW / _region.NewImageSnippet.Width;
            float sY = (float)displayH / _region.NewImageSnippet.Height;
            var hl = _region.RelativeRect;

            using (var p = new Pen(Color.Red, 1))
            {
                g.DrawRectangle(p, 5 + displayW + 10 + hl.X * sX, 25 + hl.Y * sY, hl.Width * sX, hl.Height * sY);
            }
        }
    }

    // ==========================================
    // 每一页的显示控件 (封装绘制逻辑)
    // ==========================================
    // ==========================================
    // 修改后的 PictureBox：移除了悬浮窗绘制逻辑
    // ==========================================
    public class PagePictureBox : PictureBox
    {
        public PageDiffData PageData
        {
            get; private set;
        }
        public float Zoom { get; set; } = 1.0f;
        private DiffRegion _hoverRegion;

        public PagePictureBox(PageDiffData data)
        {
            PageData = data;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = data.Image;
            this.BackColor = Color.White;
        }

        public void SetHoverRegion(DiffRegion region)
        {
            _hoverRegion = region;
            Invalidate();
        }

        public DiffRegion FindRegionAt(Point location)
        {
            if (PageData.Regions == null)
                return null;
            int imgX = (int)(location.X / Zoom);
            int imgY = (int)(location.Y / Zoom);
            return PageData.Regions.FirstOrDefault(r => r.Rect.Contains(imgX, imgY));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;

            if (Math.Abs(Zoom - 1.0f) < 0.01f)
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.None;
            }
            else
            {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            }

            if (PageData.Regions != null)
            {
                using (var brush = new SolidBrush(Color.FromArgb(30, 255, 0, 0)))
                using (var pen = new Pen(Color.Red, 2))
                using (var hBrush = new SolidBrush(Color.FromArgb(60, 255, 0, 0)))
                {
                    foreach (var region in PageData.Regions)
                    {
                        var r = new Rectangle((int)(region.Rect.X * Zoom), (int)(region.Rect.Y * Zoom), (int)(region.Rect.Width * Zoom), (int)(region.Rect.Height * Zoom));

                        g.FillRectangle(brush, r);
                        g.DrawRectangle(pen, r);

                        if (region == _hoverRegion)
                        {
                            g.FillRectangle(hBrush, r);
                        }
                    }
                }
            }

            // 【已移除】原有的 DrawDiffPopup 调用已删除，改用独立的 DiffPopup 窗口
        }
    }

    public class DiffRegion
    {
        public Rectangle Rect
        {
            get; set;
        }

        public PdfDiffEngine.DiffType Type
        {
            get; set;
        }
        public Rectangle RelativeRect
        {
            get; set;
        }
        public Bitmap OldImageSnippet
        {
            get; set;
        }
        public Bitmap NewImageSnippet
        {
            get; set;
        }
    }

    public class DocVersion
    {
        public string Id
        {
            get; set;
        }
        public string VersionTag
        {
            get; set;
        }
        public string FileName
        {
            get; set;
        }
        public string FilePath
        {
            get; set;
        }
        public string UploadTime
        {
            get; set;
        }
        public DateTime RawTime
        {
            get; set;
        }
    }


    // ==========================================
    // 核心比对算法引擎
    // 包含：像素级差异检测 + 智能区域合并优化
    // ==========================================
    public static class PdfDiffEngine
    {
        public enum DiffType
        {
            Text,   // 文本/线条 (蓝色标注)
            Image,  // 图片/色块 (红色标注)
            Unknown
        }

        /// <summary>
        /// 对比两张图片，返回合并优化后的差异区域列表
        /// </summary>
        public static List<DiffRegion> DetectAndOptimize(Bitmap bmpA, Bitmap bmpB)
        {
            // 1. 高精度像素检测 (网格 5px)
            var rawRegions = DetectPixelDiffs(bmpA, bmpB);

            // 2. 噪点过滤 (去除孤立的微小差异点)
            var filteredRegions = RemoveNoiseRegions(rawRegions);

            // 3. 智能合并
            // 【关键修改】将合并阈值从 40 降低到 10
            // 原理解析：
            // 之前设为 40px，意味着只要两个差异点距离小于 40px 就会连成一片。
            // 现在的 10px 意味着：只要中间有超过 10px 的无差异区域（大约半个汉字的宽度），
            // 红框就会自动断开。这样 "图纸示意" 这种无差异的文字就不会被框进去了。
            var optimizedRegions = MergeCloseRegions(filteredRegions, mergeThreshold: 10);

            // 4. 分类与截图
            ClassifyAndCapture(optimizedRegions, bmpA, bmpB);

            return optimizedRegions;
        }

        // 像素检测逻辑 (高精度版)
        private static List<DiffRegion> DetectPixelDiffs(Bitmap bmpA, Bitmap bmpB)
        {
            int width = Math.Min(bmpA.Width, bmpB.Width);
            int height = Math.Min(bmpA.Height, bmpB.Height);
            var regions = new List<DiffRegion>();

            int gridSize = 5;  // 【优化】网格从 10 缩小到 5，边缘更细腻
            int threshold = 40; // 【优化】提高阈值，忽略轻微的 JPEG 压缩噪点

            var rect = new Rectangle(0, 0, width, height);
            var dataA = bmpA.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var dataB = bmpB.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                int stride = dataA.Stride;
                int bytes = Math.Abs(stride) * height;
                byte[] bufferA = new byte[bytes];
                byte[] bufferB = new byte[bytes];
                Marshal.Copy(dataA.Scan0, bufferA, 0, bytes);
                Marshal.Copy(dataB.Scan0, bufferB, 0, bytes);

                for (int y = 0; y < height; y += gridSize)
                {
                    for (int x = 0; x < width; x += gridSize)
                    {
                        bool hasDiff = false;
                        int maxY = Math.Min(y + gridSize, height);
                        int maxX = Math.Min(x + gridSize, width);

                        // 步长为1，全扫描以提高小字检测率
                        for (int dy = y; dy < maxY; dy++)
                        {
                            int rowOffset = dy * stride;
                            for (int dx = x; dx < maxX; dx++)
                            {
                                int idx = rowOffset + dx * 4;
                                if (idx + 3 >= bytes)
                                    continue;

                                // 计算 RGB 差异 (忽略 Alpha)
                                int diff = Math.Abs(bufferA[idx] - bufferB[idx]) +
                                           Math.Abs(bufferA[idx + 1] - bufferB[idx + 1]) +
                                           Math.Abs(bufferA[idx + 2] - bufferB[idx + 2]);

                                if (diff > threshold)
                                {
                                    hasDiff = true;
                                    break;
                                }
                            }
                            if (hasDiff)
                                break;
                        }

                        if (hasDiff)
                        {
                            regions.Add(new DiffRegion { Rect = new Rectangle(x, y, gridSize, gridSize) });
                        }
                    }
                }
            }
            finally
            {
                bmpA.UnlockBits(dataA);
                bmpB.UnlockBits(dataB);
            }

            return regions;
        }

        // 【新增】噪点过滤：移除那些周围没有其他差异点的孤立区域
        private static List<DiffRegion> RemoveNoiseRegions(List<DiffRegion> regions)
        {
            if (regions.Count < 2)
                return regions;

            // 简单算法：如果一个 Region 距离所有其他 Region 都超过 20px，则视为噪点
            // 注意：这在 Region 数量很大时可能是性能瓶颈，可以优化为网格索引，但对于文档页面通常够用
            var result = new List<DiffRegion>();
            foreach (var r in regions)
            {
                bool hasNeighbor = false;
                foreach (var other in regions)
                {
                    if (r == other)
                        continue;
                    if (AreRectsClose(r.Rect, other.Rect, 20)) // 20px 内有邻居
                    {
                        hasNeighbor = true;
                        break;
                    }
                }
                if (hasNeighbor)
                    result.Add(r);
            }
            return result;
        }

        // 迭代合并距离相近的区域
        private static List<DiffRegion> MergeCloseRegions(List<DiffRegion> regions, int mergeThreshold)
        {
            if (regions.Count == 0)
                return regions;

            var rects = regions.Select(r => r.Rect).ToList();
            bool changed = true;

            while (changed)
            {
                changed = false;
                for (int i = 0; i < rects.Count; i++)
                {
                    for (int j = i + 1; j < rects.Count; j++)
                    {
                        if (AreRectsClose(rects[i], rects[j], mergeThreshold))
                        {
                            rects[i] = Rectangle.Union(rects[i], rects[j]);
                            rects.RemoveAt(j);
                            j--;
                            changed = true;
                        }
                    }
                }
            }

            return rects.Select(r => new DiffRegion { Rect = r }).ToList();
        }

        private static bool AreRectsClose(Rectangle r1, Rectangle r2, int tolerance)
        {
            Rectangle expanded = r1;
            expanded.Inflate(tolerance, tolerance);
            return expanded.IntersectsWith(r2);
        }

        // 【核心新增】分类与截图
        private static void ClassifyAndCapture(List<DiffRegion> regions, Bitmap bmpA, Bitmap bmpB)
        {
            int w = Math.Min(bmpA.Width, bmpB.Width);
            int h = Math.Min(bmpA.Height, bmpB.Height);
            var rect = new Rectangle(0, 0, w, h);

            var dataA = bmpA.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var dataB = bmpB.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                int stride = dataA.Stride;
                byte[] bufferA = new byte[Math.Abs(stride) * h];
                byte[] bufferB = new byte[Math.Abs(stride) * h];
                Marshal.Copy(dataA.Scan0, bufferA, 0, bufferA.Length);
                Marshal.Copy(dataB.Scan0, bufferB, 0, bufferB.Length);

                foreach (var region in regions)
                {
                    // 1. 分析类型 (Text vs Image)
                    // 基于差异密度：文字通常是稀疏的线条，图片通常是密集的色块
                    region.Type = AnalyzeRegionType(region.Rect, bufferA, bufferB, stride, 40);

                    // 2. 截图
                    // 根据类型调整 Padding，文字可以少一点上下文，图片可能需要多一点
                    int padding = (region.Type == DiffType.Text) ? 30 : 50;

                    Rectangle c = region.Rect;
                    c.Inflate(padding, padding);
                    c.Intersect(rect);

                    if (c.Width > 0 && c.Height > 0)
                    {
                        // 注意：Clone 需要 Unlock 状态吗？通常 Bitmap.Clone 需要 GDI+ 锁，
                        // 但我们在 LockBits 外部操作 Bitmap 对象本身的方法比较安全，或者先解锁再 Clone。
                        // 为了简单起见，这里我们只计算 Rect，截图留到最后或者这里解锁-截图-再锁太慢。
                        // 策略：记录截图区域，循环结束后再统一截图。
                        // 由于代码结构限制，我们在循环内无法安全调用 bmp.Clone 如果 bmp 被 Lock 了。
                        // 所以我们这里只做分析。截图逻辑移到 finally 块之后。
                    }
                }
            }
            finally
            {
                bmpA.UnlockBits(dataA);
                bmpB.UnlockBits(dataB);
            }

            // 统一执行截图 (安全操作)
            foreach (var region in regions)
            {
                int padding = (region.Type == DiffType.Text) ? 30 : 50;
                Rectangle c = region.Rect;
                c.Inflate(padding, padding);
                c.Intersect(rect);

                if (c.Width > 0 && c.Height > 0)
                {
                    if (c.Right <= bmpA.Width && c.Bottom <= bmpA.Height) 
                    {
                        region.OldImageSnippet = bmpA.Clone(c, bmpA.PixelFormat);
                        region.NewImageSnippet = bmpB.Clone(c, bmpB.PixelFormat);
                        region.RelativeRect = new Rectangle(region.Rect.X - c.X, region.Rect.Y - c.Y, region.Rect.Width, region.Rect.Height);

                    }
                }
            }
        }

        // 分析区域类型：计算差异像素的密度
        private static DiffType AnalyzeRegionType(Rectangle r, byte[] bufA, byte[] bufB, int stride, int threshold)
        {
            int diffPixelCount = 0;
            int totalPixels = r.Width * r.Height;
            if (totalPixels == 0)
                return DiffType.Text;

            for (int y = r.Y; y < r.Bottom; y++)
            {
                int rowOff = y * stride;
                for (int x = r.X; x < r.Right; x++)
                {
                    int i = rowOff + x * 4;
                    if (i + 2 >= bufA.Length)
                        continue;

                    int diff = Math.Abs(bufA[i] - bufB[i]) +
                               Math.Abs(bufA[i + 1] - bufB[i + 1]) +
                               Math.Abs(bufA[i + 2] - bufB[i + 2]);

                    if (diff > threshold)
                    {
                        diffPixelCount++;
                    }
                }
            }

            double density = (double)diffPixelCount / totalPixels;

            // 启发式阈值：
            // 如果差异像素超过区域面积的 50%，通常是图片替换或大面积高亮 -> Image
            // 否则通常是文字修改 -> Text
            return density > 0.5 ? DiffType.Image : DiffType.Text;
        }
    }

    public class TesseractExtractor
    {
        public class ProcessCardData
        {
            public string DocumentNo { get; set; }     // 文件编号
            public string ConstructionNo { get; set; } // 图纸号
            public string ProcessName { get; set; }    // 工艺名称
        }

        // 定义我们自己的文本块结构，统一处理坐标
        public class TextBlock
        {
            public string Text { get; set; }
            public Rectangle BoundingBox { get; set; }
        }

        private static Dictionary<string, string> OcrCorrections = new Dictionary<string, string>();

        /// <summary>
        /// 提供给外部（如 WinForm 加载时从数据库读取后）设置纠错字典的方法
        /// </summary>
        public static void SetCorrections(Dictionary<string, string> dict)
        {
            if (dict != null)
            {
                OcrCorrections = dict;
            }
        }

        /// <summary>
        /// 后处理文本清洗，替换 OCR 常犯的错误
        /// </summary>
        private static string CorrectOcrText(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            foreach (var kvp in OcrCorrections)
            {
                if (text.Contains(kvp.Key))
                {
                    text = text.Replace(kvp.Key, kvp.Value);
                }
            }
            return text;
        }

        /// <summary>
        /// 传入你通过 PdfiumViewer 渲染出来的 Bitmap 进行识别
        /// </summary>
        public static ProcessCardData ExtractFromImage(Bitmap pdfPageImage)
        {
            var result = new ProcessCardData();

            // 检查语言包是否存在
            string tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            if (!Directory.Exists(tessdataPath) || !File.Exists(Path.Combine(tessdataPath, "chi_sim.traineddata")))
            {
                throw new Exception("找不到 tessdata 语言包目录或文件，请确保已设置为【如果较新则复制】。");
            }

            try
            {
                // 初始化 Tesseract 引擎，指定语言为中文简体+英文 (chi_sim+eng)
                using (var engine = new TesseractEngine(tessdataPath, "chi_sim+eng", EngineMode.Default))
                {
                    // 【关键修复】：将 Bitmap 存入内存流，转换为 byte[]，避开 PixConverter
                    byte[] imgBytes;
                    using (var ms = new MemoryStream())
                    {
                        // 保存为 BMP 格式速度极快且无损
                        pdfPageImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        imgBytes = ms.ToArray();
                    }

                    // Tesseract 提供的方法：直接从内存字节加载 Pix
                    using (var pix = Pix.LoadFromMemory(imgBytes))
                    {
                        using (var page = engine.Process(pix))
                        {
                            var blocks = new List<TextBlock>();

                            // 获取迭代器，以"词(Word)"为单位遍历识别结果和坐标
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                                do
                                {
                                    if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out Rect bounds))
                                    {
                                        string text = iter.GetText(PageIteratorLevel.Word)?.Trim();
                                        if (!string.IsNullOrWhiteSpace(text))
                                        {
                                            blocks.Add(new TextBlock
                                            {
                                                Text = text,
                                                BoundingBox = new Rectangle(bounds.X1, bounds.Y1, bounds.Width, bounds.Height)
                                            });
                                        }
                                    }
                                } while (iter.Next(PageIteratorLevel.Word));
                            }

                            // 利用空间关系提取数据
                            //result.DocumentNo = FindTextBelowAnchor(blocks, "文件编号");
                            //result.ConstructionNo = FindTextBelowAnchor(blocks, "图纸号");
                            //result.ProcessName = FindTextBelowAnchor(blocks, "工艺名称");
                            // 利用空间关系提取数据，并经过【纠错字典】过滤
                            result.DocumentNo = CorrectOcrText(FindTextBelowAnchor(blocks, "文件编号", removeChinese: true));
                            result.ConstructionNo = CorrectOcrText(FindTextBelowAnchor(blocks, "图纸号", removeChinese: true));
                            result.ProcessName = CorrectOcrText(FindTextBelowAnchor(blocks, "工艺名称"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获异常
                Console.WriteLine($"Tesseract OCR识别失败: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 核心空间检索算法：寻找锚点下方指定区域内的文本
        /// </summary>
        private static string FindTextBelowAnchor(
                   List<TextBlock> blocks,
                   string anchorKeyword,
                   bool removeChinese = false, // 【新增参数】是否强制移除中文字
                   int searchHeight = 80, // 【关键修改】：适配 300 DPI，图像变大，向下搜索范围翻倍（原来是30）
                   int xToleranceLeft = 40, // 适配 300 DPI，左侧容差翻倍
                   int xToleranceRight = 300) // 适配 300 DPI，右侧容差翻倍，兼容超长编号
        {
            // 1. 寻找锚点文字 (修复 Tesseract 将中文字符拆分成多个独立单字 Block 的问题)
            Rectangle? anchorBox = null;
            string cleanKeyword = anchorKeyword.Replace(" ", "");

            for (int i = 0; i < blocks.Count; i++)
            {
                string combinedText = "";
                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;

                // 向后拼接最多 10 个词
                for (int j = i; j < Math.Min(i + 10, blocks.Count); j++)
                {
                    combinedText += blocks[j].Text.Replace(" ", "");

                    minX = Math.Min(minX, blocks[j].BoundingBox.Left);
                    minY = Math.Min(minY, blocks[j].BoundingBox.Top);
                    maxX = Math.Max(maxX, blocks[j].BoundingBox.Right);
                    maxY = Math.Max(maxY, blocks[j].BoundingBox.Bottom);

                    if (combinedText.Contains(cleanKeyword))
                    {
                        // 【核心修复】：防止由于起点太靠前，导致把无关文字（如BizLink）也框进去产生巨型锚点。
                        // 这里判断：如果合并出来的文字长度，跟我们要找的关键字长度相差不大（允许带点标点），
                        // 才说明这是真正的关键字区域，而不是一整段文字包含了关键字。
                        if (combinedText.Length <= cleanKeyword.Length + 3)
                        {
                            anchorBox = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                        }

                        // 一旦包含了该关键字，说明匹配结束，强制跳出内层循环
                        break;
                    }
                }

                if (anchorBox != null) break;
            }

            if (anchorBox == null)
                return string.Empty;

            // 原点 (0,0) 在图片左上角
            int anchorBottomY = anchorBox.Value.Bottom;
            int anchorLeftX = anchorBox.Value.Left;
            int anchorRightX = anchorBox.Value.Right;

            // 2. 筛选出落在这个不可见“矩形框”内的所有文字
            var targetBlocks = blocks.Where(b =>
                // Y轴：文字的中心点在锚点下方 0 到 searchHeight 的范围内
                b.BoundingBox.Y + (b.BoundingBox.Height / 2) > anchorBottomY &&
                b.BoundingBox.Y + (b.BoundingBox.Height / 2) < anchorBottomY + searchHeight &&
                // X轴：允许左侧略微超出，右侧大幅超出（因为目标数据通常比表头长）
                b.BoundingBox.X + (b.BoundingBox.Width / 2) >= anchorLeftX - xToleranceLeft &&
                b.BoundingBox.X + (b.BoundingBox.Width / 2) <= anchorRightX + xToleranceRight
            )
            .OrderBy(b => b.BoundingBox.Left) // 按从左到右排序
            .ToList();

            if (!targetBlocks.Any())
                return string.Empty;
            // 3. 拼合内容
            string extractedText = string.Join("", targetBlocks.Select(b => b.Text)).Trim();

            // 4. 【新增清洗逻辑】：如果指定该字段不包含中文，则直接使用正则表达式过滤掉所有中文汉字
            if (removeChinese && !string.IsNullOrEmpty(extractedText))
            {
                // \u4e00-\u9fa5 是基本汉字在 Unicode 中的范围
                extractedText = Regex.Replace(extractedText, @"[\u4e00-\u9fa5]", "");
            }

            return extractedText;
        }
    }
}
