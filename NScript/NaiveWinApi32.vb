Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Security

Namespace NScript
    <SecurityCritical(1)>
    <SuppressUnmanagedCodeSecurity()>
    Friend Module SafeNativeMethods
        Friend Structure MouseHookStruct
            Dim PT As Point
            Dim Hwnd As Integer
            Dim WHitTestCode As Integer
            Dim DwExtraInfo As Integer
        End Structure

        Friend Structure MouseLLHookStruct
            Dim PT As Point
            Dim MouseData As Integer
            Dim Flags As Integer
            Dim Time As Integer
            Dim DwExtraInfo As Integer
        End Structure

        Friend Structure KeyboardHookStruct
            Dim vkCode As Integer
            Dim ScanCode As Integer
            Dim Flags As Integer
            Dim Time As Integer
            Dim DwExtraInfo As Integer
        End Structure
        Friend Declare Function GetWindowThreadProcessId Lib "user32" (hwnd As IntPtr, ByRef ProcessId As Long) As Long
        Friend Delegate Function HookProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MouseHookStruct) As Integer
        Friend Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As HookProc, ByVal hMod As IntPtr, ByVal dwThreadId As Integer) As Integer
        Friend Declare Function UnhookWindowsHookEx Lib "user32" (ByVal idHook As Integer) As Integer
        Friend Declare Function CallNextHookEx Lib "user32" (ByVal idHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MouseHookStruct) As Integer
        Friend Declare Function ToAscii Lib "user32" (ByVal uVirtKey As Integer, ByVal uScancode As Integer, ByVal lpdKeyState As Byte(), ByVal lpwTransKey As Byte(), ByVal fuState As Integer) As Integer
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function GetProcAddress(LibHandle As IntPtr, <MarshalAs(UnmanagedType.LPStr)> funcname As String) As IntPtr

        End Function
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function LoadLibraryW(<MarshalAs(UnmanagedType.BStr)> libname As String) As IntPtr

        End Function
        Friend Declare Unicode Function LoadLibrary Lib "kernel32" Alias "LoadLibraryW" (lpLibFileName As String) As IntPtr
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function FreeLibrary(libname As IntPtr) As IntPtr

        End Function
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function VirtualAlloc(lpAddress As IntPtr, size As Long, flAllocationType As Long, flProtect As Long) As IntPtr

        End Function
        Public Declare Function GetConsoleWindow Lib "Kernel32.dll" () As IntPtr
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> Public Declare Sub ExitThread Lib "kernel32" (ByVal uExitCode As Int32)
        Public Enum PROCESSINFOCLASS
            ProcessBasicInformation = 0
            ProcessQuotaLimits
            ProcessIoCounters
            ProcessVmCounters
            ProcessTimes
            ProcessBasePriority
            ProcessRaisePriority
            ProcessDebugPort = 7
            ProcessExceptionPort
            ProcessAccessToken
            ProcessLdtInformation
            ProcessLdtSize
            ProcessDefaultHardErrorMode
            ProcessIoPortHandlers
            ProcessPooledUsageAndLimits
            ProcessWorkingSetWatch
            ProcessUserModeIOPL
            ProcessEnableAlignmentFaultFixup
            ProcessPriorityClass
            ProcessWx86Information
            ProcessHandleCount
            ProcessAffinityMask
            ProcessPriorityBoost
            ProcessDeviceMap
            ProcessSessionInformation
            ProcessForegroundInformation
            ProcessWow64Information = &H1A
            ProcessImageFileName = &H1B
            ProcessLUIDDeviceMapsEnabled
            ProcessBreakOnTermination
            ProcessDebugObjectHandle = &H1E
            ProcessDebugFlags
            ProcessHandleTracing
            ProcessIoPriority
            ProcessExecuteFlags
            ProcessResourceManagement
            ProcessCookie
            ProcessImageInformation
            ProcessCycleTime
            ProcessPagePriority
            ProcessInstrumentationCallback
            ProcessThreadStackAllocation
            ProcessWorkingSetWatchEx
            ProcessImageFileNameWin32
            ProcessImageFileMapping
            ProcessAffinityUpdateMode
            ProcessMemoryAllocationMode
            ProcessGroupInformation
            ProcessTokenVirtualizationEnabled
            ProcessConsoleHostProcess
            ProcessWindowInformation
            ProcessHandleInformation
            ProcessMitigationPolicy
            ProcessDynamicFunctionTableInformation
            ProcessHandleCheckingMode
            ProcessKeepAliveCount
            ProcessRevokeFileHandles
            ProcessWorkingSetControl
            ProcessHandleTable
            ProcessCheckStackExtentsMode
            ProcessCommandLineInformation
            ProcessProtectionInformation
            ProcessMemoryExhaustion
            ProcessFaultInformation
            ProcessTelemetryIdInformation
            ProcessCommitReleaseInformation
            ProcessDefaultCpuSetsInformation
            ProcessAllowedCpuSetsInformation
            ProcessSubsystemProcess
            ProcessJobMemoryInformation
            ProcessInPrivate
            ProcessRaiseUMExceptionOnInvalidHandleClose
            ProcessIumChallengeResponse
            ProcessChildProcessInformation
            ProcessHighGraphicsPriorityInformation
            ProcessSubsystemInformation
            ProcessEnergyValues
            ProcessActivityThrottleState
            ProcessActivityThrottlePolicy
            ProcessWin32kSyscallFilterInformation
            ProcessDisableSystemAllowedCpuSets
            ProcessWakeInformation
            ProcessEnergyTrackingState
            ProcessManageWritesToExecutableMemoryREDSTONE3
            ProcessCaptureTrustletLiveDump
            ProcessTelemetryCoverage
            ProcessEnclaveInformation
            ProcessEnableReadWriteVmLogging
            ProcessUptimeInformation
            ProcessImageSection
            ProcessDebugAuthInformation
            ProcessSystemResourceManagement
            ProcessSequenceNumber
            ProcessLoaderDetour
            ProcessSecurityDomainInformation
            ProcessCombineSecurityDomainsInformation
            ProcessEnableLogging
            ProcessLeapSecondInformation
            ProcessFiberShadowStackAllocation
            ProcessFreeFiberShadowStackAllocation
            MaxProcessInfoClass
        End Enum
        Public Structure ProcessBasicInfo
            Public ExitStatus, PebBaseAddress, AffinityMask, BasePriority, UniqueProcessId, InheritedFromUniqueProcessId As Long
        End Structure
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> Public Declare Function NtQueryInformationProcess Lib "ntdll" (Process As IntPtr, IC As PROCESSINFOCLASS, ByRef PI As Boolean, PILen As Long, ByRef ReturnLen As Long) As Int32
    End Module
End Namespace
