#pragma once

#include <filesystem>
namespace fsystem = std::filesystem;



#ifdef _DEBUG
#define IMPORT_CONFIG Debug
#else
#define IMPORT_CONFIG Release
#endif 

#ifdef _WIN64
#define IMPORT_PLATFORM bin x64
#else
#define IMPORT_PLATFORM bin
#endif


#import "D:\AliasPaul\bin\Debug\Core Components\ImportExport.dll" no_auto_exclude
#import "D:\AliasPaul\bin\Debug\Core Components\POD.dll" no_auto_exclude
#import "D:\AliasPaul\bin\Debug\Core Components\ProjectManagerWrapper.dll" 
#import "D:\AliasPaul\bin\Debug\Personal ISOGEN\pisogen.dll"




