// PODImports.h : Required imports for POD framework
//
#pragma once
#pragma once

#import <Msxml6.dll> rename ("value","Msxvalue") exclude("ISequentialStream", "_FILETIME")

/*#pragma comment (linker, "\"/manifestdependency:name='Personal Isogen' version='11.0.0.0' type='win32' processorArchitecture='x86'\"")
#pragma comment (linker, "\"/manifestdependency:name='Core Components' version='7.0.0.0' type='win32' processorArchitecture='x86'\"")
#pragma comment (linker, "\"/manifestdependency:name='Application Components' version='7.0.0.0' type='win32' processorArchitecture='x86'\"")
#pragma comment (linker, "\"/manifestdependency:name='Symbols' version='7.0.0.0' processorArchitecture='X86'\"")
#pragma comment (linker, "\"/manifestdependency:name='SPLAT' version='2.0.0.0' processorArchitecture='X86'\"")*/



#ifdef _DEBUG
#import "..\..\..\Bin\Debug\Core Components\POD.dll" no_auto_exclude
#import "..\..\Bin\Debug\Core Components\MaterialDataWrapper.dll" no_auto_exclude
//#import "..\..\Bin\Debug\Core Components\ProjectManagerWrapper.dll" rename_namespace("AliasProjectManagerWrapper") rename ("DetailSketches","PMDetailSketches")
//#import "..\..\Bin\Debug\Core Components\ProjectManagerLVB.dll" no_namespace
#import "..\..\Bin\Debug\Core Components\ImportExport.dll" no_auto_exclude 
//#import "..\..\Bin\Debug\Personal Isogen\pisogen.dll"
#else
#import "..\..\Bin\Release\Core Components\POD.dll" no_namespace no_function_mapping exclude("IStream", "ISequentialStream", "_LARGE_INTEGER", "_ULARGE_INTEGER", "tagSTATSTG", "_FILETIME")
#import "..\..\Bin\Release\Core Components\MaterialDataWrapper.dll"
#import "..\..\Bin\Release\Core Components\ProjectManagerWrapper.dll" rename_namespace("AliasProjectManagerWrapper") rename ("DetailSketches","PMDetailSketches")
#import "..\..\Bin\Release\Core Components\ProjectManagerLVB.dll" no_namespace
#import "..\..\Bin\Release\Core Components\ImportExport.dll" no_namespace
#import "..\..\Bin\Release\Personal Isogen\pisogen.dll"
#endif

#ifdef USE_SYMBOLS_LIBRARY
#import "Symbols.tlb" //moved to shared TFS so included in additional included thus removed lib path
#endif
