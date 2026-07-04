using SiliconLife.Collective;
using SiliconLife.Common;
using SiliconLife.Speedy;
using SiliconLife.Speedy;
using TravelCodeWikiWithAI.Cldr;
using TravelCodeWikiWithAI.Data;
using TravelCodeWikiWithAI.Data.OSM;
using TravelCodeWikiWithAI.Services;
using TravelCodeWikiWithAI.TCWTool;
using TravelCodeWikiWithAI.TravelCodeWikiWithAIWorkflow;

namespace TravelCodeWikiWithAI;

[PluginCapability(Capability.Network, Reason = "Access OSM API for geographic data queries and tile requests")]
[PluginCapability(Capability.FileIO, Reason = "Cache OSM API responses to local XML files and tile images")]
public class TravelCodeWikiWithAIPlugin : IPlugin
{
    public string Id => "com.siliconlife.travel-code-wiki";

    public string Version => "0.1.0";

    public string GetName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "旅游编码计划",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "旅遊編碼計劃",
        Language.JaJP => "旅行コード計画",
        Language.KoKR => "여행 코드 계획",
        Language.EsES or Language.EsMX => "Código de Viaje Wiki",
        Language.CsCZ => "Cestovní kódová Wiki",
        Language.DeDE or Language.DeAT or Language.DeCH or Language.DeLU or Language.DeLI => "Reise-Code-Wiki",
        _ => "Travel Code Wiki"
    };

    public string GetDescription(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "为 AI 集群提供 OSM 旅行数据查询和 MediaWiki 维基内容管理工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "為 AI 集群提供 OSM 旅行資料查詢和 MediaWiki 維基內容管理工具",
        Language.JaJP => "AIクラスターにOSM旅行データクエリとMediaWikiコンテンツ管理ツールを提供",
        Language.KoKR => "AI 클러스터에 OSM 여행 데이터 쿼리 및 MediaWiki 콘텐츠 관리 도구 제공",
        Language.EsES or Language.EsMX => "Proporciona herramientas de consulta de datos OSM y gestión de contenido MediaWiki para el clúster de IA",
        Language.CsCZ => "Poskytuje nástroje pro dotazy na OSM cestovní data a správu obsahu MediaWiki pro AI cluster",
        Language.DeDE or Language.DeAT or Language.DeCH or Language.DeLU or Language.DeLI => "Bietet OSM-Reisedaten-Abfrage und MediaWiki-Content-Management-Tools für den AI-Cluster",
        _ => "Provides OSM travel data query and MediaWiki content management tools for the AI cluster"
    };

    public string GetAuthor(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "天源垦骥",
        Language.ZhHK or Language.ZhMO or Language.ZhTW or Language.JaJP => "天源墾驥",
        Language.KoKR => "호시노 켄지",
        _ => "Hoshino Kennji"
    };

    public void OnLoad()
    {
        _wikiPublicationTick = new WikiPublicationTick();

        // 初始化 MediaWiki 发布服务（配置为空时禁用，不报错）
        _publishService = new MediaWikiPublishService();
        // TODO: 从项目配置中读取 MediaWiki 连接参数
        // _publishService.ApiUrl = "http://127.0.0.1/api.php";
        // _publishService.Username = "BotUser@BotName";
        // _publishService.Password = "bot_password"; 
		// 创建新的 SpeedyPack 数据包
        try
        {
            // 创建 SpeedyPack 数据包，使用插件ID作为文件名
            string packFilePath = SafePath.Combine(Environment.CurrentDirectory, "TravelCodeWiki.spk");
            _speedyPack = SpeedyPack.Open(packFilePath);
        }
        catch (Exception ex)
        {
            Debug.Log($"{Id} Failed to create SpeedyPack: {ex.Message}");
        }

        // 加载 CLDR 数据提供者（cldr.spk 已存在，直接打开只读）
        try
        {
            string cldrPackPath = "D:\\SiliconLifeCollective\\cldr\\cldr.spk";
            _cldrPack = SpeedyPack.Open(cldrPackPath, new SpeedyPackOptions { ReadOnly = true });
            _cldrProvider = new CldrDataProvider(_cldrPack);
            Debug.Log($"{Id} CLDR provider loaded");
        }
        catch (Exception ex)
        {
            Debug.Log($"{Id} CLDR provider not available: {ex.Message}");
            // 降级：SysTool 继续使用 PHPText.resx
        }

        // 注册插件自身的工具程序集，使 GeoLanguageTool 等工具可被硅基人调用
        // Register plugin's own tool assembly so tools like GeoLanguageTool are available to silicon beings
        try
        {
            ServiceLocator.Instance.RegisterToolAssembly(typeof(GeoLanguageTool).Assembly);
            Debug.Log($"{Id} Registered tool assembly");
        }
        catch (Exception ex)
        {
            Debug.Log($"{Id} Failed to register tool assembly: {ex.Message}");
        }

        // 注册插件类型到 ITypeRegistry（替代 AppDomain.CurrentDomain.GetAssemblies() 反射扫描）
        // Register plugin types to ITypeRegistry (replaces AppDomain.CurrentDomain.GetAssemblies() reflection scanning)
        try
        {
            var typeRegistry = ServiceLocator.Instance.TypeRegistry;
            var objectFactory = ServiceLocator.Instance.ObjectFactory;
            if (typeRegistry != null && objectFactory != null)
            {
                var asm = typeof(TravelCodeWikiWithAIPlugin).Assembly;
                typeRegistry.RegisterFromAssembly(asm, typeof(GeoDataNode));
                typeRegistry.RegisterFromAssembly(asm, typeof(WordBase));
                typeRegistry.RegisterType(typeof(LanguageData));
                objectFactory.RegisterAutoFactoryFromAssembly(asm, typeof(GeoDataNode));
                objectFactory.RegisterAutoFactoryFromAssembly(asm, typeof(WordBase));
                objectFactory.RegisterAutoFactory(typeof(LanguageData));
                Debug.Log($"{Id} Registered types and factories");
            }
            else
            {
                Debug.Log($"{Id} TypeRegistry/ObjectFactory not available, skipping registration");
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"{Id} Failed to register types/factories: {ex.Message}");
        }
    }

    private Thread? _startupThread;
    private SpeedyPack? _speedyPack;
    private SpeedyPack? _cldrPack;
    public static CldrDataProvider? _cldrProvider;
    public static GeoProject? _geoProject;
    public static MediaWikiPublishService? _publishService;

    public void OnStart()
    {
        // 注册工作流模板（在 OnStart 中注册，此时工作流引擎已初始化）
        try
        {
            var workflowEngine = ServiceLocator.Instance.GetService<WorkflowEngine>();
            if (workflowEngine != null)
            {
                workflowEngine.RegisterTemplate(TravelCodeWikiPublishWorkflow.CreateTemplate());
            }
            else
            {
                Debug.Log($"{Id} Workflow engine not available, skipping template registration");
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"{Id} Failed to register workflow template: {ex.Message}");
        }

        // 启动新线程，指向可修改的函数
        _startupThread = new Thread(StartupThreadFunction)
        {
            IsBackground = true,
            Name = "TravelCodeWiki"
        };
        _startupThread.Start();
    }

    /// <summary>
    /// 可修改的线程函数 - 您可以在此处添加自定义逻辑
    /// </summary>
    private void StartupThreadFunction()
    {
        // 等待 Curator 就绪
        while (SiliconBeingManager.GetCuratorBeing() == null)
        {
            Thread.Sleep(1000);
        }

        // 加载 GeoProject
        if (_speedyPack != null)
        {
            _geoProject = GeoDataBase.LoadFromPack<GeoProject>(_speedyPack, "geo/root");
        }

        if (_geoProject == null)
        {
            _geoProject = new GeoProject();
            _geoProject.Translation = new GeoTranslation(_geoProject);
            _geoProject.ExchangeRate = new ExchangeRate(_geoProject);
            _geoProject.World = new GeoWorld();
            _geoProject.APPdoc = new List<GeoWebApp>();
            _geoProject.WordTable = new GeoWordTable();
            TranslationDefaults.ApplyDefaults(_geoProject.Translation);
        }

        // 启用在线 OSM API
        OsmOnlineApiService.OK = true;
    }

    public void OnStop()
    {
        // 确保 SpeedyPack 被正确清理
        _cldrPack?.Dispose();
        _speedyPack?.Dispose();
    }

    public void OnUnload()
    {
        // 确保 SpeedyPack 被正确清理
        _cldrPack?.Dispose();
        _speedyPack?.Dispose();
    }
    
    private WikiPublicationTick _wikiPublicationTick;
}
