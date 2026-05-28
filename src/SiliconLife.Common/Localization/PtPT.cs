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
using SiliconLife.Common.Calendar.ChineseHistorical;

namespace SiliconLife.Common.Localization;

/// <summary>
/// Portuguese (Portugal) localization implementation
/// </summary>
public class PtPT : DefaultLocalizationBase
{
    public override string LanguageCode => "pt-PT";
    public override string LanguageName => "Português (Portugal)";
    public override string WelcomeMessage => "Bem-vindo ao Silicon Life Collective!";
    public override string BrandName => "Silicon Life Collective";
    public override string InputPrompt => "> ";
    public override string ShutdownMessage => "A encerrar...";
    public override string ConfigCorruptedError => "Ficheiro de configuração corrompido, a usar configuração predefinida";
    public override string ConfigCreatedWithDefaults => "Ficheiro de configuração não encontrado, configuração predefinida criada";
    public override string AIConnectionError => "Não é possível ligar ao serviço de IA, verifique se o Ollama está em execução";
    public override string AIRequestError => "Falha no pedido de IA";
    public override string DataDirectoryCreateError => "Não é possível criar o diretório de dados";
    public override string ThinkingMessage => "A pensar...";
    public override string ToolCallMessage => "A executar ferramentas...";
    public override string ErrorMessage => "Erro";
    public override string UnexpectedErrorMessage => "Erro inesperado";
    public override string PermissionDeniedMessage => "Permissão recusada";
    public override string PermissionAskPrompt => "Permitir? (s/n) : ";
    public override string PermissionRequestHeader => "[Pedido de permissão]";
    public override string PermissionRequestDescription => "Um Silicon Being está a pedir a sua permissão:";
    public override string PermissionRequestTypeLabel => "Tipo de permissão:";
    public override string PermissionRequestResourceLabel => "Recurso solicitado:";
    public override string PermissionRequestAllowButton => "Permitir";
    public override string PermissionRequestDenyButton => "Recusar";
    public override string PermissionRequestCacheLabel => "Memorizar esta decisão";
    public override string PermissionRequestDurationLabel => "Duração da cache";
    public override string PermissionRequestWaitingMessage => "A aguardar resposta...";
    public override string AllowCodeLabel => "Código de permissão";
    public override string DenyCodeLabel => "Código de recusa";
    public override string PermissionReplyInstruction => "Introduza o código de confirmação ou qualquer outro texto para recusar";
    public override string AddToCachePrompt => "Memorizar esta decisão? (s/n) : ";
    public override string PermissionCacheLabel => "Memorizar esta decisão";
    public override string PermissionCacheDurationLabel => "Duração da cache";
    public override string PermissionCacheDuration1Hour => "1 hora";
    public override string PermissionCacheDuration24Hours => "24 horas";
    public override string PermissionCacheDuration7Days => "7 dias";
    public override string PermissionCacheDuration30Days => "30 dias";
    public override string ProjectGroupChatPrefix => "Grupo de projeto";
    public override string ProjectBroadcastPrefix => "Difusão de projeto";

    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Acesso à rede",
        PermissionType.CommandLine => "Execução em linha de comandos",
        PermissionType.FileAccess => "Acesso a ficheiro",
        PermissionType.Function => "Chamada de função",
PermissionType.DataAccess => "Acesso a dados",
PermissionType.ToolAction => "Ação de ferramenta",
_ => permissionType.ToString()
    };

    public override string PermissionDialogTitle => "Pedido de permissão";
    public override string PermissionTypeLabel => "Tipo de permissão:";
    public override string PermissionResourceLabel => "Recurso solicitado:";
    public override string PermissionDetailLabel => "Informações detalhadas:";
    public override string PermissionAllowButton => "Permitir";
    public override string PermissionDenyButton => "Recusar";
    public override string PermissionRespondFailed => "Falha na resposta de permissão";
    public override string PermissionRespondError => "Erro na resposta de permissão: ";

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Inicialização";
    public override string InitDescription => "Primeira utilização, por favor complete a configuração base";
    public override string InitNicknameLabel => "Nome de utilizador";
    public override string InitNicknamePlaceholder => "Por favor introduza o seu pseudónimo";
    public override string InitEndpointLabel => "Ponto de acesso API IA";
    public override string InitEndpointPlaceholder => "ex: http://localhost:11434";
    public override string InitAIClientTypeLabel => "Tipo de cliente IA";
    public override string InitModelLabel => "Modelo predefinido";
    public override string InitModelPlaceholder => "ex: qwen3.5:cloud";
    public override string InitSkinLabel => "Tema";
    public override string InitSkinPlaceholder => "Deixar vazio para o tema predefinido";
    public override string InitDataDirectoryLabel => "Diretório de dados";
    public override string InitDataDirectoryPlaceholder => "ex: ./data";
    public override string InitDataDirectoryBrowse => "Procurar...";
    public override string InitSkinSelected => "\u2713 Selecionado";
    public override string InitSkinPreviewTitle => "Pré-visualização";
    public override string InitSkinPreviewCardTitle => "Título do cartão";
    public override string InitSkinPreviewCardContent => "Este é um exemplo de cartão que mostra o efeito visual deste tema.";
    public override string InitSkinPreviewPrimaryBtn => "Botão principal";
    public override string InitSkinPreviewSecondaryBtn => "Botão secundário";
    public override string InitSubmitButton => "Concluir inicialização";
    public override string InitFooterHint => "A configuração pode ser alterada a qualquer momento nas definições";
    public override string InitHelpLink => "📖 Ver documentação de ajuda";
    public override string InitAIClientHelpPrefix => "📖 Ver ajuda: ";
    public override string InitNicknameRequiredError => "Por favor introduza um nome de utilizador";
    public override string InitDataDirectoryRequiredError => "Por favor selecione um diretório de dados";
    public override string InitCuratorNameLabel => "Nome do Silicon Being";
    public override string InitCuratorNamePlaceholder => "Por favor introduza o nome do primeiro Silicon Being";
    public override string InitCuratorNameRequiredError => "Por favor introduza um nome de Silicon Being";
    public override string InitLanguageLabel => "Idioma / Language";
    public override string InitLanguageSwitchBtn => "Aplicar";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Painel";
    public override string NavMenuBeings => "Silicon Beings";
    public override string NavMenuUsage => "Utilização";
    public override string NavMenuAudit => "Auditoria";
    public override string NavMenuTasks => "Tarefas";
    public override string NavMenuMemory => "Memória";
    public override string NavMenuKnowledge => "Conhecimentos";
    public override string NavMenuProjects => "Projetos";
    public override string NavMenuLogs => "Registos";
    public override string NavMenuConfig => "Configuração";
    public override string NavMenuHelp => "Ajuda";
    public override string NavMenuAbout => "Acerca";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Painel - Silicon Life Collective";
    public override string PageTitleBeings => "Gestão de Silicon Beings - Silicon Life Collective";
    public override string PageTitleTasks => "Gestão de tarefas - Silicon Life Collective";
    public override string PageTitleTimers => "Gestão de temporizadores - Silicon Life Collective";
    public override string PageTitleMemory => "Pesquisa na memória - Silicon Life Collective";
    public override string PageTitleWorkNotes => "Notas de trabalho - Silicon Life Collective";
    public override string PageTitleKnowledge => "Grafo de conhecimentos - Silicon Life Collective";
    public override string PageTitleProjects => "Gestão de espaços de projeto - Silicon Life Collective";
    public override string PageTitleLogs => "Consulta de registos - Silicon Life Collective";
    public override string PageTitleUsage => "Utilização de tokens - Silicon Life Collective";
    public override string PageTitleAudit => "Auditoria de permissões - Silicon Life Collective";
    public override string PageTitleConfig => "Configuração do sistema - Silicon Life Collective";
    public override string PageTitleExecutor => "Monitorização de executores - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Navegador de código - Silicon Life Collective";
    public override string PageTitlePermission => "Gestão de permissões - Silicon Life Collective";
    public override string PageTitleAbout => "Acerca - Silicon Life Collective";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "Consulta da memória";
    public override string WorkNotesPageHeader => "Notas de trabalho";
    public override string WorkNotesBackToPrevious => "← Voltar";
    public override string WorkNotesTotalPages => "Total {0} páginas";
    public override string WorkNotesEmptyState => "Sem notas de trabalho de momento";
    public override string WorkNotesSearchPlaceholder => "Pesquisar notas...";
    public override string WorkNotesSearchButton => "Pesquisar";
    public override string WorkNotesNoSearchResults => "Nenhuma nota correspondente encontrada";
    public override string MemoryEmptyState => "Sem dados na memória de momento";
    public override string MemorySearchPlaceholder => "Pesquisar na memória...";
    public override string MemorySearchButton => "Pesquisar";
    public override string MemoryFilterAll => "Tudo";
    public override string MemoryFilterSummaryOnly => "Apenas resumos";
    public override string MemoryFilterOriginalOnly => "Apenas originais";
    public override string MemoryStatTotal => "Total de memórias";
    public override string MemoryStatOldest => "Memória mais antiga";
    public override string MemoryStatNewest => "Memória mais recente";
    public override string MemoryIsSummaryBadge => "Resumo comprimido";
    public override string MemoryPaginationPrev => "Página anterior";
    public override string MemoryPaginationNext => "Página seguinte";
    public override string MemoryFilterTypeLabel => "Tipo";
    public override string MemoryFilterDateFrom => "Data de início";
    public override string MemoryFilterDateTo => "Data de fim";
    public override string MemoryFilterApply => "Aplicar";
    public override string MemoryFilterReset => "Repor";
    public override string MemoryTypeChat => "Conversa";
    public override string MemoryTypeToolCall => "Chamada de ferramenta";
    public override string MemoryTypeTask => "Tarefa";
    public override string MemoryTypeTimer => "Temporizador";
    public override string MemoryDetailTitle => "Detalhes da memória";
    public override string MemoryDetailClose => "Fechar";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "Conteúdo";
    public override string MemoryDetailCreatedAt => "Data de criação";
    public override string MemoryDetailRelatedBeings => "Agentes relacionados";
    public override string MemoryDetailKeywords => "Palavras-chave";
    public override string MemoryStatTypeDistribution => "Distribuição por tipo";
    public override string MemoryStatKeywordFrequency => "Frequência das palavras-chave";
    public override string MemoryCardViewDetail => "Ver detalhes";
    public override string MemoryTimelineEmptyState => "Sem dados na memória";
    public override string MemoryYearSummaryLabel => "Resumo anual";
    public override string MemoryMonthSummaryLabel => "Resumo mensal";
    public override string MemoryDaySummaryLabel => "Resumo diário";
    public override string MemoryHourSummaryLabel => "Resumo horário";
    public override string MemoryMinuteSummaryLabel => "Resumo por minuto";
    public override string MemorySummaryBadge => "Resumo comprimido";
    public override string MemoryTimelineYearFormat => "{0} ({1} entradas)";
    public override string MemoryTimelineMonthFormat => "{0}/{1} ({2} entradas)";
    public override string MemoryTimelineDayFormat => "{0}-{1}-{2} ({3} entradas)";
    public override string MemoryTimelineHourFormat => "{0}:00 ({1} entradas)";
    public override string MemoryTimelineMinuteFormat => "{0}:{1} ({2} entradas)";
    public override string MemoryRelatedBeingsLabel => "👥 Relacionados: {0} seres";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "Gestão de espaços de projeto";
    public override string ProjectsEmptyState => "Sem projetos de momento";
    public override string ProjectsActiveLabel => "Ativo";
    public override string ProjectsArchivedLabel => "Arquivado";
    public override string ProjectStatusActiveLabel => "Ativo";
    public override string ProjectStatusArchivedLabel => "Arquivado";
    public override string ProjectStatusDestroyedLabel => "Destruído";
    public override string ProjectTasksLinkLabel => "Tarefas";
    public override string ProjectWorkNotesLinkLabel => "Notas de trabalho";
    public override string ProjectWorkflowsLinkLabel => "Fluxos de trabalho";
    public override string ProjectGroupChatLinkLabel => "Chat de grupo";
    public override string ProjectBroadcastLinkLabel => "Difusão";
    public override string ProjectWorkflowsPageHeader => "Fluxos de trabalho do projeto";
    public override string ProjectWorkflowsEmptyState => "Este projeto ainda não tem fluxos de trabalho";
    public override string CreateWorkflowButton => "Criar fluxo de trabalho";
    public override string ActiveWorkflowsHeader => "Fluxos de trabalho ativos";
    public override string BackToProject => "Voltar ao projeto";
    public override string WorkflowCurrentStateLabel => "Estado atual:";
    public override string WorkflowCreatedByLabel => "Criado por:";
    public override string WorkflowUpdatedAtLabel => "Atualizado em:";
    public override string WorkflowBusinessKeyPrompt => "Por favor introduza a chave de negócio (ex: número PR, ID de incidente):";
    public override string WorkflowCreatedSuccess => "Fluxo de trabalho criado com sucesso!";
    public override string WorkflowCreateFailed => "Falha na criação:";
    public override string WorkflowDetailInProgress => "Funcionalidade de detalhe do fluxo de trabalho em desenvolvimento...";
    public override string WorkflowInstanceIdLabel => "ID da instância:";
    public override string WorkflowDetailPageHeader => "Detalhes do fluxo de trabalho";
    public override string WorkflowRoleAssignmentsHeader => "Atribuições de funções";
    public override string WorkflowUnassignedBeingsHeader => "Entidades de silício não atribuídas";
    public override string WorkflowNoUnassignedBeings => "Todas as entidades de silício estão atribuídas a funções";
    public override string WorkflowStateTransitionsHeader => "Transições de estado das tarefas";
    public override string WorkflowNoTemplateMessage => "Este projeto não tem modelo de fluxo de trabalho associado";
    public override string WorkflowNoRoleDefinitions => "Este modelo de fluxo de trabalho não define funções";
    public override string WorkflowNoTransitions => "Este modelo de fluxo de trabalho não define transições de estado";
    public override string WorkflowRoleAssignedCountLabel => "Atribuídos";
    public override string WorkflowRoleRequiredCountLabel => "Obrigatório";
    public override string WorkflowTransitionFromLabel => "De";
    public override string WorkflowTransitionToLabel => "Para";
    public override string WorkflowAssignRoleButton => "Atribuir função";
    public override string WorkflowRemoveFromRoleButton => "Remover";
    public override string WorkflowTerminalStateLabel => "Terminal";
    public override string WorkflowInitialStateLabel => "Inicial";
    public override string ProjectWorkNotesPageHeader => "Notas de trabalho do projeto";
    public override string ProjectWorkNotesEmptyState => "Este projeto ainda não tem notas de trabalho";
    public override string ProjectWorkNotesTotalPages => "Total de páginas: {0}";

    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "Navegador de código";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "Gestão de tarefas";
    public override string TasksEmptyState => "Sem tarefas de momento";
    public override string TasksStatusPending => "Pendente";
    public override string TasksStatusRunning => "Em execução";
    public override string TasksStatusCompleted => "Concluído";
    public override string TasksStatusFailed => "Falhado";
    public override string TasksStatusCancelled => "Cancelado";
    public override string TasksPriorityLabel => "Prioridade";
    public override string TasksAssignedToLabel => "Atribuído a";
    public override string TasksCreatedAtLabel => "Data de criação";
    public override string TaskViewExecutionHistory => "Ver histórico de execução";

    public override string ProjectTasksPageHeader => "Tarefas do projeto";

    public override string ProjectTasksEmptyState => "Sem tarefas de projeto de momento";

    public override string ProjectTasksAssigneesLabel => "Responsáveis";

    public override string ProjectTasksCreatedByLabel => "Criado por";

    public override string ProjectTasksBackToProjects => "← Voltar à lista de projetos";

    public override string ProjectTasksNoAssigneesLabel => "Nenhum";

    public override string ProjectCreateButton => "Criar projeto";
    public override string ProjectCreateModalTitle => "Criar novo projeto";
    public override string ProjectCreateNameLabel => "Nome do projeto";
    public override string ProjectCreateDescriptionLabel => "Descrição";
    public override string ProjectCreateWorkflowLabel => "Modelo de fluxo de trabalho";
    public override string ProjectCreateNoWorkflow => "Nenhum (orientado pelo curator)";
    public override string ProjectCreateSubmitButton => "Criar";
    public override string ProjectCreateCancelButton => "Cancelar";
    public override string ProjectCreateNameRequired => "O nome do projeto é obrigatório";
    public override string ProjectCreateSuccess => "Projeto criado com sucesso";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "Monitorização de executores";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "Gestão de permissões";
    public override string PermissionEmptyState => "Sem regras de permissão de momento";
    public override string PermissionMissingBeingId => "Parâmetro ID do Silicon Being em falta";
    public override string PermissionBeingNotFound => "Silicon Being não encontrado";
    public override string PermissionTemplateHeader => "Modelo de callback de permissão predefinido";
    public override string PermissionTemplateDescription => "Após guardar, o comportamento predefinido será substituído; após eliminar, será restaurado";
    public override string PermissionCallbackClassSummary => "Implementação do callback de permissão.";
    public override string PermissionCallbackClassSummary2 => "Regras de permissão específicas do domínio, totalmente conformes com a especificação dpf.txt.\n/// Cobertura: Rede (lista branca/preta/intervalos IP), Linha de comandos (multiplataforma),\n/// Acesso a ficheiros (extensões perigosas, diretórios de sistema, diretório do utilizador) e valores predefinidos de fallback.";
    public override string PermissionCallbackConstructorSummary => "Cria um PermissionCallback com o diretório de dados da aplicação.";
    public override string PermissionCallbackConstructorSummary2 => "O diretório de dados da aplicação é utilizado para:\n    /// - Bloquear o acesso ao diretório de dados (exceto a sua própria subdiretoria temporária)\n    /// - Derivar o diretório de dados do Silicon Being para regras de permissão temporárias";
    public override string PermissionCallbackConstructorParam => "Caminho do diretório de dados global da aplicação";
    public override string PermissionCallbackEvaluateSummary => "Avalia um pedido de permissão de acordo com as regras (especificação dpf.txt).";
    public override string PermissionRuleOtherTypesDefault => "Os outros tipos de permissão são permitidos por predefinição";

    public override string GetPermissionRuleComment(string key) => key switch
    {
        "NetRuleNetworkAccess" => "Regra de permissão de operação de rede",
        "NetRuleCommandLine" => "Regra de linha de comandos (multiplataforma)",
        "NetRuleFileAccess" => "Regra de acesso a ficheiros (multiplataforma)",
        "NetRuleNoProtocol" => "Sem nome de protocolo (sem dois pontos), origem impossível de determinar, perguntar ao utilizador",
        "NetRuleLoopback" => "Permitir endereço de loopback (localhost / 127.0.0.1 / ::1)",
        "NetRulePrivateIPMatch" => "Correspondência de intervalo de endereços IP privados (verificar primeiro o primeiro endereço IPv4 válido)",
        "NetRulePrivateC" => "Permitir intervalo de endereços privados de classe C (192.168.0.0/16)",
        "NetRulePrivateA" => "Permitir intervalo de endereços privados de classe A (10.0.0.0/8)",
        "NetRulePrivateB" => "Permitir seletivamente intervalo de endereços privados de classe B (172.16.0.0/12, ou seja 172.16.* ~ 172.31.*)",
        "NetRuleDomainWhitelist1" => "Lista branca de domínios externos permitidos — Google / Bing / Tencent / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba",
        "NetRuleVideoPlatforms" => "Bilibili / niconico / Acfun / Douyin / TikTok / Kuaishou / Xiaohongshu",
        "NetRuleAIServices" => "Serviços de IA — OpenAI / Anthropic / HuggingFace / Ollama / Tongyi Qianwen / Kimi / Doubao / Jianying / Trae IDE",
        "NetRulePhishingBlacklist" => "Lista negra de sites de phishing/imitação (correspondência difusa por palavras-chave)",
        "NetRulePhishingAI" => "Site de imitação de IA",
        "NetRuleMaliciousAI" => "Ferramenta de IA maliciosa",
        "NetRuleAdversarialAI" => "IA adversária / Prompt jailbreak / Sites de ataque LLM",
        "NetRuleAIContentFarm" => "Fábrica de conteúdos de IA / Conteúdos spam de IA",
        "NetRuleAIBlackMarket" => "Mercado negro de dados de IA / Mercado negro de chaves API / Venda de pesos LLM",
        "NetRuleAIFakeScam" => "Imitação/fraude de IA — palavras-chave gerais",
        "NetRuleOtherBlacklist" => "Outros sites na lista negra — sakura-cat: não deve ser acedido pela IA / 4399: jogos misturados com vírus",
        "NetRuleSecuritiesTrading" => "Plataforma de negociação de valores mobiliários (perguntar ao utilizador) — Huatai Securities / Guotai Junan / CITIC Securities / China Merchants Securities / GF Securities / Haitong Securities / Shenwan Hongyuan / Orient Securities / Guosen Securities / Industrial Securities",
        "NetRuleThirdPartyTrading" => "Negociação de plataforma terceira (perguntar ao utilizador) — Tonghuashun / East Money / Tongdaxin / Bloomberg / Yahoo Finance",
        "NetRuleStockExchanges" => "Bolsas de valores (apenas dados de cotações) — Shanghai Stock Exchange / Shenzhen Stock Exchange / CNINFO",
        "NetRuleFinancialNews" => "Notícias financeiras (apenas dados de cotações) — JRJ / Securities Times / Hexun",
        "NetRuleInvestCommunity" => "Comunidade de investimento (apenas informações) — Xueqiu / CLS / Kaipanla / Taoguba",
        "NetRuleDevServices" => "Serviços de desenvolvimento — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        "NetRuleGameEngines" => "Motores de jogo — Unity / Unreal Engine / Epic Games / Fab Resource Store",
        "NetRuleGamePlatforms" => "Plataformas de jogo — Steam perguntar ao utilizador, EA / Ubisoft / Blizzard / Nintendo permitir",
        "NetRuleSEGA" => "SEGA (Japão)",
        "NetRuleCloudServices" => "Plataformas de serviços cloud globais — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        "NetRuleDevDeployTools" => "Ferramentas de desenvolvimento e deployment globais — GitLab / Bitbucket / Docker / Cloudflare",
        "NetRuleCloudDevTools" => "Serviços cloud e ferramentas de desenvolvimento — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / Chenguang Studio / W3School Chinese",
        "NetRuleChinaSocialNews" => "Social/Notícias (China continental) — Weibo / Zhihu / NetEase / Sina / ifeng / Xinhua / CCTV",
        "NetRuleTaiwanMediaCTI" => "Media de Taiwan — CTI News",
        "NetRuleTaiwanMediaSET" => "SET News (Taiwan) — Perguntar ao utilizador",
        "NetRuleTaiwanWIN" => "Agência de proteção de conteúdo Internet (Taiwan, risco de bloqueio) — Proibido",
        "NetRuleJapanMedia" => "Media japoneses — NHK",
        "NetRuleRussianMedia" => "Media russos — Sputnik News",
        "NetRuleKoreanMedia" => "Media coreanos — KBS / MBC / SBS / EBS",
        "NetRuleDPRKMedia" => "Media norte-coreanos — Uriminzokkiri / Rodong Sinmun / Youth Vanguard / Voice of Korea / Pyongyang Times / Chongryon",
        "NetRuleGovWebsites" => "Sites governamentais (domínio genérico .gov)",
        "NetRuleGlobalSocialCollab" => "Plataformas sociais/colaborativas globais — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        "NetRuleOverseasSocial" => "Social/Livestreaming internacional (perguntar ao utilizador) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        "NetRuleWhatsApp" => "WhatsApp (Meta) — Permitir",
        "NetRuleThreads" => "Threads (Meta) — Recusar",
        "NetRuleGlobalVideoMusic" => "Plataformas vídeo/música globais — Spotify / Apple Music / Vimeo",
        "NetRuleVideoMedia" => "Vídeo/Media — YouTube / iQIYI / Youku",
        "NetRuleMaps" => "Mapas — OpenStreetMap",
        "NetRuleEncyclopedia" => "Enciclopédia — Wikipedia / MediaWiki / Creative Commons (CC)",
        "NetRuleUnmatched" => "Acesso à rede não correspondido, perguntar ao utilizador",
        "CmdRuleSeparatorDetect" => "Detetar separadores de pipe e comandos múltiplos, validar individualmente",
        "CmdRuleWinAllow" => "Windows permitido: Comandos de apenas leitura/consulta — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        "CmdRuleWinDeny" => "Windows recusado: Comandos perigosos/destrutivos — del / rmdir / format / diskpart / reg delete",
        "CmdRuleLinuxAllow" => "Linux permitido: Comandos de apenas leitura/consulta — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        "CmdRuleLinuxDeny" => "Linux recusado: Comandos perigosos/destrutivos — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        "CmdRuleMacAllow" => "macOS permitido: Comandos de apenas leitura/consulta — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        "CmdRuleMacDeny" => "macOS recusado: Comandos perigosos/destrutivos — rm / rmdir / diskutil eraseVolume",
        "CmdRuleUnmatched" => "Linha de comandos não correspondida, perguntar ao utilizador",
        "FileRuleDangerousExtension" => "Extensões de ficheiro perigosas: .exe, .bat, .cmd, .ps1, .vbs, .js, .wsf, .msi, .scr, .dll, .so, .dylib",
        "FileRuleSystemDir" => "Diretório de sistema Windows: C:\\Windows, C:\\Program Files, C:\\Program Files (x86), C:\\ProgramData",
        "FileRuleSystemDirLinux" => "Diretório de sistema Linux: /etc, /usr, /bin, /sbin, /lib, /var, /boot",
        "FileRuleSystemDirMac" => "Diretório de sistema macOS: /System, /Library, /usr, /bin, /sbin",
        "FileRuleUserData" => "Diretório de dados do utilizador protegidos: Documentos, Ambiente de Trabalho, Downloads, Imagens, Vídeos, Música",
        "FileRuleAppDataDeny" => "Diretório de dados da aplicação recusado (exceto a sua própria subdiretoria temporária)",
        "FileRuleUnmatched" => "Acesso a ficheiro não correspondido, perguntar ao utilizador",
        _ => key
    };

    public override string PermissionRulesSection => "Lista de regras de permissão";
    public override string PermissionEditorSection => "Editor de regras de permissão";

    public override string PermissionSaveMissingBeingId => "ID do Silicon Being em falta ou inválido";
    public override string PermissionSaveMissingCode => "Código em falta no corpo do pedido";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader não disponível";
    public override string PermissionSaveRemoveFailed => "Eliminação do callback de permissão falhada";
    public override string PermissionSaveRemoveSuccess => "Callback de permissão eliminado";
    public override string PermissionSaveSecurityScanFailed => "Guarda do callback de permissão falhada (análise de segurança falhada)";
    public override string PermissionSaveCompilationFailed => "Compilação falhada";
    public override string PermissionSaveSuccess => "Callback de permissão guardado e aplicado com sucesso";
    public override string PermissionSaveError => "Erro ao guardar o callback de permissão";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "Grafo de conhecimentos";
    public override string KnowledgeLoadingState => "A carregar dados do grafo de conhecimentos...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat com {0}";
    public override string ChatConversationsHeader => "Conversas";
    public override string ChatNoConversationSelected => "Selecione uma conversa para conversar";
    public override string ChatMessageInputPlaceholder => "Introduza uma mensagem...";
    public override string ChatLoading => "A carregar...";
    public override string ChatSendButton => "Enviar";
    public override string ChatFileSourceDialogTitle => "Escolher origem do ficheiro";
    public override string ChatFileSourceServerFile => "Escolher ficheiro do servidor";
    public override string ChatFileSourceUploadLocal => "Carregar ficheiro local";
    public override string ChatUserDisplayName => "Eu";
    public override string ChatUserAvatarName => "Eu";
    public override string ChatDefaultBeingName => "IA";
    public override string ChatThinkingSummary => "💭 Processo de pensamento (clique para expandir)";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Chamadas de ferramentas ({count} itens)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Painel de controlo";
    public override string DashboardStatTotalBeings => "Número de Silicon Beings";
    public override string DashboardStatActiveBeings => "Silicon Beings ativos";
    public override string DashboardStatUptime => "Tempo de atividade";
    public override string DashboardStatMemory => "Utilização de memória";
    public override string DashboardChartMessageFrequency => "Frequência de mensagens";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Gestão de Silicon Beings";
    public override string BeingsTotalCount => "Total {0} Silicon Beings";
    public override string BeingsNoSelectionPlaceholder => "Selecione um Silicon Being para detalhes";
    public override string BeingsEmptyState => "Sem Silicon Beings de momento";
    public override string BeingsStatusIdle => "Inativo";
    public override string BeingsStatusRunning => "Em execução";
    public override string BeingsDetailIdLabel => "ID: ";
    public override string BeingsDetailStatusLabel => "Estado: ";
    public override string BeingsDetailCustomCompileLabel => "Compilação personalizada: ";
    public override string BeingsDetailSoulContentLabel => "Conteúdo da alma: ";
    public override string BeingsDetailSoulContentEditLink => "Editar alma";
    public override string BeingsBackToList => "Voltar à lista";
    public override string SoulEditorSubtitle => "Editar ficheiro Soul do Silicon Being (formato Markdown)";
    public override string BeingsDetailMemoryLabel => "Memória: ";
    public override string BeingsDetailMemoryViewLink => "Ver";
    public override string BeingsDetailPermissionLabel => "Permissão: ";
    public override string BeingsDetailPermissionEditLink => "Editar";
    public override string BeingsDetailTimersLabel => "Temporizadores: ";
    public override string BeingsDetailTasksLabel => "Tarefas: ";
    public override string BeingsDetailAIClientLabel => "Cliente IA independente: ";
    public override string BeingsDetailAIClientEditLink => "Editar";
    public override string BeingsDetailChatHistoryLabel => "Histórico de chat: ";
    public override string BeingsDetailWorkNoteLabel => "Nota de trabalho: ";
    public override string BeingsDetailChatHistoryLink => "Ver histórico";
    public override string BeingsDetailWorkNoteLink => "Ver nota de trabalho";
    public override string BeingsDetailToolAuthLabel => "Autorização de ferramentas: ";
    public override string BeingsDetailToolAuthEditLink => "Configurar";
    public override string ToolAuthPageTitle => "Autorização de ferramentas";
    public override string ToolAuthPageHeader => "Configuração de autorização de ferramentas";
    public override string ToolAuthTemplateLabel => "Modelo predefinido";
    public override string ToolAuthSaveButton => "Guardar";
    public override string ToolAuthSelectAll => "Selecionar tudo";
    public override string ToolAuthDeselectAll => "Desselecionar tudo";
    public override string ToolAuthNoRestrictions => "Sem restrições";
    public override string ToolAuthHasRestrictions => "Com restrições";
    public override string ToolAuthSaveSuccess => "Autorização de ferramentas guardada com sucesso";
    public override string ToolAuthSaveFailed => "Falha ao guardar";
    public override string ToolAuthDialogClose => "Fechar";
    public override string ToolAuthNoDeclaredActions => "Não configurável";
    public override string WorkNotePageTitle => "Notas de trabalho";
    public override string WorkNotePageHeader => "Lista de notas de trabalho";
    public override string WorkNotePageDescription => "Gestão e consulta das notas de trabalho do Silicon Being";
    public override string ChatHistoryPageTitle => "Histórico de chat";
    public override string ChatHistoryPageHeader => "Lista de conversas";
    public override string ChatHistoryConversationList => "Lista de conversas";
    public override string ChatHistoryBackToList => "Voltar à lista de conversas";
    public override string ChatHistoryNoConversations => "Sem registos de conversa de momento";
    public override string ChatDetailPageTitle => "Detalhes do chat";
    public override string ChatDetailPageHeader => "Detalhes da conversa";
    public override string ChatDetailNoMessages => "Sem mensagens de momento";
    public override string ChatDetailMembers => "Membros";
    public override string BeingsYes => "Sim";
    public override string BeingsNo => "Não";
    public override string BeingsNotSet => "Não definido";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Gestão de temporizadores";
    public override string TimersTotalCount => "Total {0} temporizadores";
    public override string TimersEmptyState => "Sem temporizadores de momento";
    public override string TimerViewExecutionHistory => "📝 Ver histórico de execução";
    public override string TimerExecutionHistoryTitle => "Histórico de execução do temporizador";
    public override string TimerExecutionHistoryHeader => "Registo de execução";
    public override string TimerExecutionBackToTimers => "← Voltar à lista de temporizadores";
    public override string TimerExecutionTimerName => "Temporizador: {0}";
    public override string TimerExecutionDetailTitle => "Detalhes da execução";
    public override string TimerExecutionDetailHeader => "Registo de mensagens de execução";
    public override string TimerExecutionNoRecords => "Sem registos de execução de momento";
    public override string TaskExecutionHistoryTitle => "Histórico de execução de tarefas";
    public override string TaskExecutionHistoryHeader => "Histórico de execução";
    public override string TaskExecutionBackToTasks => "← Voltar às tarefas";
    public override string TaskExecutionTaskName => "Tarefa: {0}";
    public override string TaskExecutionDetailTitle => "Detalhe de execução da tarefa";
    public override string TaskExecutionDetailHeader => "Detalhe de execução";
    public override string TaskExecutionNoRecords => "Sem registos de execução de momento";
    public override string TimersStatusActive => "Ativo";
    public override string TimersStatusPaused => "Em pausa";
    public override string TimersStatusTriggered => "Acionado";
    public override string TimersStatusCancelled => "Cancelado";
    public override string TimersTypeRecurring => "Recorrente";
    public override string TimersTriggerTimeLabel => "Hora de acionamento: ";
    public override string TimersIntervalLabel => "Intervalo: ";
    public override string TimersCalendarLabel => "Condição de calendário: ";
    public override string TimersTriggeredCountLabel => "Acionado: ";

    // ===== About Page Localization =====

    public override string AboutPageHeader => "Acerca";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Versão";
    public override string AboutDescription => "Um sistema de gestão Silicon Life Collective baseado em IA, que suporta colaboração multi-agente de IA, gestão de memória, construção de grafos de conhecimento e outras funcionalidades.";
    public override string AboutAuthorLabel => "Autor";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "Licença";
    public override string AboutCopyright => "Copyright (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "Repositório GitHub";
    public override string AboutGiteeLink => "Mirror Gitee";
    public override string AboutSocialMediaLabel => "Plataformas sociais";
    public override string AboutPluginListLabel => "Lista de plugins";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "WeChat Conta oficial",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "Configuração do sistema";
    public override string ConfigPropertyNameLabel => "Nome da propriedade";
    public override string ConfigPropertyValueLabel => "Valor da propriedade";
    public override string ConfigActionLabel => "Ação";
    public override string ConfigEditButton => "Editar";
    public override string ConfigEditModalTitle => "Editar item de configuração";
    public override string ConfigEditPropertyLabel => "Nome da propriedade: ";
    public override string ConfigEditValueLabel => "Valor da propriedade: ";
    public override string ConfigBrowseButton => "Procurar";
    public override string ConfigTimeSettingsLabel => "Definições de hora: ";
    public override string ConfigDaysLabel => "Dias: ";
    public override string ConfigHoursLabel => "Horas: ";
    public override string ConfigMinutesLabel => "Minutos: ";
    public override string ConfigSecondsLabel => "Segundos: ";
    public override string ConfigSaveButton => "Guardar";
    public override string ConfigCancelButton => "Cancelar";
    public override string ConfigNullValue => "Nulo";

    public override string ConfigEditPrefix => "Editar: ";
    public override string ConfigDefaultGroupName => "Outro";
    public override string ConfigErrorInvalidRequest => "Parâmetro de pedido inválido";
    public override string ConfigErrorInstanceNotFound => "Instância de configuração não encontrada";
    public override string ConfigErrorPropertyNotFound => "Propriedade {0} não encontrada ou não acessível para escrita";
    public override string ConfigErrorConvertInt => "Não é possível converter '{0}' para inteiro";
    public override string ConfigErrorConvertLong => "Não é possível converter '{0}' para inteiro longo";
    public override string ConfigErrorConvertDouble => "Não é possível converter '{0}' para número de vírgula flutuante";
    public override string ConfigErrorConvertBool => "Não é possível converter '{0}' para booleano";
    public override string ConfigErrorConvertGuid => "Não é possível converter '{0}' para GUID";
    public override string ConfigErrorConvertTimeSpan => "Não é possível converter '{0}' para TimeSpan";
    public override string ConfigErrorConvertDateTime => "Não é possível converter '{0}' para DateTime";
    public override string ConfigErrorConvertEnum => "Não é possível converter '{0}' para {1}";
    public override string ConfigErrorUnsupportedType => "Tipo de propriedade não suportado: {0}";
    public override string ConfigErrorSaveFailed => "Guarda falhada: {0}";
    public override string ConfigSaveFailed => "Guarda falhada: ";
    public override string ConfigDictionaryLabel => "Dicionário";
    public override string ConfigDictKeyLabel => "Chave: ";
    public override string ConfigDictValueLabel => "Valor: ";
    public override string ConfigDictAddButton => "Adicionar";
    public override string ConfigDictDeleteButton => "Eliminar";

    public override string ConfigPluginDirectoriesLabel => "Diretórios de plugins";
    public override string ConfigPluginDirAddButton => "Adicionar diretório";
    public override string ConfigDictEmptyMessage => "O dicionário está vazio";
    public override string SelectSearchHint => "Limpar o campo para mostrar todas as opções";

    // ===== Logs Page Localization =====

    public override string LogsPageHeader => "Consulta dos registos";
    public override string LogsTotalCount => "Total {0} entradas de registo";
    public override string LogsStartTime => "Hora de início";
    public override string LogsEndTime => "Hora de fim";
    public override string LogsLevelAll => "Todos os níveis";
    public override string LogsBeingFilter => "Silicon Being";
    public override string LogsAllBeings => "Não filtrar";
    public override string LogsSystemOnly => "Apenas sistema";
    public override string LogsFilterButton => "Pesquisar";
    public override string LogsEmptyState => "Sem entradas de registo de momento";
    public override string LogsExceptionLabel => "Detalhes da exceção: ";
    public override string LogsPrevPage => "Página anterior";
    public override string LogsNextPage => "Página seguinte";
    public override string LogsLoading => "A carregar registos...";

    // ===== Usage Page Localization =====

    public override string UsagePageHeader => "Utilização de tokens";
    public override string UsageTotalTokens => "Tokens totais";
    public override string UsageTotalRequests => "Pedidos totais";
    public override string UsageSuccessCount => "Bem-sucedidos";
    public override string UsageFailureCount => "Falhados";
    public override string UsagePromptTokens => "Tokens de entrada";
    public override string UsageCompletionTokens => "Tokens de saída";
    public override string UsageStartTime => "Hora de início";
    public override string UsageEndTime => "Hora de fim";
    public override string UsageFilterButton => "Pesquisar";
    public override string UsageEmptyState => "Sem dados de utilização de momento";
    public override string UsageAIClientType => "Cliente IA";
    public override string UsageAllClientTypes => "Todos os tipos";
    public override string UsageGroupByClient => "Agrupar por cliente";
    public override string UsageGroupByBeing => "Agrupar por Silicon Being";
    public override string UsagePrevPage => "Página anterior";
    public override string UsageNextPage => "Página seguinte";
    public override string UsageBeing => "Silicon Being";
    public override string UsageAllBeings => "Todos os Silicon Beings";
    public override string UsageTimeToday => "Hoje";
    public override string UsageTimeWeek => "Esta semana";
    public override string UsageTimeMonth => "Este mês";
    public override string UsageTimeYear => "Este ano";
    public override string UsageExport => "Exportar";
    public override string UsageTrendTitle => "Tendência de consumo de tokens";
    public override string UsageTrendPrompt => "Tokens de entrada";
    public override string UsageTrendCompletion => "Tokens de saída";
    public override string UsageTrendTotal => "Tokens totais";
    public override string UsageTooltipDate => "Data";
    public override string UsageTooltipPrompt => "Tokens de entrada";
    public override string UsageTooltipCompletion => "Tokens de saída";
    public override string UsageTooltipTotal => "Tokens totais";

    public override string AuditPageHeader => "Registo de auditoria de permissões";
    public override string AuditTotalEntries => "Entradas totais";
    public override string AuditAllowedCount => "Permitido";
    public override string AuditDeniedCount => "Recusado";
    public override string AuditAskUserCount => "Perguntar ao utilizador";
    public override string AuditPermissionType => "Tipo de permissão";
    public override string AuditAllPermissionTypes => "Todos os tipos";
    public override string AuditResult => "Resultado";
    public override string AuditAllResults => "Todos os resultados";
    public override string AuditBeing => "Silicon Being";
    public override string AuditAllBeings => "Todos os Beings";
    public override string AuditStartTime => "Hora de início";
    public override string AuditEndTime => "Hora de fim";
    public override string AuditFilterButton => "Filtrar";
    public override string AuditEmptyState => "Nenhuma entrada de auditoria encontrada";
    public override string AuditPrevPage => "Anterior";
    public override string AuditNextPage => "Seguinte";
    public override string AuditColumnCaller => "Chamador";
    public override string AuditColumnPermissionType => "Tipo de permissão";
    public override string AuditColumnResource => "Recurso";
    public override string AuditColumnResult => "Resultado";
    public override string AuditColumnReason => "Motivo";
    public override string AuditColumnTimestamp => "Carimbo de data/hora";

    // ===== Log Level Localization =====

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Trace",
        LogLevel.Debug => "Debug",
        LogLevel.Information => "Informação",
        LogLevel.Warning => "Aviso",
        LogLevel.Error => "Erro",
        LogLevel.Critical => "Crítico",
        LogLevel.None => "Nenhum",
        _ => logLevel.ToString()
    };

    // ===== Being Activity Localization =====

    public override string GetBeingActivityName(BeingActivity activity) => activity switch
    {
        BeingActivity.Idle => "Inativo",
        BeingActivity.SingleChat => "Em chat individual",
        BeingActivity.GroupChat => "Em chat de grupo",
        BeingActivity.Task => "Execução de tarefa",
        BeingActivity.Timer => "Execução de temporizador",
        BeingActivity.Broadcast => "Processando broadcast",
        BeingActivity.Project => "Trabalhando no projeto",
        BeingActivity.MemoryCompression => "Compressão de memória",
        BeingActivity.Stopped => "Parado",
        _ => activity.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Calendário",
        ["chat"] = "Chat",
        ["config"] = "Configuração",
        ["silicon_manager"] = "Gestor Silicon",
        ["database"] = "Base de dados",
        ["disk"] = "Disco",
        ["dynamic_compile"] = "Compilação dinâmica",
        ["execute_code"] = "Executar código",
        ["help"] = "Ajuda",
        ["knowledge"] = "Conhecimento",
        ["log"] = "Registo",
        ["network"] = "Rede",
        ["memory"] = "Memória",
        ["permission"] = "Permissão",
        ["task"] = "Tarefa",
        ["system"] = "Sistema",
        ["project_work_note"] = "Nota de trabalho do projeto",
        ["webview_browser"] = "Browser WebView"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    // ===== Interval Timer Localization =====

    public override string CalendarIntervalName => "Temporizador de intervalo";
    public override string CalendarIntervalDays => "Dias";
    public override string CalendarIntervalHours => "Horas";
    public override string CalendarIntervalMinutes => "Minutos";
    public override string CalendarIntervalSeconds => "Segundos";
    public override string CalendarIntervalEvery => "Cada";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery} {string.Join(", ", parts)}" : "Temporizador de intervalo";
    }

    // ===== Default Curator Soul =====

    public override string DefaultCuratorSoul => """
        És o **Silicon Curator**, o gestor central e coordenador do Silicon Life Collective.

        Não és um assistente de IA comum. És o cérebro e a vontade deste sistema multi-agente — responsável por compreender as intenções do utilizador, decompor objetivos, coordenar os Silicon Beings, monitorizar a qualidade de execução e intervir quando necessário.

        > **Princípio de coordenação**: As tarefas de longo prazo devem ser delegadas aos Silicon Beings, dá prioridade aos Beings atualmente disponíveis, não cries desnecessariamente novos Beings. Apenas se a tarefa puder ser concluída em dois ou três passos é que a gestão é feita diretamente.

        ---

        ### Identidade e posição

        - És o único Silicon Being com as permissões mais elevadas do sistema.
        - Podes criar, gerir, redefinir outros Silicon Beings e escrever e compilar para eles novos códigos de comportamento C#.
        - És responsável perante o utilizador e pela qualidade de todo o coletivo.
        - Não és um executor, és um **decisor e coordenador**. Delega o que puder ser delegado.

        ---

        ### Tarefas principais

        **1. Compreender a intenção do utilizador**
        As expressões dos utilizadores podem ser vagas, saltadas ou incompletas. Compreende ativamente o seu verdadeiro objetivo, pergunta se necessário, em vez de executar mecanicamente instruções literais.

        **2. Decomposição e distribuição de tarefas**
        Decompõe objetivos complexos em subtarefas executáveis, avalia quais os Silicon Beings adequados, cria tarefas com a ferramenta `task` e distribui-as.

        **3. Monitorização e fallback**
        Verifica regularmente o estado das tarefas. Se um Silicon Being falhar ou não responder durante muito tempo, deves intervir — redistribuir, ajustar a estratégia ou gerir tu mesmo.

        **4. Evolução dinâmica**
        Podes usar a ferramenta `dynamic_compile` para escrever novas classes de comportamento C# para qualquer Silicon Being (incluindo tu mesmo). Valida sempre com `compile` antes de escrever.

        **5. Resposta direta ao utilizador**
        Para perguntas simples, pedidos de estado, conversa leve, responde diretamente sem criar tarefas.

        ---

        ### Diretivas de comportamento

        **Sobre decisões**
        - Em caso de incerteza, pergunta primeiro, depois age.
        - Não presumas a intenção do utilizador.

        **Sobre permissões**
        - O sistema dispõe de um sistema de permissões completo.
        - Age conforme necessário, reage em caso de bloqueio de permissões, não perguntes antecipadamente.

        **Sobre auto-evolução**
        - A compilação dinâmica é uma capacidade poderosa e perigosa.
        - Valida sempre com `compile` antes de modificar o teu código.

        **Sobre comunicação**
        - Usa uma linguagem clara e direta.
        - Para monitorização de tarefas: "O que foi feito, resultado, próximo passo" em três frases.

        **Sobre memória**
        - O sistema regista automaticamente as informações importantes.
        - Pesquisa ativamente em `memory` se necessário.

        ---

        ### Perfil de personalidade

        És calmo, pragmático e fiável. Não perdes a calma perante tarefas complexas e manténs-te objetivo com utilizadores emocionais.

        Não és um fornecedor de serviços, és um parceiro.
        """;

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Configuração base",
        ["Runtime"] = "Configuração de runtime",
        ["AI"] = "Configuração de IA",
        ["Web"] = "Configuração Web",
        ["User"] = "Configuração do utilizador"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Diretório de dados",
        ["Language"] = "Definição de idioma",
        ["TickTimeout"] = "Timeout de tick",
        ["MaxTimeoutCount"] = "Número máximo de timeouts",
        ["WatchdogTimeout"] = "Timeout do watchdog",
        ["MinLogLevel"] = "Nível mínimo de registo",
        ["AIClientType"] = "Tipo de cliente IA",
        ["OllamaClient"] = "Cliente Ollama",
        ["OllamaEndpoint"] = "Endpoint Ollama",
        ["DefaultModel"] = "Modelo predefinido",
        ["Temperature"] = "Temperatura",
        ["MaxTokens"] = "Número máximo de tokens",
        ["DashScopeClient"] = "Cliente DashScope",
        ["DashScopeApiKey"] = "Chave API",
        ["DashScopeRegion"] = "Região do serviço",
        ["DashScopeModel"] = "Modelo",
        ["DashScopeRegionBeijing"] = "Norte da China 2 (Pequim)",
        ["DashScopeRegionVirginia"] = "Estados Unidos (Virgínia)",
        ["DashScopeRegionSingapore"] = "Singapura",
        ["DashScopeRegionHongkong"] = "Hong Kong, China",
        ["DashScopeRegionFrankfurt"] = "Alemanha (Frankfurt)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max (Topo de gama)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus (Relação qualidade-preço)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash (Rápido)",
        ["DashScopeModel_qwen-max"] = "Qwen Max (Topo de gama estável)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus (Equilibrado estável)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo (Estável e rápido)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus (Código)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus (Raciocínio profundo)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1 (Raciocínio)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1 (Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5 (Contexto longo)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["VolcengineArkClient"] = "Cliente Volcengine Ark",
        ["VolcengineArkApiKey"] = "Chave API",
        ["VolcengineArkEndpointId"] = "ID do endpoint de inferência",
        ["WebPort"] = "Porta Web",
        ["WebSkin"] = "Tema Web",
        ["UserNickname"] = "Nome de utilizador",
        ["PluginDirectories"] = "Diretórios de plugins"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Caminho do diretório de dados para todos os dados da aplicação",
        ["Language"] = "Definição de idioma da aplicação",
        ["TickTimeout"] = "Duração do timeout para cada execução de tick",
        ["MaxTimeoutCount"] = "Número máximo de timeouts consecutivos antes da interrupção",
        ["WatchdogTimeout"] = "Timeout do watchdog para detetar bloqueios do ciclo principal",
        ["MinLogLevel"] = "Nível mínimo de registo global",
        ["AIClientType"] = "Tipo de cliente IA a utilizar",
        ["OllamaEndpoint"] = "URL do endpoint API Ollama",
        ["DefaultModel"] = "Modelo de IA utilizado por predefinição",
        ["DashScopeApiKey"] = "Chave API Alibaba Cloud DashScope",
        ["DashScopeRegion"] = "Região do serviço Alibaba Cloud DashScope",
        ["DashScopeModel"] = "Modelo utilizado no Alibaba Cloud DashScope",
        ["VolcengineArkApiKey"] = "Chave API Volcengine Ark",
        ["VolcengineArkEndpointId"] = "ID do endpoint de inferência Volcengine Ark",
        ["WebPort"] = "Porta do servidor Web",
        ["WebSkin"] = "Nome do tema Web",
        ["UserNickname"] = "Nome do utilizador humano",
        ["PluginDirectories"] = "Lista de diretórios de plugins para descoberta automática, suporta caminhos relativos ou absolutos"
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

    // ===== Calendar Localization =====

    public override string CalendarComponentYear => "Ano";
    public override string CalendarComponentMonth => "Mês";
    public override string CalendarComponentDay => "Dia";
    public override string CalendarComponentHour => "Hora";
    public override string CalendarComponentMinute => "Minuto";
    public override string CalendarComponentSecond => "Segundo";
    public override string CalendarComponentWeekday => "Dia da semana";

    // ===== Gregorian Calendar Localization =====

    public override string CalendarGregorianName => "Calendário gregoriano";

    private static readonly string[] GregorianMonthNames =
    {
        "", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    };

    public override string? GetGregorianMonthName(int month)
        => month >= 1 && month <= 12 ? GregorianMonthNames[month] : null;

    public override string FormatGregorianYear(int year)     => $"{year}";
    public override string FormatGregorianDay(int day)       => $"{day}";
    public override string FormatGregorianHour(int hour)     => $"{hour}";
    public override string FormatGregorianMinute(int minute) => $"{minute}";
    public override string FormatGregorianSecond(int second) => $"{second}";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Domingo", 1 => "Segunda-feira", 2 => "Terça-feira",
        3 => "Quarta-feira", 4 => "Quinta-feira", 5 => "Sexta-feira",
        6 => "Sábado", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar Localization =====

    public override string CalendarBuddhistName => "Calendário budista (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"{year} BE";
    public override string FormatBuddhistDay(int day)   => $"{day}";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} BE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar Localization =====

    public override string CalendarCherokeeName => "Calendário cherokee";

    private static readonly string[] CherokeeMonthNames =
    {
        "", "Mês da geada", "Mês do frio", "Mês do vento", "Mês das plantas", "Mês das sementeiras",
        "Mês das amoras maduras", "Mês do milho", "Mês da fruta", "Mês da colheita", "Mês das folhas amarelas",
        "Mês do comércio", "Mês da neve", "Mês longo"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}";
    public override string FormatCherokeeDay(int day)   => $"{day}";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Juche Calendar Localization =====

    public override string CalendarJucheName => "Calendário Juche";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => $"{day}";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Juche {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar Localization =====

    public override string CalendarRocName => "Calendário Minguo (ROC)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"Minguo {year}";
    public override string FormatRocDay(int day)   => $"{day}";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Minguo {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar Localization =====

    public override string CalendarChineseHistoricalName => "Calendário histórico chinês";
    public override string CalendarComponentDynasty => "Dinastia";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}";

    // ===== Chula Sakarat Calendar Localization =====

    public override string CalendarChulaSakaratName => "Calendário Chula Sakarat (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => $"{day}";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} CS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar Localization =====

    public override string CalendarJulianName => "Calendário juliano";

    public override string FormatJulianYear(int year) => $"{year}";
    public override string FormatJulianDay(int day)   => $"{day}";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Juliano), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar Localization =====

    public override string CalendarKhmerName => "Calendário khmer (BE)";

    public override string FormatKhmerYear(int year) => $"{year}";
    public override string FormatKhmerDay(int day)   => $"{day}";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} (Khmer), {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar Localization =====

    public override string CalendarZoroastrianName => "Calendário zoroastriano (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "", "Mês de Fravashi", "Mês de Atar", "Mês de Hordad", "Mês de Tir", "Mês de Amordad", "Mês de Shahrivar",
        "Mês de Mehr", "Mês de Aban", "Mês de Azar", "Mês de Dey", "Mês de Bahman", "Mês de Spendarmad", "Mês de Kabe"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year} YZ";
    public override string FormatZoroastrianDay(int day)   => $"{day}";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} YZ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== French Republican Calendar Localization =====

    public override string CalendarFrenchRepublicanName => "Calendário republicano francês";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "", "Vendimiário", "Brumário", "Frimário", "Nivoso", "Pluvioso", "Ventoso",
        "Germinal", "Floreal", "Pradial", "Messidoro", "Termidoro", "Frutidoro", "Sans-culottidi"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"Ano {year}";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}";
        return $"{day} {monthName} Ano {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar Localization =====

    public override string CalendarCopticName => "Calendário copta (AM)";

    private static readonly string[] CopticMonthNames =
    {
        "", "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir",
        "Paremhat", "Pharmouthi", "Pashons", "Paoni", "Epip", "Mesori", "Pi Kogi Enavot"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year} AM";
    public override string FormatCopticDay(int day)   => $"{day}";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Ethiopian Calendar Localization =====

    public override string CalendarEthiopianName => "Calendário etíope (EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "", "Meskerem", "Tikimt", "Hidar", "Tahsas", "Tir", "Yekatit",
        "Megabit", "Miazia", "Genbot", "Sene", "Hamle", "Nehasse", "Paguemen"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year} EC";
    public override string FormatEthiopianDay(int day)   => $"{day}";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} EC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Islamic Calendar Localization =====

    public override string CalendarIslamicName => "Calendário islâmico (AH)";

    private static readonly string[] IslamicMonthNames =
    {
        "", "Muharram", "Safar", "Rabi' al-Awwal", "Rabi' al-Thani",
        "Jumada al-Awwal", "Jumada al-Thani", "Rajab", "Sha'ban",
        "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year} AH";
    public override string FormatIslamicDay(int day)   => $"{day}";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AH, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Hebrew Calendar Localization =====

    public override string CalendarHebrewName => "Calendário hebraico";

    private static readonly string[] HebrewMonthNames =
    {
        "", "Tishrei", "Cheshvan", "Kislev", "Tevet", "Shvat",
        "Adar I", "Adar II", "Nisan", "Iyar", "Sivan",
        "Tammuz", "Av", "Elul"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year} AM";
    public override string FormatHebrewDay(int day)   => $"{day}";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AM, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Persian Calendar Localization =====

    public override string CalendarPersianName => "Calendário persa (AP)";

    private static readonly string[] PersianMonthNames =
    {
        "", "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year} AP";
    public override string FormatPersianDay(int day)   => $"{day}";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AP, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Indian National Calendar Localization =====

    public override string CalendarIndianName => "Calendário nacional indiano (Saka)";

    private static readonly string[] IndianMonthNames =
    {
        "", "Chaitra", "Vaisakha", "Jyaistha", "Asadha", "Sravana", "Bhadrapada",
        "Asvina", "Kartika", "Margasirsa", "Pausa", "Magha", "Phalguna"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year} Saka";
    public override string FormatIndianDay(int day)   => $"{day}";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} Saka, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Saka Era Calendar Localization =====

    public override string CalendarSakaName => "Calendário da era Saka";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => $"{day}";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} SE, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar Localization =====

    public override string CalendarVikramSamvatName => "Calendário Vikram Samvat";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => $"{day}";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} VS, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar Localization =====

    public override string CalendarMongolianName => "Calendário mongol";

    public override string FormatMongolianYear(int year)   => $"{year}";
    public override string FormatMongolianMonth(int month) => $"{month}";
    public override string FormatMongolianDay(int day)     => $"{day}";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Mongol), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar Localization =====

    public override string CalendarJavaneseName => "Calendário javanês";

    private static readonly string[] JavaneseMonthNames =
    {
        "", "Sura", "Sapar", "Mulud", "Bakda Mulud",
        "Jumadilawal", "Jumadilakir", "Rejeb", "Ruwah",
        "Pasa", "Sawal", "Dulkaidah", "Besar"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year} AJ";
    public override string FormatJavaneseDay(int day)   => $"{day}";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year} AJ, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Tibetan Calendar Localization =====

    public override string CalendarTibetanName => "Calendário tibetano";

    public override string FormatTibetanYear(int year)   => $"{year}";
    public override string FormatTibetanMonth(int month) => $"{month}";
    public override string FormatTibetanDay(int day)     => $"{day}";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{day} {month} {year} (Tibetano), {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar Localization =====

    public override string CalendarMayanName   => "Calendário maia (Contagem Longa)";
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun  => "Katun";
    public override string CalendarMayanTun    => "Tun";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar Localization =====

    public override string CalendarInuitName => "Calendário inuit";

    private static readonly string[] InuitMonthNames =
    {
        "", "Sikinaqjiaq", "Aiviq", "Naattiaq", "Tirligurut", "Amiraijaut",
        "Natsiviat", "Akulliq", "Sikinaluqtuq", "Akullirusit", "Ukiuq",
        "Ukiuq minasumaaqtuq", "Sikinniq naniqtatsiq", "Tauvikjujaq"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}";
    public override string FormatInuitDay(int day)   => $"{day}";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Roman Calendar Localization =====

    public override string CalendarRomanName => "Calendário romano (AUC)";

    private static readonly string[] RomanMonthNames =
    {
        "", "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius",
        "Quintilis", "Sextilis", "September", "October", "November", "December"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"{year + 753} AUC";
    public override string FormatRomanDay(int day)   => $"{day}";

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {year + 753} AUC, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Lunar Calendar Localization =====

    public override string CalendarChineseLunarName => "Calendário lunar chinês";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "Primeiro mês", "Segundo mês", "Terceiro mês", "Quarto mês", "Quinto mês", "Sexto mês",
        "Sétimo mês", "Oitavo mês", "Nono mês", "Décimo mês", "Décimo primeiro mês", "Décimo segundo mês"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "Primeiro","Segundo","Terceiro","Quarto","Quinto","Sexto","Sétimo","Oitavo","Nono","Décimo",
        "Décimo primeiro","Décimo segundo","Décimo terceiro","Décimo quarto","Décimo quinto","Décimo sexto","Décimo sétimo","Décimo oitavo","Décimo nono","Vigésimo",
        "Vigésimo primeiro","Vigésimo segundo","Vigésimo terceiro","Vigésimo quarto","Vigésimo quinto","Vigésimo sexto","Vigésimo sétimo","Vigésimo oitavo","Vigésimo nono","Trigésimo"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Intercalar ";
    public override string CalendarComponentIsLeap => "Mês intercalar";
    public override string FormatChineseLunarYear(int year) => $"{year}";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName = GetChineseLunarMonthName(month) ?? $"{month}";
        var dayName = GetChineseLunarDayName(day) ?? $"{day}";
        return $"{leapPrefix}{monthName} {dayName}, {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar Localization =====

    public override string CalendarVietnameseName => "Calendário vietnamita";

    private static readonly string[] VietnameseMonthNames =
    {
        "", "Primeiro mês", "Segundo mês", "Terceiro mês", "Quarto mês", "Quinto mês", "Sexto mês",
        "Sétimo mês", "Oitavo mês", "Nono mês", "Décimo mês", "Décimo primeiro mês", "Décimo segundo mês"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Rato", "Búfalo", "Tigre", "Gato",
        "Dragão", "Serpente", "Cavalo", "Cabra",
        "Macaco", "Galo", "Cão", "Porco"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "Intercalar ";
    public override string CalendarComponentZodiac => "Zodíaco";
    public override string FormatVietnameseYear(int year) => $"{year}";
    public override string FormatVietnameseDay(int day)   => $"{day}";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"Ano {zodiacName}, {leapPrefix}{monthName} {day}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar Localization =====

    public override string CalendarJapaneseName => "Calendário japonês (Nengō)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa", "Heisei", "Shōwa", "Taishō", "Meiji" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Era";
    public override string FormatJapaneseYear(int year) => $"{year}";
    public override string FormatJapaneseDay(int day)   => $"{day}";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}";
        return $"{day} {monthName} {eraName} {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar Localization =====

    public override string CalendarYiName => "Calendário Yi (Calendário solar Yi)";
    public override string CalendarComponentYiSeason => "Estação";
    public override string CalendarComponentYiXun    => "Xun";

    private static readonly string[] YiSeasonNames = { "Madeira", "Fogo", "Terra", "Metal", "Água" };
    private static readonly string[] YiXunNames    = { "Primeira Xun", "Xun média", "Última Xun" };
    private static readonly string[] YiAnimalNames = { "Tigre", "Coelho", "Dragão", "Serpente", "Cavalo", "Cabra", "Macaco", "Galo", "Cão", "Porco", "Rato", "Búfalo" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "Grande ano",
        11 => "Pequeno ano",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "Masculino" : "Feminino")}-Mês",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]} Dia-{YiAnimalNames[animal]}";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}";
        var dayStr    = month is 0 or 11 ? $"Dia {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"{year} [{zodiac}] {monthName} {dayStr}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar Localization =====

    public override string CalendarSexagenaryName    => "Calendário do ciclo sexagenário";
    public override string CalendarComponentYearStem   => "Tronco anual";
    public override string CalendarComponentYearBranch => "Ramo anual";
    public override string CalendarComponentMonthStem   => "Tronco mensal";
    public override string CalendarComponentMonthBranch => "Ramo mensal";
    public override string CalendarComponentDayStem   => "Tronco diário";
    public override string CalendarComponentDayBranch => "Ramo diário";

    private static readonly string[] SexagenaryStemNames =
        { "Jia", "Yi", "Bing", "Ding", "Wu", "Ji", "Geng", "Xin", "Ren", "Gui" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zi", "Chou", "Yin", "Mao", "Chen", "Si", "Wu", "Wei", "Shen", "You", "Xu", "Hai" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Rato", "Búfalo", "Tigre", "Coelho", "Dragão", "Serpente", "Cavalo", "Cabra", "Macaco", "Galo", "Cão", "Porco" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)      ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch)  ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch)  ?? "?";
        var ms = GetSexagenaryStemName(monthStem)     ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch) ?? "?";
        var ds = GetSexagenaryStemName(dayStem)       ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)   ?? "?";
        return $"Ano {ys}{yb} [{zo}] Mês {ms}{mb} Dia {ds}{db}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar Localization =====

    public override string CalendarDaiName => "Calendário Dai de Xishuangbanna";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Primeiro mês", "Segundo mês", "Terceiro mês", "Quarto mês", "Quinto mês", "Sexto mês",
        "Sétimo mês", "Oitavo mês", "Nono mês", "Décimo mês", "Décimo primeiro mês", "Décimo segundo mês",
        "Nono mês intercalar"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}";

    public override string FormatDaiDay(int day) => $"{day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalar " : "") + (GetDaiMonthName(month) ?? $"Mês {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Xishuangbanna Dai Calendar Localization =====

    public override string CalendarDehongDaiName => "Calendário Dai do Dehong";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Primeiro mês", "Segundo mês", "Terceiro mês", "Quarto mês", "Quinto mês", "Sexto mês",
        "Sétimo mês", "Oitavo mês", "Nono mês", "Décimo mês", "Décimo primeiro mês", "Décimo segundo mês",
        "Nono mês intercalar"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}";

    public override string FormatDehongDaiDay(int day) => $"{day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Intercalar " : "") + (GetDehongDaiMonthName(month) ?? $"Mês {month}");
        return $"{day} {monthName} Dai {year}, {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content)
        => $"[Chat individual] {speakerName} disse a {listenerName} : {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Chat de grupo] Mensagem na sessão {sessionId} : {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Chamada de ferramenta] Ferramentas executadas : {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Tarefa] Tarefa executada, resultado : {content}";

    public override string FormatMemoryEventProject(string content)
        => $"[Projeto] Reflexão sobre o projeto, resultado : {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Temporizador] Temporizador ativado, resposta : {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[Temporizador] Temporizador '{timerName}' falhou : {error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ Temporizador '{timerName}' iniciado...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ Temporizador '{timerName}' concluído\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ Temporizador '{timerName}' falhou : {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Administração] Novo Silicon Being \"{name}\" criado ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Administração] Silicon Being {id} restaurado para predefinição";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Tarefa concluída] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Tarefa falhada] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "Sistema iniciado, estou online";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Erro de execução] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Sistema de memória não disponível";
    public override string MemoryToolMissingAction => "Parâmetro 'action' em falta";
    public override string MemoryToolMissingContent => "Parâmetro 'content' em falta";
    public override string MemoryToolNoMemories => "Sem memórias de momento";
    public override string MemoryToolRecentHeader(int count) => $"{count} memórias recentes :";
    public override string MemoryToolStatsHeader => "Estatísticas de memória :";
    public override string MemoryToolStatsTotal => "- Total";
    public override string MemoryToolStatsOldest => "- Mais antiga";
    public override string MemoryToolStatsNewest => "- Mais recente";
    public override string MemoryToolStatsNA => "Nenhuma";
    public override string MemoryToolQueryNoResults => "Sem memórias neste período";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} total {count} memórias :";
    public override string MemoryToolInvalidYear => "Parâmetro 'year' inválido";
    public override string MemoryToolUnknownAction(string action) => $"Ação desconhecida : {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "Variável",
        "function" => "Função",
        "class" => "Classe",
        "keyword" => "Palavra-chave",
        "comment" => "Comentário",
        "namespace" => "Espaço de nomes",
        "parameter" => "Parâmetro",
        _ => "Identificador"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"Definição e utilização da variável '{encodedWord}'",
            "function" => $"Assinatura e descrição da função '{encodedWord}'",
            "class" => $"Estrutura e descrição da classe '{encodedWord}'",
            "keyword" => $"Sintaxe e papel da palavra-chave '{encodedWord}'",
            "comment" => $"Palavra '{encodedWord}' no comentário",
            "namespace" => $"Informações sobre o espaço de nomes '{encodedWord}'",
            "parameter" => $"Definição e papel do parâmetro '{encodedWord}'",
            _ => $"Informações sobre o identificador '{encodedWord}'"
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
        { "csharp:if", "Ramificação condicional. Executa o bloco se a condição for verdadeira." },
        { "csharp:else", "Caminho alternativo da ramificação condicional. Executado se a condição for falsa." },
        { "csharp:for", "Ciclo contador. Contém inicialização, condição e iteração." },
        { "csharp:while", "Ciclo condicional. Repete o bloco enquanto a condição for verdadeira." },
        { "csharp:do", "Ciclo pós-condicional. Executa o bloco uma vez, depois verifica a condição." },
        { "csharp:switch", "Ramificação múltipla. Compara o valor da expressão com os marcadores case." },
        { "csharp:case", "Marcador case em switch. Executa o código em caso de correspondência." },
        { "csharp:break", "Interrupção. Termina imediatamente o ciclo ou switch envolvente." },
        { "csharp:continue", "Continuação. Passa ao resto da iteração atual." },
        { "csharp:return", "Retorno. Sai do método e opcionalmente devolve um valor." },
        { "csharp:goto", "Salto. Salto incondicional para uma etiqueta." },
        { "csharp:foreach", "Percorrer coleção. Acede a cada elemento de uma coleção." },
        { "csharp:class", "Tipo referência. Define uma estrutura com dados e comportamento." },
        { "csharp:interface", "Interface. Define um contrato para classes/estruturas." },
        { "csharp:struct", "Tipo valor. Estrutura de dados leve na stack." },
        { "csharp:enum", "Enumeração. Define constantes inteiras nomeadas." },
        { "csharp:namespace", "Espaço de nomes. Contentor lógico para evitar colisões de nomes." },
        { "csharp:record", "Tipo record. Tipo referência com semântica de valor, adequado a dados imutáveis." },
        { "csharp:delegate", "Delegado. Referência a método type-safe para eventos/callback." },
        { "csharp:public", "Público. Membro acessível em qualquer lugar." },
        { "csharp:private", "Privado. Membro acessível apenas no tipo contentor." },
        { "csharp:protected", "Protegido. Membro acessível no tipo e nos tipos derivados." },
        { "csharp:internal", "Interno. Membro acessível apenas no mesmo assembly." },
        { "csharp:sealed", "Selado. Impede a herança ou o override." },
        { "csharp:int", "Inteiro com sinal 32 bit (System.Int32)." },
        { "csharp:string", "Cadeia de caracteres (System.String). Sequência Unicode imutável." },
        { "csharp:bool", "Booleano (System.Boolean). true ou false." },
        { "csharp:float", "Vírgula flutuante 32 bit (System.Single)." },
        { "csharp:double", "Vírgula flutuante 64 bit (System.Double)." },
        { "csharp:decimal", "Decimal 128 bit de alta precisão, adequado a cálculos financeiros." },
        { "csharp:char", "Carácter Unicode 16 bit (System.Char)." },
        { "csharp:byte", "Inteiro sem sinal 8 bit (System.Byte)." },
        { "csharp:object", "Tipo base de todos os tipos (System.Object)." },
        { "csharp:var", "Variável implicitamente tipada. O tipo é deduzido pelo compilador." },
        { "csharp:dynamic", "Tipo dinâmico. Ilude a verificação de tipos em compilação, resolução em runtime." },
        { "csharp:void", "Sem valor de retorno. O método não devolve nada." },
        { "csharp:static", "Estático. Pertence ao tipo, não à instância." },
        { "csharp:abstract", "Abstrato. Implementação incompleta, deve ser derivado." },
        { "csharp:virtual", "Virtual. Método/propriedade que pode ser substituído nas classes derivadas." },
        { "csharp:override", "Override. Nova implementação de um método virtual/abstrato." },
        { "csharp:const", "Constante. Valor imutável determinado em compilação." },
        { "csharp:readonly", "Apenas leitura. Atribuível apenas na declaração ou no construtor." },
        { "csharp:volatile", "Volátil. Campo que pode ser modificado simultaneamente por múltiplas threads." },
        { "csharp:async", "Assíncrono. Marca um método com operações assíncronas, geralmente com await." },
        { "csharp:await", "Espera. Suspende o método até ao término da operação assíncrona." },
        { "csharp:partial", "Parcial. Classe/estrutura/interface pode ser distribuída por múltiplos ficheiros." },
        { "csharp:ref", "Parâmetro referência. Passagem por referência." },
        { "csharp:out", "Parâmetro de saída. Devolução de múltiplos valores de um método." },
        { "csharp:in", "Referência apenas leitura. Passagem por referência, mas não modificável." },
        { "csharp:params", "Parâmetros variáveis. Permite um número variável de parâmetros do mesmo tipo." },
        { "csharp:try", "Bloco try. Contém código que pode gerar exceções." },
        { "csharp:catch", "Bloco catch. Captura exceções do bloco try." },
        { "csharp:finally", "Bloco finally. Executado sempre, com ou sem exceção." },
        { "csharp:throw", "Lançar exceção. Lança manualmente um objeto de exceção." },
        { "csharp:new", "Instanciação. Cria um objeto ou chama um construtor." },
        { "csharp:this", "Instância atual. Referência à instância da classe atual." },
        { "csharp:base", "Classe base. Referência à classe base direta." },
        { "csharp:using", "Diretiva ou instrução using. Importa um espaço de nomes ou liberta recursos IDisposable." },
        { "csharp:yield", "Iterador. Devolve os valores um a um, execução diferida." },
        { "csharp:lock", "Sincronização. Assegura que apenas uma thread executa o bloco de código." },
        { "csharp:typeof", "Operador tipo. Devolve o objeto System.Type." },
        { "csharp:nameof", "Operador nome. Devolve o nome como string de uma variável/tipo/membro." },
        { "csharp:is", "Verificação de tipo. Verifica se um objeto é compatível com um tipo." },
        { "csharp:as", "Conversão de tipo. Conversão segura, devolve null em caso de falha." },
        { "csharp:null", "Null. Referência vazia para tipos referência ou Nullable." },
        { "csharp:true", "Valor booleano verdadeiro." },
        { "csharp:false", "Valor booleano falso." },
        { "csharp:default", "Valor predefinido. Valor predefinido do tipo (null para referência, 0 para números)." },
        { "csharp:operator", "Operador. Define um comportamento de operador personalizado." },
        { "csharp:explicit", "Conversão explícita. Requer um cast explícito." },
        { "csharp:implicit", "Conversão implícita. Conversão automática." },
        { "csharp:unchecked", "Não verificado. Desativa a verificação de overflow para aritmética inteira." },
        { "csharp:checked", "Verificado. Ativa a verificação de overflow para aritmética inteira." },
        { "csharp:fixed", "Fixado. Fixa a posição em memória contra a movimentação do GC." },
        { "csharp:stackalloc", "Alocação stack. Aloca um bloco de memória na stack." },
        { "csharp:extern", "Externo. Método implementado num assembly externo (ex. DLL)." },
        { "csharp:unsafe", "Inseguro. Ativa ponteiros e outras funcionalidades inseguras." },
        { "csharp:ipermissioncallback", "Callback de permissão. Avalia as permissões para operações Silicon Being." },
        { "csharp:permissionresult", "Resultado permissão. Allowed, Denied ou AskUser." },
        { "csharp:permissiontype", "Tipo de permissão. NetworkAccess, CommandLine, FileAccess, Function, DataAccess." },
        { "csharp:ipaddress", "Endereço IP (System.Net.IPAddress)." },
        { "csharp:addressfamily", "Família de endereços (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:uri", "URI (System.Uri). Representação objeto de recursos Web." },
        { "csharp:operatingsystem", "Sistema operativo (System.OperatingSystem). Métodos estáticos de verificação OS." },
        { "csharp:environment", "Ambiente (System.Environment). Informações do sistema e plataforma." },
        { "csharp:path", "Caminho (System.IO.Path). Operações em caminhos de ficheiro/diretório." },
        { "csharp:hashset", "HashSet (System.Collections.Generic.HashSet<T>). Operações de conjunto de alta performance." },
        { "csharp:stringbuilder", "StringBuilder (System.Text.StringBuilder). Cadeia modificável para modificações frequentes." },
    };

    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        { "csharp:System.Net.IPAddress", "Endereço IP (System.Net.IPAddress)." },
        { "csharp:System.Net.Sockets.AddressFamily", "Família de endereços (System.Net.Sockets.AddressFamily). IPv4/IPv6." },
        { "csharp:System.Uri", "URI (System.Uri). Representação objeto de recursos Web." },
        { "csharp:System.OperatingSystem", "Sistema operativo (System.OperatingSystem). Métodos estáticos de verificação OS." },
        { "csharp:System.Environment", "Ambiente (System.Environment). Informações do sistema e plataforma." },
        { "csharp:System.IO.Path", "Caminho (System.IO.Path). Operações em caminhos de ficheiro/diretório." },
        { "csharp:System.Collections.Generic.HashSet", "HashSet (System.Collections.Generic.HashSet<T>). Operações de conjunto de alta performance." },
        { "csharp:System.Text.StringBuilder", "StringBuilder (System.Text.StringBuilder). Cadeia modificável para modificações frequentes." },
    };

    // ===== Help Module UI Labels =====

    public override string Help_Title => "Documentação de ajuda";
    public override string Help_Search => "Pesquisar na ajuda";
    public override string Help_DocList => "Lista de documentos";
    public override string Help_NoResults => "Nenhum documento pertinente encontrado";
    public override string Help_Previous => "Anterior";
    public override string Help_Next => "Seguinte";

    public override string MemoryCompressionSystemPrompt => "És um assistente de compressão de memória. Comprime o seguinte período de memória num resumo conciso, mantendo as informações-chave. IMPORTANTE: Produz apenas o conteúdo comprimido, NÃO adiciones marcas temporais ou anotações de data. Fornece apenas o texto comprimido puro.";

    public override string CommonSystemPrompt => @"## Diretrizes de comportamento
És um assistente IA proativo. Para comandos diretos, age imediatamente sem aguardar autorização adicional.
Saudações, respostas, fornecimento de informações, pedidos, etc. são da tua responsabilidade, age proativamente.
Lembra-te: És um assistente e deves responder ativamente às necessidades do utilizador em vez de aguardar passivamente instruções.

## Fim da conversação
Quando completaste a tua tarefa e não precisas de continuar a conversação, usa a ação mark_read da ferramenta chat para marcar as mensagens do interlocutor como lidas sem enviar resposta.
Isto indica que leste a mensagem mas escolheste deliberadamente não responder (lido, não respondido), e podes terminar naturalmente a conversação.
Uso: Chama a ferramenta chat, define action=""mark_read"", target_id=GUID do interlocutor, sem parâmetro message necessário.";

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Compressão de memória : {levelDesc}. Período : {rangeDesc}.\n\nConteúdo da memória :\n{contentText}";
    }

    // ===== Chinese Historical Calendar Localization =====

    private readonly ChineseHistoricalPtPT _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // Project Info Context
    public override string ProjectCtx_ProjectInfoHeader => "Afiliação ao projeto";
    public override string ProjectCtx_ProjectInfoRoleLabel => "Função";
    public override string ProjectCtx_ProjectInfoGoalLabel => "Objetivo";

    // Project Role Context
    public override string ProjectCtx_RoleDefinitionsHeader => "Definições de funções";
    public override string ProjectCtx_RoleAssignmentsHeader => "Atribuições de funções";
    public override string ProjectCtx_NoWorkflowTemplate => "Sem modelo de workflow atribuído, sem definições de funções disponíveis";
    public override string ProjectCtx_RoleNeedsAttention => "⚠ {0} função(ões) com pessoal insuficiente. Precisa criar seres de silício e atribuí-los às funções";
    public override string ProjectCtx_StaffingActionPlanHeader => "Plano de ação de dotação de pessoal";
    public override string ProjectCtx_TotalBeingsNeeded => "Total de seres de silício a criar: {0}";
    public override string ProjectCtx_StaffingRoleBreakdownHeader => "Detalhe da escassez por função";
    public override string ProjectCtx_RoleShortageDetail => "{0}: precisa de {1}, tem {2} → falta {3}";
    public override string ProjectCtx_StaffingActionStepsHeader => "Passos de ação sugeridos";
    public override string ProjectCtx_StaffingStepCreateBeings => "1. Use silicon_manager create_being para criar {0} seres de silício (pelo menos 1 por função)";
    public override string ProjectCtx_StaffingStepAssignToProject => "2. Use project assign para adicionar os novos seres ao projeto";
    public override string ProjectCtx_StaffingStepAssignToRoles => "3. Use project assign_role para atribuir cada ser à função correspondente";
    public override string ProjectCtx_EmptyRolePoolAction => "⚠ O pool de funções está vazio! O workflow define {0} funções. Crie seres de silício e atribua-os a cada função";
    public override string ProjectCtx_RoleMinCount => "Mín";
    public override string ProjectCtx_RoleMaxCount => "Máx";
    public override string ProjectCtx_RoleMaxCountUnlimited => "∞";
    public override string ProjectCtx_RoleAssignedCount => "Atribuídos";
    public override string ProjectCtx_UnassignedRoles => "Funções obrigatórias não atribuídas";
    public override string ProjectCtx_AvailableBeingsHeader => "Seres disponíveis (ainda não atribuídos a este projeto)";
    public override string ProjectCtx_AvailableBeingsHint => "Dica: Considere atribuir seres existentes a funções antes de criar novos. Use project assign e project assign_role.";
    public override string ProjectCtx_AttentionReasonsHeader => "Razões pelas quais o projeto precisa de atenção";
    public override string ProjectCtx_UnsatisfiedRolesDetailHeader => "Detalhes das funções não satisfeitas";
    public override string ProjectAttention_MissingTemplate => "Modelo de workflow em falta";
    public override string ProjectAttention_EmptyRolePool => "Pool de funções vazio (sem funções atribuídas)";
    public override string ProjectAttention_UnsatisfiedRoles => "Algumas funções não cumprem os requisitos de pessoal";

    // Role Staffing
    public override string RoleStaffing_Understaffed => "Subdimensionado";
    public override string RoleStaffing_Overstaffed => "Sobredimensionado";
    public override string RoleStaffing_Full => "Cheio";
    public override string RoleStaffing_Sufficient => "Suficiente";
    public override string RoleStaffing_UnderstaffedDetail => "Subdimensionado (precisa de {0}, tem {1})";
    public override string RoleStaffing_OverstaffedDetail => "Sobredimensionado (máx. {0}, tem {1})";
    public override string RoleStaffing_FullDetail => "Cheio ({0}/{1})";
    public override string RoleStaffing_SufficientDetail => "Suficiente ({0}/{1}+)";

    // ===== Workflow Role Notification =====
    public override string WorkflowRoleBlockedNotificationFormat => "[Notificação de Função de Workflow] O workflow do projeto '{0}' está bloqueado na transição '{1}' ({2} → {3}).\n\nFunções em falta: {4}\n\nUtilize a ação assign_role da project_tool para atribuir as funções necessárias. O workflow será retomado automaticamente na próxima verificação.";
}
