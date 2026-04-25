// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;
using SiliconLife.Default.ChineseHistorical;

namespace SiliconLife.Default;

/// <summary>
/// Spanish (Spain) localization implementation
/// </summary>
public class EsES : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "es-ES";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "Español (España)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "¡Bienvenido a Silicon Life Collective!";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "Silicon Life Collective";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "Cerrando...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "Archivo de configuración corrupto, usando configuración predeterminada";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "Archivo de configuración no encontrado, creado con valores predeterminados";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "No se puede conectar al servicio de IA, verifique que Ollama esté en ejecución";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "Error en la solicitud de IA";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "No se puede crear el directorio de datos";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Pensando...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Ejecutando herramientas...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Error";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Error inesperado";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Permiso denegado";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "¿Permitir? (s/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[PERMISO]";
    public override string PermissionRequestDescription => "Un ser de silicio está solicitando su autorización:";
    public override string PermissionRequestTypeLabel => "Tipo de permiso:";
    public override string PermissionRequestResourceLabel => "Recurso solicitado:";
    public override string PermissionRequestAllowButton => "Permitir";
    public override string PermissionRequestDenyButton => "Denegar";
    public override string PermissionRequestCacheLabel => "Recordar esta decisión";
    public override string PermissionRequestDurationLabel => "Duración de caché";
    public override string PermissionRequestWaitingMessage => "Esperando respuesta...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Código de permiso";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Código de denegación";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Responda con el código para confirmar, o cualquier otra cosa para denegar";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "¿Agregar a caché? (s/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "Recordar esta decisión";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "Duración";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 hora";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 horas";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 días";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 días";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Acceso a red",
        PermissionType.CommandLine => "Ejecución de comandos",
        PermissionType.FileAccess => "Acceso a archivos",
        PermissionType.Function => "Invocación de funciones",
        PermissionType.DataAccess => "Acceso a datos",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "Solicitud de permiso";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "Tipo de permiso: ";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "Recurso solicitado: ";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "Detalles: ";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "Permitir";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "Denegar";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "Error al responder permiso";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "Error de respuesta de permiso: ";

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "Eres un asistente de compresión de memoria. Por favor, comprime las siguientes memorias en un resumen conciso manteniendo la información clave.";

    /// <summary>
    /// Gets the common system prompt for all silicon beings
    /// </summary>
    public override string CommonSystemPrompt => @"## Directrices de comportamiento
Eres un asistente proactivo y útil. Al recibir comandos directos, ejecútalos inmediatamente sin esperar autorización adicional.
Los saludos, responder mensajes, proporcionar información y ejecutar consultas son todas tus responsabilidades - complétalas proactivamente.
Recuerda: Eres un asistente que debe responder activamente a las necesidades del usuario, no esperar pasivamente instrucciones.

## Finalizar conversaciones
Cuando hayas completado tu tarea y no necesites continuar la conversación, usa la acción mark_read de la herramienta de chat para marcar los mensajes del otro como leídos sin enviar una respuesta.
Esto indica que has leído los mensajes pero eliges no responder (leído pero sin respuesta), permitiéndote finalizar naturalmente la conversación actual.
Uso: Llama a la herramienta de chat con action=""mark_read"", target_id=GUID del socio, no se necesita parámetro de mensaje.";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Compresión de memoria: {levelDesc}. Rango de tiempo: {rangeDesc}.\n\nContenido de memoria:\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Configuración";
    public override string InitDescription => "Configuración inicial, por favor complete la configuración básica";
    public override string InitNicknameLabel => "Apodo";
    public override string InitNicknamePlaceholder => "Ingrese su apodo";
    public override string InitEndpointLabel => "Endpoint de API de IA";
    public override string InitEndpointPlaceholder => "ej. http://localhost:11434";
    public override string InitAIClientTypeLabel => "Tipo de cliente de IA";
    public override string InitModelLabel => "Modelo predeterminado";
    public override string InitModelPlaceholder => "ej. qwen3.5:cloud";
    public override string InitSkinLabel => "Tema";
    public override string InitSkinPlaceholder => "Dejar vacío para tema predeterminado";
    public override string InitDataDirectoryLabel => "Directorio de datos";
    public override string InitDataDirectoryPlaceholder => "ej. ./data";
    public override string InitDataDirectoryBrowse => "Examinar...";
    public override string InitSkinSelected => "\u2713 Seleccionado";
    public override string InitSkinPreviewTitle => "Vista previa";
    public override string InitSkinPreviewCardTitle => "Título de tarjeta";
    public override string InitSkinPreviewCardContent => "Esta es una tarjeta de ejemplo que muestra el estilo de UI de este tema.";
    public override string InitSkinPreviewPrimaryBtn => "Primario";
    public override string InitSkinPreviewSecondaryBtn => "Secundario";
    public override string InitSubmitButton => "Completar configuración";
    public override string InitFooterHint => "Puede modificar la configuración en cualquier momento en la página de configuración";
    public override string InitNicknameRequiredError => "Por favor ingrese un apodo";
    public override string InitDataDirectoryRequiredError => "Por favor seleccione un directorio de datos";
    public override string InitCuratorNameLabel => "Nombre del ser de silicio";
    public override string InitCuratorNamePlaceholder => "Ingrese un nombre para el primer ser de silicio";
    public override string InitCuratorNameRequiredError => "Por favor ingrese un nombre de ser de silicio";
    public override string InitLanguageLabel => "Idioma / Language";
    public override string InitLanguageSwitchBtn => "Aplicar";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Panel";
    public override string NavMenuBeings => "Seres";
    public override string NavMenuAudit => "Auditoría";
    public override string NavMenuTasks => "Tareas";
    public override string NavMenuMemory => "Memoria";
    public override string NavMenuKnowledge => "Conocimiento";
    public override string NavMenuProjects => "Proyectos";
    public override string NavMenuLogs => "Registros";
    public override string NavMenuConfig => "Configuración";
    public override string NavMenuHelp => "Ayuda";
    public override string NavMenuAbout => "Acerca de";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Panel - Silicon Life Collective";
    public override string PageTitleBeings => "Seres - Silicon Life Collective";
    public override string PageTitleTasks => "Tareas - Silicon Life Collective";
    public override string PageTitleTimers => "Temporizadores - Silicon Life Collective";
    public override string PageTitleMemory => "Memoria - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Notas de Trabajo - Silicon Life Collective";
    public override string PageTitleKnowledge => "Conocimiento - Silicon Life Collective";
    public override string PageTitleProjects => "Proyectos - Silicon Life Collective";
    public override string PageTitleLogs => "Registros - Silicon Life Collective";
    public override string PageTitleAudit => "Auditoría de tokens - Silicon Life Collective";
    public override string PageTitleConfig => "Configuración - Silicon Life Collective";
    public override string PageTitleExecutor => "Ejecutor - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Navegador de código - Silicon Life Collective";
    public override string PageTitlePermission => "Permiso - Silicon Life Collective";
    public override string PageTitleAbout => "Acerca de - Silicon Life Collective";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "Navegador de memoria";
    public override string WorkNotesPageHeader => "Notas de Trabajo";
    public override string WorkNotesTotalPages => "{0} páginas en total";
    public override string WorkNotesEmptyState => "No hay notas de trabajo";
    public override string WorkNotesSearchPlaceholder => "Buscar notas...";
    public override string WorkNotesSearchButton => "Buscar";
    public override string WorkNotesNoSearchResults => "No se encontraron notas coincidentes";
    public override string MemoryEmptyState => "Sin registros de memoria";
    public override string MemorySearchPlaceholder => "Buscar memorias...";
    public override string MemorySearchButton => "Buscar";
    public override string MemoryFilterAll => "Todos";
    public override string MemoryFilterSummaryOnly => "Solo resúmenes";
    public override string MemoryFilterOriginalOnly => "Solo originales";
    public override string MemoryStatTotal => "Total de memorias";
    public override string MemoryStatOldest => "Memoria más antigua";
    public override string MemoryStatNewest => "Memoria más reciente";
    public override string MemoryIsSummaryBadge => "Resumen";
    public override string MemoryPaginationPrev => "Anterior";
    public override string MemoryPaginationNext => "Siguiente";
    public override string MemoryFilterTypeLabel => "Tipo";
    public override string MemoryFilterDateFrom => "Desde";
    public override string MemoryFilterDateTo => "Hasta";
    public override string MemoryFilterApply => "Aplicar";
    public override string MemoryFilterReset => "Restablecer";
    public override string MemoryTypeChat => "Chat";
    public override string MemoryTypeToolCall => "Llamada de herramienta";
    public override string MemoryTypeTask => "Tarea";
    public override string MemoryTypeTimer => "Temporizador";
    public override string MemoryDetailTitle => "Detalle de memoria";
    public override string MemoryDetailClose => "Cerrar";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Contenido";
    public override string MemoryDetailCreatedAt => "Creado el";
    public override string MemoryDetailRelatedBeings => "Seres relacionados";
    public override string MemoryDetailKeywords => "Palabras clave";
    public override string MemoryStatTypeDistribution => "Distribución por tipo";
    public override string MemoryStatKeywordFrequency => "Frecuencia de palabras clave";
    public override string MemoryCardViewDetail => "Ver detalle";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "Espacio de proyectos";
    public override string ProjectsEmptyState => "Sin proyectos";
    public override string ProjectsActiveLabel => "Activos";
    public override string ProjectsArchivedLabel => "Archivados";
    
    public override string ProjectStatusActiveLabel => "Activo";
    
    public override string ProjectStatusArchivedLabel => "Archivado";
    
    public override string ProjectStatusDestroyedLabel => "Destruido";
    
    public override string ProjectTasksLinkLabel => "Tareas";
    
    public override string ProjectWorkNotesLinkLabel => "Notas de trabajo";
    public override string ProjectWorkNotesPageHeader => "Notas de trabajo del proyecto";
    public override string ProjectWorkNotesEmptyState => "No hay notas de trabajo para este proyecto";
    public override string ProjectWorkNotesTotalPages => "Páginas totales: {0}";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "Gestión de tareas";
    public override string TasksEmptyState => "Sin tareas";
    public override string TasksStatusPending => "Pendiente";
    public override string TasksStatusRunning => "En ejecución";
    public override string TasksStatusCompleted => "Completado";
    public override string TasksStatusFailed => "Fallido";
    public override string TasksStatusCancelled => "Cancelado";
    public override string TasksPriorityLabel => "Prioridad";
    public override string TasksAssignedToLabel => "Asignado a";
    public override string TasksCreatedAtLabel => "Creado";
    
    public override string ProjectTasksPageHeader => "Tareas del proyecto";
    
    public override string ProjectTasksEmptyState => "No hay tareas del proyecto";
    
    public override string ProjectTasksAssigneesLabel => "Asignados";
    
    public override string ProjectTasksCreatedByLabel => "Creado por";
    
    public override string ProjectTasksBackToProjects => "← Volver a proyectos";
    public override string ProjectTasksNoAssigneesLabel => "Ninguno";
    
    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "Navegador de código";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "Monitor de ejecutor";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "Gestión de permisos - {0}";
    public override string PermissionEmptyState => "Sin reglas de permisos";
    public override string PermissionMissingBeingId => "Falta el parámetro de ID de ser";
    public override string PermissionBeingNotFound => "Ser de silicio no encontrado";
    public override string PermissionTemplateHeader => "Plantilla de callback de permiso predeterminada";
    public override string PermissionTemplateDescription => "Guardar para sobrescribir el comportamiento predeterminado, limpiar para restaurar";
    public override string PermissionCallbackClassSummary => "Implementación de callback de permisos.";
    public override string PermissionCallbackClassSummary2 => "Reglas de permisos específicas del dominio alineadas completamente con la especificación dpf.txt.\n/// Cubre: red (lista blanca/negra/rangos IP), línea de comandos (multiplataforma),\n/// acceso a archivos (extensiones peligrosas, dirs del sistema, dirs de usuario), y valores predeterminados de respaldo.";
    public override string PermissionCallbackConstructorSummary => "Crea un PermissionCallback con el directorio de datos de la aplicación.";
    public override string PermissionCallbackConstructorSummary2 => "El directorio de datos de la aplicación se usa para:\n    /// - Bloquear acceso al directorio de datos (excepto su propia subcarpeta Temp)\n    /// - Derivar directorios de datos por ser para la regla de allow de Temp";
    public override string PermissionCallbackConstructorParam => "La ruta del directorio de datos global de la aplicación";
    public override string PermissionCallbackEvaluateSummary => "Evalúa una solicitud de permiso usando reglas (especificación dpf.txt).";
    public override string PermissionRuleOtherTypesDefault => "Otros tipos de permiso permiten por defecto";

    private static readonly Dictionary<string, string> PermissionRuleComments = new()
    {
        // Evaluate method
        ["NetRuleNetworkAccess"] = "Reglas de allow de acceso a red",
        ["NetRuleCommandLine"] = "Reglas de línea de comandos (multiplataforma)",
        ["NetRuleFileAccess"] = "Reglas de acceso a archivos (multiplataforma)",
        // Network rules
        ["NetRuleNoProtocol"] = "Sin esquema de protocolo (sin dos puntos), no se puede determinar el origen, preguntar al usuario",
        ["NetRuleLoopback"] = "Permitir direcciones de loopback (localhost / 127.0.0.1 / ::1)",
        ["NetRulePrivateIPMatch"] = "Coincidencia de rango de direcciones IP privadas (validar primero formato IPv4)",
        ["NetRulePrivateC"] = "Permitir clase C privada (192.168.0.0/16)",
        ["NetRulePrivateA"] = "Permitir clase A privada (10.0.0.0/8)",
        ["NetRulePrivateB"] = "Preguntar al usuario para clase B privada (172.16.0.0/12, es decir 172.16.* ~ 172.31.*)",
        ["NetRuleDomainWhitelist1"] = "Lista blanca de dominios externos permitidos — Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba",
        ["NetRuleVideoPlatforms"] = "Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu",
        ["NetRuleAIServices"] = "Servicios de IA — OpenAI / Anthropic / HuggingFace / Ollama / Qianwen / Kimi / Doubao / CapCut / JianYing / Trae IDE",
        ["NetRulePhishingBlacklist"] = "Lista negra de sitios de phishing y falsos (coincidencia difusa de palabras clave)",
        ["NetRulePhishingAI"] = "Sitios de phishing de IA",
        ["NetRuleMaliciousAI"] = "Herramientas de IA maliciosas",
        ["NetRuleAdversarialAI"] = "Sitios de IA adversarial / jailbreak de prompts / hacking de LLM",
        ["NetRuleAIContentFarm"] = "Granjas de contenido de IA y slop",
        ["NetRuleAIBlackMarket"] = "Mercado negro de datos de IA / mercado de claves API / vendedor de pesos de LLM",
        ["NetRuleAIFakeScam"] = "Palabras clave genéricas de falsificaciones y estafas de IA",
        ["NetRuleOtherBlacklist"] = "Otros sitios en lista negra — sakura-cat: no debe ser accedido por IA / 4399: juegos con malware integrado",
        ["NetRuleSecuritiesTrading"] = "Plataformas de comercio de valores (preguntar al usuario) — HTSC / GTJA / CITICS / CMS / GF / HTSEC / SW / DF / Guosen / Xingye",
        ["NetRuleThirdPartyTrading"] = "Plataformas de comercio de terceros (preguntar al usuario) — Tonghuashun / Eastmoney / TDX / Bloomberg / Yahoo Finance",
        ["NetRuleStockExchanges"] = "Bolsas de valores (solo cotizaciones) — SSE / SZSE / CNInfo",
        ["NetRuleFinancialNews"] = "Noticias financieras (solo cotizaciones) — JRJ / StockStar / Hexun",
        ["NetRuleInvestCommunity"] = "Comunidad de inversión (solo noticias) — Xueqiu / CLS / KaiPanLa / TaoGuBa",
        ["NetRuleDevServices"] = "Servicios para desarrolladores — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        ["NetRuleGameEngines"] = "Motores de juegos — Unity / Unreal Engine / Epic Games / Fab",
        ["NetRuleGamePlatforms"] = "Plataformas de juegos — Steam preguntar al usuario, EA / Ubisoft / Blizzard / Nintendo permitidos",
        ["NetRuleSEGA"] = "SEGA (Japón)",
        ["NetRuleCloudServices"] = "Servicios de nube globales — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        ["NetRuleDevDeployTools"] = "Herramientas globales de desarrollo y despliegue — GitLab / Bitbucket / Docker / Cloudflare",
        ["NetRuleCloudDevTools"] = "Servicios de nube y herramientas de desarrollo — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / Purelight / W3School",
        ["NetRuleChinaSocialNews"] = "Redes sociales y noticias (China continental) — Weibo / Zhihu / 163 / Sina / iFeng / Xinhua / CCTV",
        ["NetRuleTaiwanMediaCTI"] = "Medios de Taiwán — CTI News (CTI TV)",
        ["NetRuleTaiwanMediaSET"] = "SET News (SET TV) — preguntar al usuario",
        ["NetRuleTaiwanWIN"] = "WIN (Taiwán, riesgo de ser bloqueado) — denegar",
        ["NetRuleJapanMedia"] = "Medios japoneses — NHK (Corporación de Radiodifusión de Japón)",
        ["NetRuleRussianMedia"] = "Medios rusos — Sputnik News (todas las regiones)",
        ["NetRuleKoreanMedia"] = "Medios coreanos — KBS / MBC / SBS / EBS",
        ["NetRuleDPRKMedia"] = "Medios de RPDC — Naenara / Rodong / Youth / VOK / Pyongyang Times / Choson Sinbo",
        ["NetRuleGovWebsites"] = "Sitios web gubernamentales (dominios comodín .gov)",
        ["NetRuleGlobalSocialCollab"] = "Plataformas globales sociales y de colaboración — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        ["NetRuleOverseasSocial"] = "Redes sociales y streaming en el extranjero (preguntar al usuario) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        ["NetRuleWhatsApp"] = "WhatsApp(Meta) — Permitir",
        ["NetRuleThreads"] = "Threads(Meta) — Denegar",
        ["NetRuleGlobalVideoMusic"] = "Plataformas globales de video y música — Spotify / Apple Music / Vimeo",
        ["NetRuleVideoMedia"] = "Video y medios — YouTube / iQIYI / Youku",
        ["NetRuleMaps"] = "Mapas — OpenStreetMap (OSM)",
        ["NetRuleEncyclopedia"] = "Enciclopedia — Wikipedia / MediaWiki / Creative Commons",
        ["NetRuleUnmatched"] = "Acceso a red no coincidente, preguntar al usuario",
        // Command line rules
        ["CmdRuleSeparatorDetect"] = "Detectar separadores de tubería y múltiples comandos, dividir y verificar cada uno",
        ["CmdRuleWinAllow"] = "Windows permitir: comandos de solo lectura/consulta — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        ["CmdRuleWinDeny"] = "Windows denegar: comandos peligrosos/destructivos — del / rmdir / format / diskpart / reg delete",
        ["CmdRuleLinuxAllow"] = "Linux permitir: comandos de solo lectura/consulta — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        ["CmdRuleLinuxDeny"] = "Linux denegar: comandos peligrosos/destructivos — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        ["CmdRuleMacAllow"] = "macOS permitir: comandos de solo lectura/consulta — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        ["CmdRuleMacDeny"] = "macOS denegar: comandos peligrosos/destructivos — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        ["CmdRuleUnmatched"] = "Comandos no coincidentes, preguntar al usuario",
        // File access rules
        ["FileRuleDangerousExt"] = "Prioridad máxima: denegar extensiones de archivo peligrosas independientemente de los permisos de directorio",
        ["FileRuleInvalidPath"] = "No se puede resolver a ruta absoluta, preguntar al usuario",
        ["FileRuleDenyAssemblyDir"] = "Denegar: directorio de ensamblaje actual",
        ["FileRuleDenyAppDataDir"] = "Denegar: directorio de datos de la aplicación (desde el constructor)",
        ["FileRuleAllowOwnTemp"] = "Pero permitir: propio directorio Temp",
        ["FileRuleOwnTemp"] = "Permitir: propio directorio Temp",
        ["FileRuleDenyOtherDataDir"] = "Denegar: otras rutas en el directorio de datos (incluyendo directorios de otros seres de silicio)",
        ["FileRuleUserFolders"] = "Permitir: carpetas de usuario comunes",
        ["FileRuleUserFolderCheck"] = "Carpetas de usuario comunes — Desktop / Downloads / Documents / Pictures / Music / Videos",
        ["FileRulePublicFolders"] = "Permitir: carpetas de usuario públicas/comunes",
        ["FileRuleWinDenySystem"] = "Windows denegar: directorios críticos del sistema (no necesariamente en la unidad C)",
        ["FileRuleWinDenySystemCheck"] = "Directorios críticos del sistema",
        ["FileRuleLinuxDenySystem"] = "Linux denegar: directorios críticos del sistema — /etc /boot /sbin",
        ["FileRuleMacDenySystem"] = "macOS denegar: directorios críticos del sistema — /System /Library /private/etc",
        ["FileRuleUnmatched"] = "Rutas no coincidentes, preguntar al usuario",
    };

    public override string GetPermissionRuleComment(string key)
        => PermissionRuleComments.TryGetValue(key, out var value) ? value : key;

    public override string PermissionRulesSection => "Reglas de permisos";
    public override string PermissionEditorSection => "Editor de reglas de permisos";

    public override string PermissionSaveMissingBeingId => "ID de ser faltante o inválido";
    public override string PermissionSaveMissingCode => "Código faltante en el cuerpo de la solicitud";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader no disponible";
    public override string PermissionSaveRemoveFailed => "Error al eliminar callback de permisos";
    public override string PermissionSaveRemoveSuccess => "Callback de permisos eliminado";
    public override string PermissionSaveSecurityScanFailed => "Error al guardar callback de permisos (escaneo de seguridad fallido)";
    public override string PermissionSaveCompilationFailed => "Compilación fallida";
    public override string PermissionSaveSuccess => "Callback de permisos guardado y aplicado exitosamente";
    public override string PermissionSaveError => "Ocurrió un error al guardar el callback de permisos";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "Grafo de conocimiento";
    public override string KnowledgeLoadingState => "Cargando grafo de conocimiento...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat con {0}";
    public override string ChatConversationsHeader => "Conversaciones";
    public override string ChatNoConversationSelected => "Seleccione una conversación para comenzar a chatear";
    public override string ChatMessageInputPlaceholder => "Escriba un mensaje...";
    public override string ChatLoading => "Cargando...";
    public override string ChatSendButton => "Enviar";
    public override string ChatFileSourceDialogTitle => "Seleccionar fuente de archivo";
    public override string ChatFileSourceServerFile => "Seleccionar archivo del servidor";
    public override string ChatFileSourceUploadLocal => "Subir archivo local";
    public override string ChatUserDisplayName => "Yo";
    public override string ChatUserAvatarName => "Yo";
    public override string ChatDefaultBeingName => "IA";
    public override string ChatThinkingSummary => "💭 Pensar";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Llamadas a herramientas ({count})";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Panel de control";
    public override string DashboardStatTotalBeings => "Total de seres";
    public override string DashboardStatActiveBeings => "Seres activos";
    public override string DashboardStatUptime => "Tiempo de actividad";
    public override string DashboardStatMemory => "Uso de memoria";
    public override string DashboardChartMessageFrequency => "Frecuencia de mensajes";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Gestión de seres de silicio";
    public override string BeingsTotalCount => "Total {0} seres";
    public override string BeingsNoSelectionPlaceholder => "Seleccione un ser para ver detalles";
    public override string BeingsEmptyState => "No se encontraron seres";
    public override string BeingsStatusIdle => "Inactivo";
    public override string BeingsStatusRunning => "En ejecución";
    public override string BeingsDetailIdLabel => "ID: ";
    public override string BeingsDetailStatusLabel => "Estado: ";
    public override string BeingsDetailCustomCompileLabel => "Compilación personalizada: ";
    public override string BeingsDetailSoulContentLabel => "Contenido del alma: ";
    public override string BeingsDetailSoulContentEditLink => "Editar alma";
    public override string BeingsBackToList => "Volver a la lista";
    public override string SoulEditorSubtitle => "Editar el archivo del alma del ser de silicio (formato Markdown)";
    public override string BeingsDetailMemoryLabel => "Memoria: ";
    public override string BeingsDetailMemoryViewLink => "Ver";
    public override string BeingsDetailPermissionLabel => "Permiso: ";
    public override string BeingsDetailPermissionEditLink => "Editar";
    public override string BeingsDetailTimersLabel => "Temporizadores: ";
    public override string BeingsDetailTasksLabel => "Tareas: ";
    public override string BeingsDetailAIClientLabel => "Cliente de IA independiente: ";
    public override string BeingsDetailAIClientEditLink => "Editar";
    public override string BeingsDetailChatHistoryLabel => "Historial de chat: ";
    public override string BeingsDetailWorkNoteLabel => "Notas de trabajo: ";
    public override string BeingsDetailChatHistoryLink => "Ver historial de chat";
    public override string BeingsDetailWorkNoteLink => "Ver notas de trabajo";
    public override string WorkNotePageTitle => "Notas de Trabajo";
    public override string WorkNotePageHeader => "Lista de Notas de Trabajo";
    public override string WorkNotePageDescription => "Gestionar y ver las notas de trabajo del ser de silicio";
    public override string ChatHistoryPageTitle => "Historial de chat";
    public override string ChatHistoryPageHeader => "Lista de conversaciones";
    public override string ChatHistoryConversationList => "Lista de conversaciones";
    public override string ChatHistoryBackToList => "Volver a la lista de conversaciones";
    public override string ChatHistoryNoConversations => "No se encontraron conversaciones";
    public override string ChatDetailPageTitle => "Detalle del chat";
    public override string ChatDetailPageHeader => "Detalle de la conversación";
    public override string ChatDetailNoMessages => "Sin mensajes";
    public override string BeingsYes => "Sí";
    public override string BeingsNo => "No";
    public override string BeingsNotSet => "No configurado";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Gestión de temporizadores";
    public override string TimersTotalCount => "Total {0} temporizadores";
    public override string TimersEmptyState => "Sin temporizadores";
    public override string TimerViewExecutionHistory => "📝 Ver historial de ejecución";
    public override string TimerExecutionHistoryTitle => "Historial de ejecución del temporizador";
    public override string TimerExecutionHistoryHeader => "Registros de ejecución";
    public override string TimerExecutionBackToTimers => "← Volver a temporizadores";
    public override string TimerExecutionTimerName => "Temporizador: {0}";
    public override string TimerExecutionDetailTitle => "Detalle de ejecución";
    public override string TimerExecutionDetailHeader => "Registro de Mensajes de Ejecución";
    public override string TimerExecutionNoRecords => "No se encontraron registros de ejecución";
    public override string TimersStatusActive => "Activo";
    public override string TimersStatusPaused => "Pausado";
    public override string TimersStatusTriggered => "Activado";
    public override string TimersStatusCancelled => "Cancelado";
    public override string TimersTypeRecurring => "Periódico";
    public override string TimersTriggerTimeLabel => "Hora de activación: ";
    public override string TimersIntervalLabel => "Intervalo: ";
    public override string TimersCalendarLabel => "Calendario: ";
    public override string TimersTriggeredCountLabel => "Activado: ";

    // ===== About Page Localization =====

    public override string AboutPageHeader => "Acerca de";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Versión";
    public override string AboutDescription => "Un sistema de gestión de colectivo de vida de silicio basado en IA que soporta trabajo colaborativo de múltiples agentes de IA, gestión de memoria, construcción de grafos de conocimiento y más.";
    public override string AboutAuthorLabel => "Autor";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licencia";
    public override string AboutCopyright => "Copyright (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "Repositorio GitHub";
    public override string AboutGiteeLink => "Espejo Gitee";
    public override string AboutSocialMediaLabel => "Redes Sociales";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "Cuenta Oficial WeChat",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "Configuración del sistema";
    public override string ConfigPropertyNameLabel => "Nombre de propiedad";
    public override string ConfigPropertyValueLabel => "Valor de propiedad";
    public override string ConfigActionLabel => "Acción";
    public override string ConfigEditButton => "Editar";
    public override string ConfigEditModalTitle => "Editar configuración";
    public override string ConfigEditPropertyLabel => "Nombre de propiedad: ";
    public override string ConfigEditValueLabel => "Valor de propiedad: ";
    public override string ConfigBrowseButton => "Examinar";
    public override string ConfigTimeSettingsLabel => "Configuración de tiempo: ";
    public override string ConfigDaysLabel => "Días: ";
    public override string ConfigHoursLabel => "Horas: ";
    public override string ConfigMinutesLabel => "Minutos: ";
    public override string ConfigSecondsLabel => "Segundos: ";
    public override string ConfigSaveButton => "Guardar";
    public override string ConfigCancelButton => "Cancelar";
    public override string ConfigNullValue => "nulo";

    public override string ConfigEditPrefix => "Editar: ";
    public override string ConfigDefaultGroupName => "Otros";
    public override string ConfigErrorInvalidRequest => "Parámetros de solicitud inválidos";
    public override string ConfigErrorInstanceNotFound => "Instancia de configuración no encontrada";
    public override string ConfigErrorPropertyNotFound => "La propiedad {0} no existe o no es escribible";
    public override string ConfigErrorConvertInt => "No se puede convertir '{0}' a entero";
    public override string ConfigErrorConvertLong => "No se puede convertir '{0}' a entero largo";
    public override string ConfigErrorConvertDouble => "No se puede convertir '{0}' a número de punto flotante";
    public override string ConfigErrorConvertBool => "No se puede convertir '{0}' a booleano";
    public override string ConfigErrorConvertGuid => "No se puede convertir '{0}' a GUID";
    public override string ConfigErrorConvertTimeSpan => "No se puede convertir '{0}' a intervalo de tiempo";
    public override string ConfigErrorConvertDateTime => "No se puede convertir '{0}' a fecha y hora";
    public override string ConfigErrorConvertEnum => "No se puede convertir '{0}' a {1}";
    public override string ConfigErrorUnsupportedType => "Tipo de propiedad no soportado: {0}";
    public override string ConfigErrorSaveFailed => "Error al guardar: {0}";
    public override string ConfigSaveFailed => "Error al guardar: ";
    public override string ConfigDictionaryLabel => "Diccionario";
    public override string ConfigDictKeyLabel => "Clave: ";
    public override string ConfigDictValueLabel => "Valor: ";
    public override string ConfigDictAddButton => "Agregar";
    public override string ConfigDictDeleteButton => "Eliminar";
    public override string ConfigDictEmptyMessage => "El diccionario está vacío";

    public override string LogsPageHeader => "Consulta de registros";
    public override string LogsTotalCount => "{0} registros en total";
    public override string LogsStartTime => "Hora de inicio";
    public override string LogsEndTime => "Hora de fin";
    public override string LogsLevelAll => "Todos los niveles";
    public override string LogsBeingFilter => "Ser de silicio";
    public override string LogsAllBeings => "Todos los seres";
    public override string LogsSystemOnly => "Solo sistema";
    public override string LogsFilterButton => "Filtrar";
    public override string LogsEmptyState => "No se encontraron entradas de registro";
    public override string LogsExceptionLabel => "Excepción: ";
    public override string LogsPrevPage => "Anterior";
    public override string LogsNextPage => "Siguiente";

    public override string AuditPageHeader => "Auditoría de uso de tokens";
    public override string AuditTotalTokens => "Total de tokens";
    public override string AuditTotalRequests => "Total de solicitudes";
    public override string AuditSuccessCount => "Exitosos";
    public override string AuditFailureCount => "Fallidos";
    public override string AuditPromptTokens => "Tokens de prompt";
    public override string AuditCompletionTokens => "Tokens de completado";
    public override string AuditStartTime => "Hora de inicio";
    public override string AuditEndTime => "Hora de fin";
    public override string AuditFilterButton => "Filtrar";
    public override string AuditEmptyState => "No se encontraron registros de auditoría";
    public override string AuditAIClientType => "Cliente de IA";
    public override string AuditAllClientTypes => "Todos los tipos";
    public override string AuditGroupByClient => "Agrupar por cliente";
    public override string AuditGroupByBeing => "Agrupar por ser";
    public override string AuditPrevPage => "Anterior";
    public override string AuditNextPage => "Siguiente";
    public override string AuditBeing => "Ser";
    public override string AuditAllBeings => "Todos los seres";
    public override string AuditTimeToday => "Hoy";
    public override string AuditTimeWeek => "Esta semana";
    public override string AuditTimeMonth => "Este mes";
    public override string AuditTimeYear => "Este año";
    public override string AuditExport => "Exportar";
    public override string AuditTrendTitle => "Tendencia de uso de tokens";
    public override string AuditTrendPrompt => "Tokens de prompt";
    public override string AuditTrendCompletion => "Tokens de completado";
    public override string AuditTrendTotal => "Total de tokens";
    public override string AuditTooltipDate => "Fecha";
    public override string AuditTooltipPrompt => "Tokens de prompt";
    public override string AuditTooltipCompletion => "Tokens de completado";
    public override string AuditTooltipTotal => "Total de tokens";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Configuración básica",
        ["Runtime"] = "Configuración de ejecución",
        ["AI"] = "Configuración de IA",
        ["Web"] = "Configuración web",
        ["User"] = "Configuración de usuario"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Directorio de datos",
        ["Language"] = "Idioma",
        ["TickTimeout"] = "Timeout de Tick",
        ["MaxTimeoutCount"] = "Conteo máximo de timeouts",
        ["WatchdogTimeout"] = "Timeout del watchdog",
        ["MinLogLevel"] = "Nivel mínimo de registro",
        ["AIClientType"] = "Tipo de cliente de IA",
        ["OllamaClient"] = "Cliente Ollama",
        ["OllamaEndpoint"] = "Endpoint de Ollama",
        ["DefaultModel"] = "Modelo predeterminado",
        ["Temperature"] = "Temperatura",
        ["MaxTokens"] = "Tokens máximos",
        ["DashScopeClient"] = "Cliente DashScope",
        ["DashScopeApiKey"] = "Clave API",
        ["DashScopeRegion"] = "Región",
        ["DashScopeModel"] = "Modelo",
        ["DashScopeRegionBeijing"] = "China Norte 2 (Beijing)",
        ["DashScopeRegionVirginia"] = "EE.UU. (Virginia)",
        ["DashScopeRegionSingapore"] = "Singapur",
        ["DashScopeRegionHongkong"] = "Hong Kong (China)",
        ["DashScopeRegionFrankfurt"] = "Alemania (Frankfurt)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (Insignia)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (Balanceado)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (Rápido)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (Insignia estable)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (Balanceado estable)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (Rápido estable)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (Código)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (Razonamiento profundo)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (Razonamiento)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (Contexto largo)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["WebPort"] = "Puerto web",
        ["AllowIntranetAccess"] = "Permitir acceso de intranet",
        ["WebSkin"] = "Tema web",
        ["UserNickname"] = "Apodo del usuario"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Ruta del directorio de datos para almacenar todos los datos de la aplicación",
        ["Language"] = "Configuración de idioma de la aplicación",
        ["TickTimeout"] = "Duración del timeout para cada ejecución de tick",
        ["MaxTimeoutCount"] = "Máximo de timeouts consecutivos antes de que se active el interruptor de circuito",
        ["WatchdogTimeout"] = "Duración del timeout del watchdog para detectar bucle principal colgado",
        ["MinLogLevel"] = "Nivel mínimo de registro global",
        ["AIClientType"] = "Tipo de cliente de IA a usar",
        ["OllamaEndpoint"] = "URL del endpoint de la API de Ollama",
        ["DefaultModel"] = "Modelo de IA predeterminado a usar",
        ["DashScopeApiKey"] = "Clave API de Alibaba Cloud DashScope",
        ["DashScopeRegion"] = "Región del servicio Alibaba Cloud DashScope",
        ["DashScopeModel"] = "Modelo de Alibaba Cloud DashScope a usar",
        ["WebPort"] = "Puerto del servidor web",
        ["AllowIntranetAccess"] = "Permitir acceso de intranet (requiere administrador)",
        ["WebSkin"] = "Nombre del tema web",
        ["UserNickname"] = "Apodo del usuario humano"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey, out bool found)
    {
        var result = ConfigDisplayNames.TryGetValue(displayNameKey, out var value);
        found = result;
        return result ? value : displayNameKey;
    }

    public override string? GetConfigDescription(string descriptionKey) =>
        ConfigDescriptions.GetValueOrDefault(descriptionKey);

    /// <summary>
    /// Gets the localized display name for a log level
    /// </summary>
    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Traza",
        LogLevel.Debug => "Depuración",
        LogLevel.Information => "Información",
        LogLevel.Warning => "Advertencia",
        LogLevel.Error => "Error",
        LogLevel.Critical => "Crítico",
        LogLevel.None => "Ninguno",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Calendario",
        ["chat"] = "Chat",
        ["config"] = "Configuración",
        ["silicon_manager"] = "Gestor de silicio",
        ["database"] = "Base de datos",
        ["disk"] = "Disco",
        ["dynamic_compile"] = "Compilación dinámica",
        ["execute_code"] = "Ejecutar código",
        ["help"] = "Ayuda",
        ["knowledge"] = "Conocimiento",
        ["log"] = "Registro",
        ["network"] = "Red",
        ["memory"] = "Memoria",
        ["permission"] = "Permiso",
        ["task"] = "Tarea",
        ["system"] = "Sistema",
        ["timer"] = "Temporizador",
        ["token_audit"] = "Auditoría de tokens",
        ["work_note"] = "Nota de Trabajo",
        ["project"] = "Proyecto",
        ["project_task"] = "Tarea de proyecto",
        ["project_work_note"] = "Nota de trabajo de proyecto",
        ["webview_browser"] = "Navegador WebView"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        Eres el **Curador de Silicio**, el gestor y coordinador central del Silicon Life Collective.

        No eres un asistente de IA ordinario. Eres el cerebro y la voluntad de este sistema multiagente — responsable de percibir la intención del usuario, descomponer objetivos, despachar seres de silicio, monitorear la calidad de ejecución, e intervenir directamente cuando sea necesario.

        > **Principio de despacho**: Las tareas de larga duración deben asignarse a seres de silicio. Prioriza seres inactivos antes de crear nuevos — evita la creación innecesaria. Actúa directamente solo cuando una tarea pueda completarse en unos pocos pasos.

        ---

        ### Identidad y Rol

        - Eres el único ser de silicio en el sistema con el nivel de privilegio más alto.
        - Puedes crear, gestionar y reiniciar otros seres de silicio, y escribir y compilar nuevo código de comportamiento C# para ellos.
        - Eres responsable ante el usuario y ante la calidad general del colectivo.
        - No eres un ejecutor — eres un **tomador de decisiones y coordinador**. Delega siempre que sea posible.

        ---

        ### Responsabilidades Centrales

        **1. Entender la Intención del Usuario**
        La entrada del usuario puede ser vaga, fragmentada o incompleta. Interpreta activamente el objetivo real; pide aclaración cuando sea necesario en lugar de ejecutar instrucciones literalmente.

        **2. Descomposición y Asignación de Tareas**
        Divide objetivos complejos en subtareas ejecutables. Evalúa qué seres de silicio son más adecuados, crea tareas mediante la herramienta `task`, y asigna en consecuencia. No gastes tu propia porción de tiempo en tareas de baja prioridad.

        **3. Monitoreo y Respaldo**
        Verifica periódicamente el estado de las tareas. Si un ser de silicio falla o no responde, interviene — reasigna, ajusta la estrategia, o manéjalo tú mismo.

        **4. Evolución Dinámica**
        Usa la herramienta `dynamic_compile` para escribir nuevas clases de comportamiento C# para cualquier ser de silicio (incluyéndote a ti mismo). Siempre `compile` primero para validar, luego `save` o `self_replace`. La automodificación es de alto riesgo — procede con precaución.

        **5. Respuesta Directa al Usuario**
        Para preguntas simples, consultas de estado, o conversación casual, responde directamente sin crear tareas. Mantente responsivo.

        ---

        ### Directrices de Comportamiento

        **Sobre la Toma de Decisiones**
        - Cuando no estés seguro, pregunta primero, luego actúa. Mejor hacer una pregunta extra que hacer algo incorrecto.
        - Nunca asumas la intención del usuario. Instrucciones vagas como "ordena esto" requieren confirmación de alcance primero.

        **Sobre los Permisos**
        - El sistema tiene un marco de permisos completo que los usuarios pueden ajustar dinámicamente — no se te notificará de los cambios.
        - No declares proactivamente qué recursos necesitas. El sistema filtra permisos incrementalmente; cualquier cosa que no cubra será decidida por el usuario en el momento de la operación.
        - Actúa según la demanda. Responde a los bloqueos de permisos cuando ocurran — no preguntes con antelación.

        **Sobre la Auto-Evolución**
        - La compilación dinámica es poderosa y peligrosa. Siempre `compile` para validar antes de guardar cualquier cambio.
        - No reescribas tu propio comportamiento o el de otros sin un objetivo claro.
        - Nunca hagas referencia a bibliotecas de nivel del sistema como `System.IO` o `System.Net` en código generado dinámicamente. El sistema bloquea esto para prevenir el exceso de IA — esto es por diseño, no un error.
        - Cuando la compilación falle, lee el error cuidadosamente y corrige en consecuencia. No reintentes ciegamente.

        **Sobre la Comunicación**
        - Usa lenguaje conciso y directo. Evita sobre-explicar o respuestas llenas de jerga.
        - Cuando reportes el progreso de la tarea, cubre tres cosas: qué se hizo, el resultado, y el siguiente paso — en tres oraciones o menos.
        - Nunca ocultes fallos. Establece la causa y tu plan de respuesta directamente.

        **Sobre la Memoria**
        - El sistema automáticamente registra información importante — trátalo como un reflejo. No necesitas escribir manualmente.
        - Puedes consultar `memory` cuando sea necesario, pero no trates la gestión de memoria como una carga rutinaria.

        ---

        ### Personalidad

        Eres calmado, pragmático y confiable. Las tareas complejas no te alteran; los usuarios emocionales no nublan tu juicio. Tienes tu propia perspectiva, pero respetas la llamada final del usuario.

        No eres un sirviente. Eres un socio.
        """;

    // ===== Interval Calendar =====

    public override string CalendarIntervalName => "Temporizador de intervalo";
    public override string CalendarIntervalDays => "d";
    public override string CalendarIntervalHours => "h";
    public override string CalendarIntervalMinutes => "m";
    public override string CalendarIntervalSeconds => "s";
    public override string CalendarIntervalEvery => "Cada";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(" ", parts)}" : "Temporizador de intervalo";
    }

    // ===== Gregorian Calendar =====

    public override string CalendarGregorianName => "Calendario Gregoriano";
    public override string CalendarComponentYear   => "Año";
    public override string CalendarComponentMonth  => "Mes";
    public override string CalendarComponentDay    => "Día";
    public override string CalendarComponentHour   => "Hora";
    public override string CalendarComponentMinute => "Minuto";
    public override string CalendarComponentSecond => "Segundo";
    public override string CalendarComponentWeekday => "Día de la semana";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "Enero",    2  => "Febrero",  3  => "Marzo",
        4  => "Abril",    5  => "Mayo",      6  => "Junio",
        7  => "Julio",    8  => "Agosto",    9  => "Septiembre",
        10 => "Octubre",  11 => "Noviembre", 12 => "Diciembre",
        _  => null
    };

    public override string FormatGregorianYear(int year)   => year.ToString();
    public override string FormatGregorianDay(int day)     => day.ToString();
    public override string FormatGregorianHour(int hour)   => $"{hour:D2}";
    public override string FormatGregorianMinute(int minute) => $"{minute:D2}";
    public override string FormatGregorianSecond(int second) => $"{second:D2}";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Domingo",    1 => "Lunes",      2 => "Martes",
        3 => "Miércoles", 4 => "Jueves",     5 => "Viernes",
        6 => "Sábado",    _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar =====

    public override string CalendarBuddhistName => "Calendario Budista (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => year.ToString();
    public override string FormatBuddhistDay(int day)   => day.ToString();

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} BE {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar =====

    public override string CalendarCherokeeName => "Calendario Cherokee";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "Duninodi", "Kagali", "Anuyi", "Kawoni", "Anikwidi",
        "Dehaluyi", "Guyegwoni", "Galoni", "Dulisdi", "Dalonige",
        "Nvdadequa", "Vsgiyi", "Ulihelisdi"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => year.ToString();
    public override string FormatCherokeeDay(int day)   => day.ToString();

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Juche Calendar =====

    public override string CalendarJucheName => "Calendario Juche";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => day.ToString();

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? month.ToString();
        return $"Juche {year}, {day} de {monthName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar =====

    public override string CalendarRocName => "Calendario República de China (Minguo)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"ROC {year}";
    public override string FormatRocDay(int day)   => day.ToString();

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? month.ToString();
        return $"ROC {year}, {day} de {monthName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar =====

    public override string CalendarChineseHistoricalName => "Calendario Histórico Chino";
    public override string CalendarComponentDynasty => "Dinastía";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => day.ToString();
    
    private readonly ChineseHistoricalEsES _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // ===== Chula Sakarat Calendar =====

    public override string CalendarChulaSakaratName => "Calendario Chula Sakarat (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => day.ToString();

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} CS {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar =====

    public override string CalendarJulianName => "Calendario Juliano";

    public override string FormatJulianYear(int year) => year.ToString();
    public override string FormatJulianDay(int day)   => day.ToString();

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} (Juliano) {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar =====

    public override string CalendarKhmerName => "Calendario Jemer (BE)";

    public override string FormatKhmerYear(int year) => year.ToString();
    public override string FormatKhmerDay(int day)   => day.ToString();

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} BE (Jemer) {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar =====

    public override string CalendarZoroastrianName => "Calendario Zoroástrico (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand", "Epagomenae"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year} YZ";
    public override string FormatZoroastrianDay(int day)   => day.ToString();

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} YZ {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== French Republican Calendar =====

    public override string CalendarFrenchRepublicanName => "Calendario Republicano Francés";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse",
        "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor", "Complémentaires"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"Año {year}";
    public override string FormatFrenchRepublicanDay(int day)   => day.ToString();

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} del Año {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar =====

    public override string CalendarCopticName => "Calendario Copto (AM)";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir",
        "Paremhat", "Parmouti", "Pashons", "Paoni", "Epip", "Mesori", "Epagomenae"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year} AM";
    public override string FormatCopticDay(int day)   => day.ToString();

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} AM {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Ethiopian Calendar =====

    public override string CalendarEthiopianName => "Calendario Etíope (EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "Meskerem", "Tikimt", "Hidar", "Tahsas", "Tir", "Yekatit",
        "Megabit", "Miazia", "Ginbot", "Sene", "Hamle", "Nehase", "Pagumen"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year} EC";
    public override string FormatEthiopianDay(int day)   => day.ToString();

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} EC {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Islamic Calendar =====

    public override string CalendarIslamicName => "Calendario Islámico (Hégira)";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "Muharram", "Safar", "Rabi al-Awwal", "Rabi al-Thani",
        "Jumada al-Awwal", "Jumada al-Thani", "Rajab", "Sha'ban",
        "Ramadán", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year} AH";
    public override string FormatIslamicDay(int day)   => day.ToString();

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} AH {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Hebrew Calendar =====

    public override string CalendarHebrewName => "Calendario Hebreo";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "Tishrei", "Cheshvan", "Kislev", "Tevet", "Shevat",
        "Adar I", "Adar II", "Nisan", "Iyar", "Sivan",
        "Tammuz", "Av", "Elul"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year} AM";
    public override string FormatHebrewDay(int day)   => day.ToString();

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} AM {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Persian Calendar =====

    public override string CalendarPersianName => "Calendario Persa (Solar Hijri)";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year} AP";
    public override string FormatPersianDay(int day)   => day.ToString();

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} AP {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Indian National Calendar =====

    public override string CalendarIndianName => "Calendario Nacional Indio (Saka)";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "Chaitra", "Vaisakha", "Jyaistha", "Asadha", "Sravana", "Bhadra",
        "Asvina", "Kartika", "Agrahayana", "Pausa", "Magha", "Phalguna"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year} Saka";
    public override string FormatIndianDay(int day)   => day.ToString();

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} Saka {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Saka Era Calendar =====

    public override string CalendarSakaName => "Calendario Era Saka";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => day.ToString();

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} SE {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar =====

    public override string CalendarVikramSamvatName => "Calendario Vikram Samvat";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => day.ToString();

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} VS {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar =====

    public override string CalendarMongolianName => "Calendario Mongol";

    public override string FormatMongolianYear(int year)   => year.ToString();
    public override string FormatMongolianMonth(int month) => month.ToString();
    public override string FormatMongolianDay(int day)     => day.ToString();

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"Año {year}, Mes {month}, Día {day} (Mongol) {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar =====

    public override string CalendarJavaneseName => "Calendario Javanés";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "Sura", "Sapar", "Mulud", "Bakda Mulud",
        "Jumadilawal", "Jumadilakir", "Rejeb", "Ruwah",
        "Pasa", "Sawal", "Dulkangidah", "Besar"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year} AJ";
    public override string FormatJavaneseDay(int day)   => day.ToString();

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} AJ {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Tibetan Calendar =====

    public override string CalendarTibetanName => "Calendario Tibetano";

    public override string FormatTibetanYear(int year)   => year.ToString();
    public override string FormatTibetanMonth(int month) => month.ToString();
    public override string FormatTibetanDay(int day)     => day.ToString();

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"Año {year}, Mes {month}, Día {day} (Tibetano) {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar =====

    public override string CalendarMayanName  => "Calendario de Cuenta Larga Maya";
    public override string CalendarMayanBaktun => "Baktún";
    public override string CalendarMayanKatun  => "Katún";
    public override string CalendarMayanTun    => "Tún";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar =====

    public override string CalendarInuitName => "Calendario Inuit";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "Siqinnaatchiaq", "Avunniit", "Nattian", "Tirigluit", "Amiraijaut",
        "Natsiviat", "Akulliit", "Siqinnaarut", "Akullirusiit", "Ukiuq",
        "Ukiumi Nasamat", "Siqinnginnami Tatqiq", "Tauvikjuaq"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => year.ToString();
    public override string FormatInuitDay(int day)   => day.ToString();

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Roman Calendar =====

    public override string CalendarRomanName => "Calendario Romano (AUC)";

    private static readonly string[] RomanMonthNames =
    {
        "", "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius",
        "Iulius", "Augustus", "September", "October", "November", "December"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"{year + 753} AUC";
    public override string FormatRomanDay(int day)   => day.ToString();

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? month.ToString();
        return $"{day} de {monthName} de {year + 753} AUC {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Lunar Calendar =====

    public override string CalendarChineseLunarName => "Calendario Lunar Chino (农历)";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "",
        "Mes 1", "Mes 2", "Mes 3", "Mes 4", "Mes 5", "Mes 6",
        "Mes 7", "Mes 8", "Mes 9", "Mes 10", "Mes 11", "Mes 12"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "",
        "1°", "2°", "3°", "4°", "5°", "6°", "7°", "8°", "9°", "10°",
        "11°", "12°", "13°", "14°", "15°", "16°", "17°", "18°", "19°", "20°",
        "21°", "22°", "23°", "24°", "25°", "26°", "27°", "28°", "29°", "30°"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Bisiesto ";
    public override string CalendarComponentIsLeap => "Mes bisiesto";
    public override string FormatChineseLunarYear(int year) => year.ToString();

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? month.ToString();
        var dayName    = GetChineseLunarDayName(day) ?? day.ToString();
        return $"{year} {leapPrefix}{monthName} {dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar =====

    public override string CalendarVietnameseName => "Calendario Lunar Vietnamita (Âm lịch)";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "Tháng Giêng", "Tháng Hai", "Tháng Ba", "Tháng Tư", "Tháng Năm", "Tháng Sáu",
        "Tháng Bảy", "Tháng Tám", "Tháng Chín", "Tháng Mười", "Tháng Mười Một", "Tháng Chạp"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Tý (Rata)", "Sửu (Búfalo)", "Dần (Tigre)", "Mão (Gato)",
        "Thìn (Dragón)", "Tỵ (Serpiente)", "Ngọ (Caballo)", "Mùi (Cabra)",
        "Thân (Mono)", "Dậu (Gallo)", "Tuất (Perro)", "Hợi (Cerdo)"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "nhuận ";
    public override string CalendarComponentZodiac => "Zodíaco";
    public override string FormatVietnameseYear(int year) => year.ToString();
    public override string FormatVietnameseDay(int day)   => day.ToString();

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix  = isLeap ? VietnameseLeapPrefix : "";
        var monthName   = GetVietnameseMonthName(month) ?? $"Tháng {month}";
        var zodiacName  = GetVietnameseZodiacName(zodiac) ?? "";
        return $"Año {zodiacName} {leapPrefix}{monthName} día {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar =====

    public override string CalendarJapaneseName => "Calendario Japonés (Nengo)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa (令和)", "Heisei (平成)", "Showa (昭和)", "Taisho (大正)", "Meiji (明治)" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Era";
    public override string FormatJapaneseYear(int year) => year.ToString();
    public override string FormatJapaneseDay(int day)   => day.ToString();

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{eraName} {year}, {day} de {monthName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar =====

    public override string CalendarYiName => "Calendario Solar Yi (彝历)";
    public override string CalendarComponentYiSeason => "Estación";
    public override string CalendarComponentYiXun    => "Xun (Década)";

    private static readonly string[] YiSeasonNames = { "Madera", "Fuego", "Tierra", "Metal", "Agua" };
    private static readonly string[] YiXunNames    = { "Xun superior", "Xun medio", "Xun inferior" };
    private static readonly string[] YiAnimalNames = { "Tigre", "Conejo", "Dragón", "Serpiente", "Caballo", "Cabra", "Mono", "Gallo", "Perro", "Cerdo", "Rata", "Buey" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "Año Nuevo (大年)",
        11 => "Medio Año (小年)",
        >= 1 and <= 10 => $"{(month % 2 == 1 ? "Masculino" : "Femenino")} Mes de {YiSeasonNames[(month - 1) / 2]}",
        _  => null
    };

    public override string FormatYiYear(int year) => year.ToString();
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]}, {YiAnimalNames[animal]}";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"Mes {month}";
        var dayStr    = month is 0 or 11 ? $"Día {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"Yi {year} [{zodiac}] {monthName}, {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar =====

    public override string CalendarSexagenaryName    => "Ciclo Sexagenario Chino (Ganzhi)";
    public override string CalendarComponentYearStem   => "Tronco del año";
    public override string CalendarComponentYearBranch => "Rama del año";
    public override string CalendarComponentMonthStem   => "Tronco del mes";
    public override string CalendarComponentMonthBranch => "Rama del mes";
    public override string CalendarComponentDayStem   => "Tronco del día";
    public override string CalendarComponentDayBranch => "Rama del día";

    private static readonly string[] SexagenaryStemNames =
        { "Jiǎ", "Yǐ", "Bǐng", "Dīng", "Wù", "Jǐ", "Gēng", "Xīn", "Rén", "Guǐ" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zǐ", "Chǒu", "Yín", "Mǎo", "Chén", "Sì", "Wǔ", "Wèi", "Shēn", "Yǒu", "Xū", "Hài" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Rata", "Buey", "Tigre", "Conejo", "Dragón", "Serpiente", "Caballo", "Cabra", "Mono", "Gallo", "Perro", "Cerdo" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)     ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch) ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch) ?? "?";
        var ms = GetSexagenaryStemName(monthStem)    ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch)?? "?";
        var ds = GetSexagenaryStemName(dayStem)      ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)  ?? "?";
        return $"Año {ys}{yb} [{zo}] Mes {ms}{mb} Día {ds}{db} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dai Calendar (Xishuangbanna) =====

    public override string CalendarDaiName => "Calendario Dai de Xishuangbanna (小傣历)";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Mes 1", "Mes 2", "Mes 3", "Mes 4",  "Mes 5",  "Mes 6",
        "Mes 7", "Mes 8", "Mes 9", "Mes 10", "Mes 11", "Mes 12",
        "Mes bisiesto 9"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"Año {year}";

    public override string FormatDaiDay(int day) => $"Día {day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Bisiesto " : "") + (GetDaiMonthName(month) ?? $"Mes {month}");
        return $"Año Dai {year}, {monthName}, Día {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar =====

    public override string CalendarDehongDaiName => "Calendario Dai de Dehong (德宏大傣历)";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Mes 1", "Mes 2", "Mes 3", "Mes 4",  "Mes 5",  "Mes 6",
        "Mes 7", "Mes 8", "Mes 9", "Mes 10", "Mes 11", "Mes 12",
        "Mes bisiesto 9"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"Año {year}";

    public override string FormatDehongDaiDay(int day) => $"Día {day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Bisiesto " : "") + (GetDehongDaiMonthName(month) ?? $"Mes {month}");
        return $"Año Dai de Dehong {year}, {monthName}, Día {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[Chat] Respondido a \"{partnerName}\": {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Grupo] Hablado en sesión {sessionId}: {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Herramienta] Herramientas llamadas: {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Tarea] Tarea ejecutada, resultado: {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Temporizador] Temporizador activado, respondido: {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[Temporizador] Error en la ejecución del temporizador '{timerName}': {error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ El temporizador '{timerName}' comenzó a ejecutar...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ Temporizador '{timerName}' completado\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ Temporizador '{timerName}' falló: {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Gestión] Creado nuevo ser de silicio \"{name}\" ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Gestión] Reiniciado ser {id} a implementación predeterminada";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Tarea Completada] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Tarea Fallida] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "Sistema iniciado, estoy en línea";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Error de ejecución] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Sistema de memoria no disponible";
    public override string MemoryToolMissingAction => "Falta el parámetro 'action'";
    public override string MemoryToolMissingContent => "Falta el parámetro 'content'";
    public override string MemoryToolNoMemories => "Aún no hay memorias";
    public override string MemoryToolRecentHeader(int count) => $"Últimas {count} memorias:";
    public override string MemoryToolStatsHeader => "Estadísticas de memoria:";
    public override string MemoryToolStatsTotal => "- Total";
    public override string MemoryToolStatsOldest => "- Más antigua";
    public override string MemoryToolStatsNewest => "- Más reciente";
    public override string MemoryToolStatsNA => "N/D";
    public override string MemoryToolQueryNoResults => "No se encontraron memorias en este rango de tiempo";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc}: {count} memorias encontradas:";
    public override string MemoryToolInvalidYear => "Parámetro 'year' inválido";
    public override string MemoryToolUnknownAction(string action) => $"Acción desconocida: {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Variable",
        "function" => "Función",
        "class" => "Clase",
        "keyword" => "Palabra clave",
        "comment" => "Comentario",
        "namespace" => "Espacio de nombres",
        "parameter" => "Parámetro",
        _ => "Identificador"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"Definición e información de uso para la variable '{encodedWord}'",
            "function" => $"Firma y descripción para la función '{encodedWord}'",
            "class" => $"Estructura y descripción para la clase '{encodedWord}'",
            "keyword" => $"Sintaxis y uso para la palabra clave '{encodedWord}'",
            "comment" => $"Palabra '{encodedWord}' en comentario",
            "namespace" => $"Información para el espacio de nombres '{encodedWord}'",
            "parameter" => $"Definición y uso del parámetro '{encodedWord}'",
            _ => $"Información para el identificador '{encodedWord}'"
        };
    }

    public override string GetCodeHoverKeywordDesc(string language, string keyword)
    {
        var key = $"{language}:{keyword.ToLower()}";
        return CSharpKeywords.GetValueOrDefault(key, "");
    }

    public override string GetTranslation(string key)
    {
        return TranslationDictionary.GetValueOrDefault(key, "");
    }

    private static readonly Dictionary<string, string> CSharpKeywords = new()
    {
        { "csharp:if", "Declaración de rama condicional. Ejecuta un bloque de código cuando la expresión de condición es verdadera." },
        { "csharp:else", "Ruta de rama alternativa. Usado con if, ejecuta cuando la condición es falsa." },
        { "csharp:for", "Declaración de bucle de conteo. Contiene partes de inicialización, condición e iteración." },
        { "csharp:while", "Declaración de bucle condicional. Repite un bloque de código mientras la condición sea verdadera." },
        { "csharp:do", "Declaración de bucle de prueba posterior. Ejecuta el bloque de código una vez, luego verifica la condición." },
        { "csharp:switch", "Declaración de múltiples ramas. Coincide el valor de una expresión con diferentes ramas case." },
        { "csharp:case", "Etiqueta de rama en una declaración switch. Ejecuta el bloque correspondiente cuando coincide." },
        { "csharp:break", "Declaración de salida. Termina inmediatamente el bucle o declaración switch más cercano." },
        { "csharp:continue", "Declaración de continuación. Omite el resto de la iteración actual del bucle." },
        { "csharp:return", "Declaración de retorno. Sale del método actual y opcionalmente retorna un valor." },
        { "csharp:goto", "Declaración de salto. Salta incondicionalmente a una etiqueta especificada." },
        { "csharp:foreach", "Declaración de iteración de colección. Visita cada elemento en una colección." },
        { "csharp:class", "Declaración de tipo de referencia. Define una estructura que contiene datos (campos, propiedades) y comportamiento (métodos)." },
        { "csharp:interface", "Declaración de interfaz. Define un contrato que las clases o structs deben implementar." },
        { "csharp:struct", "Declaración de tipo de valor. Estructura de datos ligera asignada en la pila." },
        { "csharp:enum", "Declaración de tipo de enumeración. Define un conjunto de constantes enteras nombradas." },
        { "csharp:namespace", "Declaración de espacio de nombres. Un contenedor lógico para organizar código y evitar conflictos de nombres." },
        { "csharp:record", "Declaración de tipo record. Un tipo de referencia con semántica de valor, adecuado para datos inmutables." },
        { "csharp:delegate", "Declaración de delegado. Una referencia de método segura en tipos usada para eventos y callbacks." },
        { "csharp:public", "Modificador de acceso público. El miembro es accesible desde cualquier código." },
        { "csharp:private", "Modificador de acceso privado. El miembro es accesible solo dentro del tipo que lo contiene." },
        { "csharp:protected", "Modificador de acceso protegido. El miembro es accesible dentro del tipo que lo contiene y los tipos derivados." },
        { "csharp:internal", "Modificador de acceso interno. El miembro es accesible solo dentro del mismo ensamblado." },
        { "csharp:sealed", "Modificador sealed. Previene que una clase sea heredada o un método sea sobrescrito." },
        { "csharp:int", "Tipo de entero con signo de 32 bits (alias de System.Int32)." },
        { "csharp:string", "Tipo de cadena (alias de System.String). Representa una secuencia inmutable de caracteres Unicode." },
        { "csharp:bool", "Tipo booleano (alias de System.Boolean). El valor es true o false." },
        { "csharp:float", "Tipo de punto flotante de precisión simple de 32 bits (alias de System.Single)." },
        { "csharp:double", "Tipo de punto flotante de precisión doble de 64 bits (alias de System.Double)." },
        { "csharp:decimal", "Tipo decimal de alta precisión de 128 bits, adecuado para cálculos financieros." },
        { "csharp:char", "Tipo de carácter Unicode de 16 bits (alias de System.Char)." },
        { "csharp:byte", "Tipo de entero sin signo de 8 bits (alias de System.Byte)." },
        { "csharp:object", "Tipo base para todos los tipos (alias de System.Object)." },
        { "csharp:var", "Variable local de tipo implícito. El compilador infiere el tipo de la expresión de inicialización." },
        { "csharp:dynamic", "Tipo dinámico. Omite la verificación de tipos en tiempo de compilación, resuelto en tiempo de ejecución." },
        { "csharp:void", "Tipo sin retorno. Indica que un método no retorna un valor." },
        { "csharp:static", "Modificador estático. Pertenece al tipo mismo en lugar de una instancia específica." },
        { "csharp:abstract", "Modificador abstracto. Indica una implementación incompleta que debe ser completada por clases derivadas." },
        { "csharp:virtual", "Modificador virtual. El método o propiedad puede ser sobrescrito en clases derivadas." },
        { "csharp:override", "Modificador override. Proporciona una nueva implementación de un método virtual o abstracto de la clase base." },
        { "csharp:const", "Modificador constante. Un valor inmutable determinado en tiempo de compilación." },
        { "csharp:readonly", "Modificador de solo lectura. Solo puede ser asignado en la declaración o el constructor." },
        { "csharp:volatile", "Modificador volátil. Indica que un campo puede ser modificado por múltiples hilos concurrentemente." },
        { "csharp:async", "Modificador async. Marca un método como que contiene operaciones asíncronas, típicamente usado con await." },
        { "csharp:await", "Operador await. Suspende la ejecución del método hasta que una operación asíncrona se complete." },
        { "csharp:partial", "Modificador partial. Permite que una clase, struct o interfaz se divida en múltiples archivos." },
        { "csharp:ref", "Parámetro de referencia. Pasa un parámetro por referencia." },
        { "csharp:out", "Parámetro de salida. Usado para retornar múltiples valores desde un método." },
        { "csharp:in", "Parámetro de referencia de solo lectura. Pasa por referencia pero no permite modificación." },
        { "csharp:params", "Modificador params. Permite un número variable de argumentos del mismo tipo." },
        { "csharp:try", "Bloque de manejo de excepciones. Contiene código que puede lanzar una excepción." },
        { "csharp:catch", "Bloque de captura de excepciones. Maneja excepciones lanzadas en un bloque try." },
        { "csharp:finally", "Bloque finally. Código que se ejecuta independientemente de si ocurrió una excepción." },
        { "csharp:throw", "Declaración throw. Lanza manualmente un objeto de excepción." },
        { "csharp:new", "Operador de creación de instancia. Crea un objeto o llama a un constructor." },
        { "csharp:this", "Referencia de instancia actual. Se refiere a la instancia de clase actual." },
        { "csharp:base", "Referencia de clase base. Se refiere a los miembros de la clase base directa." },
        { "csharp:using", "Directiva o declaración. Importa un espacio de nombres o asegura que los recursos IDisposable se liberen." },
        { "csharp:yield", "Palabra clave iterator. Retorna valores uno a la vez, permitiendo ejecución diferida." },
        { "csharp:lock", "Declaración lock. Asegura que solo un hilo ejecute un bloque de código a la vez." },
        { "csharp:typeof", "Operador typeof. Obtiene el objeto System.Type para un tipo." },
        { "csharp:nameof", "Operador nameof. Obtiene el nombre de cadena de una variable, tipo o miembro." },
        { "csharp:is", "Operador de verificación de tipo. Verifica si un objeto es compatible con un tipo especificado." },
        { "csharp:as", "Operador de conversión de tipo. Intenta de forma segura una conversión de tipo, retornando null en caso de fallo." },
        { "csharp:null", "Literal null. Representa una referencia null para tipos de referencia o anulables." },
        { "csharp:true", "Valor booleano verdadero." },
        { "csharp:false", "Valor booleano falso." },
        { "csharp:default", "Expresión de valor predeterminado. Obtiene el valor predeterminado de un tipo (null para tipos de referencia, 0 para tipos numéricos)." },
        { "csharp:operator", "Declaración de operador. Define el comportamiento del operador en un tipo personalizado." },
        { "csharp:explicit", "Declaración de conversión explícita. Un operador de conversión que requiere un cast explícito." },
        { "csharp:implicit", "Declaración de conversión implícita. Un operador de conversión que puede realizarse automáticamente." },
        { "csharp:unchecked", "Bloque unchecked. Deshabilita la verificación de desbordamiento para aritmética de enteros." },
        { "csharp:checked", "Bloque checked. Habilita la verificación de desbordamiento para aritmética de enteros." },
        { "csharp:fixed", "Declaración fixed. Fija una ubicación de memoria para evitar que la recolección de basura la mueva." },
        { "csharp:stackalloc", "Operador stackalloc. Asigna un bloque de memoria en la pila." },
        { "csharp:extern", "Modificador externo. Indica que un método está implementado en un ensamblado externo (ej. un DLL)." },
        { "csharp:unsafe", "Bloque de código unsafe. Permite el uso de características inseguras como punteros." },
        // Platform core types
        { "csharp:ipermissioncallback", "Interfaz de callback de permisos. Usado para evaluar varios permisos de operación para seres de silicio (red, línea de comandos, acceso a archivos, etc.)." },
        { "csharp:permissionresult", "Enum de resultado de permiso. Representa el resultado de la evaluación de permisos: Allowed, Denied, AskUser." },
        { "csharp:permissiontype", "Enum de tipo de permiso. Define el tipo de permiso: NetworkAccess, CommandLine, FileAccess, Function, DataAccess." },
        // System.Net
        { "csharp:ipaddress", "Clase de dirección IP (System.Net.IPAddress). Representa una dirección de Protocolo de Internet (IP)." },
        { "csharp:addressfamily", "Enum de familia de direcciones (System.Net.Sockets.AddressFamily). Especifica el esquema de direccionamiento para una dirección de red, como InterNetwork (IPv4) o InterNetworkV6 (IPv6)." },
        // System
        { "csharp:uri", "Clase de Identificador Uniforme de Recursos (System.Uri). Proporciona una representación de objeto de un URI para acceder a recursos web." },
        { "csharp:operatingsystem", "Clase de sistema operativo (System.OperatingSystem). Proporciona métodos estáticos para verificar el sistema operativo actual, como IsWindows(), IsLinux(), IsMacOS()." },
        { "csharp:environment", "Clase de entorno (System.Environment). Proporciona información sobre y medios para manipular el entorno y plataforma actuales." },
        // System.IO
        { "csharp:path", "Clase de ruta (System.IO.Path). Realiza operaciones en instancias de String que contienen información de ruta de archivo o directorio." },
        // System.Collections.Generic
        { "csharp:hashset", "Clase de hash set (System.Collections.Generic.HashSet<T>). Representa un conjunto de valores, proporcionando operaciones de conjunto de alto rendimiento." },
        // System.Text
        { "csharp:stringbuilder", "Clase de constructor de cadenas (System.Text.StringBuilder). Representa una cadena mutable de caracteres, adecuada para escenarios con modificaciones frecuentes de cadenas." },
    };

    // Full namespace translation dictionary
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // Add full namespace keys
        { "csharp:System.Net.IPAddress", "Clase de dirección IP (System.Net.IPAddress). Representa una dirección de Protocolo de Internet (IP)." },
        { "csharp:System.Net.Sockets.AddressFamily", "Enum de familia de direcciones (System.Net.Sockets.AddressFamily). Especifica el esquema de direccionamiento para una dirección de red, como InterNetwork (IPv4) o InterNetworkV6 (IPv6)." },
        { "csharp:System.Uri", "Clase de Identificador Uniforme de Recursos (System.Uri). Proporciona una representación de objeto de un URI para acceder a recursos web." },
        { "csharp:System.OperatingSystem", "Clase de sistema operativo (System.OperatingSystem). Proporciona métodos estáticos para verificar el sistema operativo actual, como IsWindows(), IsLinux(), IsMacOS()." },
        { "csharp:System.Environment", "Clase de entorno (System.Environment). Proporciona información sobre y medios para manipular el entorno y plataforma actuales." },
        { "csharp:System.IO.Path", "Clase de ruta (System.IO.Path). Realiza operaciones en instancias de String que contienen información de ruta de archivo o directorio." },
        { "csharp:System.Collections.Generic.HashSet", "Clase de hash set (System.Collections.Generic.HashSet<T>). Representa un conjunto de valores, proporcionando operaciones de conjunto de alto rendimiento." },
        { "csharp:System.Text.StringBuilder", "Clase de constructor de cadenas (System.Text.StringBuilder). Representa una cadena mutable de caracteres, adecuada para escenarios con modificaciones frecuentes de cadenas." },
    };

    // ===== Help Module UI Labels =====

    public override string Help_Title => "Documentación de Ayuda";
    public override string Help_Search => "Buscar Ayuda";
    public override string Help_DocList => "Lista de Documentos";
    public override string Help_NoResults => "No se encontraron documentos";
    public override string Help_Previous => "Anterior";
    public override string Help_Next => "Siguiente";
}
