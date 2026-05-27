# PluginDemo-09: Anti-padrão de operações de processo proibidas

## Visão geral

Este plugin demonstra operações de execução de processos **proibidas** no sistema de plugins SiliconLife. Serve como referência de anti-padrão, mostrando o que NÃO fazer e fornecendo alternativas corretas para cada violação.

## Por que os tipos Process são proibidos?

`System.Diagnostics.Process` e `ProcessStartInfo` são bloqueados em plugins porque a execução direta de processos apresenta graves riscos de segurança:

1. **Execução arbitrária de comandos**: Plugins poderiam executar qualquer comando sem auditoria ou verificação de permissões
2. **Lançamento de malware**: Plugins maliciosos poderiam executar aplicações ou scripts indesejados
3. **Acesso a recursos do sistema**: Processos poderiam aceder a recursos sensíveis fora da sandbox do plugin
4. **Sem validação de comandos**: Process.Start direto não tem proteção integrada contra injeção de comandos
5. **Sem rastro de auditoria**: Operações diretas de processo contornam o sistema de auditoria de segurança
6. **Escalada de privilégios**: Poderia gerar processos com privilégios superiores aos do plugin

## Que tipos são proibidos?

Apenas os tipos relacionados com Process são proibidos, **NÃO todo o namespace System.Diagnostics**:

| Tipo proibido | Método bloqueado | Nível de risco |
|--------------|-----------------|---------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Crítico |
| `ProcessStartInfo` | Construtor, todas as propriedades | 🔴 Crítico |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Crítico |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Alto |

## Que tipos são permitidos?

Outros tipos `System.Diagnostics` que não envolvem execução de processos permanecem disponíveis:

| Tipo permitido | Utilização | Porquê seguro |
|---------------|-----------|--------------|
| `Stopwatch` | Medição de tempo | Sem execução de processos |
| `Debug` | Saída de depuração | Sem risco de segurança |
| `Trace` | Rastreamento/registo | Sem risco de segurança |
| `PerformanceCounter` | Monitorização de desempenho | Apenas leitura, auditado |

## Como executar comandos em segurança?

### Usar CommandLineExecutor (a única forma segura)

`CommandLineExecutor` é o **ponto de entrada controlado** para execução de comandos em plugins:

```csharp
// ✅ CORRETO: Executar um comando
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Erro: {result.Error}");
}
```

**O que CommandLineExecutor fornece:**
1. **Proteção contra injeção de comandos**: Bloqueia separadores perigosos (`||`, `&&`, `|`, `&`, `;`)
2. **Aplicação de timeout**: Timeout predefinido de 30 segundos (configurável)
3. **Registo de auditoria**: Todas as execuções de comandos são registadas para revisão de segurança
4. **Captura de saída**: Captura automática de stdout e stderr
5. **Suporte multiplataforma**: Usa `cmd.exe` no Windows, `/bin/bash` no Unix
6. **Tratamento de erros**: Retorna resultado estruturado com estado de sucesso/falha

## Violações demonstradas

Este plugin mostra 5 violações comuns de execução de processos:

### Violação 1: Process.Start

```csharp
// ❌ PROIBIDO
Process.Start("notepad.exe");

// ✅ CORRETO
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Start(System.String)`

### Violação 2: ProcessStartInfo

```csharp
// ❌ PROIBIDO
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ CORRETO
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**TypeRef bloqueado**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Violação 3: Process com argumentos

```csharp
// ❌ PROIBIDO
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ CORRETO
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Violação 4: Redirecionamento de saída do processo

```csharp
// ❌ PROIBIDO
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ CORRETO
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::StandardOutput`

### Violação 5: Process.Kill

```csharp
// ❌ PROIBIDO
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ CORRETO
// Por razões de segurança, CommandLineExecutor não suporta a terminação de processos.
// Contacte o administrador do sistema se necessário.
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Kill()`

## Porquê apenas Process e não todo System.Diagnostics?

O sistema de plugins adota uma abordagem **cirúrgica** à segurança:

- **Bloquear apenas tipos perigosos**: Process/ProcessStartInfo permitem execução de código arbitrário
- **Permitir tipos seguros**: Stopwatch, Debug, Trace não têm implicações de segurança
- **Minimizar impacto**: Programadores podem continuar a usar ferramentas de diagnóstico sem risco
- **Fronteira clara**: Apenas tipos que podem criar/terminar processos são proibidos

## Mecanismo de segurança do PluginLoader

Quando o PluginLoader analisa este plugin:

1. **Análise TypeRef**: Deteta referências a tipos proibidos `Process`/`ProcessStartInfo`
2. **Análise MemberRef**: Deteta chamadas a métodos bloqueados (ex: `Process.Start`)
3. **Análise IL String**: Deteta tentativas de reflexão baseadas em strings
4. **Rejeição**: Plugin é rejeitado durante o carregamento com mensagem de erro detalhada

## Melhores práticas

1. **Usar sempre CommandLineExecutor**: Nunca usar `Process.Start` diretamente
2. **Definir timeouts razoáveis**: Prevenir que comandos bloqueiem indefinidamente
3. **Verificar resultados**: Verificar sempre `result.Success` antes de usar a saída
4. **Sanitizar entrada**: Nunca passar entrada do utilizador diretamente para comandos
5. **Declarar Capability se necessário**: Se for necessária execução de processos sem restrições, declarar `Capability.Process` (ver 15-CapabilityProcess)

## Ficheiros

- `Plugin.cs` - Plugin de demonstração anti-padrão
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Este ficheiro (Português)
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Exemplos relacionados

- **08-ForbiddenNetwork**: Operações de rede proibidas
- **15-CapabilityProcess**: Permissão declarativa Process
- **10-ForbiddenReflection**: Operações de reflexão proibidas
- **12-ForbiddenStringBypass**: Tentativas de contorno por reflexão baseada em strings
