Imports System.Runtime.InteropServices
Imports System.Security

Namespace NScript
    <SecurityCritical(1)>
    <SuppressUnmanagedCodeSecurity()>
    Friend Module SafeNativeMethods

        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function GetProcAddress(LibHandle As IntPtr, <MarshalAs(UnmanagedType.LPStr)> funcname As String) As IntPtr

        End Function
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function LoadLibraryW(<MarshalAs(UnmanagedType.BStr)> libname As String) As IntPtr

        End Function
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function FreeLibrary(libname As IntPtr) As IntPtr

        End Function
        <DefaultDllImportSearchPaths(DllImportSearchPath.System32)> <DllImport("Kernel32.dll")> Public Function VirtualAlloc(lpAddress As IntPtr, size As Long, flAllocationType As Long, flProtect As Long) As IntPtr

        End Function
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
