# PluginDemo-11: Anti-padrão de P/Invoke e código unsafe proibidos

## Visão geral

Este plugin demonstra operações de P/Invoke e código unsafe **proibidas** no sistema de plugins SiliconLife. Ao contrário de outras categorias proibidas (E/S de ficheiros, rede, processos, reflexão) que têm alternativas seguras, P/Invoke e código unsafe são **proibições absolutas** — sem alternativa segura e não isentáveis por nenhuma declaração `PluginCapability`.

## Porquê P/Invoke é a ameaça definitiva?

P/Invoke e código unsafe representam a **ameaça mais fundamental** porque operam **completamente fora do runtime gerido**:

- O código nativo executa com privilégios completos do processo
- Sem segurança de tipos gerida, segurança de memória ou recolha de lixo
- Impossível interceptar, auditar ou isolar chamadas nativas
- Falha do código nativo = falha de todo o processo (sem tratamento de exceções)
- Acesso possível a qualquer endereço de memória do espaço do processo

## Mecanismo de triplo seguro

O PluginLoader utiliza **três camadas de deteção independentes**:

### Camada 1: Varrimento da tabela TypeRef

Deteta referências diretas a tipos proibidos nos metadados PE:

| Tipo proibido | Espaço de nomes | Ameaça |
|---------------|-----------------|--------|
| `DllImportAttribute` | System.Runtime.InteropServices | Declara importação de função nativa |
| `Marshal` | System.Runtime.InteropServices | Ponte de memória gerida/não gerida |
| `NativeMemory` | System.Runtime.InteropServices | Malloc/free do heap nativo |
| `NativeLibrary` | System.Runtime.InteropServices | Carregamento dinâmico de bibliotecas nativas |
| `GCHandle` | System.Runtime.InteropServices | Fixar objeto gerido, expor ponteiro |
| `Unsafe` | System.Runtime.CompilerServices | Classe auxiliar Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Marcador de código não verificável |

### Camada 2: Varrimento de marcadores Unsafe (ScanUnsafeMarkers)

| Marcador | Método de deteção | Fonte |
|----------|-------------------|-------|
| `[assembly: UnverifiableCode]` | Tabela CustomAttribute da assembly | Palavra-chave C# `unsafe` |
| `[module: UnverifiableCode]` | Tabela CustomAttribute do módulo | Palavra-chave C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Flag da tabela MethodDef | Atributo `[DllImport]` |

### Camada 3: Varrimento de strings IL (heap #US)

```
"System.Runtime.InteropServices.Marshal"  → Sinalizado
"System.Runtime.InteropServices.*"        → Sinalizado por correspondência de prefixo
```

## Violações demonstradas

### Violação 1: Declaração [DllImport]

```csharp
// ❌ PROIBIDO
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Violação 2: Uso de Marshal

```csharp
// ❌ PROIBIDO
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Violação 3: Uso de NativeMemory

```csharp
// ❌ PROIBIDO
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Violação 4: Fixação com GCHandle

```csharp
// ❌ PROIBIDO
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Violação 5: Bloco unsafe

```csharp
// ❌ PROIBIDO
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Violação 6: Carregamento de NativeLibrary

```csharp
// ❌ PROIBIDO
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Sem alternativa segura — Comparação

| Categoria proibida | Wrapper seguro | Auditável | Declarável via PluginCapability |
|-------------------|---------------|-----------|--------------------------------|
| E/S de ficheiros | PermissionedStreamFactory | ✅ Sim | ✅ Capability.FileIO |
| Rede | NetworkExecutor | ✅ Sim | ✅ Capability.Network |
| Processo | CommandLineExecutor | ✅ Sim | ✅ Capability.Process |
| Reflexão | ITypeRegistry + IObjectFactory | ✅ Sim | ❌ Sempre proibido |
| **P/Invoke e unsafe** | **❌ Nenhum** | **❌ Impossível** | **❌ Sempre proibido** |

## Se um plugin realmente precisa de código nativo

1. **Auditoria manual pelo mantenedor do projeto**
2. **Adição à lista branca `TrustedAssemblies`** no PluginLoader
3. **Identificação por `AssemblyDefinition.Name` dos metadados PE** (não nome de ficheiro)

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

- **04-SafeSystemIO**: Tipos seguros da lista branca System.IO
- **06-TrustedDependency**: Mecanismo de lista branca TrustedAssemblies
- **10-ForbiddenReflection**: Operações de reflexão proibidas
- **12-ForbiddenStringBypass**: Tentativas de contorno por strings de reflexão
